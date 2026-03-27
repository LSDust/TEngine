using System.Collections;
using System.Collections.Generic;
using GameLogic;
using TEngine;
using UnityEngine;

namespace Procedure
{
    public class ProcedureMainMenu : ProcedureBase
    {
        public override bool UseNativeDialog { get; }
        private IFsm<IProcedureModule> _procedureOwner;

        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameModule.UI.ShowUIAsync<UI_MainMenu>();
            this._procedureOwner = procedureOwner;
            
            GameEvent.AddEventListener("ReadPersistentInfo", ReadPersistentInfo);
        }

        protected override void OnLeave(IFsm<IProcedureModule> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameModule.UI.CloseUI<UI_MainMenu>();
            GameEvent.RemoveEventListener("ReadPersistentInfo", ReadPersistentInfo);
        }

        private void ReadPersistentInfo()
        {
            // _procedureOwner.SetData("NextProcedure", typeof(ProcedureBattle));
            _procedureOwner.SetData("NextSceneName", "PlayerRoom");
            ChangeState<ProcedureReadDataLoadScene>(_procedureOwner);
        }
    }
}
