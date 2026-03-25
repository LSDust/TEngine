using System.Collections;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;
using TEngine;

namespace Procedure
{
    public class ProcedureReadPersistentInfo : ProcedureBase
    {
        public override bool UseNativeDialog { get; }
        
        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            // GameModule.UI.ShowUI<UI_Battle>();
        }

        protected override void OnLeave(IFsm<IProcedureModule> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            // GameModule.UI.CloseUI<UI_Battle>();
        }
    }
}
