//using QMC.Core;                   //  2022. 04. 08.  SCH : CDI-300 장비에는 있던 것. 주석 처리.
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading;

namespace CWA150SA_Onsemi300     //  QMC.Vision
{

    /// <summary>
    /// MVTECH 와 TCP 통신 객체
    /// </summary>
    public class TcpServer : IDisposable
    {
        /// <summary>
        /// 클라이언트 연결 여부
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return null != this.client && this.client.Connected;

            }
        }

        public delegate void AlignResultDelegate(TcpServer sender, string category, Vector3 data);
        public delegate void InspectResultDelegate(TcpServer sender, string category, int[] data);

        public event AlignResultDelegate OnAlignResult;
        public event InspectResultDelegate OnInspectResult;

        public int PortNo { get; private set; }

        TcpListener listener;
        TcpClient client;
        NetworkStream stream;
        string streamBuffer;
        Thread thread;
        bool isTerminated = false;
        const int BufferSize = 10 * 1024;
        byte[] receiveBuffer = new byte[BufferSize];
        const char STX = (char)0x02;
        const char ETX = (char)0x03;
        const char CR = (char)0x0D;
        const char LF = (char)0x0A;
        bool disposed = false;
        private Vector3 _result = new Vector3(0, 0, 0);

        public TcpServer(int port = 4900)
        {
            this.PortNo = port;
        }
        ~TcpServer()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                this.isTerminated = true;
                this.listener?.Stop();
                this.client?.Dispose();

                //this.thread.Abort();
                //this.thread.Join();         
            }
            this.disposed = true;
        }

        /// <summary>
        /// TCP 서버 초기화
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {

            this.listener = new TcpListener(IPAddress.Any, this.PortNo);
            listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.listener.Start();
            this.isTerminated = false;
            this.thread = new Thread(this.ServerAcceptThread);
            this.thread.Name = "Vision Tcp Server";
            this.thread.Start();
            //Logger.Log(Logger.Module.Vision, Logger.Type.Info, $"tcp server: started at port: {this.PortNo}");        //  SCH : Log 는 임시로 주석 처리함.
            return true;
        }

        public bool stopServer()
        {
            this.Dispose(true);
            return true;
        }
        private void ServerAcceptThread()
        {
            while (!this.isTerminated)
            {
                TcpClient newClient;
                try
                {
                    newClient = listener.AcceptTcpClient();
                    newClient.NoDelay = true; //nagle 알고리즘 끄기 (속도 향상)
                }
                catch (SocketException ex)
                {
                    //terminating ...
                    //Logger.Log(Logger.Module.Vision, ex);
                    break;
                }

                if (this.IsConnected)
                    this.client?.Close(); //기존 연결 해제 (하나의 클라이언트만 유지)


                this.client = newClient;
                this.stream?.Dispose();
                this.stream = this.client.GetStream();
                this.streamBuffer = string.Empty; //수신 임시 버퍼 초기화

                try
                {
                    // 비동기 읽기 시작
                    Array.Clear(this.receiveBuffer, 0x00, receiveBuffer.Length);
                    //Logger.Log(Logger.Module.Vision, Logger.Type.Debug, $"tcp client connected: {((IPEndPoint)this.client.Client.RemoteEndPoint).Address.ToString()}");       //  SCH : Log 는 임시로 주석 처리함.
                    this.stream.BeginRead(this.receiveBuffer, 0, BufferSize, new AsyncCallback(ReceiveCallback), null);
                }
                catch (SocketException ex)
                {
                    //Logger.Log(Logger.Module.Vision, ex);       //  SCH : Log 는 임시로 주석 처리함.
                    break;
                }
            }
            //Logger.Log(Logger.Module.Vision, Logger.Type.Info, $"tcp server: terminating...");       //  SCH : Log 는 임시로 주석 처리함.
        }

        //비동기 데이타 수신시 콜백
        private void ReceiveCallback(IAsyncResult ar)
        {
            int bytesRead = 0;
            try
            {
                bytesRead = this.stream.EndRead(ar);
            }
            catch (Exception ex)
            {
                //Logger.Log(Logger.Module.Vision, Logger.Type.Warn, $"tcp server : {ex.Message}");       //  SCH : Log 는 임시로 주석 처리함.
                // may be reset or disconnected 
                if (false == this.isTerminated)
                {
                    this.client.Close();
                    this.ServerAcceptThread();
                }
                return;
            }

            if (bytesRead <= 0)
                return;

            //수신 데이타에서 종결자(ETX)를 기준으로 앞의 데이타를 분리해 낸다
            //string receive = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);                
            //this.streamBuffer += receive;

            //test code
            this.streamBuffer += Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);
            int indexETX = streamBuffer.IndexOf(ETX);
            while (indexETX >= 0)
            {
                //Debug.Assert(STX == this.streamBuffer[0]);
                string data = this.streamBuffer.Substring(1, indexETX - 1); //from STX ~ ETX         
 //               Logger.Log(Logger.Module.Vision, Logger.Type.Debug, $"tcp server: received data and parsing ...");
                this.Parse(ref data);
                if (indexETX < this.streamBuffer.Length - 1)
                {
                    this.streamBuffer = this.streamBuffer.Substring(indexETX + 1, this.streamBuffer.Length - indexETX - 1);
                    indexETX = streamBuffer.IndexOf(CR);

                }
                else
                {
                    this.streamBuffer = string.Empty;
                    break;
                }
            }
            // retry 
            Array.Clear(this.receiveBuffer, 0x00, receiveBuffer.Length);
            this.stream.BeginRead(this.receiveBuffer, 0, BufferSize, new AsyncCallback(ReceiveCallback), null);
        }

        /// <summary>
        /// 수신 데이타 분석
        /// </summary>
        /// <param name="data">STX, EXT 가 제거된 데이타</param>
        /// <returns></returns>
        private bool Parse(ref string data)
        {
            bool success = true;
            string[] tokens = data.Split(',');

            if (int.TryParse(tokens[0], out int count1))  //데이타 갯수,,
            {
 //               Logger.Log(Logger.Module.Vision, Logger.Type.Debug, $"tcp server: parse Start");
                //Vector3[] resultArray = new Vector3[count1];
                for (int i = 0; i < count1; i++)
                {
                    _result.X = float.Parse(tokens[i * 3 + 2]);
                    _result.Y = float.Parse(tokens[i * 3 + 3]);
                    _result.Z = float.Parse(tokens[i * 3 + 4]);
                    //resultArray[i] = new Vector3(x, y, theta);
                }
                //Logger.Log(Logger.Module.Vision, Logger.Type.Debug, $"tcp server: parsed...");       //  SCH : Log 는 임시로 주석 처리함.
                //var svc = Program.SequenceVision.Service as ServiceVision;
                //svc.ServiceVision_OnAlignResult(this, tokens[1], _result);
                this.OnAlignResult?.Invoke(this, tokens[1], _result);
                //Logger.Log(Logger.Module.Vision, Logger.Type.Debug, $"tcp server: parsed align data at {tokens[1]}: count={count1}, x={_result.X:F3}, y={_result.Y:F3}, theta={_result.Z:F3}");       //  SCH : Log 는 임시로 주석 처리함.
            }
            else
            {
                string category = tokens[0];
                int.TryParse(tokens[1], out int count2);
                int[] resultArray = new int[count2];
                for (int i = 0; i < count2; i++)
                {
                    try
                    {
                        int ngType = int.Parse(tokens[i + 2]);
                        resultArray[i] = ngType;
                    }
                    catch (Exception ex)
                    {

                        //Logger.Log(Logger.Module.Vision, ex);       //  SCH : Log 는 임시로 주석 처리함.
                        return false;
                    }
                    

                }
                this.OnInspectResult?.Invoke(this, category, resultArray);
                //Logger.Log(Logger.Module.Vision, Logger.Type.Debug, $"tcp server: parsed inspect data at {category}: count={count2}, {data}");       //  SCH : Log 는 임시로 주석 처리함.
            }
            return success;
        }

        private bool Send(byte[] bytes)
        {
            if (null == this.stream || !this.IsConnected)
            {
                //Logger.Log(Logger.Module.Vision, Logger.Type.Error, $"tcp server: client is not connected yet");       //  SCH : Log 는 임시로 주석 처리함.
                return false;
            }
            try
            {
                this.stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                //Logger.Log(Logger.Module.Vision, ex);                
                return false;
            }
            return true;
        }


        /// <summary>
        /// 레시피 변경
        /// </summary>
        /// <param name="recipeName"></param>
        /// <returns></returns>
        public bool CommandRecipeChange(string groupName, string recipeName)
        {
            var sb = new StringBuilder(32);
            sb.Append($"{(char)STX}RecipeName,{groupName},{recipeName}{(char)ETX}");
            var buffer = Encoding.ASCII.GetBytes(sb.ToString());
            //Logger.Log(Logger.Module.Vision, Logger.Type.Debug, $"tcp server: sending recipe change : {groupName},{recipeName}");       //  SCH : Log 는 임시로 주석 처리함.
            return this.Send(buffer);
        }


        public bool CommandInspCount(string InspCnt)
        {
            var sb = new StringBuilder(32);
            sb.Append($"{(char)STX},InspCnt,{InspCnt},{(char)ETX}");
            var buffer = Encoding.ASCII.GetBytes(sb.ToString());
            //Logger.Log(Logger.Module.Vision, Logger.Type.Debug, $"tcp server: sending Insp Count : {InspCnt}");       //  SCH : Log 는 임시로 주석 처리함.
            return this.Send(buffer);
        }

        /// <summary>
        /// 현재 레시피 이름 얻기
        /// </summary>
        /// <returns></returns>
        public bool QueryRecipeName()
        {
            return false;
            //var sb = new StringBuilder(16);
            //sb.Append($"{(char)STX}RecipeName{(char)ETX}");
            //sb.Append($"{(char)ETX}");
            //var buffer = Encoding.ASCII.GetBytes(sb.ToString());
            //Logger.Log(Logger.Module.Vision, Logger.Type.Debug, $"tcp server: sending recipe name query ");
            //return this.Send(buffer);
        }
    }
}
