using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameLogic;
using UnityEngine;
using TEngine;
using UnityEngine.SceneManagement;

namespace Procedure
{
    public class ProcedureReadDataLoadScene : ProcedureBase
    {
        public override bool UseNativeDialog { get; }
        
        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            ReadDataLoadScene(procedureOwner).Forget();
        }

        private async UniTask ReadDataLoadScene(IFsm<IProcedureModule> procedureOwner)
        {
            await GameModule.Scene.LoadSceneAsync("Loading", LoadSceneMode.Single, false);
            var uiWait = await GameModule.UI.ShowUIAsyncAwait<UI_Wait>();
            // todo:加载资源
            await GameModule.Scene.LoadSceneAsync(procedureOwner.GetData<string>("NextSceneName"), LoadSceneMode.Single, true);
            GameModule.UI.CloseUI<UI_Wait>();
            // todo: ChangeState<PlayerRoom>(procedureOwner);
        }
    }
}
