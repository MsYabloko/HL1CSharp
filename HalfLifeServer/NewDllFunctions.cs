using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct NewDllFunctions
{
    public IntPtr pfnOnFreeEntPrivateData;
    public IntPtr pfnGameShutdown;
    public IntPtr pfnShouldCollide;
    public IntPtr pfnCvarValue;
    public IntPtr pfnCvarValue2;
}