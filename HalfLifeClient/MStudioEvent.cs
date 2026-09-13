using System.Runtime.InteropServices;

namespace HalfLifeClient;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct MStudioEvent
{
    public int frame;
    public int eventt;
    public int type;
    public fixed byte options[64];
}