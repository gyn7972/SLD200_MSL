using ACS.SPiiPlusNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.Motion.ACS.Motion
{
    public class AcsAxis : MotionAxis
    {
        public AcsAxisConfiguration Config { get; set; }
        public enum ConnectTypeKey
        {
            Serial,
            Ethernet,
            PCI,
            Simulator,
        }
        private Api ACS; 
        private bool m_bConnected;
        private ConnectTypeKey ConnectType;
        private ManualResetEvent DoMonitorEvent;
        private MotorStates m_nMotorState;
        private double m_dCommnadPos;
        private double m_dMaxVelocity;
        public AcsAxis()
        {
            m_bConnected = false;
            ConnectType = ConnectTypeKey.Serial;
            ACS = new Api();
            DoMonitorEvent = new ManualResetEvent(false);
        }
        public override void Open()
        {
            base.Open();
            if (!m_bConnected)
            {
                try
                {
                    if (ConnectType == ConnectTypeKey.Serial)                  //  Serial
                    {
                        int nBaud_Rate = 9600;
                        string strCommPort = "";

                        //if ((string)BaudRateCmB.SelectedItem == "Auto")
                        //    Baud_Rate = -1;
                        //else
                        //    Baud_Rate = Convert.ToInt32(BaudRateCmB.SelectedItem);

                        // Open serial communuication.
                        // CommPortCmB.SelectedIndex + 1 defines the COM port.
                        ACS.OpenCommSerial(1, nBaud_Rate);
                    }
                    else if (ConnectType == ConnectTypeKey.Ethernet)             //  Ethernet
                    {
                        int nProtocol;
                        int nConnType = 0;
                        string RemoteAddressTB = "172.16.0.12";
                        if (nConnType == 0)             //  Point to Point
                        {
                            nProtocol = (int)EthernetCommOption.ACSC_SOCKET_DGRAM_PORT;
                        }
                        else
                        {
                            nProtocol = (int)EthernetCommOption.ACSC_SOCKET_STREAM_PORT;
                        }
                        // Open ethernet communuication.
                        // RemoteAddress.Text defines the controller's TCP/IP address.
                        // Protocol is TCP/IP in case of network connection, and UDP in case of point-to-point connection.
                        ACS.OpenCommEthernet(RemoteAddressTB, nProtocol);
                    }
                    else if (ConnectType == ConnectTypeKey.PCI)             //  PCI
                    {

                        //Open PCI Bus communuication
                        int SlotNumber = -1;
                        ACS.OpenCommPCI(SlotNumber);
                    }
                    else //(CComTypeCmB.SelectedIndex == 3)
                         //Open communuication with Simulator
                        ACS.OpenCommSimulator();

                    // Resume MotorState Thread 
                    DoMonitorEvent.Set();
                    // Set connection bit to true
                    m_bConnected = true;
                    // Make ConnInd green 
                    //ConnIndPB.Image = Green;

                    // Save new communication media
                    //ComTypeOld = ComTypeCmB.SelectedIndex;

                }
                catch (COMException Ex)
                {
                    ErorMsg(Ex);			//  Throw exception if this fails
                }
                catch (ACSException Ex)
                {
                    ErorMsg(Ex);			//  Throw exception if this fails
                }
            }
        }
        public override void Close()
        {
            base.Close();
            ACS.CloseComm();
        }
        public override int Stop(double dDeccel)
        {
            int ret = 0;
            return ret;
        }
        public override int Stop()
        {
            int ret = 0;
            try
            {
                ACS.Halt((Axis)this.No);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ret = -1;
            }
            return ret;
        }
        public override int StopEmergency()
        {
            int ret = 0;
            ACS.RegisterEmergencyStop();
            return ret;
        }

        public override int GetAxisState(ref AxisState axisState)
        {
            int ret = 0;
            m_nMotorState = ACS.GetMotorState((Axis)this.No);
            SafetyControlMasks fault = ACS.GetFault((Axis)this.No);
            if(fault != SafetyControlMasks.ACSC_NONE)
            {
                ret = 3;
            }
            else if ((m_nMotorState & MotorStates.ACSC_MST_MOVE) != 0) //각 상황별로 State 넣어줄것
            {
                ret = 1;
            }
            ACS.GetMotionError((Axis)0);//GetErrorCode....

            //else if ((m_nMotorState & MotorStates.ACSC_MST_INPOS) != 0)
            //{
            //    ret = 0;
            //}
            return ret;
        }

        public override int GetCommandPosition(ref double pulse)// 허허.....
        {
            int ret = 0;
            pulse = m_dCommnadPos;
            return ret;
        }

        public override int MovePosition(double dPosition, double dVelocity, double dAccel, double dDeccel)
        {
            int ret = 0;
            ACS.SetVelocityImm((Axis)this.No, dVelocity); 
            ACS.SetAccelerationImm((Axis)this.No, dAccel); 
            ACS.SetDecelerationImm((Axis)this.No, dDeccel); 
            
            try
            {
                if (dPosition > 0)
                {
                    
                    ACS.ToPoint(
                        0,                          // '0' - Absolute position
                        (Axis)this.No,  // Axis number
                        dPosition                 // Target position
                        );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return ret;
        }

        public override int GetAmpFault(ref bool bAmpFault)
        {
            int ret = 0;
            SafetyControlMasks fault = ACS.GetFault((Axis)this.No);
            if (fault != SafetyControlMasks.ACSC_NONE)
            {
                ret = -1;
                bAmpFault = true;
            }
            return ret;
        }

        public override int Load(FileStream fs) //????
        {
            throw new NotImplementedException();
        }

        public override int MoveVelocity(double dVelocity, double dAccel, double dDeccel)
        {
            int ret = 0;
            try
            {
                ACS.Jog(MotionFlags.ACSC_AMF_VELOCITY, (Axis)this.No, dVelocity);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ret = -1;
            }
            return ret;
        }

        public override int GetActualPosition(ref double pulse) //허허
        {
            int ret = 0;

            pulse = ACS.GetRPosition((Axis)this.No); // Get Reference Position

            return ret;
        }

        public override int Clear() //허허...
        {
            int ret = 0;
            
            return ret;
        }
    
        public override int GetEnable(ref bool bEnable)
        {
            int ret = 0;
            m_nMotorState = ACS.GetMotorState((Axis)this.No);
            if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)
            {
                bEnable = true;
            }
            else
            {
                bEnable = false;
            }

            return ret;
        }
        public override int SetEnable(bool bEnable)
        {
            int ret = 0;
            if(bEnable)
            {
                ACS.Enable((Axis)this.No);
            }
            else
            {
                ACS.Disable((Axis)this.No);
            }
            return ret;
        }
      
        public override int ModifyPosition(double position, double velocity, double acceleration, double deceleration) // 허허..
        {
            throw new NotImplementedException();
        }

        public override int ModifyVelocity(double velocity, double acceleration, double deceleration) // 허허...
        {
            throw new NotImplementedException();
        }
        public override int Reset()
        {
            int ret = 0;
            ACS.FaultClear((Axis)this.No);
            return ret;
        }

        public override int SetActualPosition(double dPosition)
        {
            int ret = 0;
            try
            {
                ACS.SetRPosition((Axis)this.No, dPosition);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ret = -1;
            }
            return ret;
        }

        public override int SetCommandPosition(double dPosition)//허허...
        {
            int ret = 0;
            m_dCommnadPos = dPosition;
            return ret;
        }

        public override int WaitMotionDone(double dTimeout)
        {
            int ret = 0;
            ACS.WaitMotionEnd((Axis)this.No, (int)dTimeout);
            return ret;
        }
        private int WaitMotionDone(bool checkStatus, double dTimeOut)
        {
            int ret = 0;
            bool done = false;
            bool ampFaultValue = false;
            DateTime startTime = DateTime.Now;
            TimeSpan current;
            double dCommand = 0;
            double dActual = 0;
            this.GetCommandPosition(ref dCommand);

            while (true)
            {
                Thread.Sleep(1);
                if (Simulated)
                {
                    
                    if (ACS.GetRPosition((Axis)this.No) == m_dCommnadPos)
                    {
                        break;
                    }
                }
                else
                {
                    this.GetCommandPosition(ref dCommand);
                    this.GetActualPosition(ref dActual);
                    if ((ret = this.GetMotionDone(ref done)) != 0) return ret;
                    if (checkStatus == true)
                    {
                        if ((ret = this.GetAmpFault(ref ampFaultValue)) != 0) return ret;
                        if (ampFaultValue)
                        {
                            //this.WriteLog(LogLevel.Highest, "AjinAxis.WaitMotionDone() AmpFaultDetected");
                            Console.WriteLine("ACS.WaitMotionDone() AmpFaultDetected");
                            return -1;
                        }
                    }
                    if (done && Math.Abs(dCommand - dActual) <= 0.004) break; //허허...확인필요
                }

                current = DateTime.Now - startTime;
                if (dTimeOut > 0 && current.TotalMilliseconds > dTimeOut)
                {
                    Console.WriteLine("ACS.WaitMotionDone() Timeout");
                    return -1;
                }
            }

            return ret;
        }
        public override int GetIsHomeSensor(ref bool bEnable)
        {
            int ret = 0;
            if (this.Simulated == false)
            {
                //ret = AXM.GetHomeSensorValue(this.No, ref bEnable);
            }
            return ret;
        }

        protected override int GetInPosition(ref bool value)
        {
            int ret = -1;

            m_nMotorState = ACS.GetMotorState((Axis)this.No);
            if ((m_nMotorState & MotorStates.ACSC_MST_INPOS) != 0)
            {
                ret = 0;
            }

            return ret;
        }

        protected override int GetMaxVelocity(ref double pulse) //????
        {
            //return AXM.GetMaxVelocity(this.No, ref pulse);
            pulse = m_dMaxVelocity;
            return 0;
        }

        protected override int GetPositionError(ref double pulse)
        {
            //return AXM.GetPositionError(this.No, ref pulse);

            return 0;
        }

        public override int GetIsNegativeLimit(ref bool bEnable) // 허허....
        {
            int ret = 0;
            if (this.Simulated == false)
            {
                //ret = AXM.GetNegativeLimitValue(this.No, ref bEnable);
            }
            return ret;
        }

        public override int GetIsPositiveLimit(ref bool bEnable) // 허허....
        {
            int ret = 0;
            if (this.Simulated == false)
            {
                //ret = AXM.GetPositiveLimitValue(this.No, ref bEnable);
            }
            return ret;
        }
        protected override int SetMaxVelocity(double pulse)
        {
            int ret = 0;

            //if ((ret = AXM.SetMaxVelocity(this.No, pulse)) != 0) return ret;
            m_dMaxVelocity = pulse;

            return ret;
        }

        protected override int OnHoming(HomingSpecification specification)
        {
            int ret = 0;

            ACS.RunBuffer((ProgramBuffer)0, null);
            ACS.RunBuffer((ProgramBuffer)2, null);

            return ret;
        }

        protected override int GetMotionDone(ref bool done)
        {
            int ret = 1;
            bool value = done ? false : true;
            m_nMotorState = ACS.GetMotorState((Axis)this.No);
            if ((m_nMotorState & MotorStates.ACSC_MST_INPOS) != 0)
            {
                ret = 0;
            }
            
            return ret;
        }

        protected override int GetVelocity(ref double pulse)
        {
            int ret = 0;
            ACS.GetVelocity((Axis)this.No);
            return ret;
        }
        private void ErorMsg(COMException Ex)
        {
            string Str = "Error from " + Ex.Source + "\n\r";
            Str = Str + Ex.Message + "\n\r";
            Str = Str + "HRESULT:" + String.Format("0x{0:X}", (Ex.ErrorCode));
            MessageBox.Show(Str, "EnableEvent");
        }
        private void ErorMsg(ACSException Ex)
        {
            string Str = "Error from " + Ex.Source + "\n\r";
            Str = Str + Ex.Message + "\n\r";
            Str = Str + "HRESULT:" + String.Format("0x{0:X}", (Ex.ErrorCode));
            MessageBox.Show(Str, "EnableEvent");
        }

        //public override int SetInPositionEnable(bool enable)
        //{
        //    throw new NotImplementedException();
        //}

        //public override int SetInPositionRange(double dInposRange)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
