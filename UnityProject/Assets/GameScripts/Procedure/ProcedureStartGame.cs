using System;
using Cysharp.Threading.Tasks;
using Launcher;
using TEngine;

namespace Procedure
{
    public class ProcedureStartGame : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

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
