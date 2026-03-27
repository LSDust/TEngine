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
            
            string nextSceneName = procedureOwner.GetData<string>("NextSceneName");
            
            // 使用 UniTaskCompletionSource 跟踪场景资源加载完成
            var sceneResourceLoadedTcs = new UniTaskCompletionSource<bool>();
            bool hasSetResult = false;
            
            // 启动场景加载（suspendLoad = true）
            GameModule.Scene.LoadSceneAsync(
                nextSceneName, 
                LoadSceneMode.Single, 
                suspendLoad: true,
                progressCallBack: progress =>
                {
                    // 当进度达到 0.9（90%）时，认为场景资源加载完成
                    if (progress >= 0.9f && !hasSetResult)
                    {
                        hasSetResult = true;
                        sceneResourceLoadedTcs.TrySetResult(true);
                    }
                }
            ).Forget();
            
            // 在场景加载的同时，在主线程加载数据
            if (PersistenceSystem.TryLoadLocalData<SaveData>(out var data))
            {
                PersistenceSystem.SaveData = data;
            }
            
            // 等待场景资源加载完成
            await sceneResourceLoadedTcs.Task;
            
            // 解除场景挂起并激活
            GameModule.Scene.UnSuspend(nextSceneName);
            
            GameModule.UI.CloseUI<UI_Wait>();
            // todo: ChangeState<PlayerRoom>(procedureOwner);
        }
    }
}
