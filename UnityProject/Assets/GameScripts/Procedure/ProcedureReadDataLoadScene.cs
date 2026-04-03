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
            try
            {
                await LoadLoadingScene();
                var uiWait = await ShowWaitUI();
                
                string nextSceneName = procedureOwner.GetData<string>("NextSceneName");
                
                // 并行执行场景加载和数据加载
                var sceneLoadTask = LoadSceneWithSuspend(nextSceneName);
                var dataLoadTask = LoadGameData();
                
                // 等待所有任务完成
                await UniTask.WhenAll(sceneLoadTask, dataLoadTask);
                
                // 解除场景挂起并激活
                GameModule.Scene.UnSuspend(nextSceneName);
                
                GameModule.UI.CloseUI<UI_Wait>();
                ChangeState<ProcedurePlayerRoom>(procedureOwner);
            }
            catch (System.Exception e)
            {
                Log.Error("ProcedureReadDataLoadScene", $"加载过程出错: {e.Message}");
                // 可以在这里添加错误处理逻辑，例如显示错误UI或返回上一个流程
            }
        }

        private async UniTask LoadLoadingScene()
        {
            await GameModule.Scene.LoadSceneAsync("Loading", LoadSceneMode.Single, false);
        }

        private async UniTask<UI_Wait> ShowWaitUI()
        {
            return await GameModule.UI.ShowUIAsyncAwait<UI_Wait>();
        }

        private async UniTask LoadSceneWithSuspend(string sceneName)
        {
            // 使用 UniTaskCompletionSource 跟踪场景资源加载完成
            var sceneResourceLoadedTcs = new UniTaskCompletionSource<bool>();
            bool hasSetResult = false;
            
            // 启动场景加载（suspendLoad = true）
            GameModule.Scene.LoadSceneAsync(
                sceneName, 
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
            
            // 等待场景资源加载完成
            await sceneResourceLoadedTcs.Task;
        }

        private async UniTask LoadGameData()
        {
            // 加载本地存档数据
            if (PersistenceSystem.TryLoadLocalData<SaveData>(out var data))
            {
                PersistenceSystem.SaveData = data;
            }
            
            // 加载能力模板
            await AbilityTemplate.LoadTemplatesFromJson();
            
            foreach (var v in PersistenceSystem.SaveData.player.skills)
            {
                AbilityTemplate.GetTemplate(v.skillId).Level = v.level;
            }
        }
    }
}
