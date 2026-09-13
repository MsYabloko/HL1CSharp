using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EntityState
{
    public int		entityType;
    // Index into cl_entities array for this entity.
    public int		number;      
    public float		msg_time;

    // Message number last time the player/entity state was updated.
    public int		messagenum;

    public Vector origin;
    public Vector angles;
    
    public int		modelindex;
    public int		sequence;
    public float		frame;
    public int		colormap;
    public short		skin;
    public short		solid;
    public int		effects;
    public float		scale;
    public byte		eflags;
    
    public int		rendermode;
    public int		renderamt;
    public Color24		rendercolor;
    public int		renderfx;
    
    public int		movetype;
    public float		animtime;
    public float		framerate;
    public int		body;
    public fixed byte		controller[4];
    public fixed byte		blending[4];
    public Vector		velocity;

    public Vector mins;
    public Vector maxs;

    public int aiment;
    public int owner;

    public float friction;
    public float gravity;
    
    public int		team;
    public int		playerclass;
    public int		health;
    public int		spectator;  
    public int		weaponmodel;
    public int		gaitsequence;

    public Vector basevelocity;
    
    public int		usehull;		
    // Latched buttons last time state updated.
    public int		oldbuttons;     
    // -1 = in air, else pmove entity number
    public int		onground;		
    public int		iStepLeft;
    // How fast we are falling
    public float		flFallVelocity;  

    public float		fov;
    public int		weaponanim;

    public Vector startpos;
    public Vector endpos;
    public float		impacttime;
    public float		starttime;
    
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