using System.Runtime.InteropServices;
using XashGameDLL;

namespace HalfLifeClient;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct RefParams
{
    public Vector vieworg;
    public Vector viewangles;
    
    public Vector forward;
    public Vector right;
    public Vector up;

    public float frametime;
    public float time;
    
    public int		intermission;
    public int		paused;
    public int		spectator;
    public int		onground;
    public int		waterlevel;

    public Vector simvel;
    public Vector simorg;
    
    public Vector viewheight;
    public float idealpitch;
    
    public Vector		cl_viewangles;
    public int		health;
    public Vector		crosshairangle;
    public float		viewsize;

    public Vector		punchangle;
    public int		maxclients;
    public int		viewentity;
    public int		playernum;
    public int		max_entities;
    public int		demoplayback;	
    public int		hardware;
    public int		smoothing;

    public UserCmd* cmd;
    public IntPtr movevars;

    public fixed int viewport[4];
    public int nextView;

    public int onlyClientDraw;

    public float fov_x;
    public float fov_y;
}