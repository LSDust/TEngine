using System.Collections;
using System.Collections.Generic;
using TEngine;

namespace Procedure
{
    public class ProcedurePlaying : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            Log.Debug("完成流程跳转");
        }
    }
}
