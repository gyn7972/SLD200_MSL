using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class AlarmSaver
    {
        private SqlConnection m_sqlConnection;

        public string Server { set; get; }
        public string Database { set; get; }
        public string UID { set; get; }
        public string Password { set; get; }

        public AlarmSaver()
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
                bRet = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                bRet = false;
                Console.WriteLine(ex.Message);
            }
           

            return bRet;
        }

        public bool Close()
        {
            bool bRet = true;

            m_sqlConnection.Close();

            return bRet;
        }

        public void AddAlarm(Alarm alarm)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("INSERT INTO dbo.TB_ALARM_HISTORY ");
                sb.AppendFormat("               (TITLE, CODE, MESSAGE, SOURCE, GRADE, UPDATE_DATE) ");
                sb.AppendFormat("VALUES  ('{0}', {1}, N'{2}', '{3}', '{4}', CONVERT(DATETIME, '{5}', 102))",
                    alarm.Title, alarm.Code, alarm.Cause, alarm.Source, alarm.Grade, alarm.GeneratedTime.ToString("yyyy-MM-dd HH:mm:ss"));
                string strSql = sb.ToString();
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Console.WriteLine(ex.Message);
            }

        }

        public List<Alarm> GetAlarms(DateTime startTime, DateTime endTime)
        {
            List<Alarm> alarms = new List<Alarm>();            
            StringBuilder sb = new StringBuilder();
            endTime = endTime.AddDays(1);

            sb.AppendFormat("SELECT  ROW, TITLE, CODE, MESSAGE, SOURCE, GRADE, UPDATE_DATE ");
            sb.AppendFormat("FROM     dbo.TB_ALARM_HISTORY ");
            sb.AppendFormat("WHERE  (UPDATE_DATE > CONVERT(DATETIME, '{0} 00:00:00', 102)) AND", startTime.ToString("yyyy-MM-dd"));
            sb.AppendFormat("               (UPDATE_DATE < CONVERT(DATETIME, '{0} 00:00:00', 102)) ", endTime.ToString("yyyy-MM-dd"));
            sb.AppendFormat("ORDER BY ROW");

            string strSql = sb.ToString();
            SqlDataReader reader = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand(strSql, m_sqlConnection);
                reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Alarm alarm = new Alarm();
                    alarm.Title = Convert.ToString(reader["TITLE"]);
                    alarm.Code = Convert.ToInt16(reader["CODE"]);
                    alarm.Cause = Convert.ToString(reader["MESSAGE"]);
                    alarm.Grade = Convert.ToString(reader["GRADE"]);
                    alarm.Source = Convert.ToString(reader["SOURCE"]);
                    alarm.GeneratedTime = Convert.ToDateTime(reader["UPDATE_DATE"]);
                    alarms.Add(alarm);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Console.WriteLine(ex.Message);
            }

            return alarms;
        }
    }
}
