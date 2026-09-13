using System.Runtime.InteropServices;

namespace HalfLifeClient;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ScreenInfo
{
    public int		iSize;
    public int		iWidth;
    public int		iHeight;
    public int		iFlags;
    public int		iCharHeight;
    public fixed short charWidths[256];
}