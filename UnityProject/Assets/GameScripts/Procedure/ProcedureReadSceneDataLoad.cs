using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameLogic;
using UnityEngine;
using TEngine;
using UnityEngine.SceneManagement;

namespace Procedure
{
    public class ProcedureReadSceneDataLoad : ProcedureBase
    {
        public override bool UseNativeDialog { get; }
        
        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            ReadSceneDataLoad(procedureOwner).Forget();
        }

        private async UniTask ReadSceneDataLoad(IFsm<IProcedureModule> procedureOwner)
        {
            await GameModule.Scene.LoadSceneAsync("Loading", LoadSceneMode.Single, false);
            var uiWait = await GameModule.UI.ShowUIAsyncAwait<UI_Wait>();
            // todo:加载资源
            // await GameModule.Resource.LoadAssetAsync<TextAsset>(string.Empty);
            if (PersistenceSystem.TryLoadLocalData<SaveData>(out var data))
            {
                PersistenceSystem.SaveData = data;
            }

            Debug.Log(PersistenceSystem.SaveData);
            await GameModule.Scene.LoadSceneAsync(procedureOwner.GetData<string>("NextSceneName"), LoadSceneMode.Single, true);
            GameModule.UI.CloseUI<UI_Wait>();
            // todo: ChangeState<PlayerRoom>(procedureOwner);
        }
    }
}
