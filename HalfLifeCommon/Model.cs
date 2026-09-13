using System.Runtime.InteropServices;

namespace XashGameDLL;

public enum ModType : int
{
    mod_brush = 0,
    mod_sprite = 1,
    mod_alias = 2,
    mod_studio = 3
}
public enum SyncType : int
{
    ST_SYNC = 0,
    ST_RAND = 1
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Model
{
    public fixed char name[64];
    public int needload;
    public ModType type;
    public int numframes;
    public SyncType synctype;

    public int flags;

    public Vector mins;
    public Vector maxs;
    public float radius;
}