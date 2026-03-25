using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using TEngine;

namespace GameLogic
{
    /// <summary>
    /// SceneModule扩展方法类
    /// </summary>
    public static class SceneModuleExtensions
    {
        /// <summary>
        /// 带进度条UI的场景加载。
        /// </summary>
        /// <param name="sceneModule">场景模块实例</param>
        /// <param name="targetSceneLocation">目标场景的定位地址</param>
        /// <param name="priority">优先级</param>
        /// <param name="gcCollect">加载主场景是否回收垃圾。</param>
        /// <param name="progressCallBack">加载进度回调。</param>
        /// <returns>加载的场景实例</returns>
        public static async UniTask<Scene> LoadSceneWithProgressBarAsync(this ISceneModule sceneModule, 
            string targetSceneLocation, 
            uint priority = 100, 
            bool gcCollect = true,
            Action<float> progressCallBack = null)
        {
            // 1. 显示进度条的UI
            var uiProgressBar = await GameModule.UI.ShowUIAsyncAwait<UI_ProgressBar>();
            
            // 2. 跳转到Loading过渡场景
            await sceneModule.LoadSceneAsync("Loading", LoadSceneMode.Single, false, priority, gcCollect);
            
            // 3. 创建进度回调，更新UI_ProgressBar的进度
            void CombinedProgressCallBack(float progress)
            {
                // 更新UI_Loading的进度条
                uiProgressBar.UpdateProgress(progress);

                // 调用外部传入的进度回调
                progressCallBack?.Invoke(progress);
            }

            // 4. 在Loading过渡场景执行到目标场景的加载
            Scene targetScene = await sceneModule.LoadSceneAsync(targetSceneLocation, LoadSceneMode.Single, false, priority, gcCollect, CombinedProgressCallBack);
            
            // 5. 加载完成后关闭UI_ProgressBar
            GameModule.UI.CloseUI<UI_ProgressBar>();
            
            return targetScene;
        }
    }
}
