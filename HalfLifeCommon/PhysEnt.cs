using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PhysEnt
{
    public fixed char name[32];
    public int player;
    public Vector origin;
    public Model* model;
    public Model* studiomodel;
    public Vector mins, maxs;
    public int info;
    public Vector angles;

    public int solid;
    public int skin;
    public int rendermode;

    public float frame;
    public int sequence;
    public fixed byte controller[4];
    public fixed byte blending[2];
    
    public int		movetype;
    public int		takedamage;
    public int		blooddecal;
    public int		team;
    public int		classnumber;
    
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
    
    //public const int Size = sizeof(char) * 32 + sizeof(int) * 15 + sizeof(float) * 5 + Vector.Size * 8 + Util.IntPtrSize * 2 + sizeof(byte) * 6;
    public const int Size = 232;
}