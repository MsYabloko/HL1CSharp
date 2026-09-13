namespace XashGameDLL;

[AttributeUsage(AttributeTargets.Class)]
public class EntityClassAttribute : Attribute
{
    public string ClassName;

    public EntityClassAttribute(string className)
    {
        ClassName = className;
    }
}