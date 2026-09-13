using System.Runtime.InteropServices;
using XashGameDLL;

namespace HalfLifeClient;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ClEntity
{
    public int index;
    public int player;

    public EntityState baseline;
    public EntityState prevstate;
    public EntityState curstate;

    public int current_position;
    private fixed byte ph[64 * sizeof(float) * 7];

    public Mouth mouth;
    public LatchedVars latched;

    public float lastmove;

    public Vector origin;
    public Vector angles;

    public fixed float attachment[4 * 3];

    public int trivial_accept;

    public Model* model;
    private IntPtr efrag;
    private IntPtr topnode;

    public float syncbase;
    public int visframe;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct LatchedVars
{
    public float		prevanimtime;  
    public float		sequencetime;
    public fixed byte		prevseqblending[2];
    public Vector		prevorigin;
    public Vector		prevangles;

    public int		prevsequence;
    public float		prevframe;

    public fixed byte		prevcontroller[4];
    public fixed byte		prevblending[2];
}