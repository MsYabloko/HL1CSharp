using System.Runtime.InteropServices;

namespace XashGameDLL;

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct DLL_FUNCTIONS
    {
        public IntPtr pfnGameInit;
        public IntPtr pfnSpawn;
        public IntPtr pfnThink;
        public IntPtr pfnUse;
        public IntPtr pfnTouch;
        public IntPtr pfnBlocked;
        public IntPtr pfnKeyValue;
        public IntPtr pfnSave;
        public IntPtr pfnRestore;
        public IntPtr pfnSetAbsBox;

        public IntPtr pfnSaveWriteFields;
        public IntPtr pfnSaveReadFields;
        public IntPtr pfnSaveGlobalState;
        public IntPtr pfnRestoreGlobalState;
        public IntPtr pfnResetGlobalState;

        public IntPtr pfnClientConnect;

        public IntPtr pfnClientDisconnect;
        public IntPtr pfnClientKill;
        public IntPtr pfnClientPutInServer;
        public IntPtr pfnClientCommand;
        public IntPtr pfnClientUserInfoChanged;
        public IntPtr pfnServerActivate;
        public IntPtr pfnServerDeactivate;
        public IntPtr pfnPlayerPreThink;
        public IntPtr pfnPlayerPostThink;

        public IntPtr pfnStartFrame;
        public IntPtr pfnParmsNewLevel;
        public IntPtr pfnParmsChangeLevel;

        public IntPtr pfnGetGameDescription;

        public IntPtr pfnPlayerCustomization;

        public IntPtr pfnSpectatorConnect;
        public IntPtr pfnSpectatorDisconnect;
        public IntPtr pfnSpectatorThink;

        public IntPtr pfnSys_Error;

        public IntPtr pfnPM_Move;
        public IntPtr pfnPM_Init;
        public IntPtr pfnPM_FindTextureType;
        public IntPtr pfnSetupVisibility;
        public IntPtr pfnUpdateClientData;
        public IntPtr pfnAddToFullPack;
        public IntPtr pfnCreateBaseline;
        public IntPtr pfnRegisterEncoders;
        public IntPtr pfnGetWeaponData;

        public IntPtr pfnCmdStart;
        public IntPtr pfnCmdEnd;

        public IntPtr pfnConnectionlessPacket;

        public IntPtr pfnGetHullBounds;

        public IntPtr pfnCreateInstancedBaselines;

        public IntPtr pfnInconsistentFile;

        public IntPtr pfnAllowLagCompensation;
    }