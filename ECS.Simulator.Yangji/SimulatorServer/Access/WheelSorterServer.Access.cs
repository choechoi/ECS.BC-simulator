using System.Collections.Generic;
using System.ComponentModel;

using LGCNS.ezControl.Core;

namespace ECS.Simulator
{
    /// <summary>
    /// WheelSorterServer의 요약
    /// </summary>
    public partial class WheelSorterSimulator
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

        [Browsable(false)]
        public CVariable @__TEST_AUTO_START
        {
            get
            {
                return this.Variables["TEST_AUTO_START"];
            }
        }

        [Browsable(false)]
        public CVariable @__TEST_INDUCTION_BCR_SEND
        {
            get
            {
                return this.Variables["TEST_INDUCTION_BCR_SEND"];
            }
        }

        [Browsable(false)]
        public CVariable @__TEST_INDUCTED
        {
            get
            {
                return this.Variables["TEST_INDUCTED"];
            }
        }

        [Browsable(false)]
        public CVariable @__TEST_BCRREAD
        {
            get
            {
                return this.Variables["TEST_BCRREAD"];
            }
        }

        [Browsable(false)]
        public CVariable @__TEST_DISCHARGED
        {
            get
            {
                return this.Variables["TEST_DISCHARGED"];
            }
        }

        [Browsable(false)]
        public CVariable @__TEST_SORTEDCONFIRM
        {
            get
            {
                return this.Variables["TEST_SORTEDCONFIRM"];
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
        ///  (Access Type : In/Out, Data Type : bool)
        /// </summary>
        [Browsable(false)]
        public bool TEST_AUTO_START
        {
            get
            {
                return this.Variables["TEST_AUTO_START"].AsBoolean;
            }
            set
            {
                this.Variables["TEST_AUTO_START"].AsBoolean = value;
            }
        }
  
        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : bool)
        /// </summary>
        [Browsable(false)]
        public bool TEST_INDUCTION_BCR_SEND
        {
            get
            {
                return this.Variables["TEST_INDUCTION_BCR_SEND"].AsBoolean;
            }
            set
            {
                this.Variables["TEST_INDUCTION_BCR_SEND"].AsBoolean = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : bool)
        /// </summary>
        [Browsable(false)]
        public bool TEST_INDUCTED
        {
            get
            {
                return this.Variables["TEST_INDUCTED"].AsBoolean;
            }
            set
            {
                this.Variables["TEST_INDUCTED"].AsBoolean = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : bool)
        /// </summary>
        [Browsable(false)]
        public bool TEST_BCRREAD
        {
            get
            {
                return this.Variables["TEST_BCRREAD"].AsBoolean;
            }
            set
            {
                this.Variables["TEST_BCRREAD"].AsBoolean = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : bool)
        /// </summary>
        [Browsable(false)]
        public bool TEST_DISCHARGED
        {
            get
            {
                return this.Variables["TEST_DISCHARGED"].AsBoolean;
            }
            set
            {
                this.Variables["TEST_DISCHARGED"].AsBoolean = value;
            }
        }

        /// <summary>
        /// 
        ///  (Access Type : In/Out, Data Type : bool)
        /// </summary>
        [Browsable(false)]
        public bool TEST_SORTEDCONFIRM
        {
            get
            {
                return this.Variables["TEST_SORTEDCONFIRM"].AsBoolean;
            }
            set
            {
                this.Variables["TEST_SORTEDCONFIRM"].AsBoolean = value;
            }
        }

        #endregion
    }
}
