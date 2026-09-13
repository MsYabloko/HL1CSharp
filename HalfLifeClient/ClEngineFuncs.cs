using System.Runtime.InteropServices;
using XashGameDLL;

namespace HalfLifeClient;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ClEngineFuncs
{
    public delegate* unmanaged[Cdecl]<IntPtr, int> pfnSPR_Load;
    public delegate* unmanaged[Cdecl]<int, int> pfnSPR_Frames;
    public delegate* unmanaged[Cdecl]<int, int, int> pfnSPR_Height;
    public delegate* unmanaged[Cdecl]<int, int, int> pfnSPR_Width;
    public delegate* unmanaged[Cdecl]<int, int, int, int, void> pfnSPR_Set;
    public delegate* unmanaged[Cdecl]<int, int, int, int> pfnSPR_Draw;
    public delegate* unmanaged[Cdecl]<int, int, int, IntPtr, void> pfnSPR_DrawHoles;
    public delegate* unmanaged[Cdecl]<int, int, int, IntPtr, void> pfnSPR_DrawAdditive;
    public delegate* unmanaged[Cdecl]<int, int, int, int, void> pfnSPR_EnableScissor;
    public delegate* unmanaged[Cdecl]<void> pfnSPR_DisableScissor;
    public delegate* unmanaged[Cdecl]<sbyte*, int*, IntPtr> pfnSPR_GetList;

    // screen handlers
    public delegate* unmanaged[Cdecl]<int, int, int, int, int, int, int, int, void> pfnFillRGBA;
    public delegate* unmanaged[Cdecl]<ScreenInfo*, int> pfnGetScreenInfo;
    public delegate* unmanaged[Cdecl]<int, IntPtr, int, int, int, void> pfnSetCrosshair; // HSPRITE=int, wrect_t=IntPtr

    // cvar handlers
    public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, int, CVar*> pfnRegisterVariable; // returns cvar_s* as IntPtr
    public delegate* unmanaged[Cdecl]<sbyte*, float> pfnGetCvarFloat;
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*> pfnGetCvarString;

    // command handlers
    public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<void>, int> pfnAddCommand;
    public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, int>, int> pfnHookUserMsg; // pfnUserMsgHook assumed as delegate* unmanaged[Cdecl]<IntPtr, int>
    public delegate* unmanaged[Cdecl]<IntPtr, int> pfnServerCmd;
    public delegate* unmanaged[Cdecl]<IntPtr, int> pfnClientCmd;

    public delegate* unmanaged[Cdecl]<int, IntPtr, void> pfnGetPlayerInfo; // hud_player_info_t* as IntPtr

    // sound handlers
    public delegate* unmanaged[Cdecl]<sbyte*, float, void> pfnPlaySoundByName;
    public delegate* unmanaged[Cdecl]<int, float, void> pfnPlaySoundByIndex;

    // vector helpers
    public delegate* unmanaged[Cdecl]<float*, float*, float*, float*, void> pfnAngleVectors;

    // text message system
    public delegate* unmanaged[Cdecl]<sbyte*, IntPtr> pfnTextMessageGet; // client_textmessage_t* as IntPtr
    public delegate* unmanaged[Cdecl]<int, int, int, int, int, int, int> pfnDrawCharacter;
    public delegate* unmanaged[Cdecl]<int, int, IntPtr, int> pfnDrawConsoleString;
    public delegate* unmanaged[Cdecl]<float, float, float, void> pfnDrawSetTextColor;
    public delegate* unmanaged[Cdecl]<sbyte*, int*, int*, void> pfnDrawConsoleStringLen;

    public delegate* unmanaged[Cdecl]<sbyte*, void> pfnConsolePrint;
    public delegate* unmanaged[Cdecl]<sbyte*, void> pfnCenterPrint;

    // Added for user input processing
    public delegate* unmanaged[Cdecl]<int> GetWindowCenterX;
    public delegate* unmanaged[Cdecl]<int> GetWindowCenterY;
    public delegate* unmanaged[Cdecl]<float*, void> GetViewAngles;
    public delegate* unmanaged[Cdecl]<float*, void> SetViewAngles;
    public delegate* unmanaged[Cdecl]<int> GetMaxClients;
    public delegate* unmanaged[Cdecl]<sbyte*, float, void> Cvar_SetValue;

    public delegate* unmanaged[Cdecl]<int> Cmd_Argc;
    public delegate* unmanaged[Cdecl]<int, IntPtr> Cmd_Argv;

    // Variadic functions cannot be directly represented as function pointers in C#
    // You may consider delegates with UnmanagedFunctionPointer attribute for these or IntPtr
    public IntPtr Con_Printf;   // void(*)(const char*, ...)
    public IntPtr Con_DPrintf;  // void(*)(const char*, ...)
    public IntPtr Con_NPrintf;  // void(*)(int, const char*, ...)
    public IntPtr Con_NXPrintf; // void(*)(con_nprint_s*, const char*, ...)

    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*> PhysInfo_ValueForKey;
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*> ServerInfo_ValueForKey;
    public delegate* unmanaged[Cdecl]<float> GetClientMaxspeed;
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte**, int> CheckParm;

    public delegate* unmanaged[Cdecl]<int, int, void> Key_Event;
    public delegate* unmanaged[Cdecl]<int*, int*, void> GetMousePosition;
    public delegate* unmanaged[Cdecl]<int> IsNoClipping;

    public delegate* unmanaged[Cdecl]<ClEntity*> GetLocalPlayer;   
    public delegate* unmanaged[Cdecl]<ClEntity*> GetViewModel;    
    public delegate* unmanaged[Cdecl]<int, ClEntity*> GetEntityByIndex;

    public delegate* unmanaged[Cdecl]<float> GetClientTime;
    public delegate* unmanaged[Cdecl]<void> V_CalcShake;
    public delegate* unmanaged[Cdecl]<float*, float*, float, void> V_ApplyShake;

    public delegate* unmanaged[Cdecl]<float*, int*, int> PM_PointContents;
    public delegate* unmanaged[Cdecl]<float*, int> PM_WaterEntity;
    public delegate* unmanaged[Cdecl]<float*, float*, int, int, int, IntPtr> PM_TraceLine; // pmtrace_s* as IntPtr

    public delegate* unmanaged[Cdecl]<sbyte*, int*, IntPtr> CL_LoadModel; // model_s* as IntPtr
    public delegate* unmanaged[Cdecl]<int, IntPtr, int> CL_CreateVisibleEntity;

    public delegate* unmanaged[Cdecl]<int, IntPtr> GetSpritePointer; // const model_s* as IntPtr
    public delegate* unmanaged[Cdecl]<sbyte*, float, float*, void> pfnPlaySoundByNameAtLocation;

    public delegate* unmanaged[Cdecl]<int, sbyte*, ushort> pfnPrecacheEvent;
    public delegate* unmanaged[Cdecl]<int, IntPtr, ushort, float, float*, float*, float, float, int, int, int, int, void> pfnPlaybackEvent;
    public delegate* unmanaged[Cdecl]<int, int, void> pfnWeaponAnim;
    public delegate* unmanaged[Cdecl]<float, float, float> pfnRandomFloat;
    public delegate* unmanaged[Cdecl]<int, int, int> pfnRandomLong;
    public delegate* unmanaged[Cdecl]<sbyte*, delegate* unmanaged[Cdecl]<IntPtr, void>, void> pfnHookEvent;
    public delegate* unmanaged[Cdecl]<int> Con_IsVisible;
    public delegate* unmanaged[Cdecl]<void*> pfnGetGameDirectory;
    public delegate* unmanaged[Cdecl]<sbyte*, IntPtr> pfnGetCvarPointer;
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*> Key_LookupBinding;
    public delegate* unmanaged[Cdecl]<void*> pfnGetLevelName;
    public delegate* unmanaged[Cdecl]<IntPtr, void> pfnGetScreenFade;
    public delegate* unmanaged[Cdecl]<IntPtr, void> pfnSetScreenFade;
    public delegate* unmanaged[Cdecl]<IntPtr> VGui_GetPanel;
    public delegate* unmanaged[Cdecl]<int*, void> VGui_ViewportPaintBackground;

    public delegate* unmanaged[Cdecl]<sbyte*, int, int*, IntPtr> COM_LoadFile;
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, sbyte*> COM_ParseFile;
    public delegate* unmanaged[Cdecl]<IntPtr, void> COM_FreeFile;

    public IntPtr pTriAPI;   // triangleapi_s*
    public IntPtr pEfxAPI;   // efx_api_s*
    public EventApi* pEventAPI; // event_api_s*
    public IntPtr pDemoAPI;  // demo_api_s*
    public IntPtr pNetAPI;   // net_api_s*
    public IntPtr pVoiceTweak; // IVoiceTweak_s*

    public delegate* unmanaged[Cdecl]<int> IsSpectateOnly;
    public delegate* unmanaged[Cdecl]<sbyte*, IntPtr> LoadMapSprite;

    // file search functions
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, void> COM_AddAppDirectoryToSearchPath;
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, int, int> COM_ExpandFilename;

    // User info
    public delegate* unmanaged[Cdecl]<int, sbyte*, sbyte*> PlayerInfo_ValueForKey;
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, void> PlayerInfo_SetValueForKey;

    public delegate* unmanaged[Cdecl]<int, byte[], int> GetPlayerUniqueID;

    public delegate* unmanaged[Cdecl]<int, int> GetTrackerIDForPlayer;
    public delegate* unmanaged[Cdecl]<int, int> GetPlayerForTrackerID;

    public delegate* unmanaged[Cdecl]<sbyte*, int> pfnServerCmdUnreliable;

    public delegate* unmanaged[Cdecl]<IntPtr, void> pfnGetMousePos;
    public delegate* unmanaged[Cdecl]<int, int, void> pfnSetMousePos;
    public delegate* unmanaged[Cdecl]<int, void> pfnSetMouseEnable;

    // undocumented interface starts here
    public delegate* unmanaged[Cdecl]<IntPtr> pfnGetFirstCvarPtr;
    public delegate* unmanaged[Cdecl]<IntPtr> pfnGetFirstCmdFunctionHandle;
    public delegate* unmanaged[Cdecl]<IntPtr, IntPtr> pfnGetNextCmdFunctionHandle;
    public delegate* unmanaged[Cdecl]<IntPtr, sbyte*> pfnGetCmdFunctionName;
    public delegate* unmanaged[Cdecl]<float> pfnGetClientOldTime;
    public delegate* unmanaged[Cdecl]<float> pfnGetGravity;
    public delegate* unmanaged[Cdecl]<int, IntPtr> pfnGetModelByIndex;
    public delegate* unmanaged[Cdecl]<int, void> pfnSetFilterMode;
    public delegate* unmanaged[Cdecl]<float, float, float, void> pfnSetFilterColor;
    public delegate* unmanaged[Cdecl]<float, void> pfnSetFilterBrightness;
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, IntPtr> pfnSequenceGet;
    public delegate* unmanaged[Cdecl]<int, int, int, IntPtr, int, int, int, int, void> pfnSPR_DrawGeneric;
    public delegate* unmanaged[Cdecl]<sbyte*, int, int*, IntPtr> pfnSequencePickSentence;
    public delegate* unmanaged[Cdecl]<int, int, IntPtr, int, int, int, int> pfnDrawString;
    public delegate* unmanaged[Cdecl]<int, int, sbyte*, int, int, int, int> pfnDrawStringReverse;
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*> LocalPlayerInfo_ValueForKey;
    public delegate* unmanaged[Cdecl]<int, int, int, uint> pfnVGUI2DrawCharacter;
    public delegate* unmanaged[Cdecl]<int, int, int, int, int, int, uint> pfnVGUI2DrawCharacterAdditive;
    public delegate* unmanaged[Cdecl]<sbyte*, uint> pfnGetApproxWavePlayLen;
    public delegate* unmanaged[Cdecl]<void*> GetCareerGameUI;
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, void> Cvar_Set;
    public delegate* unmanaged[Cdecl]<int> pfnIsPlayingCareerMatch;
    public delegate* unmanaged[Cdecl]<sbyte*, float, int, void> pfnPlaySoundVoiceByName;
    public delegate* unmanaged[Cdecl]<sbyte*, int, void> pfnPrimeMusicStream;
    public delegate* unmanaged[Cdecl]<double> pfnSys_FloatTime;

    // decay funcs
    public delegate* unmanaged[Cdecl]<int*, int, void> pfnProcessTutorMessageDecayBuffer;
    public delegate* unmanaged[Cdecl]<int*, int, void> pfnConstructTutorMessageDecayBuffer;
    public delegate* unmanaged[Cdecl]<void> pfnResetTutorMessageDecayData;

    public delegate* unmanaged[Cdecl]<sbyte*, float, int, void> pfnPlaySoundByNameAtPitch;
    public delegate* unmanaged[Cdecl]<int, int, int, int, int, int, int, int, void> pfnFillRGBABlend;
    public delegate* unmanaged[Cdecl]<int> pfnGetAppID;
    public delegate* unmanaged[Cdecl]<IntPtr> pfnGetAliases;
    public delegate* unmanaged[Cdecl]<int*, int*, void> pfnVguiWrap2_GetMouseDelta;

    // added in 2019 update, not documented yet
    public delegate* unmanaged[Cdecl]<sbyte*, int> pfnFilteredClientCmd;
}