using System.Runtime.InteropServices;

namespace HalfLifeClient;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EventApi
{
    public delegate* unmanaged[Cdecl]<int, float*, int, IntPtr, float, float, int, int, void> EV_PlaySound;
    public delegate* unmanaged[Cdecl]<int, int, IntPtr, void> EV_StopSound;
    public delegate* unmanaged[Cdecl]<IntPtr, int> EV_FindModelIndex;
    public delegate* unmanaged[Cdecl]<int, int> EV_IsLocal;
    public delegate* unmanaged[Cdecl]<int> EV_LocalPlayerDucking;
    public delegate* unmanaged[Cdecl]<float*, void> EV_LocalPlayerViewheight;
    public delegate* unmanaged[Cdecl]<int, float*, float*, void> EV_LocalPlayerBounds;
    public delegate* unmanaged[Cdecl]<IntPtr, int> EV_IndexFromTrace; // pmtrace_s* as IntPtr
    public delegate* unmanaged[Cdecl]<int, IntPtr> EV_GetPhysent; // physent_s* as IntPtr
    public delegate* unmanaged[Cdecl]<int, int, void> EV_SetUpPlayerPrediction;
    public delegate* unmanaged[Cdecl]<void> EV_PushPMStates;
    public delegate* unmanaged[Cdecl]<void> EV_PopPMStates;
    public delegate* unmanaged[Cdecl]<int, void> EV_SetSolidPlayers;
    public delegate* unmanaged[Cdecl]<int, void> EV_SetTraceHull;
    public delegate* unmanaged[Cdecl]<float*, float*, int, int, IntPtr, void> EV_PlayerTrace;
    public delegate* unmanaged[Cdecl]<int, int, void> EV_WeaponAnimation;
    public delegate* unmanaged[Cdecl]<int, IntPtr, ushort> EV_PrecacheEvent;
    public delegate* unmanaged[Cdecl]<int, IntPtr, ushort, float, float*, float*, float, float, int, int, int, int, void> EV_PlaybackEvent;
    public delegate* unmanaged[Cdecl]<int, float*, float*, IntPtr> EV_TraceTexture;
    public delegate* unmanaged[Cdecl]<int, int, void> EV_StopAllSounds;
    public delegate* unmanaged[Cdecl]<int, IntPtr, void> EV_KillEvents;

    // Xash3D extension
    public delegate* unmanaged[Cdecl]<IntPtr, ushort> EV_IndexForEvent;
    public delegate* unmanaged[Cdecl]<ushort, IntPtr> EV_EventForIndex;
    public delegate* unmanaged[Cdecl]<float*, float*, int, delegate* unmanaged[Cdecl]<IntPtr, int>, IntPtr, void> EV_PlayerTraceExt;
    public delegate* unmanaged[Cdecl]<int, IntPtr> EV_SoundForIndex;
    public delegate* unmanaged[Cdecl]<int, float*, float*, IntPtr> EV_TraceSurface;
}