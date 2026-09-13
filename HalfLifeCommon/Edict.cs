using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Link
{
    public Link* prev;
    public Link* next;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Edict
{
    public int free;
    public int serialnumber;
    public Link area;
    public int headnode;

    public int num_leafs;
    public fixed short leafnums[48]; // 48

    public float freetime;
    
    public void* pvPrivateData;
    public EntVars v;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EntVars
{
    public int classname;
    public int globalname;
    
    public Vector origin;
    public Vector oldorigin;
    public Vector velocity;
    public Vector basevelocity;
    public Vector clbasevelocity;

    public Vector movedir;

    public Vector angles;
    public Vector avelocity;
    public Vector punchangle;
    public Vector v_angle;

    public Vector endpos;
    public Vector startpos;
    public float impacttime;
    public float starttime;

    public int fixangle;
    public float idealpitch;
    public float pitch_speed;
    public float ideal_yaw;
    public float yaw_speed;

    public int modelindex;

    public int model;
    public int viewmodel;
    public int weaponmodel;

    public Vector absmin;
    public Vector absmax;
    public Vector mins;
    public Vector maxs;
    public Vector size;

    public float ltime;
    public float nextthink;

    public int movetype;
    public int solid;

    public int skin;
    public int body;
    public int effects;
    public float gravity;
    public float friction;

    public int light_level;

    public int sequence;
    public int gaitsequence;
    public float frame;
    public float animtime;
    public float framerate;
    public fixed byte controller[4];
    public fixed byte blending[2];

    public float scale;
    public int rendermode;
    public float renderamt;
    public Vector rendercolor;
    public int renderfx;

    public float health;
    public float frags;
    public int weapons;
    public float takedamage;

    public int deadflag;
    public Vector view_ofs;

    public int button;
    public int impulse;

    public Edict* chain;
    public Edict* dmg_inflictor;
    public Edict* enemy;
    public Edict* aiment;
    public Edict* owner;
    public Edict* groundentity;

    public int spawnflags;
    public int flags;
    
    public int colormap;
    public int team;

    public float max_health;
    public float teleport_time;
    public float armortype;
    public float armorvalue;
    public int waterlevel;
    public int watertype;

    public int target;
    public int targetname;
    public int netname;
    public int message;

    public float dmg_take;
    public float dmg_save;
    public float dmg;
    public float dmgtime;

    public int noise;
    public int noise1;
    public int noise2;
    public int noise3;

    public float speed;
    public float air_finished;
    public float pain_finished;
    public float radsuit_finished;

    public Edict* pContainingEntity;

    public int playerclass;
    public float maxspeed;

    public float fov;
    public int weaponanim;

    public int pushmsec;

    public int bInDuck;
    public int flTimeStepSound;
    public int flSwimTime;
    public int flDuckTime;
    public int iStepLeft;
    public float flFallVelocity;

    public int gamestate;

    public int oldbuttons;

    public int groupinfo;

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
    public Edict* euser1;
    public Edict* euser2;
    public Edict* euser3;
    public Edict* euser4;
}
[StructLayout(LayoutKind.Sequential)]
public unsafe struct Delta
{
    public IntPtr name;
    public int offset;
    public int size;
    public int flags;
    public float multiplier;
    public float post_multiplier;
    public int bits;
    public int bInactive;
}