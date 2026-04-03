using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameLogic;
using Launcher;
using Newtonsoft.Json;
using TEngine;

namespace Procedure
{
    public class ProcedureStartGame : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        protected override void OnInit(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnInit(procedureOwner);
            NewtonsoftJsonHelper.Settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto, // 或 All，如果 JSON 中有 $type
                SerializationBinder = new KnownTypesBinder
                {
                    KnownTypes = new List<System.Type>
                    {
                        typeof(MoveSegment),
                        typeof(ThrowSegment)
                    }
                }
            };
            // AbilityTemplate.LoadTemplatesFromJson().Forget();
        }

        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            StartGame(procedureOwner).Forget();
        }

        private async UniTaskVoid StartGame(IFsm<IProcedureModule> procedureOwner)
        {
            await UniTask.Yield();
            LauncherMgr.HideAllUI();
            ChangeState<ProcedureMainMenu>(procedureOwner);
        }
    }
}
