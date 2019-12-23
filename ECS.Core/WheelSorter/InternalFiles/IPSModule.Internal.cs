using System.Collections.Generic;
using System.ComponentModel;

using LGCNS.ezControl.Common;
using LGCNS.ezControl.Core;
using ECS.Common;

namespace ECS.Core
{
    /// <summary>
    /// ConveyorServer 요약
    /// </summary>
    public partial class IpsModule
    {
        protected override void DefineInternalVariable()
        {
            base.DefineInternalVariable();

            __INTERNAL_VARIABLE_STRING("EQP_ID", "", enumAccessType.Virtual, false, false, "", "", "설비통신ID");
            __INTERNAL_VARIABLE_STRING("IP", "", enumAccessType.Virtual, false, false, "", "", "설비 통신IP");
            __INTERNAL_VARIABLE_INTEGER("PORT", "", enumAccessType.Virtual, 300000, 0, false, false, 0, "", "설비 통신PORT");
            __INTERNAL_VARIABLE_BOOLEAN("IS_CONNECT", "", enumAccessType.Virtual, false, false, false, "", "연결 상태");
            __INTERNAL_VARIABLE_BOOLEAN("DEBUG", "", enumAccessType.Virtual, false, false, false, "", "DEBUG 모드");
        }
    }
}
