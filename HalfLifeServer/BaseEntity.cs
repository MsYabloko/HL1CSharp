using System.Runtime.InteropServices;

namespace XashGameDLL;

public abstract unsafe class BaseEntity
{
    public abstract void Spawn();
    public abstract void Think();
    public abstract void Use();
    public abstract void Touch();

    private bool haveSetup = false;
    private EntVars* _PEV;
    internal EntVars* PEV => _PEV;

    public bool ClassnameIs(string classname)
    {
        return classname == Marshal.PtrToStringAnsi(EngineFuncs.Funcs->pfnSzFromIndex(_PEV->classname));
    }

    public static BaseEntity? Instance(Edict* ent)
    {
        var entity = (BaseEntity?)GCHandle.FromIntPtr((IntPtr)ent->pvPrivateData).Target;
        return entity;
    }

    public void Setup(EntVars* vars)
    {
        if(haveSetup) return;
        _PEV = vars;
        haveSetup = true;
    }

    public virtual int Classify()
    {
        return 0;
    }

    public static void SetObjectCollisionBox(EntVars* pev)
    {
        if (pev->solid == 4 && (pev->angles.X != 0 || pev->angles.Y != 0 || pev->angles.Z != 0))
        {
            float max, v;
            int i;
            max = 0;
            for( i = 0; i < 3; i++ )
            {
                v = MathF.Abs((&pev->mins.X)[i]);
                if( v > max )
                    max = v;
                v = MathF.Abs((&pev->maxs.X)[i]);
                if( v > max )
                    max = v;
            }

            for (i = 0; i < 3; i++)
            {
                (&pev->absmin.X)[i] = (&pev->origin.X)[i] - max;
                (&pev->absmax.X)[i] = (&pev->origin.X)[i] + max;
            }
        }
        else
        {
            pev->absmin = pev->origin + pev->mins;
            pev->absmax = pev->origin + pev->maxs;
        }
        
        pev->absmin.X -= 1;
        pev->absmin.Y -= 1;
        pev->absmin.Z -= 1;
        pev->absmax.X += 1;
        pev->absmax.Y += 1;
        pev->absmax.Z += 1;
    }
    
    public static T CreateEntity<T>(Edict* edict) where T : BaseEntity, new()
    {
        var ent = new T();
        GCHandle handle = GCHandle.Alloc(ent, GCHandleType.Normal);
        edict->pvPrivateData = (void*)GCHandle.ToIntPtr(handle);
        ent.Setup(&edict->v);
        return ent;
    }

    public void SetNextThink(float plustime)
    {
        PEV->nextthink = GlobalVars.Vars->time + plustime;
    }
    
}