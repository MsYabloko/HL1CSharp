using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using XashGameDLL;

namespace HalfLifeClient;

public unsafe static class FUNCTIONS
{
    private static ClEngineFuncs* g_pEngineFuncs = null;
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "Initialize")]
    public static void Initialize(ClEngineFuncs* pengfuncsFromEngine, int iVersion)
    {
        g_pEngineFuncs = (ClEngineFuncs*)Marshal.AllocHGlobal(sizeof(ClEngineFuncs)).ToPointer();
        Buffer.MemoryCopy(pengfuncsFromEngine, g_pEngineFuncs, sizeof(ClEngineFuncs), sizeof(ClEngineFuncs));
        Console.WriteLine("ClientDLL initialized with version " + iVersion);
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_VidInit")]
    public static int HUD_VidInit()
    {
        Console.WriteLine("Hud VidInit");
        return 1;
    }

    private static bool isForward, isBackward, isRight, isLeft;
    private static bool isShoot = false;
    private static int impulse;
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void KeyDownUp()
    {
        string bind = Marshal.PtrToStringAnsi(g_pEngineFuncs->Cmd_Argv(0));
        if (bind == "+forward") isForward = true;
        if (bind == "-forward") isForward = false;
        if (bind == "+moveleft") isLeft = true;
        if (bind == "-moveleft") isLeft = false;
        if (bind == "+moveright") isRight = true;
        if (bind == "-moveright") isRight = false;
        if (bind == "+back") isBackward = true;
        if (bind == "-back") isBackward = false;
        if (bind == "impulse") impulse = int.Parse(Marshal.PtrToStringAnsi(g_pEngineFuncs->Cmd_Argv(1)));
        if (bind == "+attack") isShoot = true;
        if (bind == "-attack") isShoot = false;
        Console.WriteLine("KeyDownUp    " + bind);
    }

    private static readonly IntPtr escapeStr = Marshal.StringToHGlobalAnsi("escape");
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void CancelSelect()
    {
        g_pEngineFuncs->pfnClientCmd(escapeStr);
    }

    private static CVar* sensitivity = null;

    //public static readonly delegate* unmanaged[Cdecl]<void> pfnKeyDownUp = &KeyDownUp;
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_Init")]
    public static int HUD_Init()
    {
        sensitivity = g_pEngineFuncs->pfnRegisterVariable(Marshal.StringToHGlobalAnsi("sensitivity"), Marshal.StringToHGlobalAnsi("3"),
            (1<<0) | (1<<11));
        
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("cancelselect"), &CancelSelect);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("+attack"), &KeyDownUp);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("-attack"), &KeyDownUp);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("+forward"), &KeyDownUp);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("-forward"), &KeyDownUp);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("+moveleft"), &KeyDownUp);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("-moveleft"), &KeyDownUp);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("+moveright"), &KeyDownUp);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("-moveright"), &KeyDownUp);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("+back"), &KeyDownUp);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("-back"), &KeyDownUp);
        g_pEngineFuncs->pfnAddCommand(Marshal.StringToHGlobalAnsi("impulse"), &KeyDownUp);
        //TODO: Init Input
        return 1;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_Shutdown")]
    public static void HUD_Shutdown()
    {
        Console.WriteLine("Hud Shutdown Called");
        //TODO: Shutdown input
    }
    private static IntPtr TestString = Marshal.StringToHGlobalAnsi("Test String");
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_Redraw")]
    public static int HUD_Redraw(float time, int intermission)
    {
        Console.WriteLine("HUD Redraw");
        //TODO: Draw
        //g_pEngineFuncs->pfnDrawString(50, 50, TestString, 255, 0, 0);
        //var test = (delegate* unmanaged[Cdecl]<int, int, int, int, int, int, int, int, int, int, int, int>)g_pEngineFuncs->pfnDrawString;
        //test(0,0,0,0,0,0,0,0,0,0,0);
        //g_pEngineFuncs->pfnDrawCharacter(50, 50, (int)'A', 255, 0, 0);

        ScreenInfo screenInfo;
        screenInfo.iSize = Marshal.SizeOf<ScreenInfo>();
        g_pEngineFuncs->pfnGetScreenInfo(&screenInfo);
        Console.WriteLine("Screen Width: " + screenInfo.iWidth);
        g_pEngineFuncs->pfnDrawSetTextColor(1, 0, 0);
        g_pEngineFuncs->pfnDrawConsoleString(50, 50, TestString);
        g_pEngineFuncs->pfnDrawConsoleString(50, 250, TestString);
        
        Console.WriteLine("M Size: " + screenInfo.charWidths[(int)'M']);
        
        g_pEngineFuncs->pfnDrawCharacter(250, 50, (int)'5', 255, 0, 0);
        g_pEngineFuncs->pfnDrawCharacter(280, 50, (int)'6', 255, 0, 0);
        g_pEngineFuncs->pfnDrawCharacter(310, 50, (int)'9', 255, 0, 0);
        
        return 1;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_UpdateClientData")]
    public static int HUD_UpdateClientData(ClientData* pcldata, float flTime)
    {
        //INPUT Commands
        Console.WriteLine("HUD_UpdateClientData");
        return 1;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_Reset")]
    public static void HUD_Reset()
    {
        Console.WriteLine("HUD Reset");
        //VidInit again
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_PlayerMoveInit")]
    public static void HUD_PlayerMoveInit()
    {
        Console.WriteLine("Plyare Move Init");
        //TODO: Run PM_Init here
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_PlayerMove")]
    public static void HUD_PlayerMove(PlayerMove* ppmove, int server)
    {
        Console.WriteLine("Plyare Move");
        //TODO: Call PM_Move
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_PlayerMoveTexture")]
    public static byte HUD_PlayerMoveTexture(IntPtr name)
    {
        return (byte)'C';
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_ConnectionlessPacket")]
    public static int HUD_ConnectionlessPacket(IntPtr net_from, IntPtr args, byte* response_buffer, int* response_buffer_size)
    {
        *response_buffer_size = 0;

        return 0;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_GetHullBounds")]
    public static int HUD_GetHullBounds(int hullnumber, float* mins, float* maxs)
    {
        switch (hullnumber)
        {
            case 0:
                mins[0] = -16;
                mins[1] = -16;
                mins[2] = -36;
                maxs[0] = 16;
                maxs[1] = 16;
                maxs[2] = 36;
                break;
            case 1:
                mins[0] = -16;
                mins[1] = -16;
                mins[2] = -18;
                maxs[0] = 16;
                maxs[1] = 16;
                maxs[2] = 18;
                break;
            case 2:
                mins[0] = 0;
                mins[1] = 0;
                mins[2] = 0;
                maxs[0] = 0;
                maxs[1] = 0;
                maxs[2] = 0;
                break;
        }
            
        return 1;
    }
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_Frame")]
    public static void HUD_Frame(double time)
    {
        
    }

    private static int weaponanim = 0;
    private static int toSetWeaponanim = -1;
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_PostRunCmd")]
    public static void HUD_PostRunCmd(LocalState* from, LocalState* to, UserCmd* cmd, int runfuncs, double time,
        uint random_seed)
    {
        //if(from == null || to == null) return;
        //weaponanim = from->playerstate.weaponanim;
        to->client.weaponanim = from->client.weaponanim;
        if (toSetWeaponanim != -1)
        {
            to->client.weaponanim = toSetWeaponanim;
            toSetWeaponanim = -1;
        }
        if (weaponanim != to->client.weaponanim)
        {
            weaponanim = to->client.weaponanim;
            g_pEngineFuncs->pfnWeaponAnim(weaponanim, 0);
        }
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_Key_Event")]
    public static int HUD_Key_Event(int down, int keynum, IntPtr pszCurrentBinding)
    {
        Console.WriteLine("HUD_Key_Event: " + down + " " + keynum);
        return 1;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_AddEntity")]
    public static int HUD_AddEntity(int type, IntPtr ent, IntPtr modelname)
    {
        return 1;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_CreateEntities")]
    public static void HUD_CreateEntities()
    {
        //TODO:Update Beams
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_StudioEvent")]
    public static void HUD_StudioEvent(MStudioEvent* eventt, ClEntity* entity)
    {
        switch (eventt->eventt)
        {
            case 5001:
                break;
        }
        //TODO: Studio Events
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_TxferLocalOverrides")]
    public static void HUD_TxferLocalOverrides(EntityState* state, ClientData* client)
    {
        state->origin = client->origin;
        state->iuser1 = client->iuser1;
        state->iuser2 = client->iuser2;
        state->iuser3 = client->iuser3;
        state->iuser4 = client->iuser4;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_ProcessPlayerState")]
    public static void HUD_ProcessPlayerState(EntityState* dst, EntityState* src)
    {
        dst->origin = src->origin;
        dst->angles = src->angles;
        
        dst->frame				= src->frame;
        dst->modelindex				= src->modelindex;
        dst->skin				= src->skin;
        dst->effects				= src->effects;
        dst->weaponmodel			= src->weaponmodel;
        dst->movetype				= src->movetype;
        dst->sequence				= src->sequence;
        dst->animtime				= src->animtime;
        dst->solid				= src->solid;
        
	
        dst->rendermode				= src->rendermode;
        dst->renderamt				= src->renderamt;	
        dst->rendercolor.R			= src->rendercolor.R;
        dst->rendercolor.G			= src->rendercolor.G;
        dst->rendercolor.B			= src->rendercolor.B;
        dst->renderfx				= src->renderfx;

        dst->framerate				= src->framerate;
        dst->body				= src->body;
        
        Buffer.MemoryCopy(&src->controller[0], &dst->controller[0], 4 * sizeof(byte), 4 * sizeof(byte));
        Buffer.MemoryCopy(&src->blending[0], &dst->blending[0], 2 * sizeof(byte), 2 * sizeof(byte));

        dst->basevelocity = src->basevelocity;
        
        dst->friction				= src->friction;
        dst->gravity				= src->gravity;
        dst->gaitsequence			= src->gaitsequence;
        dst->spectator				= src->spectator;
        dst->usehull				= src->usehull;
        dst->playerclass			= src->playerclass;
        dst->team				= src->team;
        dst->colormap				= src->colormap;
        
        //TODO: LocalPlayer
    }
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_TxferPredictionData")]
    public static void HUD_TxferPredictionData(EntityState* ps, EntityState* pps, ClientData* pcd, ClientData* ppcd)
    {
        ps->oldbuttons				= pps->oldbuttons;
        ps->flFallVelocity			= pps->flFallVelocity;
        ps->iStepLeft				= pps->iStepLeft;
        ps->playerclass				= pps->playerclass;

        pcd->viewmodel				= ppcd->viewmodel;
        pcd->m_iId				= ppcd->m_iId;
        pcd->ammo_shells			= ppcd->ammo_shells;
        pcd->ammo_nails				= ppcd->ammo_nails;
        pcd->ammo_cells				= ppcd->ammo_cells;
        pcd->ammo_rockets			= ppcd->ammo_rockets;
        pcd->m_flNextAttack			= ppcd->m_flNextAttack;
        pcd->fov				= ppcd->fov;
        pcd->weaponanim				= ppcd->weaponanim;
        pcd->tfstate				= ppcd->tfstate;
        pcd->maxspeed				= ppcd->maxspeed;

        pcd->deadflag				= ppcd->deadflag;

        pcd->iuser1 = ppcd->iuser1;
        pcd->iuser2 = ppcd->iuser2;
        pcd->iuser3 = ppcd->iuser3;
        pcd->iuser4 = ppcd->iuser4;
        
        pcd->fuser2					= ppcd->fuser2;
        pcd->fuser3					= ppcd->fuser3;

        pcd->vuser1 = ppcd->vuser1;
        pcd->vuser2 = ppcd->vuser2;
        pcd->vuser3 = ppcd->vuser3;
        pcd->vuser4 = ppcd->vuser4;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_TempEntUpdate")]
    public static void HUD_TempEntUpdate(double frametime, double client_time, double cl_gravity
        )
    {
        Console.WriteLine("TempEnt update");
        //TODO: TempEnt Update
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_DrawNormalTriangles")]
    public static void HUD_DrawNormalTriangles()
    {
        
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_DrawTransparentTriangles")]
    public static void HUD_DrawTransparentTriangles()
    {
        
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HUD_GetUserEntity")]
    public static ClEntity* HUD_GetUserEntity(int index)
    {
        return null;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "Demo_ReadBuffer")]
    public static void Demo_ReadBuffer(int size, byte* buffer)
    {
        
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "CL_IsThirdPerson")]
    public static int CL_IsThirdPerson()
    {
        Console.WriteLine("Is Third Person");
        return 0;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "CL_CameraOffset")]
    public static void CL_CameraOffset(float* ofs)
    {
        ofs[0] = 0;
        ofs[1] = 0;
        ofs[2] = 0;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "CL_CreateMove")]
    public static void CL_CreateMove(float frametime, UserCmd* cmd, int active)
    {
        Console.WriteLine("CL_CreateMove");
        g_pEngineFuncs->GetViewAngles(&cmd->viewangles.X);
        cmd->forwardmove = 0;
        cmd->sidemove = 0;
        cmd->forwardmove += isForward ? 400 : 0;
        cmd->forwardmove -= isBackward ? 400 : 0;
        cmd->sidemove -= isLeft ? 400 : 0;
        cmd->sidemove += isRight ? 400 : 0;

        if (impulse != 0)
        {
            cmd->impulse = (byte)impulse;
            impulse = 0;
        }

        if (isShoot)
        {
            cmd->buttons |= (1 << 0);
            toSetWeaponanim = 3;
        }
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "IN_ActivateMouse")]
    public static void IN_ActivateMouse()
    {
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "IN_DeactivateMouse")]
    public static void IN_DeactivateMouse()
    {
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "IN_MouseEvent")]
    public static void IN_MouseEvent()
    {
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "IN_Accumulate")]
    public static void IN_Accumulate()
    {
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "IN_ClearStates")]
    public static void IN_ClearStates()
    {
    }

    private static float bobtime;
    private static float bob;
    private static float lasttime;

    private static float CalcBob(RefParams* pparams)
    {
        if (pparams->onground == -1 || lasttime == pparams->time) return bob;
        lasttime = pparams->time;

        bobtime += pparams->frametime;
        float cl_bobcycle = 0.8f;
        float cl_bobup = 0.5f;
        float cycle = bobtime - (int)(bobtime / cl_bobcycle) * cl_bobcycle;
        cycle /= cl_bobcycle;
        if (cycle < cl_bobup)
            cycle = MathF.PI * cycle / cl_bobup;
        else
            cycle = MathF.PI + MathF.PI * (cycle - cl_bobup) / (1.0f - cl_bobup);

        Vector vel = pparams->simvel;

        bob = MathF.Sqrt(vel.X * vel.X + vel.Y * vel.Y) * 0.01f;
        bob = bob * 0.7f * MathF.Sin(cycle);
        bob = MathF.Min(bob, 7.0f);
        bob = MathF.Max(bob, -7.0f);
        return bob;
    }

    private static float weaponPitch = 0;
    private static float weaponYaw = 0;

    private static float minBob = 1000;
    private static float maxBob = -1000;
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "V_CalcRefdef")]
    public static void V_CalcRefdef(RefParams* pparams)
    {
        ClEntity* entity = g_pEngineFuncs->GetLocalPlayer();
        ClEntity* view = g_pEngineFuncs->GetViewModel();
        pparams->vieworg = entity->origin + pparams->viewheight;
        pparams->viewangles = pparams->cl_viewangles;
        
        view->angles = pparams->viewangles;
        view->angles.X = -view->angles.X;
        view->angles.X += weaponPitch;
        view->angles.Y += weaponYaw;
        
        float bobb = CalcBob(pparams);
        if (bobb > maxBob) maxBob = bobb;
        if (bobb < minBob) minBob = bobb;
        Console.WriteLine("Bob: " + bobb);
        Console.WriteLine("Min Bob: " + minBob);
        Console.WriteLine("Max Bob: " + maxBob);
        pparams->vieworg.Z += bobb * 2;
        pparams->viewangles.Z = bobb / 2.0f;
        
        view->origin = pparams->vieworg;
        view->origin.Z -= bobb / 2.0f;
        
        //view->curstate.frame = g_pEngineFuncs->GetClientTime();
        //view->curstate.animtime = g_pEngineFuncs->GetClientTime();

        //pparams->vieworg = entity->origin + new Vector(150, 150, 150);
        //Vector normal = entity->origin - pparams->vieworg;
        //pparams->viewangles.X = MathF.Atan2(-normal.Z, MathF.Sqrt(normal.X * normal.X + normal.Y * normal.Y)) * (180.0f / MathF.PI);
        //pparams->viewangles.Y = MathF.Atan2(normal.Y, normal.X) * (180.0f / MathF.PI);
        //pparams->viewangles.Z = 0;

    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "KB_Find")]
    public static void* KB_Find(IntPtr name)
    {
        Console.WriteLine("KB_Find: " + Marshal.PtrToStringAnsi(name));
        return null;
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "CAM_Think")]
    public static void CAM_Think()
    {
        Console.WriteLine("CAM_Think");
    }
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "IN_ClientLookEvent")]
    public static void IN_ClientLookEvent(float relyaw, float relpitch)
    {
        Vector viewAngles = new Vector(0,0,0);
        g_pEngineFuncs->GetViewAngles(&viewAngles.X);
        viewAngles.X += relpitch * sensitivity->value;
        viewAngles.Y += relyaw * sensitivity->value;

        weaponPitch -= relpitch * sensitivity->value / 2.0f;
        weaponYaw += relyaw * sensitivity->value / 2.0f;
        weaponPitch = MathF.Min(weaponPitch, 10.0f);
        weaponPitch = MathF.Max(weaponPitch, -20.0f);
        weaponYaw = MathF.Min(weaponYaw, 20.0f);
        weaponYaw = MathF.Max(weaponYaw, -20.0f);
        
        
        Console.WriteLine("Weapon Pitch: " + weaponPitch);
        Console.WriteLine("Weapon Yaw: " + weaponYaw);
        
        
        g_pEngineFuncs->SetViewAngles(&viewAngles.X);
    }
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "IN_ClientMoveEvent")]
    public static void IN_ClientMoveEvent(float forwardmove, float sidemove)
    {
        
        //Console.WriteLine("IN_ClientMoveEvent: " + forwardmove + "  " + sidemove);
    }
}