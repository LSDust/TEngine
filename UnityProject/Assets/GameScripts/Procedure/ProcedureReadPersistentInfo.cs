using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameLogic;
using UnityEngine;
using TEngine;
using UnityEngine.SceneManagement;

namespace Procedure
{
    public class ProcedureReadPersistentInfo : ProcedureBase
    {
        public override bool UseNativeDialog { get; }
        
        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //todo: 失败处理
            ReadPersistentInfo(procedureOwner).Forget();
        }

        protected override void OnUpdate(IFsm<IProcedureModule> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<IProcedureModule> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            // GameModule.UI.CloseUI<UI_Battle>();
        }

        private async UniTask ReadPersistentInfo(IFsm<IProcedureModule> procedureOwner)
        {
            await GameModule.Scene.LoadSceneAsync("Loading", LoadSceneMode.Single, false);
            // 1. 显示进度条的UI
            var uiProgressBar = await GameModule.UI.ShowUIAsyncAwait<UI_ProgressBar>();
            // 加载资源
            await UniTask.Delay(10);
            await GameModule.Scene.LoadSceneAsync(procedureOwner.GetData<string>("NextSceneName"), LoadSceneMode.Single, true);
            GameModule.UI.CloseUI<UI_ProgressBar>();
            // ChangeState<PlayerRoom>(procedureOwner);
        }
    }
}
