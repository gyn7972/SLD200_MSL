using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Threading;
using CEPHEUSImport;
using System.Windows.Forms;
using System.Diagnostics;

namespace qmc.cepheus
{
    class Program
    {
        public static ServiceHost host;
        public static ManualResetEvent manualResetEvent;
        static void Main(string[] args)
        {
            // Address 
            string address = "net.tcp://localhost:8080/myAddress";
            // Binding : TCP 사용
            NetTcpBinding binding = new NetTcpBinding();
            binding.SendTimeout = new TimeSpan(0, 5, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
            // Service Host 만들기
            host = new ServiceHost(typeof(MyService));
            // End Point 추가
            host.AddServiceEndpoint(typeof(IMyContract), binding, address);
            // Service Host 시작
            host.Open();

            manualResetEvent = new ManualResetEvent(false);
            manualResetEvent.WaitOne();
        }

        [ServiceContract]
        public interface IMyContract
        {
            [OperationContract]
            int SetArm();

            [OperationContract]
            int ResetArm();

            [OperationContract]
            int SetRepRate(int repRate);

            [OperationContract]
            int GetRepRate(ref int repRate);

            [OperationContract]
            int SetPower(int power);

            [OperationContract]
            int GetPower(ref int power);

            [OperationContract]
            int GetStatus(ref int status);

            //[OperationContract]
            //int GetTemperature(out double ld1Temp, out double ld2Temp, out double ld3Temp, out double paTemp);

            //[OperationContract]
            //int GetTemperatureState(out int ld1, out int ld2, out int ld3, out int pa);

            [OperationContract]
            int Initialize();

            [OperationContract]
            void ShutDown();
        }

        // 실제로 Client 에서 호출될 함수
        public class MyService : IMyContract
        {
            public int SetArm()
            {
                int nRet = 0;
                nRet = CepheusWrap.cp_SetArmLaser();
                return nRet;
            }

            public int ResetArm()
            {
                int nRet = 0;
                nRet = CepheusWrap.cp_ResetArmLaser();
                return nRet;
            }

            public int SetRepRate(int repRate)
            {
                int nRet = 0;
                nRet = CepheusWrap.cp_SetRepRate(repRate);
                return nRet;
            }

            public int GetRepRate(ref int repRate)
            {
                int nRet = 0;
                nRet = CepheusWrap.cp_GetRepRate(ref repRate);
                return nRet;
            }

            public int SetPower(int power)
            {
                int nRet = 0;
                nRet = CepheusWrap.cp_SetPower(power);
                return nRet;
            }

            public int GetPower(ref int power)
            {
                int nRet = 0;
                power = 0;
                nRet = CepheusWrap.cp_GetPower(ref power);
                return nRet;
            }

            public int GetStatus(ref int status)
            {
                int nRet = 0;
                status = 0;
                nRet = CepheusWrap.cp_GetStatus(ref status);
                return nRet;
            }

            //public int GetTemperature(out double ld1Temp, out double ld2Temp, out double ld3Temp, out double paTemp)
            //{
            //    int nRet = 0;
            //    ld1Temp = ld2Temp = ld3Temp = paTemp = 0.0;
            //    nRet = CepheusWrap.cp_GetTemperature(ref ld1Temp, ref ld2Temp, ref ld3Temp, ref paTemp);
            //    return nRet;
            //}

            //public int GetTemperatureState(out int ld1, out int ld2, out int ld3, out int pa)
            //{
            //    int nRet = 0;
            //    ld1 = ld2 = ld3 = pa = 0;
            //    nRet = CepheusWrap.cp_GetTemperature_State(ref ld1, ref ld2, ref ld3, ref pa);
            //    return nRet;
            //}

            public int Initialize()
            {
                int nRet = 0;
                nRet = CepheusWrap.cp_InitCtrlDll();
                return nRet;
            }

            public void ShutDown()
            {
                CepheusWrap.cp_ShutDown();
                manualResetEvent.Set();
            }
        }
    }
}
