using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Color24
{
    public byte R;
    public byte G;
    public byte B;
}