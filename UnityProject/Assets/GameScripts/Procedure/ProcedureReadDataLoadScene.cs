using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameLogic;
using UnityEngine;
using TEngine;
using UnityEngine.SceneManagement;
using System;

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
            // todo:加载资源,获取NextSceneName
            await GameModule.Scene.LoadSceneAsync(procedureOwner.GetData<string>("NextSceneName"), LoadSceneMode.Single, false);
            GameModule.UI.CloseUI<UI_Wait>();
            Type nextProcedureType = procedureOwner.GetData<Type>("NextProcedure");
            ChangeState(procedureOwner, nextProcedureType);
        }
    }
}
