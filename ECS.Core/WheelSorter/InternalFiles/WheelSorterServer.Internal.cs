using System.Collections.Generic;
using System.ComponentModel;

using LGCNS.ezControl.Common;
using LGCNS.ezControl.Core;
using ECS.Common;

namespace ECS.Core
{
    /// <summary>
    /// WheelSorterServer 요약
    /// </summary>
    public partial class WheelSorterServer
    {
        protected override void DefineInternalVariable()
        {
            base.DefineInternalVariable();

            __INTERNAL_VARIABLE_STRING("COMPANY_CD", "", enumAccessType.Virtual, false, false, "", "", "회사 CD");
            __INTERNAL_VARIABLE_STRING("CENTER_CD", "", enumAccessType.Virtual, false, false, "", "", "센터ID");
            __INTERNAL_VARIABLE_STRING("EQP_ID", "", enumAccessType.Virtual, false, false, "", "", "설비통신ID");
            __INTERNAL_VARIABLE_STRING("IP", "", enumAccessType.Virtual, false, false, "", "", "설비 통신IP");
            __INTERNAL_VARIABLE_INTEGER("PORT", "", enumAccessType.Virtual, 300000, 0, false, false, 0, "", "설비 통신PORT");
            __INTERNAL_VARIABLE_INTEGER("REJECT_CHUTE", "", enumAccessType.Virtual, 300000, 0, false, false, 0, "", "Reject Chute");
            __INTERNAL_VARIABLE_BOOLEAN("DEBUG", "", enumAccessType.Virtual, false, false, false, "", "DEBUG 모드");

            __INTERNAL_VARIABLE_SHORT("ERROR_CHUTE1", "SET_CONFIGURATION", enumAccessType.Virtual, 30000, 0, false, false, 0, "", "ErrorChute1");
            __INTERNAL_VARIABLE_SHORT("ERROR_CHUTE2", "SET_CONFIGURATION", enumAccessType.Virtual, 30000, 0, false, false, 0, "", "ErrorChute2");
            __INTERNAL_VARIABLE_BOOLEAN("SET_CONFIG_SEND", "", enumAccessType.Virtual, false, false, false, "", "Set Configuration");


            __INTERNAL_VARIABLE_SHORT("TYPE", "COMMAND", enumAccessType.Out, 10000, 0, false, false, 0, "ADDRESS_NO=0020, DEVICE_TYPE=W", "");
            __INTERNAL_VARIABLE_SHORT("UNIT", "COMMAND", enumAccessType.Out, 10000, 0, false, false, 0, "ADDRESS_NO=0021, DEVICE_TYPE=W", "");
            __INTERNAL_VARIABLE_SHORT("DATA1", "COMMAND", enumAccessType.Out, 10000, 0, false, false, 0, "ADDRESS_NO=0022, DEVICE_TYPE=W", "");
            __INTERNAL_VARIABLE_SHORT("DATA2", "COMMAND", enumAccessType.Out, 10000, 0, false, false, 0, "ADDRESS_NO=0023, DEVICE_TYPE=W", "");
            __INTERNAL_VARIABLE_SHORT("DATA3", "COMMAND", enumAccessType.Out, 10000, 0, false, false, 0, "ADDRESS_NO=0024, DEVICE_TYPE=W", "");
            __INTERNAL_VARIABLE_SHORT("DATA4", "COMMAND", enumAccessType.Out, 10000, 0, false, false, 0, "ADDRESS_NO=0025, DEVICE_TYPE=W", "");
            __INTERNAL_VARIABLE_SHORT("DATA5", "COMMAND", enumAccessType.Out, 10000, 0, false, false, 0, "ADDRESS_NO=0026, DEVICE_TYPE=W", "");
            __INTERNAL_VARIABLE_SHORT("REPLY", "COMMAND", enumAccessType.Out, 10000, 0, false, false, 0, "ADDRESS_NO=0020, DEVICE_TYPE=W", "");

            __INTERNAL_VARIABLE_SHORT("REASON_CODE", "COMMAND", enumAccessType.In, 100, 0, true, false, 0, "ADDRESS_NO=11e0, DEVICE_TYPE=W", "");

            __INTERNAL_VARIABLE_STRING("SP_SORTED_CFM", "", enumAccessType.Virtual, false, false, "", "", "Item Sorted Confirm");
            __INTERNAL_VARIABLE_STRING("SP_IPS_SCAN", "", enumAccessType.Virtual, false, false, "", "", "Item BCR Scan");
            __INTERNAL_VARIABLE_STRING("SP_CONFIG", "", enumAccessType.Virtual, false, false, "", "", "소터 옵션 설정 신호");
           
        }
    }
}
