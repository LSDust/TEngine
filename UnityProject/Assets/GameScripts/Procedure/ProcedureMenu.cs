using System.Collections;
using System.Collections.Generic;
using GameLogic;
using TEngine;
using UnityEngine;

namespace Procedure
{
    public class ProcedureMenu : ProcedureBase
    {
        public override bool UseNativeDialog { get; }
        private IFsm<IProcedureModule> _procedureOwner;

        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameModule.UI.ShowUIAsync<UI_Menu>();
            this._procedureOwner = procedureOwner;
            
            GameEvent.AddEventListener("ReadPersistentInfo", ReadPersistentInfo);
        }

        protected override void OnLeave(IFsm<IProcedureModule> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameModule.UI.CloseUI<UI_Menu>();
            GameEvent.RemoveEventListener("ReadPersistentInfo", ReadPersistentInfo);
        }

        private async void ReadPersistentInfo()
        {
            try
            {
                await GameModule.Scene.LoadSceneWithProgressBarAsync("PersistentInfo");
                ChangeState<ProcedureReadPersistentInfo>(_procedureOwner);
            }
            catch (System.Exception e)
            {
                Log.Error($"加载场景失败: {e.Message}");
            }
        }
    }
}
