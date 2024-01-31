using QMC.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class MeasureDataManager
    {
        private SqlConnection m_sqlConnection;

        public string Server { set; get; }
        public string Database { set; get; }
        public string UID { set; get; }
        public string Password { set; get; }

        public MeasureDataManager()
        {
            m_sqlConnection = new SqlConnection();
            Server = "LPM-100\\SQLEXPRESS";
            Database = "SMT_INLINE";
            UID = "qmc1";
            Password = "q1234!";
        }

        public bool Open()
        {
            bool bRet = false;

            try
            {
                string strConnection = "SERVER=" + Server + ";DATABASE=" + Database + ";UID=" + UID + ";PASSWORD=" + Password;
                m_sqlConnection.ConnectionString = strConnection;
                m_sqlConnection.Open();
            }
            catch (Exception ex)
            {
                Log.Write("UnitInfoManager", String.Format("Exception : {0}", ex.Message));
            }

            return bRet;
        }

        public bool Close()
        {
            bool bRet = false;

            m_sqlConnection.Close();

            return bRet;
        }

        public void AddMeasureValue(string strCarrierID, string strBarcodeID, int Index, double dEtchingWidth, double dEtchingHeight, double dRivet1, double dRivet2, bool bDispecing)
        {
            /*
             *INSERT INTO dbo.TB_LPM_MEASURE_VALUE
              (BARCODE_ID, CARRIER_ID, ETCHING_WIDTH, ETCHING_HEIGHT, RIVET1_DIAMETER, RIVET2_DIAMETER, LOCATION_NO, UPDATE_DATE, DISPECING_YN)
              VALUES  ('G123', '1234', 56.22, 55.11, 8, 7, 1, CONVERT(DATETIME, '2022-07-01 14:39:00', 102), 1) 
             */
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO dbo.TB_LPM_MEASURE_VALUE ");
            sb.AppendFormat("(BARCODE_ID, CARRIER_ID, ETCHING_WIDTH, ETCHING_HEIGHT, RIVET1_DIAMETER, RIVET2_DIAMETER, LOCATION_NO, UPDATE_DATE, DISPECING_YN) ");
            sb.AppendFormat("VALUES  ('{0}', '{1}', {2}, {3}, {4}, {5}, {6}, GETDATE(), {7}) "
                , strBarcodeID, strCarrierID, dEtchingWidth, dEtchingHeight, dRivet1, dRivet2, Index, bDispecing ? 1 : 0);
            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Write("MeasureDataManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
            }
        }

        public void AddMeasureValue(string strBarcodeID, double dMountX, double dMountY)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE dbo.TB_LPM_MEASURE_VALUE ");
            sb.AppendFormat("SET        MOUNT_SHIFT_X = {0} , MOUNT_SHIFT_Y = {1} ", dMountX, dMountY);
            sb.AppendFormat("WHERE  (BARCODE_ID = '{0}') ", strBarcodeID);
            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Write("MeasureDataManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
            }
        }

        public List<LPMSPCData> GetFunctionTesterSPCDatas(DateTime startTime, DateTime endTime, string strBarcodeID)
        {
            List<LPMSPCData> list = new List<LPMSPCData>();
            endTime = endTime.AddDays(1);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT  BARCODE_ID, CARRIER_ID, ETCHING_WIDTH, ETCHING_HEIGHT, RIVET1_DIAMETER, RIVET2_DIAMETER, MOUNT_SHIFT_X, MOUNT_SHIFT_Y, LOCATION_NO, UPDATE_DATE, DISPECING_YN");
            sb.AppendFormat("FROM     TB_LPM_MEASURE_VALUE ");
            sb.AppendFormat("WHERE  ");
            if(!string.IsNullOrEmpty(strBarcodeID))
            {
                sb.AppendFormat("(BARCODE_ID = '{0}') AND  ", strBarcodeID);
            }
            sb.AppendFormat(" (UPDATE_DATE > CONVERT(DATETIME, '{0} 00:00:00', 102) AND", startTime.ToString("yyyy-MM-dd"));
            sb.AppendFormat("               UPDATE_DATE <= CONVERT(DATETIME, '{0} 00:00:00', 102))", endTime.ToString("yyyy-MM-dd"));
            sb.AppendFormat(" ORDER BY UPDATE_DATE");
            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    LPMSPCData data = new LPMSPCData();
                    data.UnitID = Convert.ToString(reader["BARCODE_ID"]);
                    data.CarrierID = Convert.ToString(reader["CARRIER_ID"]);
                    data.LoactionNO = Convert.ToInt32(reader["LOCATION_NO"]);
                    data.EtchingWidth = Convert.ToDouble(reader["ETCHING_WIDTH"]);
                    data.EtchingHeight = Convert.ToDouble(reader["ETCHING_HEIGHT"]);
                    data.Rivet1 = Convert.ToDouble(reader["RIVET1_DIAMETER"]);
                    data.Rivet2 = Convert.ToDouble(reader["RIVET2_DIAMETER"]);
                    data.MountShiftX = Convert.ToDouble(reader["MOUNT_SHIFT_X"]);
                    data.MountShiftY = Convert.ToDouble(reader["MOUNT_SHIFT_Y"]);
                    data.Dispensing = Convert.ToBoolean(reader["DISPECING_YN"]);
                    data.Date = Convert.ToDateTime(reader["UPDATE_DATE"]);
                    
                    list.Add(data);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Log.Write("UnitInfoManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
            }

            return list;
        }
    }
}
