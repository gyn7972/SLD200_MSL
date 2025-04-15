using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

namespace SocketLaserHeightSensor
{
    public class LaserSensorSocketServer
    {
        Socket mainSock;
        List<Socket> connectedClients = new List<Socket>();
        bool m_bConnected = false;
        int m_port = 5000;

        public void Start()
        {
            try
            {
                mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, m_port);
                mainSock.Bind(serverEP);
                mainSock.Listen(10);
                mainSock.BeginAccept(AcceptCallback, null);
            }
            catch (Exception e)
            {
            }
        }

        public void Close()
        {
            if (mainSock != null)
            {
                mainSock.Close();
                mainSock.Dispose();
            }

            foreach (Socket socket in connectedClients)
            {
                socket.Close();
                socket.Dispose();
            }
            connectedClients.Clear();

            //mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        public bool IsConnected()
        {
            return m_bConnected;
        }

        public class AsyncObject
        {
            public byte[] Buffer;
            public Socket WorkingSocket;
            public readonly int BufferSize;
            public AsyncObject(int bufferSize)
            {
                BufferSize = bufferSize;
                Buffer = new byte[(long)BufferSize];
            }

            public void ClearBuffer()
            {
                Array.Clear(Buffer, 0, BufferSize);
            }
        }

        void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = mainSock.EndAccept(ar);
                AsyncObject obj = new AsyncObject(1920 * 1080 * 3);
                obj.WorkingSocket = client;
                connectedClients.Add(client);
                client.BeginReceive(obj.Buffer, 0, 1920 * 1080 * 3, 0, DataReceived, obj);

                mainSock.BeginAccept(AcceptCallback, null);
            }
            catch (Exception e)
            { }
        }

        void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;

            try
            {
                int received = obj.WorkingSocket.EndReceive(ar);

                byte[] buffer = new byte[received];

                Array.Copy(obj.Buffer, 0, buffer, 0, received);
            }
            catch (Exception e)
            {
                m_bConnected = mainSock.Connected;
            }
        }

        public void Send(byte[] msg)
        {
            mainSock.Send(msg);
        }

    }

    public class LaserSensorSocketClient
    {
        Socket mainSock;
        bool m_bConnected = false;
        int m_port = 5000;

        public void Connect(string strIP, int nPort)
        {
            //mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPAddress serverAddr = IPAddress.Parse("10.0.0.10");
            IPAddress serverAddr = IPAddress.Parse(strIP);
            //IPEndPoint clientEP = new IPEndPoint(serverAddr, m_port);
            IPEndPoint clientEP = new IPEndPoint(serverAddr, nPort);
            mainSock.BeginConnect(clientEP, new AsyncCallback(ConnectCallback), mainSock);
        }

        public void Close()
        {
            if (mainSock != null)
            {
                mainSock.Close();
                mainSock.Dispose();
            }
        }

        public bool IsConnected()
        {
            return m_bConnected;
        }

        public class AsyncObject
        {
            public byte[] Buffer;
            public Socket WorkingSocket;
            public readonly int BufferSize;
            public AsyncObject(int bufferSize)
            {
                BufferSize = bufferSize;
                Buffer = new byte[(long)BufferSize];
            }

            public void ClearBuffer()
            {
                Array.Clear(Buffer, 0, BufferSize);
            }
        }

        void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                AsyncObject obj = new AsyncObject(4096);
                obj.WorkingSocket = mainSock;
                mainSock.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
            }
            catch (Exception e)
            {
            }
        }

        void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;

            int received = obj.WorkingSocket.EndReceive(ar);

            byte[] buffer = new byte[received];

            Array.Copy(obj.Buffer, 0, buffer, 0, received);
        }

        public void Send(byte[] msg)
        {
            mainSock.Send(msg);
        }
    }
}