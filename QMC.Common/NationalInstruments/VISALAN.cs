using Ivi.Visa;
using NationalInstruments.Visa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.NationalInstruments
{
    public class VISALAN
    {
        #region DLL Imports
        /// <summary>
        /// This function returns a session to the Default Resource Manager resource.
        /// </summary>
        /// <param name="session">Resource Manager session(should always be a session returned from viOpenDefaultRM()).</param>
        [DllImport("visa32.dll")]
        private static extern Int32 viOpenDefaultRM(ref UInt32 session);

        /// <summary>
        /// Opens a session to the specified resource.
        /// </summary>
        /// <param name="session">Resource Manager session(should always be a session returned from viOpenDefaultRM()).</param>
        /// <param name="resourceName">Unique symbolic name of a resource. Refer to the Description section for more information.</param>
        /// <param name="accessMode">Specifies the mode by which the resource is to be accessed.Refer to the Description section for valid values.If the parameter value is VI_NULL, the session uses VISA-supplied default values.</param>
        /// <param name="openTimeout">Specifies the maximum time period(in milliseconds) that this operation waits before returning an error. This does not set the I/O timeout – to do that you must call viSetAttribute() with the attribute VI_ATTR_TMO_VALUE.</param>
        /// <param name="vi">Unique logical identifier reference to a session.</param>
        /// <returns></returns>
        [DllImport("visa32.dll")]
        private static extern Int32 viOpen(UInt32 session, [MarshalAs(UnmanagedType.LPStr)] string resourceName, UInt32 accessMode, UInt32 openTimeout, ref UInt32 vi);

        /// <summary>
        /// ViStatus viClose(ViObject vi)
        /// </summary>
        /// <param name="vi">Unique logical identifier to a session, event, or find list.</param>
        /// <returns></returns>
        [DllImport("visa32.dll")]
        private static extern Int32 viClose(UInt32 vi);

        /// <summary>
        /// Clears a device.
        /// </summary>
        /// <param name="session">Unique logical identifier to a session.</param>
        /// <returns></returns>
        [DllImport("visa32.dll")]
        private static extern Int32 viClear(UInt32 session);

        /// <summary>
        /// Sets the state of an attribute.
        /// </summary>
        /// <param name="vi">Unique logical identifier to a session.</param>
        /// <param name="attribute">Attribute for which the state is to be modified.</param>
        /// <param name="attrState">The state of the attribute to be set for the specified object. The interpretation of the individual attribute value is defined by the object.</param>
        /// <returns></returns>
        [DllImport("visa32.dll")]
        private static extern Int32 viSetAttribute(UInt32 vi, UInt32 attribute, UInt32 attrState);

        /// <summary>
        /// Writes data to device or interface synchronously.
        /// </summary>
        /// <param name="vi">Unique logical identifier to a session.</param>
        /// <param name="buf">Location of a data block to be sent to a device.</param>
        /// <param name="count">Number of bytes to be written.</param>
        /// <param name="retCount">Number of bytes actually transferred.</param>
        /// <returns></returns>
        [DllImport("visa32.dll")]
        private static extern Int32 viWrite(UInt32 vi, byte[] buf, UInt32 count, out UInt32 retCount);

        /// <summary>
        /// Reads data from device or interface synchronously.
        /// </summary>
        /// <param name="vi">Unique logical identifier to a session.</param>
        /// <param name="buf">Location of a buffer to receive data from device.</param>
        /// <param name="count">Number of bytes to be read.</param>
        /// <param name="retCount">Number of bytes actually transferred.</param>
        /// <returns></returns>
        [DllImport("visa32.dll")]
        private static extern Int32 viRead(UInt32 vi, out byte[] buf, UInt32 count, out UInt32 retCount);

        /// <summary>
        /// Reads string data from device or interface synchronously.
        /// </summary>
        /// <param name="vi">Unique logical identifier to a session.</param>
        /// <param name="buf">Location of a buffer to receive data from device.</param>
        /// <param name="count">Number of bytes to be read.</param>
        /// <param name="retCount">Number of bytes actually transferred.</param>
        /// <returns></returns>
        [DllImport("visa32.dll")]
        private static extern Int32 viRead(UInt32 vi, StringBuilder buf, UInt32 count, out UInt32 retCount);

        [DllImport("visa32.dll")]
        private static extern Int32 viFindRsrc(UInt32 vi, StringBuilder buf, out UInt32[] findList, out UInt32 retcnt, out string instrDesc);
        #endregion

        #region Field
        private MessageBasedSession Session;
        private IVisaAsyncResult asyncHandle = null;
        string filter;
        ParseResult parseResult;
        private bool m_IsOpen;
        #endregion

        #region Constructor
        public VISALAN()
        {
            IsOpen = false;
            filter = "(TCPIP)?*INSTR";
            parseResult = null;
            ReceiveTimeout = 10000.0;
        }
        #endregion

        #region Property
        [DefaultValue(false)]
        public bool IsOpen
        {
            get { return this.m_IsOpen; }
            set { this.m_IsOpen = value; }
        }

        public double ReceiveTimeout { get; set; }
        #endregion

        #region Method
        public int Init()
        {
            int ret = 0;

            using (var rm = new ResourceManager())
            {
                try
                {
                    IEnumerable<string> resources = rm.Find(filter);
                    foreach (string s in resources)
                    {
                        parseResult = rm.Parse(s);
                    }
                }
                catch (Exception ex)
                {
                    // MessageBox.Show(ex.Message);
                }
            }

            using (var rmSession = new ResourceManager())
            {
                try
                {
                    if (this.IsOpen != true)
                    {
                        ResourceOpenStatus openStatus = new ResourceOpenStatus();
                        //TODO : SMU에 LAN에 불 안들어왔을때 Open시 Exception발생 해결 필요.
                        if (Session == null)
                            Session = (MessageBasedSession)rmSession.Open(parseResult.OriginalResourceName.ToString(), AccessModes.LoadConfig, 10000, out openStatus);
                        if (openStatus != ResourceOpenStatus.Success) return ret;
                        this.IsOpen = true;
                    }
                }
                catch (InvalidCastException)
                {
                    Log.Write("VISALAN", "Resource selected must be a message-based session");
                    MessageBox.Show("Resource selected must be a message-based session");
                    this.IsOpen = false;
                }
            }

            return ret;
        }

        public int Close()
        {
            int ret = 0;
            if (this.IsOpen == true)
            {
                if (this.Session != null)
                {
                    this.Session.Dispose();
                    this.Session = null;
                    this.IsOpen = false;
                }
            }

            return ret;
        }

        public int Send(string text)
        {
            int ret = 0;

            if (this.IsOpen == false) return ret;

            try
            {
                string textToWrite = ReplaceCommonEscapeSequences(text);
                if (!string.IsNullOrEmpty(textToWrite))
                {
                    //byte[] bytes = Encoding.ASCII.GetBytes(text);
                    //Session.RawIO.Write(bytes);
                    Session.RawIO.Write(textToWrite);
                }

            }
            catch (Exception exp)
            {
                Log.Write("VISALAN", exp.Message);
                MessageBox.Show(exp.Message);
            }

            return ret;
        }

        public int BeginSend(string text)
        {
            int ret = 0;

            if (this.IsOpen == false) return ret;

            try
            {
                string textToWrite = ReplaceCommonEscapeSequences(text);
                asyncHandle = Session.RawIO.BeginWrite(
                    textToWrite,
                    new VisaAsyncCallback(OnWriteComplete),
                    (object)textToWrite.Length);
            }
            catch (Exception exp)
            {
                Log.Write("VISALAN", exp.Message);
                MessageBox.Show(exp.Message);
            }

            return ret;
        }

        public int Receive(out string data)
        {
            int ret = 0;
            data = null;

            string readBuffer = string.Empty;
            StringBuilder sb = new StringBuilder();
            string receivestring = string.Empty;
            if (this.IsOpen == false) return ret;
            ///
            DateTime startTime = DateTime.Now;
            //
            try
            {
                while (true)
                {
                    readBuffer = CheckReceive();

                    TimeSpan processTime = DateTime.Now - startTime;

                    if(string.IsNullOrEmpty(readBuffer) == false)
                    {
                        receivestring += readBuffer;
                        break;
                    }

                    
                    //if(receivestring.Length != 0)
                    //{
                    //    string[] temp = data.Trim().Split(',');
                    //    if(temp)
                    //}

                    if (receivestring.Contains(@"\n"))
                        break;

                    if (processTime.TotalMilliseconds > ReceiveTimeout)
                    {
                        Alarm alarm = new Alarm();
                        alarm.Title = this.ToString();
                        alarm.Grade = "Stop";
                        alarm.Source = "ReceiveTimeOut";
                        alarm.Code = 2001;
                        alarm.Cause = "ReceiveTimeOut";

                        AlarmManager.Instance.ShowAlarm(alarm);
                        return -1;
                    }

                    Thread.Sleep(10);
                }
                data = receivestring;
                //data = readBuffer;
            }
            catch (Exception exp)
            {
                Log.Write("VISALAN", exp.Message);

                Alarm alarm = new Alarm();
                alarm.Title = this.ToString();
                alarm.Grade = "Stop";
                alarm.Source = "ReceiveTimeOut";
                alarm.Code = 2001;
                alarm.Cause = "ReceiveTimeOut";

                AlarmManager.Instance.ShowAlarm(alarm);
                return -1;
            }

            return ret;
        }

        public string CheckReceive()
        {
            string data = string.Empty;

            try
            {
                data = Session.RawIO.ReadString(10000);
               // data = InsertCommonEscapeSequences(Session.RawIO.ReadString());
            }
            catch (Exception e)
            {

            }

            return data;
        }

        /// <summary>
        /// Receive실패시 남아있는 Data Check로 측정시 새로운 데이터를 받을 수 있도록 (Test필요)
        /// </summary>
        /// <returns></returns>
        public bool CheckRemainDatas()
        {
            bool check = false;
            string data = string.Empty;

            while (true)
            {
                try
                {
                    //data = InsertCommonEscapeSequences(Session.RawIO.ReadString());
                    data = Session.RawIO.ReadString();
                    check = false;
                }
                catch (Exception e)
                {
                    check = true;
                    break;
                }
                
                Thread.Sleep(10);
            }

            return check;
        }

        public int BeginReceive(out string data)
        {
            int ret = 0;
            data = null;

            string readBuffer = string.Empty;

            if (this.IsOpen == false) return ret;

            try
            {
                asyncHandle = Session.RawIO.BeginRead(
           1024,
           new VisaAsyncCallback(OnReadComplete),
           null);
            }
            catch (Exception exp)
            {
                Log.Write("VISALAN", exp.Message);
                MessageBox.Show(exp.Message);
            }

            data = readBuffer;

            return ret;
        }

        private string ReplaceCommonEscapeSequences(string s)
        {
            //return s.Replace("\\n", "\n").Replace("\\r", "\r");
            s.Replace("\"", "");
            return s;
        }

        private string InsertCommonEscapeSequences(string s)
        {
            return s.Replace("\n", "\\n").Replace("\r", "\\r");
        }

        public bool CheckOpen()
        {
            if (this.IsOpen == true)
                return true;
            else
                return false;
        }
        #endregion

        #region EventHandler
        private void OnWriteComplete(IVisaAsyncResult result)
        {
            //try
            //{
            //    Session.RawIO.EndWrite(result);
            //    //lastIOStatusTextBox.Text = "Success";
            //}
            //catch (Exception exp)
            //{
            //    //lastIOStatusTextBox.Text = exp.Message;
            //}
            ////elementsTransferredTextBox.Text = ((int)result.Count).ToString();
        }

        private void OnReadComplete(IVisaAsyncResult result)
        {
            //try
            //{
            //    string responseString = Session.RawIO.EndReadString(result);
            //    readTextBox.Text = InsertCommonEscapeSequences(responseString);
            //    lastIOStatusTextBox.Text = "Success";
            //}
            //catch (Exception exp)
            //{
            //    lastIOStatusTextBox.Text = exp.Message;
            //}
            //elementsTransferredTextBox.Text = ((int)result.Count).ToString();
        }

        public void Dispose()
        {
            if (Session != null)
                Session.Dispose();
        }
        #endregion
    }
}
