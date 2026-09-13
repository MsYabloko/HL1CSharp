using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PmPlane
{
    public Vector normal;
    public float dist;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PmTrace
{
    public int allsolid;
    public int startsolid;
    public int inopen, inwater;
    public float fraction;
    public Vector endpos;
    public PmPlane plane;
    public int ent;
    public Vector deltavelocity;

    public int hitgroup;
}