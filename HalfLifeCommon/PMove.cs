using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PlayerMove
{
    public int PlayerIndex;
    public int Server;
    public int Multiplayer;

    public float Time;
    public float FrameTime;

    public Vector Forward;
    public Vector Right;
    public Vector Up;

    public Vector Origin;
    public Vector Angles;
    public Vector OldAngles;
    public Vector Velocity;
    public Vector MoveDir;
    public Vector BaseVelocity;
    
    public Vector ViewOfs;
    public float flDuckTime;
    public int bInDuck;

    public int flTimeStepSound;
    public int iStepLeft;

    public float flFallVelocity;
    public Vector punchangle;
    
    public float		flSwimTime;
    public float		flNextPrimaryAttack;

    public int	effects;

    public int	flags;
    public int	usehull;
    public float gravity;
    public float friction;
    public int	oldbuttons;
    public float waterjumptime;

    public int dead;
    public int deadflag;
    public int spectator;
    public int movetype;
    
    public int		onground;
    public int		waterlevel;
    public int		watertype;
    public int		oldwaterlevel;
    
    public fixed byte sztexturename[256];
    public byte chtexturetype;

    public float maxspeed;
    public float clientmaxspeed;

    public int iuser1;
    public int iuser2;
    public int iuser3;
    public int iuser4;
    public float fuser1;
    public float fuser2;
    public float fuser3;
    public float fuser4;
    public Vector vuser1;
    public Vector vuser2;
    public Vector vuser3;
    public Vector vuser4;

    public int numphysent;
    public fixed byte physents[PhysEnt.Size * 600];
    public int nummovent;
    public fixed byte moveents[PhysEnt.Size * 64];
    public int numvisent;
    public fixed byte visents[PhysEnt.Size * 600];

    public int fix;
    public int fix2;
    
    public UserCmd cmd;
    
    public int numtouch;
    public fixed byte touchindex[68 * 600];
    public fixed byte physinfo[256];

    public IntPtr movevars;
    public fixed byte player_mins[4 * sizeof(float) * 3];
    public fixed byte player_maxs[4 * sizeof(float) * 3];
    
    public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr> PM_Info_ValueForKey;
    IntPtr PM_Particle;
    public delegate* unmanaged[Cdecl]<float*, PmTrace*, int> PM_TestPlayerPosition;
    IntPtr Con_NPrintf;
    IntPtr Con_DPrintf;
    IntPtr Con_Printf;
    IntPtr Sys_FloatTime;
    IntPtr PM_StuckTouch;
    IntPtr PM_PointContents;
    IntPtr PM_TruePointContents;
    IntPtr PM_HullPointContents;
    public delegate* unmanaged[Cdecl]<float*, float*, int, int, PmTrace> PM_PlayerTrace;
}