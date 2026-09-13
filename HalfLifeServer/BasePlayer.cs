using System.Runtime.InteropServices;

namespace XashGameDLL;

public unsafe class BasePlayer : BaseEntity
{
    private int flashlightMsg = 0;
    private int resetHudMsg = 0;
    private int initHudMsg = 0;
    private bool hudInitialized = false;
    public void RegUserMessages()
    {
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("SelAmmo"), 4);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("CurWeapon"), 3);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("Geiger"), 1);
        flashlightMsg = EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("Flashlight"), 2);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("FlashBat"), 1);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("Health"), 1);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("Damage"), 12);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("Battery"), 2);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("Train"), 2);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("Train"), -1);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("SayText"), -1);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("TextMsg"), -1);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("WeaponList"), -1);
        resetHudMsg = EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("ResetHUD"), 1);
        initHudMsg = EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("InitHUD"), 0);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("GameTitle"), 1);
        EngineFuncs.Funcs->pfnRegUserMsg(Marshal.StringToHGlobalAnsi("DeathMsg"), -1);
    }
    
    public override void Spawn()
    {
        PEV->classname = EngineFuncs.Funcs->pfnAllocString(Marshal.StringToHGlobalAnsi("player"));
        PEV->health = 100;
        PEV->max_health = PEV->health;
        PEV->solid = 3;
        PEV->movetype = 3;
        //PEV->movetype = 8; //NOCLIP
        PEV->flags = 0;
        PEV->flags &= (1<<20);
        PEV->flags |= (1<<3);
        EngineFuncs.Funcs->pfnSetModel(PEV->pContainingEntity, Marshal.StringToHGlobalAnsi("models/player.mdl"));
        PEV->effects = 0;
        PEV->effects |= 8;
        PEV->deadflag = 0; //ALIVE
        PEV->view_ofs = new Vector(0, 0, 18);
        PEV->origin = new Vector(0, 0, 0);
        PEV->weapons = 0;
        PEV->weapons |= ( 1 << 31 );
        
        RegUserMessages();
    }

    public override void Think()
    {
        
    }

    public override void Use()
    {
        
    }

    public override void Touch()
    {
        
    }

    public virtual void PlayerPreThink()
    {
        //Console.WriteLine("Think" + PEV->origin.X + " " + PEV->origin.Y + " " + PEV->origin.Z + " ");
        UpdateClientData();
        //if(PEV->button != 0) Console.WriteLine(PEV->button);
    }
    public virtual void PlayerPostThink()
    {
        
    }

    private void UpdateClientData()
    {
        EngineFuncs.Funcs->pfnMessageBegin(1, resetHudMsg, null, PEV->pContainingEntity);
        EngineFuncs.Funcs->pfnWriteByte(0);
        EngineFuncs.Funcs->pfnMessageEnd();

        if (!hudInitialized)
        {
            EngineFuncs.Funcs->pfnMessageBegin(1, initHudMsg, null, PEV->pContainingEntity);
            EngineFuncs.Funcs->pfnMessageEnd();
            hudInitialized = true;
        }

        EngineFuncs.Funcs->pfnMessageBegin(1, flashlightMsg, null, PEV->pContainingEntity);
        EngineFuncs.Funcs->pfnWriteByte(1);
        EngineFuncs.Funcs->pfnWriteByte(100);
        EngineFuncs.Funcs->pfnMessageEnd();
    }
}