using System.Runtime.InteropServices;

namespace HalfLifeClient;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Mouth
{
    public byte mouthopen;
    public byte sndcount;
    public int sndavg;
}