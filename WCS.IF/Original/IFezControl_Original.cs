/*----------------------------------------------------------------------
 * IFezControl : WCS와 ezControl간 인터페이스를 위한 클래스
 * 2016.06.20. : IHLee, 최초 프로그램 작성
 *----------------------------------------------------------------------
 */

using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace WCS.IF
{
    public class IFezControl
    {
        private static string _connectionString = "Data Source = (DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.10.100)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE))); User ID = SYSTEM; Password = lgcnswcs";
        //private static string _returnFlag;

        public static void SP_WCS_PCS_INDUCTION(string P_INDUCT_DATE, string P_EQUIP_ID, decimal P_PRODUCT_ID, decimal P_INDUCT_NO, decimal P_INDUCT_MODE, decimal P_DEST_CHUTE_NO, decimal P_CART_NO)
        {
            string procedureName = "SP_WCS_PCS_INDUCTION";

            using (OracleConnection oracleConnection = new OracleConnection(_connectionString))
            {
                oracleConnection.Open();

                OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);

                oracleCommand.CommandType = CommandType.StoredProcedure;

                oracleCommand.Parameters.Clear();
                oracleCommand.Parameters.Add("P_INDUCT_DATE", OracleDbType.Varchar2, 32767).Value = P_INDUCT_DATE;
                oracleCommand.Parameters.Add("P_EQUIP_ID", OracleDbType.Varchar2, 32767).Value = P_EQUIP_ID;
                oracleCommand.Parameters.Add("P_PRODUCT_ID", OracleDbType.Decimal).Value = P_PRODUCT_ID;
                oracleCommand.Parameters.Add("P_INDUCT_NO", OracleDbType.Decimal).Value = P_INDUCT_NO;
                oracleCommand.Parameters.Add("P_INDUCT_MODE", OracleDbType.Decimal).Value = P_INDUCT_MODE;
                oracleCommand.Parameters.Add("P_DEST_CHUTE_NO", OracleDbType.Decimal).Value = P_DEST_CHUTE_NO;
                oracleCommand.Parameters.Add("P_CART_NO", OracleDbType.Decimal).Value = P_CART_NO;

                oracleCommand.Parameters.Add("P_RTN_FLAG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_ERR_MSG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;

                oracleCommand.ExecuteNonQuery();

                oracleConnection.Close();

                InductionList.SetValue("P_RTN_FLAG", oracleCommand.Parameters["P_RTN_FLAG"].Value.ToString());
                InductionList.SetValue("P_ERR_MSG", oracleCommand.Parameters["P_ERR_MSG"].Value.ToString());
            }
        }

        public static void SP_WCS_PCS_BARCODE_SCAN(string P_INDUCT_DATE, decimal P_PRODUCT_ID, int P_SCAN_TYPE, string P_BARCODE, int P_CART_NO, int P_BCR_LOC)
        {
            string procedureName = "SP_WCS_PCS_BARCODE_SCAN";

            using (OracleConnection oracleConnection = new OracleConnection(_connectionString))
            {
                oracleConnection.Open();

                OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);

                oracleCommand.CommandType = CommandType.StoredProcedure;

                oracleCommand.Parameters.Clear();
                oracleCommand.Parameters.Add("P_INDUCT_DATE", OracleDbType.Varchar2, 32767).Value = P_INDUCT_DATE;
                oracleCommand.Parameters.Add("P_PRODUCT_ID", OracleDbType.Decimal).Value = P_PRODUCT_ID;
                oracleCommand.Parameters.Add("P_SCAN_TYPE", OracleDbType.Int32, 32767).Value = P_SCAN_TYPE;
                oracleCommand.Parameters.Add("P_BARCODE", OracleDbType.Varchar2, 32767).Value = P_BARCODE;
                oracleCommand.Parameters.Add("P_CART_NO", OracleDbType.Int32).Value = P_CART_NO;
                oracleCommand.Parameters.Add("P_BCR_LOC", OracleDbType.Int32).Value = P_BCR_LOC;

                oracleCommand.Parameters.Add("P_RTN_FLAG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_ERR_MSG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_DEST_CHUTE", OracleDbType.Int32).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_LAST_FLAG", OracleDbType.Int32).Direction = ParameterDirection.Output;

                oracleCommand.ExecuteNonQuery();

                oracleConnection.Close();

                /*
                return _returnFlag = oracleCommand.Parameters["P_RTN_FLAG"].Value.ToString()
                    + oracleCommand.Parameters["P_ERR_MSG"].Value.ToString()
                    + oracleCommand.Parameters["P_DEST_CHUTE"].Value.ToString()
                    + oracleCommand.Parameters["P_LAST_FLAG"].Value.ToString();
                */
                BarcodeList.SetValue("P_RTN_FLAG", oracleCommand.Parameters["P_RTN_FLAG"].Value.ToString());
                BarcodeList.SetValue("P_ERR_MSG", oracleCommand.Parameters["P_ERR_MSG"].Value.ToString());
                BarcodeList.SetValue("P_DEST_CHUTE", oracleCommand.Parameters["P_DEST_CHUTE"].Value.ToString());
                BarcodeList.SetValue("P_LAST_FLAG", oracleCommand.Parameters["P_LAST_FLAG"].Value.ToString());
            }
        }

        public static void SP_WCS_PCS_DISCHARGE(string P_INDUCT_DATE, decimal P_PRODUCT_ID, int P_CART_NO, int P_ACTUAL_CHUTE_NO, int P_INDUCT_NO, int P_INDUCT_MODE, int P_RECIRCLE_COUNT)
        {
            string procedureName = "SP_WCS_PCS_DISCHARGE";

            using (OracleConnection oracleConnection = new OracleConnection(_connectionString))
            {
                oracleConnection.Open();

                OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);

                oracleCommand.CommandType = CommandType.StoredProcedure;

                oracleCommand.Parameters.Clear();
                oracleCommand.Parameters.Add("P_INDUCT_DATE", OracleDbType.Varchar2, 32767).Value = P_INDUCT_DATE;
                oracleCommand.Parameters.Add("P_PRODUCT_ID", OracleDbType.Decimal).Value = P_PRODUCT_ID;
                oracleCommand.Parameters.Add("P_CART_NO", OracleDbType.Int32).Value = P_CART_NO;
                oracleCommand.Parameters.Add("P_ACTUAL_CHUTE_NO", OracleDbType.Int32).Value = P_ACTUAL_CHUTE_NO;
                oracleCommand.Parameters.Add("P_INDUCT_NO", OracleDbType.Int32).Value = P_INDUCT_NO;
                oracleCommand.Parameters.Add("P_INDUCT_MODE", OracleDbType.Int32).Value = P_INDUCT_MODE;
                oracleCommand.Parameters.Add("P_RECIRCLE_COUNT", OracleDbType.Int32).Value = P_RECIRCLE_COUNT;

                oracleCommand.Parameters.Add("P_RTN_FLAG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_ERR_MSG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;

                oracleCommand.ExecuteNonQuery();

                oracleConnection.Close();

                DischargeList.SetValue("P_RTN_FLAG", oracleCommand.Parameters["P_RTN_FLAG"].Value.ToString());
                DischargeList.SetValue("P_ERR_MSG", oracleCommand.Parameters["P_ERR_MSG"].Value.ToString());
            }
        }

        public static void SP_WCS_PCS_SORTED_CFRM(string P_INDUCT_DATE, decimal P_PRODUCT_ID, int P_CART_NO, int P_ACTUAL_CHUTE_NO, int P_INDUCT_NO, int P_INDUCT_MODE, int P_RECIRCLE_COUNT, int P_REASON_CODE)
        {
            string procedureName = "SP_WCS_PCS_SORTED_CFRM";

            using (OracleConnection oracleConnection = new OracleConnection(_connectionString))
            {
                oracleConnection.Open();

                OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);

                oracleCommand.CommandType = CommandType.StoredProcedure;

                oracleCommand.Parameters.Clear();
                oracleCommand.Parameters.Add("P_INDUCT_DATE", OracleDbType.Varchar2, 32767).Value = P_INDUCT_DATE;
                oracleCommand.Parameters.Add("P_PRODUCT_ID", OracleDbType.Decimal).Value = P_PRODUCT_ID;
                oracleCommand.Parameters.Add("P_CART_NO", OracleDbType.Int32).Value = P_CART_NO;
                oracleCommand.Parameters.Add("P_ACTUAL_CHUTE_NO", OracleDbType.Int32).Value = P_ACTUAL_CHUTE_NO;
                oracleCommand.Parameters.Add("P_INDUCT_NO", OracleDbType.Int32).Value = P_INDUCT_NO;
                oracleCommand.Parameters.Add("P_INDUCT_MODE", OracleDbType.Int32).Value = P_INDUCT_MODE;
                oracleCommand.Parameters.Add("P_RECIRCLE_COUNT", OracleDbType.Int32).Value = P_RECIRCLE_COUNT;
                oracleCommand.Parameters.Add("P_REASON_CODE", OracleDbType.Int32).Value = P_REASON_CODE;

                oracleCommand.Parameters.Add("P_RTN_FLAG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_ERR_MSG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;

                oracleCommand.ExecuteNonQuery();

                oracleConnection.Close();

                SortedList.SetValue("P_RTN_FLAG", oracleCommand.Parameters["P_RTN_FLAG"].Value.ToString());
                SortedList.SetValue("P_ERR_MSG", oracleCommand.Parameters["P_ERR_MSG"].Value.ToString());
            }
        }
        
        public static void SP_WCS_BOX_LABEL_INFO(decimal P_CHUTE_NO, string P_PUSH_MODE, string P_PUSH_DATE)
        {
            string procedureName = "SP_WCS_BOX_LABEL_INFO";

            using (OracleConnection oracleConnection = new OracleConnection(_connectionString))
            {
                oracleConnection.Open();

                OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);

                oracleCommand.CommandType = CommandType.StoredProcedure;

                oracleCommand.Parameters.Clear();
                oracleCommand.Parameters.Add("P_CHUTE_NO", OracleDbType.Decimal).Value = P_CHUTE_NO;
                oracleCommand.Parameters.Add("P_PUSH_MODE", OracleDbType.Varchar2, 32767).Value = P_PUSH_MODE;
                oracleCommand.Parameters.Add("P_PUSH_DATE", OracleDbType.Varchar2, 32767).Value = P_PUSH_DATE;

                oracleCommand.Parameters.Add("P_RTN_FLAG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_ERR_MSG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_LABEL_TYPE", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_BOX_ID", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_BRAND_NM", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_CARR_NM", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_AREA_NM", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_AREA_GROUP", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_AREA_CD", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_SHOP_CD", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_SHOP_NM", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_SHOP_ADDR", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_SHOP_TEL", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_PRINT_DATE", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_BOX_SEQ", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_STYLE_CNT", OracleDbType.Decimal, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_STYLE_LIST", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_STYLE_TOTAL", OracleDbType.Decimal, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_SKU_LIST", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;

                oracleCommand.ExecuteNonQuery();

                oracleConnection.Close();

                LabelList.SetValue("P_RTN_FLAG", oracleCommand.Parameters["P_RTN_FLAG"].Value.ToString());
                LabelList.SetValue("P_ERR_MSG", oracleCommand.Parameters["P_ERR_MSG"].Value.ToString());
                LabelList.SetValue("P_LABEL_TYPE", oracleCommand.Parameters["P_LABEL_TYPE"].Value.ToString());
                LabelList.SetValue("P_BOX_ID", oracleCommand.Parameters["P_BOX_ID"].Value.ToString());
                LabelList.SetValue("P_BRAND_NM", oracleCommand.Parameters["P_BRAND_NM"].Value.ToString());
                LabelList.SetValue("P_CARR_NM", oracleCommand.Parameters["P_CARR_NM"].Value.ToString());
                LabelList.SetValue("P_AREA_NM", oracleCommand.Parameters["P_AREA_NM"].Value.ToString());
                LabelList.SetValue("P_AREA_GROUP", oracleCommand.Parameters["P_AREA_GROUP"].Value.ToString());
                LabelList.SetValue("P_AREA_CD", oracleCommand.Parameters["P_AREA_CD"].Value.ToString());
                LabelList.SetValue("P_SHOP_CD", oracleCommand.Parameters["P_SHOP_CD"].Value.ToString());
                LabelList.SetValue("P_SHOP_NM", oracleCommand.Parameters["P_SHOP_NM"].Value.ToString());
                LabelList.SetValue("P_SHOP_ADDR", oracleCommand.Parameters["P_SHOP_ADDR"].Value.ToString());
                LabelList.SetValue("P_SHOP_TEL", oracleCommand.Parameters["P_SHOP_TEL"].Value.ToString());
                LabelList.SetValue("P_PRINT_DATE", oracleCommand.Parameters["P_PRINT_DATE"].Value.ToString());
                LabelList.SetValue("P_BOX_SEQ", oracleCommand.Parameters["P_BOX_SEQ"].Value.ToString());
                LabelList.SetValue("P_STYLE_CNT", oracleCommand.Parameters["P_STYLE_CNT"].Value.ToString());
                LabelList.SetValue("P_STYLE_LIST", oracleCommand.Parameters["P_STYLE_LIST"].Value.ToString());
                LabelList.SetValue("P_STYLE_TOTAL", oracleCommand.Parameters["P_STYLE_TOTAL"].Value.ToString());
                LabelList.SetValue("P_SKU_LIST", oracleCommand.Parameters["P_SKU_LIST"].Value.ToString());
            }
        }

        public static void SP_WCS_SHP_INIT(string P_EZ_STATUS)
        {
            string procedureName = "SP_WCS_SHP_INIT";

            using (OracleConnection oracleConnection = new OracleConnection(_connectionString))
            {
                oracleConnection.Open();

                OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);

                oracleCommand.CommandType = CommandType.StoredProcedure;

                oracleCommand.Parameters.Clear();
                oracleCommand.Parameters.Add("P_EZ_STATUS", OracleDbType.Varchar2, 32767).Value = P_EZ_STATUS;
                                
                oracleCommand.Parameters.Add("P_TARGET", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_RTN_STATUS", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_RTN_FLAG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("P_ERR_MSG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;                

                oracleCommand.ExecuteNonQuery();

                oracleConnection.Close();

                InitList.SetValue("P_TARGET", oracleCommand.Parameters["P_TARGET"].Value.ToString());
                InitList.SetValue("P_RTN_STATUS", oracleCommand.Parameters["P_RTN_STATUS"].Value.ToString());
                InitList.SetValue("P_RTN_FLAG", oracleCommand.Parameters["P_RTN_FLAG"].Value.ToString());
                InitList.SetValue("P_ERR_MSG", oracleCommand.Parameters["P_ERR_MSG"].Value.ToString());                
            }
        }
    }
}