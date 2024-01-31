/*
 * Purpose
 * 
 * Revision
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;


namespace QMC.Common.Motion.Ajin.IO
{
    public class DOFunction : DioModule
    {
        #region Enum List

        public enum ConveyorPart : int
        {
            LoadingZone = 0,
            DispenseZone,
            RivetInspZone,
            MountingZone,
            MountInspZone
        }

        public enum ConveyorDir : int
        {
            DirCW = 0,
            DirCCW
        }

        public enum ReelDir : int
        {
            DirCW = 0,
            DirCCW
        }

        public enum DispenserSignal : int
        {
            DIS = 0,
            TMS_PRESET,
            TDM_STEP,
            CHS,
            CD01,
            CD02,
            CD04,
            CD08,
            CD10,
            CD20,
            CD40,
            CD80
        }

        public enum DigitalContactSensorSignal : int
        {
            PRESET = 0,
            BANK_A,
            TIMING,
            BANK_B,
            RESET
        }

        public enum VisionUnit : int
        {
            Vision1 = 0,
            Vision2
        }

        #endregion


        #region Field
        private AXT_MODULE m_ModuleType;
        #endregion

        #region Constructor
        public DOFunction() : base()
        {
            this.Configuration = new AjinAxlDioModuleConfiguration();
        }
        #endregion

        #region Property
        public AXT_MODULE ModuleType
        {
            get { return this.m_ModuleType; }
        }
        #endregion

        #region Method
        private void GetPortBit(int address, ref int port, ref int bit)
        {
            port = (int)(address / 8);
            bit = address - (port * 8);
        }

        private void InterruptProcedure(int acitiveNo, uint flag)
        {
            int ret = 0;
            int address = 0;
            DigitalEdge edge = DigitalEdge.Up;
            bool value = false;
            byte[] bytes = null;

            // get address and edge
            bytes = BytesConverter.ToBytes(flag);
            Array.Reverse(bytes);
            for (int i = 0; i < this.Configuration.InputCount; i++)
            {
                if (bytes[i] != 0x00)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((bytes[i] & ((byte)0x01 << j)) == 0x00) continue;
                        // get edge
                        address = 8 * i + j;
                        if ((ret = AXD.Read((int)this.Configuration.No, address, ref value)) != 0) continue;
                        edge = value == true ? DigitalEdge.Up : DigitalEdge.Down;
                        this.FireInterrupt(address, edge);
                    }
                }
            }
        }
        #endregion

        #region DioModule Members
 
        protected override int OnRead(ref byte[] values)
        {
            int ret = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if ((ret = AXD.Read((int)this.Configuration.No, i, ref values[i])) != 0) return ret;
            }

            return ret;
        }

        protected override int OnReadOutput(ref byte[] values)
        {
            int ret = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if ((ret = AXD.ReadOutput((int)this.Configuration.No, i, ref values[i])) != 0) return ret;
            }

            return ret;
        }

        protected override int OnWrite(byte[] values)
        {
            int ret = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if ((ret = AXD.Write((int)this.Configuration.No, i, values[i])) != 0) return ret;
            }

            return ret;
        }

        protected override int OnClose()
        {
            return 0;
        }

        protected override int OnOpen()
        {
            int ret = 0;

            AjinAxlIoBoard board = this.Board as AjinAxlIoBoard;
            int node, moduleId;

            if (board == null)
                throw new ArgumentNullException("AjinDioModule.Board", "Board type is not AjinIoBoard");

            // get module information
            node = moduleId = 0;
            this.m_ModuleType = AXT_MODULE.AXT_SIO_DI32;
            if ((ret = AXD.GetModuleInformation((int)this.Configuration.No, ref node, ref moduleId, ref this.m_ModuleType)) != 0) return ret;
            if ((ret = AXD.SetInterruptEnabled((int)this.Configuration.No, false)) != 0) return ret;

            return ret;
        }

        public override bool SupportsInterrupt
        {
            get { return true; }
        }

        protected override int OnResetInterrupt(int address, DigitalEdge edge)
        {
            int ret = 0;
            if ((ret = AXD.SetInterruptEdge((int)this.Configuration.No, address, false, edge)) != 0) return ret;
            return ret;
        }

        protected override int OnSetInterrupt(int address, DigitalEdge edge)
        {
            int ret = 0;
            bool enabled = false;
            uint handleEvent = 0;

            if ((ret = AXD.GetInterruptEnabled((int)this.Configuration.No, ref enabled)) != 0) return ret;
            if (enabled == false)
            {
                if ((ret = AXD.SetInterruptModule((int)this.Configuration.No, IntPtr.Zero, 0, new CAXHS.AXT_INTERRUPT_PROC(this.InterruptProcedure), ref handleEvent)) != 0) return ret;
                if ((ret = AXD.SetInterruptEnabled((int)this.Configuration.No, true)) != 0) return ret;
            }
            if ((ret = AXD.SetInterruptEdge((int)this.Configuration.No, address, true, edge)) != 0) return ret;

            return ret;
        }

        public override int Load(FileStream fs)
        {
            int ret = 0;
            int nPointCount = 0;
            AjinAxlDioModuleConfiguration configuration = new AjinAxlDioModuleConfiguration();
            if ((ret = SaveManager.BinaryDeserialize<AjinAxlDioModuleConfiguration>(fs, out configuration)) != 0) return ret; ;
            this.Configuration = configuration;
            nPointCount = this.Configuration.PointCount;

            for (int i = 0; i < nPointCount; i++)
            {
                DioPoint point = new DioPoint();
                point.Load(fs);
                point.SetModule(this);
                this.Points.Add(point);
            }
            return ret;

        }
        #endregion
        public int GetPointCount()
        {
            return Points.Count;
        }

        public IOPoint GetPoint(int nIndex)
        {
            IOPoint point = null;
            if (VerifyPointIndex(nIndex))
            {
                point = Points[nIndex] as IOPoint;
            }

            return point;
        }

        public bool VerifyPointIndex(int nIndex)
        {
            bool bRet = false;

            if (Points.Count > nIndex && nIndex >= 0)
            {
                bRet = true;
            }

            return bRet;
        }


        #region DO Functions

        public bool DO_StartLamp( bool m_bOnOff )
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_StopLamp(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_ResetLamp(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_TowerLampRed(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_TowerLampYellow(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_TowerLampGreen(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_TowerLampBuzzer(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_TapingIonizer(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_ConveyorRun(int m_nConveyorPart, int m_nDir, bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴
            switch (m_nConveyorPart)
            {
                case (int)ConveyorPart.LoadingZone:
                    if (m_nDir == (int)ConveyorDir.DirCW)
                        bRet = true;
                    else
                        bRet = true;
                    break;

                case (int)ConveyorPart.DispenseZone:
                    if (m_nDir == (int)ConveyorDir.DirCW)
                        bRet = true;
                    else
                        bRet = true;
                    break;

                case (int)ConveyorPart.RivetInspZone:
                    if (m_nDir == (int)ConveyorDir.DirCW)
                        bRet = true;
                    else
                        bRet = true;
                    break;

                case (int)ConveyorPart.MountingZone:
                    if (m_nDir == (int)ConveyorDir.DirCW)
                        bRet = true;
                    else
                        bRet = true;
                    break;

                case (int)ConveyorPart.MountInspZone:
                    if (m_nDir == (int)ConveyorDir.DirCW)
                        bRet = true;
                    else
                        bRet = true;
                    break;
            }

            return bRet;
        }

        public bool DO_SMEMA_WorkReady(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_SMEMA_WorkEnd(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_ConveyorZoneStopperUpDown(int m_nConveyorPart, bool m_bUpDown)
        {
            bool bRet = false;

            //  복동 실린더 couple 로 동작

            //  해당 채널 출력 성공 여부 리턴
            switch (m_nConveyorPart)
            {
                case (int)ConveyorPart.LoadingZone:
                    bRet = true;
                    break;

                case (int)ConveyorPart.DispenseZone:
                    bRet = true;
                    break;

                case (int)ConveyorPart.RivetInspZone:
                    bRet = true;
                    break;

                case (int)ConveyorPart.MountingZone:
                    bRet = true;
                    break;

                case (int)ConveyorPart.MountInspZone:
                    bRet = true;
                    break;
            }

            return bRet;
        }

        public bool DO_ConveyorZoneClampUpDown(int m_nConveyorPart, bool m_bUpDown)
        {
            bool bRet = false;

            //  복동 실린더 couple 로 동작

            //  해당 채널 출력 성공 여부 리턴
            switch (m_nConveyorPart)
            {
                case (int)ConveyorPart.LoadingZone:            //  Loading Zone 에는 Clamp 가 없음.
                    bRet = false;
                    break;

                case (int)ConveyorPart.DispenseZone:
                    bRet = true;
                    break;

                case (int)ConveyorPart.RivetInspZone:
                    bRet = true;
                    break;

                case (int)ConveyorPart.MountingZone:
                    bRet = true;
                    break;

                case (int)ConveyorPart.MountInspZone:
                    bRet = true;
                    break;
            }

            return bRet;
        }

        public bool DO_DigitalContactSensorAirOnOff(bool m_bOnOff)
        {
            bool bRet = false;

            //  Air Signal, couple 로 동작

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_LEDPickerVacuum(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_LEDPickerBlow(bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴


            return bRet;
        }

        public bool DO_ReelTapeRun(int m_nDir, bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴
            if (m_nDir == (int)ReelDir.DirCW)
                bRet = true;
            else
                bRet = true;

            return bRet;
        }

        public bool DO_CoverTapeRun(int m_nDir, bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴
            if (m_nDir == (int)ReelDir.DirCW)
                bRet = true;
            else
                bRet = true;

            return bRet;
        }

        public bool DO_Dispenser(int m_nDispenserSig, bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴
            switch (m_nDispenserSig)
            {
                case (int)DispenserSignal.DIS:
                    bRet = true;
                    break;

                case (int)DispenserSignal.TMS_PRESET:
                    bRet = true;
                    break;

                case (int)DispenserSignal.TDM_STEP:
                    bRet = true;
                    break;

                case (int)DispenserSignal.CHS:
                    bRet = true;
                    break;

                case (int)DispenserSignal.CD01:
                    bRet = true;
                    break;

                case (int)DispenserSignal.CD02:
                    bRet = true;
                    break;

                case (int)DispenserSignal.CD04:
                    bRet = true;
                    break;

                case (int)DispenserSignal.CD08:
                    bRet = true;
                    break;

                case (int)DispenserSignal.CD10:
                    bRet = true;
                    break;

                case (int)DispenserSignal.CD20:
                    bRet = true;
                    break;

                case (int)DispenserSignal.CD40:
                    bRet = true;
                    break;

                case (int)DispenserSignal.CD80:
                    bRet = true;
                    break;
            }

            return bRet;
        }

        public bool DO_DigitalContactSensor(int m_nContactSensor, bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 출력 성공 여부 리턴
            switch (m_nContactSensor)
            {
                case (int)DigitalContactSensorSignal.PRESET:
                    bRet = true;
                    break;

                case (int)DigitalContactSensorSignal.BANK_A:
                    bRet = true;
                    break;

                case (int)DigitalContactSensorSignal.TIMING:
                    bRet = true;
                    break;

                case (int)DigitalContactSensorSignal.BANK_B:
                    bRet = true;
                    break;

                case (int)DigitalContactSensorSignal.RESET:
                    bRet = true;
                    break;
            }

            return bRet;
        }

        public bool DO_VisionReset(int m_nVisionUnit, bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 상태 리턴
            switch (m_nVisionUnit)
            {
                case (int)VisionUnit.Vision1:
                    bRet = true;

                    break;

                case (int)VisionUnit.Vision2:
                    bRet = true;

                    break;
            }

            return bRet;
        }

        public bool DO_VisionTrigger(int m_nVisionUnit, bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 상태 리턴
            switch (m_nVisionUnit)
            {
                case (int)VisionUnit.Vision1:
                    bRet = true;

                    break;

                case (int)VisionUnit.Vision2:
                    bRet = true;

                    break;
            }

            return bRet;
        }

        public bool DO_VisionInspDataRequest(int m_nVisionUnit, bool m_bOnOff)
        {
            bool bRet = false;

            //  해당 채널 상태 리턴
            switch (m_nVisionUnit)
            {
                case (int)VisionUnit.Vision1:
                    bRet = true;

                    break;

                case (int)VisionUnit.Vision2:
                    bRet = true;

                    break;
            }

            return bRet;
        }

        #endregion
    }
}
