using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class MesDataManager
    {
        public enum Process_Name
        {
            Rivet,
            Etching,
            LEDMOUNT,
            WireBonding,
            Work,
        }
        protected Dictionary<Process_Name, string> m_dicTableName;
        private SqlConnection m_sqlConnection;

        public string Server { set; get; }
        public string Database { set; get; }
        public string UID { set; get; }
        public string Password { set; get; }

        public MesDataManager()
        {
            m_sqlConnection = new SqlConnection();
            Server = "AMR-100\\SQLEXPRESS";
            Database = "MES_TEST";
            UID = "qmc";
            Password = "q1234!";

            m_dicTableName = new Dictionary<Process_Name, string>();
            foreach (Process_Name process in Enum.GetValues(typeof(Process_Name)))
            {
                m_dicTableName.Add(process, process.ToString());
            }
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
            catch(Exception ex)
            {
                Log.Write(ex);
                Log.Write("MesDataManager", String.Format("Open Failed. Exception : {0}", ex.Message));
            }

            return bRet;
        }

        public bool Close()
        {
            bool bRet = false;

            m_sqlConnection.Close();

            return bRet;
        }

        public List<MesInfo> GetLineInfo()
        {
            List<MesInfo> infos = null;
            int ret = 0;
            infos = GetLineInfo(out ret);

            if (ret != 0)
            {
                Open();
                infos = GetLineInfo(out ret);
            }

            return infos;
        }
        protected List<MesInfo> GetLineInfo(out int ret)
        {
            List<MesInfo> infos = new List<MesInfo>();
            ret = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT WPSeq, Floor, Line, [Proc], EquipCD ");
            sb.AppendFormat("FROM LineInfo ");
            string strSql = sb.ToString();
            SqlDataReader reader = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    MesInfo info = new MesInfo();
                    info.WPSeq = Convert.ToInt64(reader["WPSeq"]);
                    info.Floor = Convert.ToString(reader["Floor"]);
                    info.Line = Convert.ToString(reader["Line"]);
                    info.Proc = Convert.ToString(reader["Proc"]);
                    info.EquipCD = Convert.ToString(reader["EquipCD"]);

                    infos.Add(info);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                ret = -1;
                Log.Write("MesDataManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
            }

            return infos;
        }

        public void SendMesResult(Process_Name process, MesInfo mesInfo, string strProcessTime, string strBarcodeID, int nResult, object MeasureData, string strRepresentationID)
        {
            if (m_dicTableName.ContainsKey(process))
            {
                if (SendMesResult(m_dicTableName[process], mesInfo, strProcessTime, strBarcodeID, nResult, MeasureData, strRepresentationID) != 0)
                {
                    Open();
                    SendMesResult(m_dicTableName[process], mesInfo, strProcessTime, strBarcodeID, nResult, MeasureData, strRepresentationID);
                }
            }
        }

        public void SendMesResult(Process_Name process, MesInfo mesInfo, SmtUnit unit, string strRepresentationID)
        {
            SendMesResult(process, mesInfo, unit.ProcessTime.ToString("yyyy-MM-dd HH:mm:ss"), unit.ObjID, unit.Result == SmtUnit.ResultKey.Ok ? 1 : 0, unit.MeasureData, strRepresentationID);
        }

        protected int SendMesResult(string strTableName, MesInfo mesInfo, string strProcessTime, string strBarcodeID, int nResult, object MeasureData, string strRepresentationID)
        {
            int ret = 0;
            StringBuilder sb = new StringBuilder();
            /*
             * INSERT INTO dbo.Rivet
               (Dtm, PID, DelegatePID, OKYN, Comment, WPSeq, EquipCD)
               VALUES  (CONVERT(DATETIME, '2022-07-01 09:37:52', 102), 'G123345', 'G12345', 1, N'FORCE:0.001@:120.001', 1, N'ARM-100')
             */
            sb.AppendFormat("INSERT INTO {0} ", strTableName);
            sb.AppendFormat("(Dtm, PID, DelegatePID, OKYN, Comment, WPSeq, EquipCD) ");
            sb.AppendFormat("VALUES  (CONVERT(DATETIME, '{0}', 102), '{1}', '{2}', {3}, N'{4}', {5}, N'{6}') ",
                strProcessTime, strBarcodeID, strRepresentationID, nResult, MeasureData.ToString(), mesInfo.WPSeq, mesInfo.EquipCD);

            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Log.Write("MesDataManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
                ret = -1;
            }

            return ret;
        }

        public void SendMesMountPickup(MesInfo mesInfo, int nPickupCount, int nMissCount)
        {
            int ret = 0;
            if (SendMesMountPickup(mesInfo, nPickupCount, nMissCount, out ret) != 0)
            {
                Open();
                SendMesMountPickup(mesInfo, nPickupCount, nMissCount, out ret);
            }

        }

        protected int SendMesMountPickup(MesInfo mesInfo, int nPickupCount, int nMissCount, out int ret)
        {
            ret = 0;
            StringBuilder sb = new StringBuilder();
            /*
             * INSERT INTO dbo.MOUNTER
               (Dtm, EquipCD, WPSeq, PickUpQty, LossQty)
               VALUES  (GETDATE(), N'G12345', 2, 100, 0)
             */
            sb.AppendFormat("INSERT INTO MOUNTER ");
            sb.AppendFormat("(Dtm, EquipCD, WPSeq, PickUpQty, LossQty) ");
            sb.AppendFormat("VALUES  (GETDATE(), N'{0}', {1}, {2}, {3}) ",
                mesInfo.EquipCD, mesInfo.WPSeq, nPickupCount, nMissCount);

            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Log.Write("MesDataManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
                ret = -1;
            }
            return ret;
        }

        public int CheckInterlock(string BarcodeID, MesInfo mesInfo, out string strInterlockMsg)
        {
            int ret = 0;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT Row, ReVal ");
            sb.AppendFormat("FROM uspOT_InterLockCheck ");
            sb.AppendFormat("WHERE WPSeq = {0}, PID = '{1}' ", mesInfo.WPSeq, BarcodeID);
            string strSql = sb.ToString();
            SqlDataReader reader = null;
            strInterlockMsg = string.Empty;
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    strInterlockMsg = Convert.ToString(reader["ReVal"]);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                ret = -1;
                Log.Write("MesDataManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));

            }

            return ret;
        }

    }
}
