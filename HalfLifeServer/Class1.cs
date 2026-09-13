using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace XashGameDLL
{
    public static unsafe class HLExports
    {
        // Store pointers passed from engine
        private static EngineFuncs* g_pEngineFuncs = null;
        private static GlobalVars* g_pGlobals = null;
        private static PlayerMove* pmove = null;

        // Static delegates to keep alive
        private static readonly delegate* unmanaged[Cdecl]<void> GameInit = &GameDLLInit;
        private static readonly delegate* unmanaged[Cdecl]<void> StartFrameDelegate = &StartFrame;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, int> SpawnDelegate = &DispatchSpawn;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, int> ThinkDelegate = &DispatchThink;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, int> UseDelegate = &DispatchUse;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, int> TouchDelegate = &DispatchTouch;
        private static readonly delegate* unmanaged[Cdecl]<IntPtr, int> BlockedDelegate = &DispatchBlocked;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, KeyValueData*, void> KeyValueDelegate = &DispatchKeyValue;
        
        private static readonly delegate* unmanaged[Cdecl]<void> RegisterEncodersDelegate = &RegisterEncoders;
        
        private static readonly delegate* unmanaged[Cdecl]<int, float*, float*, int> GetHullBoundsDelegate = &GetHullBounds;
        
        private static readonly delegate* unmanaged[Cdecl]<PlayerMove*, void> PM_InitDelegate = &PM_Init;

        private static readonly delegate* unmanaged[Cdecl]<IntPtr> GetGameDescriptionDelegate = &GetGameDescription;
        private static readonly delegate* unmanaged[Cdecl]<void> DeactivateServerDelegate = &ServerDeactivate;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, int, int, void> ActivateServerDelegate = &ServerActivate;
        
        private static readonly delegate* unmanaged[Cdecl]<Edict*, void> OnFreeEntPrivateData = &DestroyEntity;
        
        private static readonly delegate* unmanaged[Cdecl]<int, int, EntityState*, Edict*, int, Vector, Vector, void> CreateBaseLineDelegate = &CreateBaseline;
        private static readonly delegate* unmanaged[Cdecl]<void> CreateInstancedBaselinesDelegate = &CreateInstancedBaselines;
        
        private static readonly delegate* unmanaged[Cdecl]<Edict*, char*, void> ClientUserInfoChangedDelegate = &ClientUserInfoChanged;
        
        private static readonly delegate* unmanaged[Cdecl]<Edict*, IntPtr, IntPtr, char*, int> ClientConnectDelegate = &ClientConnect;
        
        private static readonly delegate* unmanaged[Cdecl]<Edict*, void> ClientPutInServerDelegate = &ClientPutInServer;
        
        private static readonly delegate* unmanaged[Cdecl]<Edict*, IntPtr, void> PlayerCustomizationDelegate = &PlayerCustomization;
        
        private static readonly delegate* unmanaged[Cdecl]<Edict*, int, ClientData*, void> UpdateClientDataDelegate = &UpdateClientData;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, IntPtr, int> GetWeaponDataDelegate = &GetWeaponData;
        
        private static readonly delegate* unmanaged[Cdecl]<Edict*, Edict*, byte**, byte**, void> SetupVisibilityDelegate = &SetupVisibility;
        private static readonly delegate* unmanaged[Cdecl]<EntityState*, int, Edict*, Edict*, int, int, byte*, int> AddToFullPackDelegate = &AddToFullPack;
        
        private static readonly delegate* unmanaged[Cdecl]<Edict*, UserCmd*, uint, void> CmdStartDelegate = &CmdStart;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, void> PlayerPreThinkDelegate = &PlayerPreThink;
        private static readonly delegate* unmanaged[Cdecl]<PlayerMove*, int, void> PM_MoveDelegate = &PM_Move;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, void> PlayerPostThinkDelegate = &PlayerPostThink;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, void> CmdEndDelegate = &CmdEnd;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, void> DispatchObjectCollisionBoxDelegate = &DispatchObjectCollisionBox;
        private static readonly delegate* unmanaged[Cdecl]<Edict*, void> ClientCommandDelegate = &ClientCommand;
        
        

        // Static unmanaged string pointer for game description
        private static readonly IntPtr _gameDescriptionPtr;

        // Static constructor: allocate unmanaged memory for game description string
        static HLExports()
        {
            string description = "C# Half Life Mod";
            byte[] descBytes = Encoding.ASCII.GetBytes(description + '\0'); // null-terminated
            _gameDescriptionPtr = Marshal.AllocHGlobal(descBytes.Length);
            Marshal.Copy(descBytes, 0, _gameDescriptionPtr, descBytes.Length);
        }

        // Called by engine to provide engine function pointers and globals
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "GiveFnptrsToDll")]
        public static void GiveFnptrsToDll(EngineFuncs* pengfuncsFromEngine, GlobalVars* pGlobals)
        {
            g_pEngineFuncs = (EngineFuncs*)Marshal.AllocHGlobal(sizeof(EngineFuncs)).ToPointer();
            Buffer.MemoryCopy(pengfuncsFromEngine, g_pEngineFuncs, sizeof(EngineFuncs), sizeof(EngineFuncs));
            g_pGlobals = pGlobals;
            GlobalVars.Vars = g_pGlobals;
            EngineFuncs.Funcs = g_pEngineFuncs;
            Console.WriteLine("GiveFnptrsToDll called.");
        }
        
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "worldspawn")]
        public static void SpawnWorld(EntVars* e)
        {
            var ent = BaseEntity.CreateEntity<WorldEntity>(e->pContainingEntity);
        }

        private static NothingEntity spawnEntity = null;
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "info_player_start")]
        public static void SpawnPlayerSpawn(EntVars* e)
        {
            spawnEntity = BaseEntity.CreateEntity<NothingEntity>(e->pContainingEntity);
        }

        // Provide entity dispatch functions to engine
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "GetEntityAPI")]
        public static int GetEntityAPI(DLL_FUNCTIONS* pFunctionTable)
        {
            pFunctionTable->pfnGameInit = (IntPtr)GameInit;
            pFunctionTable->pfnSpawn = (IntPtr)SpawnDelegate;
            pFunctionTable->pfnThink = (IntPtr)ThinkDelegate;
            pFunctionTable->pfnUse = (IntPtr)UseDelegate;
            pFunctionTable->pfnTouch = (IntPtr)TouchDelegate;
            pFunctionTable->pfnBlocked = (IntPtr)BlockedDelegate;
            pFunctionTable->pfnKeyValue = (IntPtr)KeyValueDelegate;
            pFunctionTable->pfnGetGameDescription = (IntPtr)GetGameDescriptionDelegate;
            pFunctionTable->pfnGetHullBounds = (IntPtr)GetHullBoundsDelegate;
            pFunctionTable->pfnRegisterEncoders = (IntPtr)RegisterEncodersDelegate;
            pFunctionTable->pfnPM_Init = (IntPtr)PM_InitDelegate;
            pFunctionTable->pfnServerDeactivate = (IntPtr) DeactivateServerDelegate;
            pFunctionTable->pfnServerActivate = (IntPtr)ActivateServerDelegate;
            pFunctionTable->pfnStartFrame = (IntPtr)StartFrameDelegate;
            pFunctionTable->pfnCreateBaseline = (IntPtr)CreateBaseLineDelegate;
            pFunctionTable->pfnCreateInstancedBaselines = (IntPtr)CreateInstancedBaselinesDelegate;
            pFunctionTable->pfnClientUserInfoChanged = (IntPtr)ClientUserInfoChangedDelegate;
            pFunctionTable->pfnClientConnect = (IntPtr)ClientConnectDelegate;
            pFunctionTable->pfnClientPutInServer = (IntPtr)ClientPutInServerDelegate;
            pFunctionTable->pfnPlayerCustomization = (IntPtr)PlayerCustomizationDelegate;
            pFunctionTable->pfnUpdateClientData = (IntPtr)UpdateClientDataDelegate;
            pFunctionTable->pfnGetWeaponData = (IntPtr)GetWeaponDataDelegate;
            pFunctionTable->pfnAddToFullPack = (IntPtr)AddToFullPackDelegate;
            pFunctionTable->pfnSetupVisibility = (IntPtr)SetupVisibilityDelegate;
            pFunctionTable->pfnCmdStart = (IntPtr)CmdStartDelegate;
            pFunctionTable->pfnCmdEnd = (IntPtr)CmdEndDelegate;
            pFunctionTable->pfnPlayerPreThink = (IntPtr)PlayerPreThinkDelegate;
            pFunctionTable->pfnPlayerPostThink = (IntPtr)PlayerPostThinkDelegate;
            pFunctionTable->pfnPM_Move = (IntPtr)PM_MoveDelegate;
            pFunctionTable->pfnSetAbsBox = (IntPtr)DispatchObjectCollisionBoxDelegate;
            pFunctionTable->pfnClientCommand = (IntPtr)ClientCommandDelegate;
            
            Console.WriteLine("GetEntityAPI called.");
            return 1;
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "GetNewDLLFunctions")]
        public static int GetNewDLLFunctions(NewDllFunctions* funcs)
        {
            funcs->pfnOnFreeEntPrivateData = (IntPtr)OnFreeEntPrivateData;
            return 1;
        }

        // Game DLL shutdown
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "GameDLLShutdown")]
        public static void GameDLLShutdown()
        {
            Console.WriteLine("GameDLLShutdown called.");
            // Cleanup if needed
            Marshal.FreeHGlobal(_gameDescriptionPtr);
        }
        
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void DestroyEntity(Edict* ent)
        {
            Console.WriteLine("Destroy Entity Called");
            GCHandle.FromIntPtr((IntPtr)ent->pvPrivateData).Free();
            ent->pvPrivateData = null;
        }

        // Return pointer to static game description string
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static IntPtr GetGameDescription()
        {
            Console.WriteLine("GetGameDescriptor called.");
            return _gameDescriptionPtr;
        }

        // Entity dispatch functions (simplified stubs)

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int DispatchSpawn(Edict* pEntity)
        {
            var entity = (BaseEntity?)GCHandle.FromIntPtr((IntPtr)pEntity->pvPrivateData).Target;
            entity?.Spawn();
            return 1;
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void GameDLLInit()
        {
            Console.WriteLine("GameDLLInit called.");
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void CreateBaseline(int player, int eindex, EntityState* baseline, Edict* entity,
            int playermodelindex, Vector player_mins, Vector player_maxs)
        {
            baseline->origin = entity->v.origin;
            baseline->angles		= entity->v.angles;
            baseline->frame			= entity->v.frame;
            baseline->skin = (short)entity->v.skin;
            
            baseline->rendermode		= (byte)entity->v.rendermode;
            baseline->renderamt		= (byte)entity->v.renderamt;
            baseline->rendercolor.R		= (byte)entity->v.rendercolor.X;
            baseline->rendercolor.G		= (byte)entity->v.rendercolor.Y;
            baseline->rendercolor.B		= (byte)entity->v.rendercolor.Z;
            baseline->renderfx		= (byte)entity->v.renderfx;

            if (player != 0)
            {
                baseline->mins		= player_mins;
                baseline->maxs = player_maxs;
                
                baseline->colormap	= eindex;
                baseline->modelindex	= playermodelindex;
                baseline->friction	= 1.0f;
                baseline->movetype	= 3; //MOVETYPE_WALK
                
                baseline->scale		= entity->v.scale;
                baseline->solid		= 3; // SOLID_SLIDEBOX
                baseline->framerate	= 1.0f;
                baseline->gravity	= 1.0f;
            }
            else
            {
                baseline->mins = entity->v.mins;
                baseline->maxs		= entity->v.maxs;

                baseline->colormap	= 0;
                baseline->modelindex	= entity->v.modelindex;//SV_ModelIndex(pr_strings + entity->v.model);
                baseline->movetype	= entity->v.movetype;
                
                baseline->scale		= entity->v.scale;
                baseline->solid		= (short)entity->v.solid;
                baseline->framerate	= entity->v.framerate;
                baseline->gravity	= entity->v.gravity;
            }
        }
        
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void CreateInstancedBaselines()
        {
            
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int DispatchThink(Edict* pEntity)
        {
            
            var entity = (BaseEntity?)GCHandle.FromIntPtr((IntPtr)pEntity->pvPrivateData).Target;
            entity?.Think();
            return 1;
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int GetHullBounds(int hullNumber, float* mins, float* maxs)
        {
            switch (hullNumber)
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

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int DispatchUse(Edict* pEntity)
        {
            var entity = (BaseEntity?)GCHandle.FromIntPtr((IntPtr)pEntity->pvPrivateData).Target;
            entity?.Use();
            return 1;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int DispatchTouch(Edict* pEntity)
        {
            var entity = (BaseEntity?)GCHandle.FromIntPtr((IntPtr)pEntity->pvPrivateData).Target;
            entity?.Touch();
            return 1;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int DispatchBlocked(IntPtr pEntity)
        {
            Console.WriteLine("DispatchBlocked called.");
            return 1;
        }

        public static readonly (string fieldName, string fieldType, IntPtr offset)[] entDecl = new []
            {
                ("classname", "FIELD_STRING", Marshal.OffsetOf<EntVars>("classname")),
    ("globalname", "FIELD_STRING", Marshal.OffsetOf<EntVars>("globalname")),

    ("origin", "FIELD_POSITION_VECTOR", Marshal.OffsetOf<EntVars>("origin")),
    ("oldorigin", "FIELD_POSITION_VECTOR", Marshal.OffsetOf<EntVars>("oldorigin")),
    ("velocity", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("velocity")),
    ("basevelocity", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("basevelocity")),
    ("movedir", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("movedir")),

    ("angles", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("angles")),
    ("avelocity", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("avelocity")),
    ("punchangle", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("punchangle")),
    ("v_angle", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("v_angle")),
    ("fixangle", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("fixangle")),
    ("idealpitch", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("idealpitch")),
    ("pitch_speed", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("pitch_speed")),
    ("ideal_yaw", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("ideal_yaw")),
    ("yaw_speed", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("yaw_speed")),

    ("modelindex", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("modelindex")),
    ("model", "FIELD_MODELNAME", Marshal.OffsetOf<EntVars>("model")),

    ("viewmodel", "FIELD_MODELNAME", Marshal.OffsetOf<EntVars>("viewmodel")),
    ("weaponmodel", "FIELD_MODELNAME", Marshal.OffsetOf<EntVars>("weaponmodel")),

    ("absmin", "FIELD_POSITION_VECTOR", Marshal.OffsetOf<EntVars>("absmin")),
    ("absmax", "FIELD_POSITION_VECTOR", Marshal.OffsetOf<EntVars>("absmax")),
    ("mins", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("mins")),
    ("maxs", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("maxs")),
    ("size", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("size")),

    ("ltime", "FIELD_TIME", Marshal.OffsetOf<EntVars>("ltime")),
    ("nextthink", "FIELD_TIME", Marshal.OffsetOf<EntVars>("nextthink")),

    ("solid", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("solid")),
    ("movetype", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("movetype")),

    ("skin", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("skin")),
    ("body", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("body")),
    ("effects", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("effects")),

    ("gravity", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("gravity")),
    ("friction", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("friction")),
    ("light_level", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("light_level")),
    
    ("frame", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("frame")),
("scale", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("scale")),
("sequence", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("sequence")),
("animtime", "FIELD_TIME", Marshal.OffsetOf<EntVars>("animtime")),
("framerate", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("framerate")),
("controller", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("controller")),
("blending", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("blending")),

("rendermode", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("rendermode")),
("renderamt", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("renderamt")),
("rendercolor", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("rendercolor")),
("renderfx", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("renderfx")),

("health", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("health")),
("frags", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("frags")),
("weapons", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("weapons")),
("takedamage", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("takedamage")),

("deadflag", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("deadflag")),
("view_ofs", "FIELD_VECTOR", Marshal.OffsetOf<EntVars>("view_ofs")),
("button", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("button")),
("impulse", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("impulse")),

("chain", "FIELD_EDICT", Marshal.OffsetOf<EntVars>("chain")),
("dmg_inflictor", "FIELD_EDICT", Marshal.OffsetOf<EntVars>("dmg_inflictor")),
("enemy", "FIELD_EDICT", Marshal.OffsetOf<EntVars>("enemy")),
("aiment", "FIELD_EDICT", Marshal.OffsetOf<EntVars>("aiment")),
("owner", "FIELD_EDICT", Marshal.OffsetOf<EntVars>("owner")),
("groundentity", "FIELD_EDICT", Marshal.OffsetOf<EntVars>("groundentity")),

("spawnflags", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("spawnflags")),
("flags", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("flags")),

("colormap", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("colormap")),
("team", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("team")),

("max_health", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("max_health")),
("teleport_time", "FIELD_TIME", Marshal.OffsetOf<EntVars>("teleport_time")),
("armortype", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("armortype")),
("armorvalue", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("armorvalue")),
("waterlevel", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("waterlevel")),
("watertype", "FIELD_INTEGER", Marshal.OffsetOf<EntVars>("watertype")),

("target", "FIELD_STRING", Marshal.OffsetOf<EntVars>("target")),
("netname", "FIELD_STRING", Marshal.OffsetOf<EntVars>("netname")),
("message", "FIELD_STRING", Marshal.OffsetOf<EntVars>("message")),

("dmg_take", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("dmg_take")),
("dmg_save", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("dmg_save")),
("dmg", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("dmg")),
("dmgtime", "FIELD_TIME", Marshal.OffsetOf<EntVars>("dmgtime")),

("noise", "FIELD_SOUNDNAME", Marshal.OffsetOf<EntVars>("noise")),
("noise1", "FIELD_SOUNDNAME", Marshal.OffsetOf<EntVars>("noise1")),
("noise2", "FIELD_SOUNDNAME", Marshal.OffsetOf<EntVars>("noise2")),
("noise3", "FIELD_SOUNDNAME", Marshal.OffsetOf<EntVars>("noise3")),
("speed", "FIELD_FLOAT", Marshal.OffsetOf<EntVars>("speed")),
("air_finished", "FIELD_TIME", Marshal.OffsetOf<EntVars>("air_finished")),
("pain_finished", "FIELD_TIME", Marshal.OffsetOf<EntVars>("pain_finished")),
("radsuit_finished", "FIELD_TIME", Marshal.OffsetOf<EntVars>("radsuit_finished"))
            };

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void DispatchKeyValue(Edict* pentKeyValue, KeyValueData* pkvd)
        {
            if(pentKeyValue == null || pkvd == null) return;
            
            //.WriteLine("DispatchKeyValue called.");
            //Console.WriteLine("Class Name: " + pkvd->ClassName);
            //Console.WriteLine("Value Name: " + pkvd->KeyName);
            //Console.WriteLine("Value: " + pkvd->Value);

            foreach (var field in entDecl)
            {
                
                if (pkvd->KeyName == field.fieldName)
                {
                    IntPtr v = (IntPtr)(&pentKeyValue->v);
                    IntPtr fieldPtr = IntPtr.Add(v, (int)field.offset);
                    switch (field.fieldType)
                    {
                        case "FIELD_MODELNAME":
                        case "FIELD_SOUNDNAME":
                        case "FIELD_STRING":
                            Marshal.WriteIntPtr(fieldPtr, g_pEngineFuncs->pfnAllocString(pkvd->szValue));
                            break;
                        case "FIELD_TIME":
                        case "FIELD_FLOAT":
                            *((float*)fieldPtr.ToPointer()) = float.Parse(pkvd->Value);
                            break;
                        case "FIELD_INTEGER":
                            *((int*)fieldPtr.ToPointer()) = int.Parse(pkvd->Value);
                            break;
                        case "FIELD_POSITION_VECTOR":
                        case "FIELD_VECTOR":
                            var splt = pkvd->Value.Split(' ');
                            ((float*)fieldPtr.ToPointer())[0] = float.Parse(splt[0]);
                            ((float*)fieldPtr.ToPointer())[1] = float.Parse(splt[1]);
                            ((float*)fieldPtr.ToPointer())[2] = float.Parse(splt[2]);
                            break;
                    }
                    pkvd->Handled = true;
                }
            }
            
            if( pkvd->Handled || pkvd->ClassName == null ) return;
        }

        public static Vector[] rgv3tStuckTable = new Vector[53];
        
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })] // 1/2 done
        public static void PM_Init(PlayerMove* ppmove)
        {
            pmove = ppmove;

            //PM_CreateStuckTable

            #region PM_CreateStuckTable
            {
                float x, y, z;
                int idx;
                int i;
                float[] zi = new float[3];
                idx = 0;
                x = 0;
                y = 0;
                for (z = -0.125f; z <= 0.125f; z += 0.125f)
                {
                    rgv3tStuckTable[idx].X = x;
                    rgv3tStuckTable[idx].Y = y;
                    rgv3tStuckTable[idx].Z = z;
                    idx++;
                }

                x = 0;
                z = 0;
                for (y = -0.125f; y <= 0.125f; y += 0.125f)
                {
                    rgv3tStuckTable[idx].X = x;
                    rgv3tStuckTable[idx].Y = y;
                    rgv3tStuckTable[idx].Z = z;
                    idx++;
                }

                y = 0;
                z = 0;
                for (x = -0.125f; x <= 0.125f; x += 0.125f)
                {
                    rgv3tStuckTable[idx].X = x;
                    rgv3tStuckTable[idx].Y = y;
                    rgv3tStuckTable[idx].Z = z;
                    idx++;
                }

                for (x = -0.125f; x <= 0.125f; x += 0.250f)
                {
                    for (y = -0.125f; y <= 0.125f; y += 0.250f)
                    {
                        for (z = -0.125f; z <= 0.125f; z += 0.250f)
                        {
                            rgv3tStuckTable[idx].X = x;
                            rgv3tStuckTable[idx].Y = y;
                            rgv3tStuckTable[idx].Z = z;
                            idx++;
                        }
                    }
                }

                x = y = 0;
                zi[0] = 0.0f;
                zi[1] = 1.0f;
                zi[2] = 6.0f;
                for (i = 0; i < 3; i++)
                {
                    // Z moves
                    z = zi[i];
                    rgv3tStuckTable[idx].X = x;
                    rgv3tStuckTable[idx].Y = y;
                    rgv3tStuckTable[idx].Z = z;
                    idx++;
                }

                x = z = 0;

                // Y moves
                for (y = -2.0f; y <= 2.0f; y += 2.0f)
                {
                    rgv3tStuckTable[idx].X = x;
                    rgv3tStuckTable[idx].Y = y;
                    rgv3tStuckTable[idx].Z = z;
                    idx++;
                }

                y = z = 0;
                // X moves
                for (x = -2.0f; x <= 2.0f; x += 2.0f)
                {
                    rgv3tStuckTable[idx].X = x;
                    rgv3tStuckTable[idx].Y = y;
                    rgv3tStuckTable[idx].Z = z;
                    idx++;
                }

                for (i = 0; i < 3; i++)
                {
                    z = zi[i];

                    for (x = -2.0f; x <= 2.0f; x += 2.0f)
                    {
                        for (y = -2.0f; y <= 2.0f; y += 2.0f)
                        {
                            rgv3tStuckTable[idx].X = x;
                            rgv3tStuckTable[idx].Y = y;
                            rgv3tStuckTable[idx].Z = z;
                            idx++;
                        }
                    }
                }
            }
            #endregion

            #region PM_InitTextureTypes
            {
                
            }
            #endregion
        }

        #region Encoders
        
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void RegisterEncoders()
        {
            g_pEngineFuncs->pfnDeltaAddEncoder(Marshal.StringToHGlobalAnsi("Entity_Encode"), EntityEncodeDelegate);
            g_pEngineFuncs->pfnDeltaAddEncoder(Marshal.StringToHGlobalAnsi("Custom_Encode"), CustomEncodeDelegate);
            g_pEngineFuncs->pfnDeltaAddEncoder(Marshal.StringToHGlobalAnsi("Player_Encode"), PlayerEncodeDelegate);
        }

        private static bool EncodeEntityInitialized = false;

        public static (IntPtr name, int field)[] entity_field_alias = new[]
        {
            (Marshal.StringToHGlobalAnsi("origin[0]"), 0),
            (Marshal.StringToHGlobalAnsi("origin[1]"), 0),
            (Marshal.StringToHGlobalAnsi("origin[2]"), 0),
            (Marshal.StringToHGlobalAnsi("angles[0]"), 0),
            (Marshal.StringToHGlobalAnsi("angles[1]"), 0),
            (Marshal.StringToHGlobalAnsi("angles[2]"), 0),
        };
        
        public static readonly delegate* unmanaged[Cdecl]<Delta*, byte*, byte*, void> EntityEncodeDelegate = &EncodeEntity;
        
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void EncodeEntity(Delta* pFields, byte* from, byte* to)
        {
            EntityState* f, t;
            bool localplayer = false;
            if (!EncodeEntityInitialized)
            {
                entity_field_alias[0].field = g_pEngineFuncs->pfnDeltaFindField(pFields, entity_field_alias[0].name);
                entity_field_alias[1].field = g_pEngineFuncs->pfnDeltaFindField(pFields, entity_field_alias[1].name);
                entity_field_alias[2].field = g_pEngineFuncs->pfnDeltaFindField(pFields, entity_field_alias[2].name);
                entity_field_alias[3].field = g_pEngineFuncs->pfnDeltaFindField(pFields, entity_field_alias[3].name);
                entity_field_alias[4].field = g_pEngineFuncs->pfnDeltaFindField(pFields, entity_field_alias[4].name);
                entity_field_alias[5].field = g_pEngineFuncs->pfnDeltaFindField(pFields, entity_field_alias[5].name);
                EncodeEntityInitialized = true;
            }

            f = (EntityState*)from;
            t = (EntityState*)to;

            localplayer = (t->number - 1) == g_pEngineFuncs->pfnGetCurrentPlayer();
            if (localplayer)
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[0].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[1].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[2].field);
            }

            if ((t->impacttime != 0) && (t->starttime != 0))
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[0].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[1].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[2].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[3].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[4].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[5].field);
            }

            if ((t->movetype == 12) && (t->aiment != 0)) // 12 = MOVETYPE_FOLLOW
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[0].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[1].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[2].field);
            }
            else if (t->aiment != f->aiment)
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[0].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[1].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, entity_field_alias[2].field);
            }
        }
        
        private static bool EncodePlayerInitialized = false;

        public static (IntPtr name, int field)[] player_field_alias = new[]
        {
            (Marshal.StringToHGlobalAnsi("origin[0]"), 0),
            (Marshal.StringToHGlobalAnsi("origin[1]"), 0),
            (Marshal.StringToHGlobalAnsi("origin[2]"), 0),
        };

        public static readonly delegate* unmanaged[Cdecl]<Delta*, byte*, byte*, void> PlayerEncodeDelegate = &EncodePlayer;

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void EncodePlayer(Delta* pFields, byte* from, byte* to)
        {
            EntityState* f, t;
            bool localplayer = false;
            if (!EncodePlayerInitialized)
            {
                player_field_alias[0].field = g_pEngineFuncs->pfnDeltaFindField(pFields, player_field_alias[0].name);
                player_field_alias[1].field = g_pEngineFuncs->pfnDeltaFindField(pFields, player_field_alias[1].name);
                player_field_alias[2].field = g_pEngineFuncs->pfnDeltaFindField(pFields, player_field_alias[2].name);
                EncodePlayerInitialized = true;
            }

            f = (EntityState*)from;
            t = (EntityState*)to;

            localplayer = (t->number - 1) == g_pEngineFuncs->pfnGetCurrentPlayer();
            if (localplayer)
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[0].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[1].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[2].field);
            }

            if ((t->impacttime != 0) && (t->starttime != 0))
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[0].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[1].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[2].field);
            }

            if ((t->movetype == 12) && (t->aiment != 0)) // 12 = MOVETYPE_FOLLOW
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[0].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[1].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[2].field);
            }
            else if (t->aiment != f->aiment)
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[0].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[1].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, player_field_alias[2].field);
            }
        }
        private static bool EncodeCustomEntityInitialized = false;

        public static (IntPtr name, int field)[] custom_entity_field_alias = new[]
        {
            (Marshal.StringToHGlobalAnsi("origin[0]"), 0),
            (Marshal.StringToHGlobalAnsi("origin[1]"), 0),
            (Marshal.StringToHGlobalAnsi("origin[2]"), 0),
            (Marshal.StringToHGlobalAnsi("angles[0]"), 0),
            (Marshal.StringToHGlobalAnsi("angles[1]"), 0),
            (Marshal.StringToHGlobalAnsi("angles[2]"), 0),
            (Marshal.StringToHGlobalAnsi("skin"), 0),
            (Marshal.StringToHGlobalAnsi("sequence"), 0),
            (Marshal.StringToHGlobalAnsi("animtime"), 0),
        };

        public static readonly delegate* unmanaged[Cdecl]<Delta*, byte*, byte*, void> CustomEncodeDelegate = &EncodeCustom;

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void EncodeCustom(Delta* pFields, byte* from, byte* to)
        {
            EntityState* f, t;
            int beamtype;
            if (!EncodeCustomEntityInitialized)
            {
                custom_entity_field_alias[0].field = g_pEngineFuncs->pfnDeltaFindField(pFields, custom_entity_field_alias[0].name);
                custom_entity_field_alias[1].field = g_pEngineFuncs->pfnDeltaFindField(pFields, custom_entity_field_alias[1].name);
                custom_entity_field_alias[2].field = g_pEngineFuncs->pfnDeltaFindField(pFields, custom_entity_field_alias[2].name);
                custom_entity_field_alias[3].field = g_pEngineFuncs->pfnDeltaFindField(pFields, custom_entity_field_alias[3].name);
                custom_entity_field_alias[4].field = g_pEngineFuncs->pfnDeltaFindField(pFields, custom_entity_field_alias[4].name);
                custom_entity_field_alias[5].field = g_pEngineFuncs->pfnDeltaFindField(pFields, custom_entity_field_alias[5].name);
                custom_entity_field_alias[6].field = g_pEngineFuncs->pfnDeltaFindField(pFields, custom_entity_field_alias[6].name);
                custom_entity_field_alias[7].field = g_pEngineFuncs->pfnDeltaFindField(pFields, custom_entity_field_alias[7].name);
                custom_entity_field_alias[8].field = g_pEngineFuncs->pfnDeltaFindField(pFields, custom_entity_field_alias[8].name);
                EncodeCustomEntityInitialized = true;
            }

            f = (EntityState*)from;
            t = (EntityState*)to;

            beamtype = t->renderamt & 0x0f;
            if (beamtype != 0 && beamtype != 1)
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, custom_entity_field_alias[0].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, custom_entity_field_alias[1].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, custom_entity_field_alias[2].field);
            }
            if (beamtype != 0)
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, custom_entity_field_alias[3].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, custom_entity_field_alias[4].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, custom_entity_field_alias[5].field);
            }
            if (beamtype != 2 && beamtype != 1)
            {
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, custom_entity_field_alias[6].field);
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, custom_entity_field_alias[7].field);
            }
            if(((int)f->animtime) == ((int)t->animtime))
                g_pEngineFuncs->pfnDeltaUnsetFieldByIndex(pFields, custom_entity_field_alias[8].field);
        }
        
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        
        #endregion
        public static void ServerDeactivate()
        {
            Console.WriteLine("ServerDeactivate Called");
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void ServerActivate(Edict* edictList, int edictCount, int clientMax)
        {
            Console.WriteLine("Called Server Activate");
            for (int i = 0; i < edictCount; i++)
            {
                if(edictList[i].free != 0) continue;
                if( (i > 0 && i <= clientMax) || edictList[i].pvPrivateData == null ) continue;
                
                
            }
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void StartFrame()
        {
            
        }

        private static IntPtr nameStr = Marshal.StringToHGlobalAnsi("name");
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void ClientUserInfoChanged(Edict* pEntity, char* infobuffer)
        {
            if(pEntity->pvPrivateData == null ) return;
            if (pEntity->v.netname != 0)
            {
                IntPtr strPtr = g_pEngineFuncs->pfnSzFromIndex(pEntity->v.netname);
                string netName = Marshal.PtrToStringAnsi(strPtr);
                string infoKeyValueString = Marshal.PtrToStringAnsi(g_pEngineFuncs->pfnInfoKeyValue((IntPtr)infobuffer, nameStr));
                if (netName.Length > 0 && netName != infoKeyValueString)
                {
                    string sName = infoKeyValueString.Replace('%', ' ');

                    char[] finalName = new char[256];

                    int length = Math.Min(sName.Length, 255);
                    sName.CopyTo(0, finalName, 0, length);
                    finalName[length] = '\0';
                    fixed (char* p = &finalName[0])
                    {
                        g_pEngineFuncs->pfnSetClientKeyValue( g_pEngineFuncs->pfnIndexOfEdict(pEntity), (IntPtr)infobuffer, nameStr, (IntPtr) p );
                    }
                }
            }
            
            //TODO: GameRules event
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int ClientConnect(Edict* pEntity, IntPtr pszName, IntPtr pszAddress, char* szRejectReason)
        {
            return 1;
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void ClientPutInServer(Edict* edict)
        {
            
            var player = BaseEntity.CreateEntity<BasePlayer>(edict);
            player.Spawn();
            player.PEV->effects |= 32;
            player.PEV->origin = spawnEntity.PEV->origin;
            player.PEV->iuser1 = 0;
            player.PEV->iuser2 = 0;
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void PlayerCustomization(Edict* pEntity, IntPtr pCust)
        {
            var player = (BasePlayer?)GCHandle.FromIntPtr((IntPtr)pEntity->pvPrivateData).Target;
            if(player == null) return;
            if(pCust == 0) return;
            
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void UpdateClientData(Edict* ent, int sendweapons, ClientData* cd)
        {
            if(ent == null || ent->pvPrivateData == null) return;
            EntVars* pev = &ent->v;
            var player = (BasePlayer?)GCHandle.FromIntPtr((IntPtr)ent->pvPrivateData).Target;
            EntVars* pevOrg = null;

            if (player != null && player.PEV->iuser1 == 4)
            {
                
            }

            cd->flags =     pev->flags;
            cd->health		= pev->health;
            
            cd->viewmodel = g_pEngineFuncs->pfnModelIndex( g_pEngineFuncs->pfnSzFromIndex(pev->viewmodel) );

            cd->waterlevel		= pev->waterlevel;
            cd->watertype		= pev->watertype;
            cd->weapons		= pev->weapons;

            // Vectors
            cd->origin		= pev->origin;
            cd->velocity		= pev->velocity;
            cd->view_ofs		= pev->view_ofs;
            cd->punchangle		= pev->punchangle;

            cd->bInDuck		= pev->bInDuck;
            cd->flTimeStepSound	= pev->flTimeStepSound;
            cd->flDuckTime		= pev->flDuckTime;
            cd->flSwimTime		= pev->flSwimTime;
            cd->waterjumptime	= (int)pev->teleport_time;

            string physinfo = Marshal.PtrToStringAnsi(g_pEngineFuncs->pfnGetPhysicsInfoString(ent));
            int length = Math.Min(physinfo.Length, 255);
            for (int i = 0; i < length; i++)
            {
                cd->physinfo[i] = physinfo[i];
            }
            cd->physinfo[length] = '\0';

            cd->maxspeed		= pev->maxspeed;
            cd->fov			= pev->fov;
            cd->weaponanim		= pev->weaponanim;

            cd->pushmsec		= pev->pushmsec;

            cd->iuser1 = pev->iuser1;
            cd->iuser2 = pev->iuser2;
            
            //TODO: Client Weapons
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int GetWeaponData(Edict* player, IntPtr info)
        {
            return 1;
            //TODO: Client Weapon
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void SetupVisibility(Edict* pViewEntity, Edict* pClient, byte** pvs, byte** pas)
        {
            Vector org;
            Edict* pView = pClient;

            if (pViewEntity != null)
            {
                pView = pViewEntity;
            }

            if ((pClient->v.flags & (1 << 20)) != 0)
            {
                *pvs = null;
                *pas = null;
                return;
            }

            if ((pView->v.effects & (1 << 29)) != 0)
            {
                if (BaseEntity.Instance(pView).ClassnameIs("env_sky"))
                {
                    org = pView->v.origin;
                }
                else return;
            }
            else
            {
                org = pView->v.origin + pView->v.view_ofs;
                if ((pView->v.flags & (1 << 14)) != 0)
                {
                    org = org + new Vector(0, 0, -18);
                }
            }
            
            *pvs = g_pEngineFuncs->pfnSetFatPVS(&org.X);
            *pas = g_pEngineFuncs->pfnSetFatPAS(&org.X);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int AddToFullPack(EntityState* state, int e, Edict* ent, Edict* host, int hostflags, int player, byte* pSet)
        {
            int i;
            BaseEntity? Entity = null;

            if ((ent->v.effects & 128) != 0 && ent != host) return 0;
            if (ent->v.modelindex == 0 || g_pEngineFuncs->pfnSzFromIndex(ent->v.movetype) == 0) return 0;
            if ((ent->v.flags & (1 << 26)) != 0 && ent != host) return 0;
            if (ent != host)
            {
                if (g_pEngineFuncs->pfnCheckVisibility(ent, pSet) == 0)
                {
                    if (!BaseEntity.Instance(ent).ClassnameIs("env_sky"))
                    {
                        return 0;
                    }
                }
            }
            

            if ((ent->v.flags & (1 << 8)) != 0)
            {
                if ((hostflags & 4) != 0) return 0;
                if ((hostflags & 1) != 0 && ent->v.owner == host) return 0;
            }

            if (host->v.groupinfo != 0)
            {
                int g_groupmask = host->v.groupinfo;
                int g_groupop = 0;
                g_pEngineFuncs->pfnSetGroupMask(g_groupmask, g_groupop);
                if (ent->v.groupinfo != 0)
                {
                    if ((ent->v.groupinfo & host->v.groupinfo) == 0) return 0;
                }
                g_pEngineFuncs->pfnSetGroupMask(0, 0);
                //TODO: Check if works correctly
            }
            Util.MemSet(state, 0, Marshal.SizeOf(*state));
            state->number = e;
            state->entityType = (1<<0);

            if ((ent->v.flags & (1 << 29)) != 0)
            {
                state->entityType = (1<<1);
            }

            state->animtime = (int)(1000.0f * ent->v.animtime) / 1000.0f;
            
            Buffer.MemoryCopy( &ent->v.origin,&state->origin, 3 * sizeof(float), 3 * sizeof(float));
            Buffer.MemoryCopy( &ent->v.angles, &state->angles,3 * sizeof(float), 3 * sizeof(float));
            Buffer.MemoryCopy( &ent->v.mins,&state->mins, 3 * sizeof(float), 3 * sizeof(float));
            Buffer.MemoryCopy( &ent->v.maxs,&state->maxs, 3 * sizeof(float), 3 * sizeof(float));
            
            Buffer.MemoryCopy( &ent->v.startpos,&state->startpos, 3 * sizeof(float), 3 * sizeof(float));
            Buffer.MemoryCopy( &ent->v.endpos,&state->endpos, 3 * sizeof(float), 3 * sizeof(float));
            Buffer.MemoryCopy( &ent->v.velocity,&state->velocity, 3 * sizeof(float), 3 * sizeof(float));

            state->impacttime = ent->v.impacttime;
            state->starttime = ent->v.starttime;

            state->modelindex = ent->v.modelindex;

            state->frame = ent->v.frame;

            state->skin = (short)ent->v.skin;
            state->effects = ent->v.effects;

            if (player == 0 &&
                ent->v.animtime != 0 &&
                ent->v.velocity.X == 0 &&
                ent->v.velocity.Y == 0 &&
                ent->v.velocity.Z == 0)
            {
                state->eflags |= 1;
            }
            
            state->scale		= ent->v.scale;
            state->solid		= (short)ent->v.solid;
            state->colormap		= ent->v.colormap;

            state->movetype		= ent->v.movetype;
            state->sequence		= ent->v.sequence;
            state->framerate	= ent->v.framerate;
            state->body		= ent->v.body;
            
            for( i = 0; i < 4; i++ )
            {
                state->controller[i] = ent->v.controller[i];
            }
            for( i = 0; i < 2; i++ )
            {
                state->blending[i] = ent->v.blending[i];
            }
            state->rendermode	= ent->v.rendermode;
            state->renderamt	= (int)ent->v.renderamt; 
            state->renderfx		= ent->v.renderfx;
            state->rendercolor.R	= (byte)ent->v.rendercolor.X;
            state->rendercolor.G	= (byte)ent->v.rendercolor.Y;
            state->rendercolor.B	= (byte)ent->v.rendercolor.Z;
            
            state->aiment = 0;
            if( ent->v.aiment != null )
            {
                state->aiment = g_pEngineFuncs->pfnIndexOfEdict( ent->v.aiment );
            }
            state->owner = 0;
            if( ent->v.owner != null )
            {
                int owner = g_pEngineFuncs->pfnIndexOfEdict( ent->v.owner );

                // Only care if owned by a player
                if( owner >= 1 && owner <= g_pGlobals->maxClients )
                {
                    state->owner = owner;	
                }
            }

            state->onground = 0;
            if( ent->v.groundentity != null )
            {
                state->onground = g_pEngineFuncs->pfnIndexOfEdict( ent->v.groundentity );
            }

            if (player == 0)
            {
                state->playerclass  = ent->v.playerclass;
            }
            if (player != 0)
            {
                Buffer.MemoryCopy(&ent->v.basevelocity, &state->basevelocity, 3 * sizeof(float), 3 * sizeof(float));
                
                state->weaponmodel	= g_pEngineFuncs->pfnModelIndex( g_pEngineFuncs->pfnSzFromIndex( ent->v.weaponmodel ) );
                state->gaitsequence	= ent->v.gaitsequence;
                state->spectator	= ent->v.flags & (1<<26);
                state->friction		= ent->v.friction;

                state->gravity		= ent->v.gravity;
                //state->team		= ent->v.team;

                state->usehull		= ( ent->v.flags & (1<<14) ) != 0 ? 1 : 0;
                state->health		= (int)ent->v.health;
            }

            Entity = BaseEntity.Instance(ent);
            if (Entity != null && Entity.Classify() != 0 && Entity.Classify() != 1)
            {
                state->eflags |= 2;
            }
            else
            {
                state->eflags = (byte)(state->eflags & ~(2));
            }
            
            return 1;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void CmdStart(Edict* player, UserCmd* cmd, uint random_seed)
        {
            //Console.WriteLine("Offset: " + Marshal.OffsetOf<PlayerMove>("vuser4"));
            //pmove->cmd = *cmd;
            //Console.WriteLine(pmove->cmd.msec);
            /*byte* ptr = (byte*)&pmove->numphysent;
            for (int i = 0; i < 1 * (1264); i++)
            {
                UserCmd* cmdPtr = (UserCmd*)ptr;
                if (cmdPtr->msec == cmd->msec && (cmdPtr->forwardmove - cmd->forwardmove) < 0.01f)
                {
                    
                }
            }*/
            //if(cmd->impulse != 0) Console.WriteLine("Impulse: " + cmd->impulse);
            
            BasePlayer? pl = (BasePlayer?)BaseEntity.Instance(player);
            if(pl == null) return;

            if (pl.PEV->groupinfo != 0)
            {
                g_pEngineFuncs->pfnSetGroupMask(pl.PEV->groupinfo, 0);
            }
            
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void PlayerPreThink(Edict* player)
        {
            BasePlayer? ply = (BasePlayer?)BaseEntity.Instance(player);
            ply?.PlayerPreThink();
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void PM_Move(PlayerMove* ppmove, int server)
        {
            pmove = ppmove;
            #region PlayerMove

            {
                pmove->Server = server;
                pmove->numtouch = 0;
                pmove->FrameTime = pmove->cmd.msec * 0.001f;
                
                
                
                Vector wishvel;
                float fmove, smove;
                fmove = pmove->cmd.forwardmove;
                smove = pmove->cmd.sidemove;

                Vector v_angle = new Vector();
                
                if (pmove->dead == 0)
                {
                    v_angle.X = pmove->cmd.viewangles.X;
                    v_angle.Y = pmove->cmd.viewangles.Y;
                    v_angle.Z = pmove->cmd.viewangles.Z;
                    pmove->Angles.Z = v_angle.Z;
                    pmove->Angles.X = v_angle.X;
                    pmove->Angles.Y = v_angle.Y;
                }
                
                g_pEngineFuncs->pfnAngleVectors(&pmove->Angles.X, &pmove->Forward.X, &pmove->Right.X, &pmove->Up.X);
                
                pmove->Forward = pmove->Forward.Normalize();
                pmove->Right = pmove->Right.Normalize();

                Vector point = new Vector(pmove->Origin.X, pmove->Origin.Y, pmove->Origin.Z - 2);
                PmTrace tr = pmove->PM_PlayerTrace(&pmove->Origin.X, &point.X, 0x00000000, -1);

                if (tr.plane.normal.Z < 0.7f)
                    pmove->onground = -1;
                else
                    pmove->onground = tr.ent;
                
                //Console.WriteLine("Ent: " + tr.ent);

                if (pmove->onground != -1)
                {
                    pmove->Velocity.Z = 0;
                }
                
                //WALK
                //TODO: PlayerMove

                wishvel.X = pmove->Forward.X * fmove + pmove->Right.X * smove;
                wishvel.Y = pmove->Forward.Y * fmove + pmove->Right.Y * smove;
                wishvel.Z = pmove->Forward.Z * fmove + pmove->Right.Z * smove;
                //wishvel.Y += pmove->cmd.upmove;

                //pmove->Origin.X += pmove->FrameTime * wishvel.X;
                //pmove->Origin.Y += pmove->FrameTime * wishvel.Y;
                //pmove->Origin.Z += pmove->FrameTime * wishvel.Z;

                pmove->Velocity.X = pmove->BaseVelocity.X + wishvel.X;
                pmove->Velocity.Y = pmove->BaseVelocity.Y + wishvel.Y;
                pmove->Velocity.Z = pmove->BaseVelocity.Z;

                Vector dest = new Vector(pmove->Velocity.X * pmove->FrameTime, pmove->Velocity.Y * pmove->FrameTime, pmove->Velocity.Z * pmove->FrameTime);
                if (pmove->onground != -1) dest.Z = 0;
                dest.X += pmove->Origin.X;
                dest.Y += pmove->Origin.Y;
                dest.Z += pmove->Origin.Z;

                tr = pmove->PM_PlayerTrace(&pmove->Origin.X, &dest.X, 0x00000000, -1);
                
                if (tr.fraction > 0.99)
                {
                    pmove->Origin.X = dest.X;
                    pmove->Origin.Y = dest.Y;
                    pmove->Origin.Z = dest.Z;
                    return;
                }
                
                //pmove->Velocity = new Vector(0,0,0);
            }
            #endregion

            if (pmove->onground != -1)
            {
                pmove->flags |= (1 << 9);
            }
            else
            {
                pmove->flags &= ~(1 << 9);
            }
            //TODO: friction
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void PlayerPostThink(Edict* player)
        {
            BasePlayer? ply = (BasePlayer?)BaseEntity.Instance(player);
            ply?.PlayerPostThink();
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void CmdEnd(Edict* player)
        {
            BasePlayer? ply = (BasePlayer?)BaseEntity.Instance(player);
            if(ply == null) return;
            if (ply.PEV->groupinfo != 0) g_pEngineFuncs->pfnSetGroupMask(0, 0);
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void DispatchObjectCollisionBox(Edict* pent)
        {
            BaseEntity.SetObjectCollisionBox(&pent->v);
        }
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void ClientCommand(Edict* pEntity)
        {
            string? pcmd = Marshal.PtrToStringAnsi(g_pEngineFuncs->pfnCmd_Argv(0));
            string? fulstr = Marshal.PtrToStringAnsi(g_pEngineFuncs->pfnCmd_Args());
            if(pcmd == null)return;
            Console.WriteLine("Client Entered: " + pcmd + " Full Str: " + fulstr + " Count: " + g_pEngineFuncs->pfnCmd_Argc());
        }
    }
}
