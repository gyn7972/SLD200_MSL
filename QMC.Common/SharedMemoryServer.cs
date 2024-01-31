using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace QMC.Common
{
    public class SharedMemoryServer
    {
        #region Event
        public delegate void EventExpose();
        public delegate void EventGrab(out Bitmap image);
        public delegate void EventReadout(out Bitmap image);
        public delegate void EventInspect1(ref bool IsSuccess, ref XytCoordinate matchCoordinate);
        public delegate void EventInspect2(XytCoordinate alignPitch,ref bool IsSuccess, ref XytCoordinate matchCoordinate,ref XytCoordinate alignResultCoordinate);
        public delegate void EventInspect3();
        public delegate void EventInspect4();
        public delegate void EventInspect5();
        public delegate void EventInspect6();
        public delegate void EventInspect7();
        public delegate void EventInspect8();
        public delegate void EventInspect9();
        public delegate void EventInspect10();

        public event EventExpose OnExposeRequest;
        public event EventGrab OnGrabRequest;
        public event EventReadout OnReadoutRequest;
        public event EventInspect1 OnInspect1;
        public event EventInspect2 OnInspect2;
        public event EventInspect3 OnInspect3;
        public event EventInspect4 OnInspect4;
        public event EventInspect5 OnInspect5;
        public event EventInspect6 OnInspect6;
        public event EventInspect7 OnInspect7;
        public event EventInspect8 OnInspect8;
        public event EventInspect9 OnInspect9;
        public event EventInspect10 OnInspect10;
        #endregion

        #region Set Values
        // 아래 항목은 Client와 동일해야합니다.
        private const string DefaultMemoryName = "QMC_SharedMemory";
        private const string DefaultMutexName = "QMC_SharedMemory_Mutex";
        // capacity는 sharedMemoryStructSize + sharedMemoryHeapSize보다 크게 설정해야합니다.
        private const long memoryCapacity = 1000;
        private const long sharedMemoryHeapSize = 20;


        // 아래 항목은 Server 고유 설정입니다.
        private const int serverProcSleepDelay = 1; // ms
        #endregion

        #region Memory Mapped File Instance
        private MemoryMappedFile mmf;
        private MemoryMappedViewAccessor mmfvaStruct;
        private MemoryMappedViewAccessor mmfvaHeap;
        private bool mmfIsCreated;
        private bool mmfvaIsCreated;
        public MemoryMappedViewAccessor structViewAccessor
        {
            get { return mmfvaStruct; }
            private set { mmfvaStruct = value; }
        }
        public MemoryMappedViewAccessor heapViewAccessor
        {
            get { return mmfvaHeap; }
            private set { mmfvaHeap = value; }
        }
        #endregion

        #region Memory Mapped File Method
        bool MemoryMappedFileInstanceIsReady()
        {
            return mmfIsCreated && mmfvaIsCreated;
        }
        #endregion

        #region Shared Memory
        // [Shared Memory 
        // [Heap][================Struct================]
        // [Heap][[------------Flag--------------][Data]]
        // [Heap][[ClientRecvFlag][ServerRecvFlag][Data]]
        //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HeartbeatFlagStruct
        {
            public byte bHeartbeat;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ClientRecvFlagStruct
        {
            //public byte bReply;
            public byte ExposeReply;
            public byte GrabReply;
            public byte ReadoutReply;
            public byte Inspect1Reply;
            public byte Inspect2Reply;
            public byte Inspect3Reply;
            public byte Inspect4Reply;
            public byte Inspect5Reply;
            public byte Inspect6Reply;
            public byte Inspect7Reply;
            public byte Inspect8Reply;
            public byte Inspect9Reply;
            public byte Inspect10Reply;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ServerRecvFlagStruct
        {
            //public byte bRequest;
            public byte ExposeRequest;
            public byte GrabRequest;
            public byte ReadoutRequest;
            public byte Inspect1Request;
            public byte Inspect2Request;
            public byte Inspect3Request;
            public byte Inspect4Request;
            public byte Inspect5Request;
            public byte Inspect6Request;
            public byte Inspect7Request;
            public byte Inspect8Request;
            public byte Inspect9Request;
            public byte Inspect10Request;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SharedMemoryFlagStruct
        {
            public HeartbeatFlagStruct heartbeatFlag;
            public ClientRecvFlagStruct clientRecvFlag;
            public ServerRecvFlagStruct serverRecvFlag;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SharedMemoryDataStructUnit
        {
            public double serverData1;
            public double serverData2;
            public double serverData3;
            public double serverData4;
            public double serverData5;
            public double serverData6;
            public bool serverData7;
            public double serverData8;
            public double serverData9;
            public double serverData10;
            public double clientData1;
            public double clientData2;
            public double clientData3;
            public double clientData4;
            public double clientData5;
            public double clientData6;
            public double clientData7;
            public double clientData8;
            public double clientData9;
            public double clientData10;
        }
        public struct SharedMemoryDataStruct
        {
            public SharedMemoryDataStructUnit InspectData1;
            public SharedMemoryDataStructUnit InspectData2;
            public SharedMemoryDataStructUnit InspectData3;
            public SharedMemoryDataStructUnit InspectData4;
            public SharedMemoryDataStructUnit InspectData5;
            public SharedMemoryDataStructUnit InspectData6;
            public SharedMemoryDataStructUnit InspectData7;
            public SharedMemoryDataStructUnit InspectData8;
            public SharedMemoryDataStructUnit InspectData9;
            public SharedMemoryDataStructUnit InspectData10;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SharedMemoryStruct
        {
            public SharedMemoryFlagStruct flag;
            public SharedMemoryDataStruct data;
        }
        public SharedMemoryStruct sharedMemoryStruct;
        private SharedMemoryFlagStruct lastFlag;
        public byte[] sharedMemoryHeap;

        long heartbeatFlagStructSize;
        long clientRecvFlagStructSize;
        long serverRecvFlagStructSize;
        long sharedMemoryFlagStructSize;
        long sharedMemoryDataStructSize;
        long sharedMemoryStructSize;

        public long GetSharedMemoryHeapSize()
        {
            return sharedMemoryHeapSize;
        }
        public long GetSharedMemoryStructSize()
        {
            return sharedMemoryStructSize;
        }
        #endregion

        #region Shared Memory Can Read Method
        bool SharedMemoryStructCanRead()
        {
            return MemoryMappedFileInstanceIsReady()
                && mmfvaStruct.CanRead
                && (sharedMemoryStructSize > 0);
        }
        bool SharedMemoryHeapCanRead()
        {
            return MemoryMappedFileInstanceIsReady()
                && mmfvaHeap.CanRead
                && (sharedMemoryHeapSize > 0)
                && (sharedMemoryHeap != null);
        }
        bool HeartbeatCanRead()
        {
            return SharedMemoryStructCanRead()
                && (heartbeatFlagStructSize > 0);
        }
        bool ClientRecvFlagCanRead()
        {
            return SharedMemoryStructCanRead()
                && (clientRecvFlagStructSize > 0);
        }
        bool ServerRecvFlagCanRead()
        {
            return SharedMemoryStructCanRead()
                && (serverRecvFlagStructSize > 0);
        }
        bool SharedMemoryFlagStructCanRead()
        {
            return SharedMemoryStructCanRead()
                && (sharedMemoryFlagStructSize > 0);
        }
        bool SharedMemoryDataStructCanRead()
        {
            return SharedMemoryStructCanRead()
                && (sharedMemoryDataStructSize > 0);
        }
        #endregion

        #region Shared Memory Can Write Method
        bool SharedMemoryStructCanWrite()
        {
            return MemoryMappedFileInstanceIsReady()
                && mmfvaStruct.CanWrite
                && (sharedMemoryStructSize > 0);
        }
        bool SharedMemoryHeapCanWrite()
        {
            return MemoryMappedFileInstanceIsReady()
                && mmfvaHeap.CanWrite
                && (sharedMemoryHeapSize > 0)
                && (sharedMemoryHeap != null);
        }
        bool HeartbeatCanWrite()
        {
            return SharedMemoryStructCanWrite()
                && (heartbeatFlagStructSize > 0);
        }
        bool ClientRecvFlagCanWrite()
        {
            return SharedMemoryStructCanWrite()
                && (clientRecvFlagStructSize > 0);
        }
        bool ServerRecvFlagCanWrite()
        {
            return SharedMemoryStructCanWrite()
                && (serverRecvFlagStructSize > 0);
        }
        bool SharedMemoryFlagStructCanWrite()
        {
            return SharedMemoryStructCanWrite()
                && (sharedMemoryFlagStructSize > 0);
        }
        bool SharedMemoryDataStructCanWrite()
        {
            return SharedMemoryStructCanWrite()
                && (sharedMemoryDataStructSize > 0);
        }
        #endregion

        public string except_str;

        void Initialize()
        {
            mmfIsCreated = false;
            mmfvaIsCreated = false;
            mutexIsCreated = false;
            //
            heartbeatFlagStructSize = -1;
            clientRecvFlagStructSize = -1;
            serverRecvFlagStructSize = -1;
            sharedMemoryFlagStructSize = -1;
            sharedMemoryDataStructSize = -1;
            sharedMemoryStructSize = -1;
            //
            except_str = "";
            //
            sharedMemoryHeap = null;
        }

        int GetSize<T>()
        {
            return Marshal.SizeOf(typeof(T));
        }

        void CheckHeartbeat()
        {
            // Server: heartbeat 0 -> 1
            HeartbeatRead();
            if (this.sharedMemoryStruct.flag.heartbeatFlag.bHeartbeat == (byte)0)
            {
                this.sharedMemoryStruct.flag.heartbeatFlag.bHeartbeat = 1;
                HeartbeatWrite();
            }
        }

        #region Server, Client 공용 Mutex
        private Mutex mutex;
        private bool mutexIsCreated;
        #endregion

        #region Constructor
        public SharedMemoryServer(string strName)
        {
            string memoryName = string.Empty;
            string mutexName = string.Empty;
            XytCoordinate alignPitch = new XytCoordinate(0.0, 0.0, 0.0);
            XytCoordinate matchResultCoordinate = new XytCoordinate(0.0, 0.0, 0.0);
            XytCoordinate alignResultCoordinate = new XytCoordinate(0.0, 0.0, 0.0);
            bool IsSuccess = false;
            Bitmap image = null;
            mutexIsCreated = false;
            Initialize();

            heartbeatFlagStructSize = GetSize<HeartbeatFlagStruct>();
            clientRecvFlagStructSize = GetSize<ClientRecvFlagStruct>();
            serverRecvFlagStructSize = GetSize<ServerRecvFlagStruct>();
            sharedMemoryFlagStructSize = GetSize<SharedMemoryFlagStruct>();
            sharedMemoryDataStructSize = GetSize<SharedMemoryDataStruct>();
            sharedMemoryStructSize = GetSize<SharedMemoryStruct>();

            memoryName = DefaultMemoryName + strName;
            
            try
            {
                mmf = MemoryMappedFile.CreateOrOpen(memoryName, memoryCapacity);
                mmfIsCreated = true;
                Log.Write("SharedMemoryServer", string.Format($"[INFO] memoryName : {memoryName}, IsCreated : {mmfIsCreated}"));
            }
            catch (Exception ex)
            {
                Initialize();
                except_str = ex.Message;
                return;
            }
            
            try
            {
                mmfvaStruct = mmf.CreateViewAccessor(sharedMemoryHeapSize, sharedMemoryStructSize);
            }
            catch (Exception ex)
            {
                Initialize();
                mmf.Dispose();
                except_str = ex.Message;
                return;
            }
            try
            {
                mmfvaHeap = mmf.CreateViewAccessor(0, sharedMemoryHeapSize);
            }
            catch (Exception ex)
            {
                Initialize();
                mmfvaStruct.Dispose();
                mmf.Dispose();
                except_str = ex.Message;
                return;
            }
            mmfvaIsCreated = true;

            // 공용 메모리 힙 영역 할당
            sharedMemoryHeap = new byte[sharedMemoryHeapSize];

            mutexName = DefaultMutexName + strName;

            // 공용 뮤텍스 생성
            try
            {
                mutex = new Mutex(false, mutexName, out mutexIsCreated);
                Log.Write("SharedMemoryServer", string.Format($"[INFO] mutexName : {mutexName}, IsCreated : {mutexIsCreated}"));
            }
            catch (Exception ex)
            {
                Initialize();
                mmfvaHeap.Dispose();
                mmfvaStruct.Dispose();
                mmf.Dispose();
                except_str = ex.Message;
                return;
            }
            //mutexIsCreated = true;


            // IPC Server Thread Run
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(serverProcSleepDelay);

                    // Server Doing This                   
                    mutex.WaitOne();
                    {
                        image = null;

                        alignPitch.X = 0.0;
                        alignPitch.Y = 0.0;
                        alignPitch.T = 0.0;

                        matchResultCoordinate.X = 0.0;
                        matchResultCoordinate.Y = 0.0;
                        matchResultCoordinate.T = 0.0;

                        alignResultCoordinate.X = 0.0;
                        alignResultCoordinate.Y = 0.0;
                        alignResultCoordinate.T = 0.0;

                        // HeartBeat
                        CheckHeartbeat();

                        // Read
                        SharedMemoryFlagRead();
                        SharedMemoryDataRead();
                        SharedMemoryHeapRead();

                        //if (!lastFlag.Equals(this.sharedMemoryStruct.flag))
                        {
                            // Server Do This ------------------------------------------------------------
                            // serverRecvFlag의 Request flag에 변화가 있을 경우 수행할 동작을 이곳에서 정의합니다.
                            //if(sharedMemoryStruct.flag.serverRecvFlag.bRequest != 0)
                            #region ExposeRequest
                            if (sharedMemoryStruct.flag.serverRecvFlag.ExposeRequest != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.ExposeRequest = 0;
                                ServerRecvFlagWrite();

                                if (this.OnExposeRequest != null)
                                    this.OnExposeRequest();

                                sharedMemoryStruct.flag.clientRecvFlag.ExposeReply = 1;
                                ClientRecvFlagWrite();
                            }
                            #endregion
                            #region GrabRequest
                            else if (sharedMemoryStruct.flag.serverRecvFlag.GrabRequest != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.GrabRequest = 0;
                                ServerRecvFlagWrite();

                                if (this.OnGrabRequest != null)
                                    this.OnGrabRequest(out image);

                                //sharedMemoryHeap[0] = image;
                                //SharedMemoryHeapWrite();
                                sharedMemoryStruct.flag.clientRecvFlag.GrabReply = 1;
                                ClientRecvFlagWrite();
                            }
                            #endregion
                            #region ReadoutRequest
                            else if (sharedMemoryStruct.flag.serverRecvFlag.ReadoutRequest != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.ReadoutRequest = 0;
                                ServerRecvFlagWrite();

                                if (this.OnReadoutRequest != null)
                                    this.OnReadoutRequest(out image);

                                //sharedMemoryHeap[0] = image;
                                //SharedMemoryHeapWrite();
                                sharedMemoryStruct.flag.clientRecvFlag.ReadoutReply = 1;
                                ClientRecvFlagWrite();
                            }
                            #endregion
                            #region Inspect1Request - First Position Align
                            else if (sharedMemoryStruct.flag.serverRecvFlag.Inspect1Request != 0)
                            {
                                Log.Write("SharedMemoryServer", string.Format($"[Inspect1Request] Start"));
                                sharedMemoryStruct.flag.serverRecvFlag.Inspect1Request = 0;
                                ServerRecvFlagWrite();

                                Log.Write("SharedMemoryServer", string.Format($"[Inspect1Request] Inspect Start"));
                                //First Align
                                if (this.OnInspect1 != null)
                                    this.OnInspect1(ref IsSuccess,ref matchResultCoordinate);
                                Log.Write("SharedMemoryServer", string.Format($"[Inspect1Request] Inspect End"));

                                sharedMemoryStruct.data.InspectData1.serverData1 = matchResultCoordinate.X;
                                sharedMemoryStruct.data.InspectData1.serverData2 = matchResultCoordinate.Y;
                                sharedMemoryStruct.data.InspectData1.serverData3 = matchResultCoordinate.T;
                                sharedMemoryStruct.data.InspectData1.serverData7 = IsSuccess;

                                Log.Write("SharedMemoryServer", string.Format($"[Inspect1Request]Data : matchResultCoordinate : {matchResultCoordinate.ToString()}, IsSuccess : {IsSuccess.ToString()}"));

                                SharedMemoryDataWrite();

                                sharedMemoryStruct.flag.clientRecvFlag.Inspect1Reply = 1;
                                ClientRecvFlagWrite();
                                Log.Write("SharedMemoryServer", string.Format($"[Inspect1Request] End"));
                            }
                            #endregion
                            #region Inspect2Request - Second Position Align
                            else if (sharedMemoryStruct.flag.serverRecvFlag.Inspect2Request != 0)
                            {
                                Log.Write("SharedMemoryServer", string.Format($"[Inspect2Request] Start"));
                                sharedMemoryStruct.flag.serverRecvFlag.Inspect2Request = 0;
                                ServerRecvFlagWrite();

                                alignPitch.X = sharedMemoryStruct.data.InspectData2.clientData1;
                                alignPitch.Y = sharedMemoryStruct.data.InspectData2.clientData2;
                                alignPitch.T = sharedMemoryStruct.data.InspectData2.clientData3;

                                Log.Write("SharedMemoryServer", string.Format($"[Inspect2Request] ClientData : {alignPitch}"));

                                Log.Write("SharedMemoryServer", string.Format($"[Inspect2Request] Inspect Start"));
                                //Second Align
                                if (this.OnInspect2 != null)
                                    this.OnInspect2(alignPitch,ref IsSuccess, ref matchResultCoordinate, ref alignResultCoordinate);
                                Log.Write("SharedMemoryServer", string.Format($"[Inspect2Request] Inspect End"));

                                sharedMemoryStruct.data.InspectData2.serverData1 = matchResultCoordinate.X;
                                sharedMemoryStruct.data.InspectData2.serverData2 = matchResultCoordinate.Y;
                                sharedMemoryStruct.data.InspectData2.serverData3 = matchResultCoordinate.T;

                                sharedMemoryStruct.data.InspectData2.serverData4 = alignResultCoordinate.X;
                                sharedMemoryStruct.data.InspectData2.serverData5 = alignResultCoordinate.Y;
                                sharedMemoryStruct.data.InspectData2.serverData6 = alignResultCoordinate.T;

                                sharedMemoryStruct.data.InspectData2.serverData7 = IsSuccess;
                                SharedMemoryDataWrite();

                                Log.Write("SharedMemoryServer", string.Format($"[Inspect2Request]Data : matchResultCoordinate : {matchResultCoordinate.ToString()}, alignResultCoordinate : {alignResultCoordinate.ToString()}, IsSuccess : {IsSuccess.ToString()}"));

                                sharedMemoryStruct.flag.clientRecvFlag.Inspect2Reply = 1;
                                ClientRecvFlagWrite();
                                Log.Write("SharedMemoryServer", string.Format($"[Inspect2Request] End"));
                            }
                            #endregion
                            #region Inspect3Request
                            else if (sharedMemoryStruct.flag.serverRecvFlag.Inspect3Request != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.Inspect3Request = 0;
                                ServerRecvFlagWrite();

                                if (this.OnInspect3 != null)
                                    this.OnInspect3();
                                sharedMemoryStruct.flag.clientRecvFlag.Inspect3Reply = 1;
                                ClientRecvFlagWrite();
                            }
                            #endregion
                            #region Inspect4Request
                            else if (sharedMemoryStruct.flag.serverRecvFlag.Inspect4Request != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.Inspect4Request = 0;

                                if (this.OnInspect4 != null)
                                    this.OnInspect4();

                                sharedMemoryStruct.flag.clientRecvFlag.Inspect4Reply = 1;
                                ClientRecvFlagWrite();
                            }
                            #endregion
                            #region Others
                            else if (sharedMemoryStruct.flag.serverRecvFlag.Inspect5Request != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.Inspect5Request = 0;
                                ServerRecvFlagWrite();

                                if (this.OnInspect5 != null)
                                    this.OnInspect5();

                                sharedMemoryStruct.flag.clientRecvFlag.Inspect5Reply = 1;
                                ClientRecvFlagWrite();
                            }
                            else if (sharedMemoryStruct.flag.serverRecvFlag.Inspect6Request != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.Inspect6Request = 0;
                                ServerRecvFlagWrite();

                                if (this.OnInspect6 != null)
                                    this.OnInspect6();

                                sharedMemoryStruct.flag.clientRecvFlag.Inspect6Reply = 1;
                                ClientRecvFlagWrite();
                            }
                            else if (sharedMemoryStruct.flag.serverRecvFlag.Inspect7Request != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.Inspect7Request = 0;
                                ServerRecvFlagWrite();

                                if (this.OnInspect7 != null)
                                    this.OnInspect7();

                                sharedMemoryStruct.flag.clientRecvFlag.Inspect7Reply = 1;
                                ClientRecvFlagWrite();
                            }
                            else if (sharedMemoryStruct.flag.serverRecvFlag.Inspect8Request != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.Inspect8Request = 0;
                                ServerRecvFlagWrite();

                                if (this.OnInspect8 != null)
                                    this.OnInspect8();

                                sharedMemoryStruct.flag.clientRecvFlag.Inspect8Reply = 1;
                                ClientRecvFlagWrite();
                            }
                            else if (sharedMemoryStruct.flag.serverRecvFlag.Inspect9Request != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.Inspect9Request = 0;
                                ServerRecvFlagWrite();

                                if (this.OnInspect9 != null)
                                    this.OnInspect9();

                                sharedMemoryStruct.flag.clientRecvFlag.Inspect9Reply = 1;
                                ClientRecvFlagWrite();
                            }
                            else if (sharedMemoryStruct.flag.serverRecvFlag.Inspect10Request != 0)
                            {
                                sharedMemoryStruct.flag.serverRecvFlag.Inspect10Request = 0;
                                ServerRecvFlagWrite();

                                if (this.OnInspect10 != null)
                                    this.OnInspect10();

                                sharedMemoryStruct.flag.clientRecvFlag.Inspect10Reply = 1;
                                ClientRecvFlagWrite();
                            }
                            #endregion
                            //----------------------------------------------------------------------------
                            lastFlag = this.sharedMemoryStruct.flag;
                        }
                    }
                    mutex.ReleaseMutex();
                }
            });
        }

        #region Destructor
        ~SharedMemoryServer()
        {
            if (mmfvaHeap != null)
                mmfvaHeap.Dispose();
            if (mmfvaStruct != null)
                mmfvaStruct.Dispose();
            if (mmf != null)
                mmf.Dispose();
            if (mutex != null)
            {
                mutex.Close();
                mutex.Dispose();
            }
            Initialize();
        }
        #endregion
        #endregion

        #region Read/Write Method
        #region Read/Write Method : Heartbeat Flag Struct
        public bool HeartbeatRead()
        {
            if (!HeartbeatCanRead())
                return false;
            try
            {
                mmfvaStruct.Read(0, out sharedMemoryStruct.flag.heartbeatFlag);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        public bool HeartbeatWrite()
        {
            if (!HeartbeatCanWrite())
                return false;
            try
            {
                mmfvaStruct.Write(0, ref sharedMemoryStruct.flag.heartbeatFlag);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        #endregion

        #region Read/Write Method : Client Receive Flag Struct
        public bool ClientRecvFlagRead()
        {
            if (!ClientRecvFlagCanRead())
                return false;
            try
            {
                long clientRecvFlagOffset = heartbeatFlagStructSize;
                mmfvaStruct.Read(clientRecvFlagOffset, out sharedMemoryStruct.flag.clientRecvFlag);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        public bool ClientRecvFlagWrite()
        {
            if (!ClientRecvFlagCanWrite())
                return false;
            try
            {
                long clientRecvFlagOffset = heartbeatFlagStructSize;
                mmfvaStruct.Write(clientRecvFlagOffset, ref sharedMemoryStruct.flag.clientRecvFlag);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        #endregion

        #region Read/Write Method : Server Receive Flag Struct
        public bool ServerRecvFlagRead()
        {
            if (!ServerRecvFlagCanRead())
                return false;
            try
            {
                long serverRecvFlagOffset = heartbeatFlagStructSize + clientRecvFlagStructSize;
                mmfvaStruct.Read(serverRecvFlagOffset,
                    out sharedMemoryStruct.flag.serverRecvFlag);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        public bool ServerRecvFlagWrite()
        {
            if (!ClientRecvFlagCanWrite())
                return false;
            try
            {
                long serverRecvFlagOffset = heartbeatFlagStructSize + clientRecvFlagStructSize;
                mmfvaStruct.Write(serverRecvFlagOffset,
                    ref sharedMemoryStruct.flag.serverRecvFlag);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }


        #endregion

        #region Read/Write Method : Shared Memory Flag Struct
        public bool SharedMemoryFlagRead()
        {
            if (!SharedMemoryFlagStructCanRead())
                return false;
            try
            {
                mmfvaStruct.Read(0, out sharedMemoryStruct.flag);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        public bool SharedMemoryFlagWrite()
        {
            if (!SharedMemoryFlagStructCanWrite())
                return false;
            try
            {
                mmfvaStruct.Write(0, ref sharedMemoryStruct.flag);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        #endregion

        #region Read/Write Method : Shared Memory Data Struct
        public bool SharedMemoryDataRead()
        {
            if (!SharedMemoryDataStructCanRead())
                return false;
            try
            {
                mmfvaStruct.Read(sharedMemoryFlagStructSize,
                   out sharedMemoryStruct.data);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        public bool SharedMemoryDataWrite()
        {
            if (!SharedMemoryDataStructCanWrite())
                return false;
            try
            {
                mmfvaStruct.Write(sharedMemoryFlagStructSize,
                    ref sharedMemoryStruct.data);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        #endregion

        #region Read/Write Method : Shared Memory Heap Data
        public bool SharedMemoryHeapRead()
        {
            if (!SharedMemoryHeapCanRead())
                return false;
            try
            {
                mmfvaHeap.ReadArray(0, sharedMemoryHeap,
                                    0, sharedMemoryHeap.Length);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        public bool SharedMemoryHeapWrite()
        {
            if (!SharedMemoryHeapCanWrite())
                return false;
            try
            {
                mmfvaHeap.WriteArray(0, sharedMemoryHeap,
                                     0, sharedMemoryHeap.Length);
            }
            catch (Exception ex)
            {
                except_str = ex.Message;
                return false;
            }
            return true;
        }
        #endregion
        #endregion
    }
}
