using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct UserCmd
{
    public short lerp_msec;
    public short msec;
    public Vector viewangles;
    
    public float forwardmove;
    public float sidemove;
    public float upmove;
    public byte lightlevel;
    public ushort buttons;
    public byte impulse;
    public byte weaponselect;

    public int impact_index;
    public Vector impact_position;
}