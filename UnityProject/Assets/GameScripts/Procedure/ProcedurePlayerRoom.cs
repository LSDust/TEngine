using System.Collections;
using System.Collections.Generic;
using GameLogic;
using TEngine;
using UnityEngine;

namespace Procedure
{
    public class ProcedurePlayerRoom : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            // AbilityTemplate.LoadTemplatesFromJson().Forget();
        }
    }
}
