using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ClientData
{
    public Vector origin;
    public Vector velocity;

    public int viewmodel;
    public Vector		punchangle;
    public int		flags;
    public int		waterlevel;
    public int		watertype;
    public Vector		view_ofs;
    public float		health;
    
    public int		bInDuck;
    public int		weapons; // remove?
	
    public int		flTimeStepSound;
    public int		flDuckTime;
    public int		flSwimTime;
    public int		waterjumptime;

    public float		maxspeed;

    public float		fov;
    public int		weaponanim;

    public int		m_iId;
    public int		ammo_shells;
    public int		ammo_nails;
    public int		ammo_cells;
    public int		ammo_rockets;
    public float		m_flNextAttack;
	
    public int		tfstate;
    public int		pushmsec;
    public int		deadflag;

    public fixed char physinfo[256];
    
    public int		iuser1;
    public int		iuser2;
    public int		iuser3;
    public int		iuser4;
    public float		fuser1;
    public float		fuser2;
    public float		fuser3;
    public float		fuser4;
    public Vector		vuser1;
    public Vector		vuser2;
    public Vector		vuser3;
    public Vector		vuser4;
}