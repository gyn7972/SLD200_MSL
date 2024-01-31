using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class UnitStatus
    {
        public string Barcode { set; get; }
        public int Status { set; get; }

        public UnitStatus()
        {

        }
        public UnitStatus(string strBarcode, int nStatus)
        {
            Barcode = strBarcode;
            Status = nStatus;
        }
    }

    public class UnitInfoManager
    {
        private SqlConnection m_sqlConnection;

        public string Server { set; get; }
        public string Database { set; get; }
        public string UID { set; get; }
        public string Password { set; get; }

        
        public UnitInfoManager()
        {
            m_sqlConnection = new SqlConnection();

            Server = "AMR-100\\SQLEXPRESS";
            Database = "SMT_INLINE";
            UID = "qmc";
            Password = "q1234!";
        }

        public bool Open()
        {
            bool bRet = false;
            string strConnection = "SERVER=" + Server + ";DATABASE=" + Database +";UID=" + UID +";PASSWORD=" + Password;
            m_sqlConnection.ConnectionString = strConnection;
            m_sqlConnection.Open();

            return bRet;
        }

        public bool Close()
        {
            bool bRet = false;

            m_sqlConnection.Close();

            return bRet;
        }

        public bool GetCarrierID(string strBarcodeID, ref string strCarrerID, ref string strRepresentationID)
        {
            bool bRet = false;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT     TB_CARRIER_INFO.CARRIER_ID, TB_UNIT_INFO.BARCODE_ID, TB_CARRIER_INFO.REPRESENTATION_UNIT_ID ");
            sb.AppendFormat("FROM         TB_CARRIER_INFO INNER JOIN ");
            sb.AppendFormat("TB_UNIT_INFO ON TB_CARRIER_INFO.CARRIER_ID = TB_UNIT_INFO.CARRIER_ID ");
            sb.AppendFormat("WHERE     (TB_UNIT_INFO.BARCODE_ID = '{0}')", strBarcodeID);
            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    bRet = true;
                    strCarrerID = (string)reader["CARRIER_ID"];
                    strRepresentationID = (string)reader["REPRESENTATION_UNIT_ID"];
                }
                else
                {
                    strCarrerID = string.Empty;
                    strRepresentationID = string.Empty;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Log.Write("UnitInfoManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
            }
            
            return bRet;
        }

        public List<UnitStatus> GetUnitList(string strCarrierID)
        {
            List<UnitStatus> listUnit = new List<UnitStatus>();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT     TB_UNIT_INFO.BARCODE_ID, TB_UNIT_INFO.STATUS ");
            sb.AppendFormat("FROM         TB_UNIT_INFO ");
            sb.AppendFormat("WHERE     (TB_UNIT_INFO.CARRIER_ID = '{0}') ", strCarrierID);
            sb.AppendFormat("ORDER BY  TB_UNIT_INFO.LOCATION_NO ");

            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    string strBarcode = (string)reader["BARCODE_ID"];
                    int status = (Int16)reader["STATUS"];
                    listUnit.Add(new UnitStatus(strBarcode, status));
                }

                reader.Close();
            }
            catch(Exception ex)
            {
                Log.Write("UnitInfoManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
            }
            

            return listUnit;
        }

        public void SetCarrierLocation(string strCarrierID, string strLocation)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE     TB_CARRIER_INFO ");
            sb.AppendFormat("SET			TB_CARRIER_INFO.LOCATION = '{0}', TB_CARRIER_INFO.LAST_MODIFY_DATE = GETDATE() ", strLocation);
            sb.AppendFormat("WHERE     (SMT_INLINE.dbo.TB_CARRIER_INFO.CARRIER_ID = '{0}')", strCarrierID);
            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Write("UnitInfoManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
            }

        }

        public void SetUnitStatus(List<UnitStatus> units)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE     SMT_INLINE.dbo.TB_UNIT_INFO ");
            sb.AppendFormat("SET			SMT_INLINE.dbo.TB_UNIT_INFO.STATUS = @Status, SMT_INLINE.dbo.TB_UNIT_INFO.LAST_MODIFY_DATE = GETDATE() ");
            sb.AppendFormat("WHERE     (SMT_INLINE.dbo.TB_UNIT_INFO.BARCODE_ID = @Barcode) ");

            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                foreach (UnitStatus unit in units)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@Barcode", unit.Barcode);
                    sqlCommand.Parameters.AddWithValue("@Status", unit.Status);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Log.Write("UnitInfoManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
            }
        }

        public void AddCarrierInfo(string strCarrierID, string strRepresentationID, string strLocation)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO TB_CARRIER_INFO (CARRIER_ID,REPRESENTATION_UNIT_ID, LOCATION) VALUES ('{0}', '{1}', '{2}')", strCarrierID, strRepresentationID, strLocation);
            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Write("UnitInfoManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
            }
        }

        public void AddUnitStatus(string strCarrierID, List<UnitStatus> listUnit)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO TB_UNIT_INFO (BARCODE_ID, CARRIER_ID, LOCATION_NO, STATUS) VALUES (@Barcode, '{0}', @Location, @Status)", strCarrierID);
            string strSql = sb.ToString();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                int nLocation = 1;
                foreach(UnitStatus unit in listUnit)
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("@Barcode", unit.Barcode);
                    sqlCommand.Parameters.AddWithValue("@Status", unit.Status);
                    sqlCommand.Parameters.AddWithValue("@Location", nLocation++);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Log.Write("UnitInfoManager", String.Format("Exception : {0}, SQL : {1} ", ex.Message, strSql));
            }
        }


    }
}
