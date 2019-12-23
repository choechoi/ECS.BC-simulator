using LGCNS.ezControl.Diagnostics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Common
{
    public class CSorterItemData
    {
        public short pid;

        public string eqpId;
        public string inductedTime; // Unique KEY              

        /// <summary>
        /// IPS Read
        /// </summary>
        public string ipsData;
        public List<short> Ldestination;
        public short destination1;
        public short destination2;
        public short destination3;
        public short destination4;
        public short destination5;        

        /// <summary>
        ///  Discharged
        /// </summary>
        public short dischargedChuteNumber;

        /// <summary>
        ///  SortedConfirm
        /// </summary>
        public short sortedConfirmedChuteNumber;
        public short reasonCode;
        public short sensorYN; 

        /// <summary>
        /// Option
        /// </summary>        
        public short option1;
        public short option2;

        public CSorterItemData()
        {
            eqpId = string.Empty;
            inductedTime = string.Empty; // Unique KEY            
            pid = 0;
            
            ipsData = string.Empty;
            Ldestination = new List<short> { 0, 0, 0, 0, 0 };
            destination1 = 0;
            destination2 = 0;
            destination3 = 0;
            destination4 = 0;
            destination5 = 0;
           
            sortedConfirmedChuteNumber = 0;            
            reasonCode = 0;
            sensorYN = 0; 

            option1 = 0;
            option2 = 0;
        }
    }

    public class CSorterItemDataManager
    {
        ConcurrentDictionary<int, CSorterItemData> dconveyorItemData;

        public CSorterItemDataManager()
        {
            dconveyorItemData = new ConcurrentDictionary<int, CSorterItemData>();
            CSorterItemData item = new CSorterItemData();
        }

        public CSorterItemData GetItem(int _pid)
        {
            try
            {
                if (dconveyorItemData.ContainsKey(_pid))
                {
                    return dconveyorItemData[_pid];
                }
                return null;
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex, "GetItem");
                return null;
            }            
        }

        public bool Remove(CSorterItemData _item, int _pid)
        {
            if (dconveyorItemData.ContainsKey(_pid) == false)
            {
                return true;
            }
            dconveyorItemData.TryRemove(_pid, out _item);
            return true;
        }

        public void OldDataProcess(int _pid)
        {
            if (dconveyorItemData.ContainsKey(_pid))
            {
                CSorterItemData item = GetItem(_pid);
                Remove(item, _pid);
            }
        }

        public bool Add(CSorterItemData _item, int _pid)
        {
            try
            {
                if (dconveyorItemData.ContainsKey(_pid))
                {
                    SystemLogger.Log(Level.Debug, "Data Add Conflict " + _pid);
                    return false;
                }
                else
                {
                    dconveyorItemData.TryAdd(_pid, _item);
                }
                return true;
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex, "CSorterItemDataMgr");
                return false;
            }
        }
    }


}
