using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct GlobalVars
{
    public float		time;
    public float		frametime;
    public float		force_retouch;
    public int mapname;
    public int startspot;
    public float		deathmatch;
    public float		coop;
    public float		teamplay;
    public float		serverflags;
    public float		found_secrets;
    public Vector		v_forward;
    public Vector		v_up;
    public Vector		v_right;
    public float		trace_allsolid;
    public float		trace_startsolid;
    public float		trace_fraction;
    public Vector		trace_endpos;
    public Vector		trace_plane_normal;
    public float		trace_plane_dist;
    public Edict*       trace_ent;
    public float		trace_inopen;
    public float		trace_inwater;
    public int		trace_hitgroup;
    public int		trace_flags;
    public int		changelevel;	// transition in progress when true (was msg_entity)
    public int		cdAudioTrack;
    public int		maxClients;
    public int		maxEntities;
    public IntPtr pStringBase;
    public void* pSaveData;
    public Vector vecLandmarkOffset;

    internal static GlobalVars* Vars;
}