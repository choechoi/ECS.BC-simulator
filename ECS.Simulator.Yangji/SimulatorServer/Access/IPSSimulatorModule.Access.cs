using System.Collections.Generic;
using System.ComponentModel;

using LGCNS.ezControl.Core;

namespace ECS.Simulator
{
    /// <summary>
    /// IpsModule의 요약
    /// </summary>
    public partial class IpsSimulatorModule
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
        /// parcelID
        /// </summary>
        [Browsable(false)]
        public CVariable @__parcelID
        {
            get
            {
                return this.Variables["parcelID"];
            }
        }

        /// <summary>
        /// EQP_ID
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
        /// 
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
        public CVariable @__IS_CONNECT
        {
            get
            {
                return this.Variables["IS_CONNECT"];
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
        #endregion

        #region Properties for Variable Access By Value

        /// <summary>
        /// parcelID 값을 생성한다.
        ///  (Access Type : In/Out, Data Type : int)
        /// </summary>
        [Browsable(false)]
        public int parcelID
        {
            get
            {
                return this.Variables["parcelID"].AsInteger;
            }
            set
            {
                this.Variables["parcelID"].AsInteger = value;
            }
        }

        /// <summary>
        /// EQP_ID
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
        /// 
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
        ///  (Access Type : In/Out, Data Type : bool)
        /// </summary>
        [Browsable(false)]
        public bool IS_CONNECT
        {
            get
            {
                return this.Variables["IS_CONNECT"].AsBoolean;
            }
            set
            {
                this.Variables["IS_CONNECT"].AsBoolean = value;
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
        #endregion
    }
}
