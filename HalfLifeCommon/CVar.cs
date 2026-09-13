using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct CVar
{
    public IntPtr name;
    public IntPtr stringg;
    public int flags;
    public float value;
    public CVar* next;
}