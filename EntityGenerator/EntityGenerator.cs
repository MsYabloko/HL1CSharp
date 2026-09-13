using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EntityGenerator;

[Generator]
public class EntityGenerator : IIncrementalGenerator
{
    static private bool IsDerivedFrom(INamedTypeSymbol type, string name)
    {
        var current = type.BaseType;
        while (current != null)
        {
            if (current.Name.Trim() == name)
                return true;
            current = current.BaseType;
        }
        return false;
    }

    static private void AddEntityInitializer(INamedTypeSymbol classSymbol, ref StringBuilder builder)
    {
        var attibute = classSymbol.GetAttributes().FirstOrDefault(t => t.AttributeClass.Name.Trim() == "EntityClassAttribute");
        if(attibute == null) return;

        string classname = attibute.ConstructorArguments[0].Value.ToString();
        
        builder.AppendLine(
            $"      [UnmanagedCallersOnly(CallConvs = new[] {{ typeof(CallConvCdecl) }}, EntryPoint = \"{classname}\")]");
        builder.AppendLine("        public static void SpawnEntity(EntVars* e)");
        builder.AppendLine("        {");
        builder.AppendLine($"               BaseEntity.CreateEntity<{classSymbol.Name.Trim()}>(e->pContainingEntity);");
        builder.AppendLine("        }");
    }

    static private void AddMemberLines(MemberDeclarationSyntax member, ref StringBuilder builder)
    {
        var memberText = member.ToFullString();
        var indentedMember = string.Join("\n", memberText.Split('\n').Select(line => "  " + line));
        builder.AppendLine(indentedMember);
    }
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node)
            .Collect();
        var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations);
        
        var compilationUnits = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (node, _) => node is CompilationUnitSyntax,
            transform: static (ctx, _) => (CompilationUnitSyntax)ctx.Node
        );
        var allUsings = compilationUnits.Select((compilationUnit, _) => compilationUnit.Usings);
        
        var sourceBuilderServer = new StringBuilder();
        
        context.RegisterSourceOutput(allUsings, (spc, usings) =>
        {
            foreach (var usingDirective in usings)
            {
                var usingText = usingDirective.Name.ToString();
                sourceBuilderServer.AppendLine("using " + usingText + ";");
            }
        });

        sourceBuilderServer.AppendLine("using System.Runtime.CompilerServices;\nusing System.Runtime.InteropServices;");

        sourceBuilderServer.AppendLine("namespace GameMod.Entities.Server;");
        
        context.RegisterSourceOutput(compilationAndClasses, (spc, source) =>
        {
            var (compilation, classes) = source;
            
            var relevantClasses = classes
                .Select(c => ModelExtensions.GetDeclaredSymbol(compilation.GetSemanticModel(c.SyntaxTree), c))
                .OfType<INamedTypeSymbol>()
                .Where(t => IsDerivedFrom(t, "BaseEntity"))
                .ToList();

            foreach (var classSymbol in relevantClasses)
            {
                var syntaxRef = classSymbol.DeclaringSyntaxReferences.FirstOrDefault();
                if(syntaxRef == null) continue;

                var classSyntax = syntaxRef.GetSyntax() as ClassDeclarationSyntax;
                if(classSyntax == null) continue;

                var modifiers = classSyntax.Modifiers.Add(SyntaxFactory.Token(SyntaxKind.UnsafeKeyword)).ToFullString().Trim();
                var baseClass = classSymbol.BaseType;
                sourceBuilderServer.Append($"{modifiers} class {classSymbol.Name}");
                if (baseClass != null) sourceBuilderServer.AppendLine($" : {baseClass.Name}");
                sourceBuilderServer.AppendLine("{");
                foreach (var member in classSyntax.Members)
                {
                    AddMemberLines(member, ref sourceBuilderServer);
                }

                AddEntityInitializer(classSymbol, ref sourceBuilderServer);
                
                sourceBuilderServer.AppendLine("}");
            }
            
            spc.AddSource("ServerEntities.g.cs", SourceText.From(sourceBuilderServer.ToString(), Encoding.UTF8));
        });
        
        
    }
}