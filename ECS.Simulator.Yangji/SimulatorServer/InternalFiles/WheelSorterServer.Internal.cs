using System.Collections.Generic;
using System.ComponentModel;

using LGCNS.ezControl.Common;
using LGCNS.ezControl.Core;
using ECS.Common;

namespace ECS.Simulator
{
    /// <summary>
    /// WheelSorterServer 요약
    /// </summary>
    public partial class WheelSorterSimulator
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

            __INTERNAL_VARIABLE_SHORT("RECIRCULATION", "SET_CONFIGURATION", enumAccessType.Virtual, 30000, 0, false, false, 0, "", "Max Recirculation Count");
            __INTERNAL_VARIABLE_SHORT("OVERFLOW_CHUTE1", "SET_CONFIGURATION", enumAccessType.Virtual, 30000, 0, false, false, 0, "", "OverflowChute1");
            __INTERNAL_VARIABLE_SHORT("OVERFLOW_CHUTE2", "SET_CONFIGURATION", enumAccessType.Virtual, 30000, 0, false, false, 0, "", "OverflowChute2");
            __INTERNAL_VARIABLE_SHORT("ERROR_CHUTE1", "SET_CONFIGURATION", enumAccessType.Virtual, 30000, 0, false, false, 0, "", "ErrorChute1");
            __INTERNAL_VARIABLE_SHORT("ERROR_CUTE2", "SET_CONFIGURATION", enumAccessType.Virtual, 30000, 0, false, false, 0, "", "ErrorChute2");
            __INTERNAL_VARIABLE_BOOLEAN("SET_CONFIG_SEND", "", enumAccessType.Virtual, false, false, false, "", "Set Configuration");

            __INTERNAL_VARIABLE_BOOLEAN("TEST_AUTO_START", "", enumAccessType.Virtual, false, false, false, "", "AUTO START TEST");
            __INTERNAL_VARIABLE_BOOLEAN("TEST_INDUCTION_BCR_SEND", "", enumAccessType.Virtual, false, false, false, "", "START TEST");
            __INTERNAL_VARIABLE_BOOLEAN("TEST_INDUCTED", "", enumAccessType.Virtual, false, false, false, "", "START TEST");
            __INTERNAL_VARIABLE_BOOLEAN("TEST_BCRREAD", "", enumAccessType.Virtual, false, false, false, "", " TEST");
            __INTERNAL_VARIABLE_BOOLEAN("TEST_DISCHARGED", "", enumAccessType.Virtual, false, false, false, "", " TEST");
            __INTERNAL_VARIABLE_BOOLEAN("TEST_SORTEDCONFIRM", "", enumAccessType.Virtual, false, false, false, "", " TEST");
            
        }
    }
}
