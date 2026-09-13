using System.Runtime.InteropServices;
using XashGameDLL;

namespace HalfLifeClient;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct LocalState
{
    public EntityState playerstate;
    public ClientData client;
}