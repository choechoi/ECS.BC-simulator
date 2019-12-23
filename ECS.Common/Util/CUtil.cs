using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

using System.Data;
using System.Runtime.InteropServices;

namespace ECS.Common 
{
    public class CUtil
    {
        static Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static int GetInt(string s)
        {
            const int DEFAULT = 0;
            int result;
            if (int.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return DEFAULT;
            }
        }
        public static List<short> IntToShortList(int A)
        {
            List<short> listShort = new List<short>();
            byte[] B = BitConverter.GetBytes(A);

            listShort.Add(BitConverter.ToInt16(B, 0));
            listShort.Add(BitConverter.ToInt16(B, 2));
            return listShort;
        }

        public static int ShortMergeToInt(short A, short B)
        {
            int result = 0;
            byte[] bytefromshortA = BitConverter.GetBytes(A);
            byte[] bytefromshortB = BitConverter.GetBytes(B);
            byte[] all = { bytefromshortA[0], bytefromshortA[1], bytefromshortB[0], bytefromshortB[1] };
            result = BitConverter.ToInt32(all, 0);

            return result;
        }

        public static string ByteToString(byte[] bytes)
        {
            return new string(Encoding.Default.GetChars(bytes));
        }

        public static string GetASCII(int Hex)
        {
            string asciicode;
            if (Hex < 10)
            {
                asciicode = Hex.ToString();
            }
            else
            {
                switch(Hex)
                {
                    case 10:
                        asciicode = "A";
                        break;
                    case 11:
                        asciicode = "B";
                        break;
                    case 12:
                        asciicode = "C";
                        break;
                    default:
                        asciicode = "0";
                        break;
                }
            }

            return asciicode;
        }

        public static string GetClientID()
        {
            foreach (KeyValueConfigurationElement setting in _config.AppSettings.Settings)
            {
                if (setting.Key.Equals(ClientConstant.AppConfig.ClientID))
                {
                    return setting.Value.ToString();
                }
            }

            return string.Empty;
        }

        public static string GetConfigValue(String type)
        {
            string value = string.Empty;
            foreach (KeyValueConfigurationElement setting in _config.AppSettings.Settings)
            {
                if (setting.Key.Equals(type))
                {
                    value = setting.Value;
                }
            }

            return value;
        }

        public static double GetDNumber(String strNum)
        {
            double num = 0.000;

            double.TryParse(strNum, out num);

            return num;
        }

        public static string SetPoint(string value , string point)
        {
            try
            {
                double value1 = Convert.ToDouble(value);
                double point1 = 1;
                double result1 = 0;

                point1 = Math.Pow(10, Convert.ToDouble(point));

                result1 = value1 / point1;

                return result1.ToString();
            }
            catch(Exception ex)
            {
                return "0";
            }
        }

        public static int GetNumber(String strNum)
        {
            int num = 0;

            int.TryParse(strNum, out num);

            return num;
        }

        public static int GetNumber999(String strNum)
        {
            int num = 0;

            int.TryParse(strNum, out num);

            if(num > 999)
            {
                num = 999;
            }

            return num;
        }

        public static short GetShortNumber(string _strNum)
        {
            short num = 0;

            short.TryParse(_strNum, out num);

            return num;
        }        
        
        public static int GetNumber(char B, char A)
        {
            String strNum = B.ToString() + A.ToString();

            int num = 0;
            int.TryParse(strNum, out num);

            return num;
        }

        // unsigned 때문에 문제가 생김. 내부 용도로만 사용
        private static short CharToShort(char B, char A)
        {
            byte Ba = Convert.ToByte(B);
            byte Aa = Convert.ToByte(A);

            int num16 = (Ba << 8) | Aa;

            return (short)num16;

        }

        public static short BitCipherToShort(bool D, bool C, bool B, bool A)
        {
            if (D)
            {
                return 3;
            }
            else if (C)
            {
                return 2;
            }
            else if (B)
            {
                return 1;
            }

            return 0;
        }

        public static short BitToShort(bool B, bool A)
        {
            int lower = A ? 1 : 0;
            int high = B ? 1 : 0;

            int num16 = (high << 1) | lower;

            return (short)num16;
        }

        public static short ByteToShort(byte B, byte A)
        {
            int num16 = (B << 8) | A;

            return (short) num16;
        }

        public static List<short> StringToShortList(String strOriData)
        {
            List<short> listShort = new List<short>();

            if (strOriData.Length % 2 != 0)
                return listShort;

            for (int i = 0; i < strOriData.Length; i += 2)
            {
                short item = CharToShort(strOriData[i + 1], strOriData[i]);
                listShort.Add(item);
            }

            return listShort;
        }

        public static List<byte> StringToUTF8ByteList(string strOriData)
        {
            List<byte> listbyte = new List<byte>();

            byte[] strMessage = Encoding.UTF8.GetBytes(strOriData);
            foreach (byte element in strMessage)
            {
                listbyte.Add(element);
            }           

            return listbyte;
        }

        public static List<byte> StringToByteList(String strOriData)
        {
            List<byte> listbyte = new List<byte>();

            for (int i = 0; i < strOriData.Length; i ++)
            {
                byte item = Convert.ToByte(strOriData[i]);
                listbyte.Add(item);
            }

            return listbyte;
        }

        public static List<short> ByteToShortList(byte[] bOriData)
        {
            List<short> listShort = new List<short>();

            if (bOriData.Length % 2 != 0)
                return listShort;

            for (int i = 0; i < bOriData.Length; i += 2)
            {
                short item = ByteToShort(bOriData[i + 1], bOriData[i]);
                listShort.Add(item);
            }

            return listShort;
        }


        public static byte[] ShortToByte(short oriData)
        {
            byte[] byteData = new byte[2];

            byteData[1] = (byte)(oriData >> 8);
            byteData[0] = (byte)(oriData & 0xff);

            return byteData;
        }

        public static String ShortToASCII(short oriData)
        {
            byte[] byteData = new byte[2];

            byteData[1] = (byte)(oriData >> 8);
            byteData[0] = (byte)(oriData & 0xff);

            return GetASCII(byteData[0]) + GetASCII(byteData[1]);
        }

        public static String ShortlistToASCII(List<short> listShort, int ASCIILength)
        {
            try
            {
                byte[] byteData = new byte[ASCIILength * 2];

                string ret = string.Empty;

                // 2 바이트 이후부터 특정 자리수까지 변환 
                for (int i = 0; i < ASCIILength; i++)
                {
                    byteData[i * 2 + 1] = (byte)(listShort[i] >> 8);
                    byteData[i * 2] = (byte)(listShort[i] & 0xff);
                }
                ret = System.Text.Encoding.ASCII.GetString(byteData);
                return ret;
            }
            catch(Exception ex)
            {
                return "BCRDATAERR";
            }
        }

        public static String ShortlistToASCII(List<short> listShort, int ASCIILength, int bcrLength, int startIndex)
        {
            try
            {
                if(bcrLength > ASCIILength)
                {
                    bcrLength = ASCIILength;
                }

                byte[] byteData = new byte[ASCIILength * 2];

                string ret = string.Empty;

                // 2 바이트 이후부터 특정 자리수까지 변환 
                for (int i = 0; i < ASCIILength; i++)
                {
                    if (listShort.Count - startIndex/2 == i)
                    {
                        break;
                    }
                    byteData[i * 2 + 1] = (byte)(listShort[i+ startIndex/2] >> 8);
                    byteData[i * 2] = (byte)(listShort[i + startIndex/2] & 0xff);

                }
                ret = System.Text.Encoding.ASCII.GetString(byteData);
                return ret;
            }
            catch (Exception ex)
            {
                return "BCRDATAERR";
            }
        }

        public static string ShortlistToASCII(List<short> listShort, int bcrLength, int startIndex)
        {
            try
            {                
                byte[] byteData = new byte[bcrLength * 2];

                string ret = string.Empty;

                // 2 바이트 이후부터 특정 자리수까지 변환 
                for (int i = 0; i < bcrLength; i++)
                {
                    if (listShort.Count - startIndex / 2 == i)
                    {
                        break;
                    }
                    byteData[i * 2 + 1] = (byte)(listShort[i + startIndex] >> 8);
                    byteData[i * 2] = (byte)(listShort[i + startIndex] & 0xff);

                }
                ret = System.Text.Encoding.ASCII.GetString(byteData);
                return ret;
            }
            catch (Exception ex)
            {
                return "BCRDATAERR";
            }
        }

        public static string ShortlistToUtf8String(List<short> listShort, int bcrLength, int startIndex)
        {
            try
            {
                byte[] byteData = new byte[bcrLength * 2];

                string ret = string.Empty;

                // 2 바이트 이후부터 특정 자리수까지 변환 
                for (int i = 0; i < bcrLength; i++)
                {
                    if (listShort.Count - startIndex / 2 == i)
                    {
                        break;
                    }
                    byteData[i * 2 + 1] = (byte)(listShort[i + startIndex] >> 8);
                    byteData[i * 2] = (byte)(listShort[i + startIndex] & 0xff);

                }
                ret = System.Text.Encoding.UTF8.GetString(byteData);
                return ret;
            }
            catch (Exception ex)
            {
                return "BCRDATAERR";
            }
        }

        public static byte[] ShortListToByte(List<short> listShort)
        {
            byte[] byteData = new byte[listShort.Count * 2];

            for (int i = 0; i < listShort.Count; i++)
            {
                byteData[i * 2 + 1] = (byte)(listShort[i] >> 8);
                byteData[i * 2] = (byte)(listShort[i] & 0xff);
            }

            return byteData;
        }

        public static String StreamToString(byte[] buffer)
        {
            StringBuilder _debug = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                _debug.Append(buffer[i].ToString() + " ");
            }

            return _debug.ToString();
        }

        public static object ByteToStructure(byte[] data, Type type)
        {
            IntPtr buff = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, buff, data.Length);

            object obj = Marshal.PtrToStructure(buff, type);
            Marshal.FreeHGlobal(buff);

            if (Marshal.SizeOf(obj) != data.Length)
            {
                return null;
            }

            return obj;
        }

        public static byte[] StructureToByte(object obj)
        {
            int datasize = Marshal.SizeOf(obj);

            IntPtr buff = Marshal.AllocHGlobal(datasize);
            Marshal.StructureToPtr(obj, buff, false);
            byte[] data = new byte[datasize];

            Marshal.Copy(buff, data, 0, datasize);
            Marshal.FreeHGlobal(buff);

            return data;
        }

        public static String GetInsertSQLString(DataRow row)
        {
            StringBuilder sColumn = new StringBuilder();
            StringBuilder sValue = new StringBuilder();

            DataTable table = row.Table;

            int count = 0;
            foreach(DataColumn column in table.Columns)
            {
                String name = column.ColumnName;
                String value = row[name].ToString();

                if (count > 0)
                {
                    sColumn.Append(",");
                    sValue.Append(",");
                }

                sColumn.Append(name);

                if(value.GetType() == typeof(string))
                {
                    sValue.Append("'" + value + "'");
                }
                else
                {
                    sValue.Append(value);
                }

                count++;
            }

            return String.Format("INSERT INTO {0} ({1}) VALUES ({2})", table.TableName, sColumn.ToString(), sValue.ToString() );
        }

        public static bool ChangeColumnDataType(DataTable table, string columnname, Type newtype)
        {
            if (table.Columns.Contains(columnname) == false)
                return false;

            DataColumn column = table.Columns[columnname];
            if (column.DataType == newtype)
                return true;

            try
            {
                DataColumn newcolumn = new DataColumn("temperary", newtype);
                table.Columns.Add(newcolumn);
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        row["temperary"] = Convert.ChangeType(row[columnname], newtype);
                    }
                    catch
                    {
                    }
                }
                table.Columns.Remove(columnname);
                newcolumn.ColumnName = columnname;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static int PatternAt(byte[] source, byte[] pattern)
        {
            try
            {
                if(pattern == null)
                {
                    return -1;
                }

                for (int i = 0; i < source.Length; i++)
                {
                    if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                    {
                        return i;
                    }
                }

                return -1;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }
    }
}
