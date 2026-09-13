using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace GameMod.Entities.Server;
using XashGameDLL;
using global::System;
using global::System.Collections.Generic;
using global::System.IO;
using global::System.Linq;
using global::System.Net.Http;
using global::System.Threading;
using global::System.Threading.Tasks;
using System;
using System.Reflection;
using System;
using System.Reflection;
public unsafe class TestEntity : BaseEntity
{
      public override void Spawn()
      {
          
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
  
      [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "test_entity")]
        public static void SpawnEntity(EntVars* e)
        {
               BaseEntity.CreateEntity<TestEntity>(e->pContainingEntity);
        }
}
