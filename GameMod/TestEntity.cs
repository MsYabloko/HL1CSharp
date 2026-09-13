using XashGameDLL;

namespace GameMod;

[EntityClass("test_entity")]
public class TestEntity : BaseEntity
{
    public override void Spawn()
    {
        SetNextThink(1);
    }

    public override void Think()
    {
        Console.WriteLine("Test Entity Thinking");
        SetNextThink(1);
    }

    public override void Use()
    {
        
    }

    public override void Touch()
    {
        
    }
}