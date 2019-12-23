using System.Collections.Generic;
using System.ComponentModel;

using LGCNS.ezControl.Core;
using ECS.Core;

namespace ECS.Core
{
    /// <summary>
    /// WheelSorterServer의 요약
    /// </summary>
    public partial class WheelSorterServer
    {
        /// <summary>
        /// Component의 모든 구성요소가 Instancing완료 되었을 때 호출
        /// </summary>
        protected override void OnInstancing()
        {
            base.OnInstancing();
        }

        #region Properties for Variable Access
        /// <summary>
        /// COMPANY_CD
        /// </summary>
        [Browsable(false)]
        public CVariable @__COMPANY_CD
        {
            get
            {
                return this.Variables["COMPANY_CD"];
            }
        }

        /// <summary>
        /// CENTER_CD
        /// </summary>
        [Browsable(false)]
        public CVariable @__CENTER_CD
        {
            get
            {
                return this.Variables["CENTER_CD"];
            }
        }

        /// <summary>
        /// EQP ID 
        /// </summary>
        [Browsable(false)]
        public CVariable @__EQP_ID
        {
            get
            {
                return this.Variables["EQP_ID"];
            }
        }

        /// <summary>
        /// 실제IP= 
        /// </summary>
        [Browsable(false)]
        public CVariable @__IP
        {
            get
            {
                return this.Variables["IP"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__PORT
        {
            get
            {
                return this.Variables["PORT"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__REJECT_CHUTE
        {
            get
            {
                return this.Variables["REJECT_CHUTE"];
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__DEBUG
        {
            get
            {
                return this.Variables["DEBUG"];
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__SET_CONFIGURATION__ERROR_CHUTE1
        {
            get
            {
                return this.Variables["SET_CONFIGURATION:ERROR_CHUTE1"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__SET_CONFIGURATION__ERROR_CHUTE2
        {
            get
            {
                return this.Variables["SET_CONFIGURATION:ERROR_CHUTE2"];
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__SET_CONFIG_SEND
        {
            get
            {
                return this.Variables["SET_CONFIG_SEND"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__COMMAND__TYPE
        {
            get
            {
                return this.Variables["COMMAND:TYPE"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__COMMAND__UNIT
        {
            get
            {
                return this.Variables["COMMAND:UNIT"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__COMMAND__DATA1
        {
            get
            {
                return this.Variables["COMMAND:DATA1"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__COMMAND__DATA2
        {
            get
            {
                return this.Variables["COMMAND:DATA2"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__COMMAND__DATA3
        {
            get
            {
                return this.Variables["COMMAND:DATA3"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__COMMAND__DATA4
        {
            get
            {
                return this.Variables["COMMAND:DATA4"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__COMMAND__DATA5
        {
            get
            {
                return this.Variables["COMMAND:DATA5"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__COMMAND__REPLY
        {
            get
            {
                return this.Variables["COMMAND:REPLY"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public CVariable @__COMMAND__REASON_CODE
        {
            get
            {
                return this.Variables["COMMAND:REASON_CODE"];
            }
        }

        /// <summary>
        /// SP_
        /// </summary>
        [Browsable(false)]
        public CVariable @__SP_SORTED_CFM
        {
            get
            {
                return this.Variables["SP_SORTED_CFM"];
            }
        }
      
        /// <summary>
        /// SP_IPS_SCAN
        /// </summary>
        [Browsable(false)]
        public CVariable @__SP_IPS_SCAN
        {
            get
            {
                return this.Variables["SP_IPS_SCAN"];
            }
        }        

        /// <summary>
        /// SP_CONFIG 
        /// </summary>
        [Browsable(false)]
        public CVariable @__SP_CONFIG
        {
            get
            {
                return this.Variables["SP_CONFIG"];
            }
        }

        #endregion

        #region Properties for Variable Access By Value
        /// <summary>
        /// COMPANY_CD 
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string COMPANY_CD
        {
            get
            {
                return this.Variables["COMPANY_CD"].AsString;
            }
            set
            {
                this.Variables["COMPANY_CD"].AsString = value;
            }
        }
        /// <summary>
        /// CENTER CD  
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string CENTER_CD
        {
            get
            {
                return this.Variables["CENTER_CD"].AsString;
            }
            set
            {
                this.Variables["CENTER_CD"].AsString = value;
            }
        }

        /// <summary>
        /// EQP ID  
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string EQP_ID
        {
            get
            {
                return this.Variables["EQP_ID"].AsString;
            }
            set
            {
                this.Variables["EQP_ID"].AsString = value;
            }
        }

        /// <summary>
        /// 실재IP= 
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string IP
        {
            get
            {
                return this.Variables["IP"].AsString;
            }
            set
            {
                this.Variables["IP"].AsString = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : int)
        /// </summary>
        [Browsable(false)]
        public int PORT
        {
            get
            {
                return this.Variables["PORT"].AsInteger;
            }
            set
            {
                this.Variables["PORT"].AsInteger = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : int)
        /// </summary>
        [Browsable(false)]
        public int REJECT_CHUTE
        {
            get
            {
                return this.Variables["REJECT_CHUTE"].AsInteger;
            }
            set
            {
                this.Variables["REJECT_CHUTE"].AsInteger = value;
            }
        }       

        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : bool)
        /// </summary>
        [Browsable(false)]
        public bool DEBUG
        {
            get
            {
                return this.Variables["DEBUG"].AsBoolean;
            }
            set
            {
                this.Variables["DEBUG"].AsBoolean = value;
            }
        }       

        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort SET_CONFIGURATION__ERROR_CHUTE1
        {
            get
            {
                return this.Variables["SET_CONFIGURATION:ERROR_CHUTE1"].AsShort;
            }
            set
            {
                this.Variables["SET_CONFIGURATION:ERROR_CHUTE1"].AsShort = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort SET_CONFIGURATION__ERROR_CHUTE2
        {
            get
            {
                return this.Variables["SET_CONFIGURATION:ERROR_CHUTE2"].AsShort;
            }
            set
            {
                this.Variables["SET_CONFIGURATION:ERROR_CHUTE2"].AsShort = value;
            }
        }        

        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : bool)
        /// </summary>
        [Browsable(false)]
        public bool SET_CONFIG_SEND
        {
            get
            {
                return this.Variables["SET_CONFIG_SEND"].AsBoolean;
            }
            set
            {
                this.Variables["SET_CONFIG_SEND"].AsBoolean = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : Out, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort COMMAND__TYPE
        {
            get
            {
                return this.Variables["COMMAND:TYPE"].AsShort;
            }
            set
            {
                this.Variables["COMMAND:TYPE"].AsShort = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : Out, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort COMMAND__UNIT
        {
            get
            {
                return this.Variables["COMMAND:UNIT"].AsShort;
            }
            set
            {
                this.Variables["COMMAND:UNIT"].AsShort = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : Out, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort COMMAND__DATA1
        {
            get
            {
                return this.Variables["COMMAND:DATA1"].AsShort;
            }
            set
            {
                this.Variables["COMMAND:DATA1"].AsShort = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : Out, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort COMMAND__DATA2
        {
            get
            {
                return this.Variables["COMMAND:DATA2"].AsShort;
            }
            set
            {
                this.Variables["COMMAND:DATA2"].AsShort = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : Out, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort COMMAND__DATA3
        {
            get
            {
                return this.Variables["COMMAND:DATA3"].AsShort;
            }
            set
            {
                this.Variables["COMMAND:DATA3"].AsShort = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : Out, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort COMMAND__DATA4
        {
            get
            {
                return this.Variables["COMMAND:DATA4"].AsShort;
            }
            set
            {
                this.Variables["COMMAND:DATA4"].AsShort = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : Out, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort COMMAND__DATA5
        {
            get
            {
                return this.Variables["COMMAND:DATA5"].AsShort;
            }
            set
            {
                this.Variables["COMMAND:DATA5"].AsShort = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : Out, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort COMMAND__REPLY
        {
            get
            {
                return this.Variables["COMMAND:REPLY"].AsShort;
            }
            set
            {
                this.Variables["COMMAND:REPLY"].AsShort = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : In, Data Type : ushort)
        /// </summary>
        [Browsable(false)]
        public ushort COMMAND__REASON_CODE
        {
            get
            {
                return this.Variables["COMMAND:REASON_CODE"].AsShort;
            }
        }


        /// <summary>
        /// SP_INDUCTED_BCR 
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string SP_INDUCTED_BCR
        {
            get
            {
                return this.Variables["SP_INDUCTED_BCR"].AsString;
            }
            set
            {
                this.Variables["SP_INDUCTED_BCR"].AsString = value;
            }
        }

        /// <summary>
        /// SP_INDUCTED 
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string SP_INDUCTED
        {
            get
            {
                return this.Variables["SP_INDUCTED"].AsString;
            }
            set
            {
                this.Variables["SP_INDUCTED"].AsString = value;
            }
        }

        /// <summary>
        /// SP_DISCHARED 
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string SP_DISCHARED
        {
            get
            {
                return this.Variables["SP_DISCHARED"].AsString;
            }
            set
            {
                this.Variables["SP_DISCHARED"].AsString = value;
            }
        }

        /// <summary>
        /// SP_SORTED_CFM 
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string SP_SORTED_CFM
        {
            get
            {
                return this.Variables["SP_SORTED_CFM"].AsString;
            }
            set
            {
                this.Variables["SP_SORTED_CFM"].AsString = value;
            }
        }

        /// <summary>
        /// SP_IPS_SCAN
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string SP_IPS_SCAN
        {
            get
            {
                return this.Variables["SP_IPS_SCAN"].AsString;
            }
            set
            {
                this.Variables["SP_IPS_SCAN"].AsString = value;
            }
        }

        /// <summary>
        /// SP_IPS_SCAN
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string SP_KIOSK_SCAN
        {
            get
            {
                return this.Variables["SP_KIOSK_SCAN"].AsString;
            }
            set
            {
                this.Variables["SP_KIOSK_SCAN"].AsString = value;
            }
        }

        /// <summary>
        /// SP_CONFIG
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string SP_CONFIG
        {
            get
            {
                return this.Variables["SP_CONFIG"].AsString;
            }
            set
            {
                this.Variables["SP_CONFIG"].AsString = value;
            }
        }

        /// <summary>
        /// SP_INDCUTION_ERROR
        ///  (Access Type : In/Out, Data Type : string)
        /// </summary>
        [Browsable(false)]
        public string SP_INDCUTION_ERROR
        {
            get
            {
                return this.Variables["SP_INDCUTION_ERROR"].AsString;
            }
            set
            {
                this.Variables["SP_INDCUTION_ERROR"].AsString = value;
            }
        }
        #endregion
    }
}
