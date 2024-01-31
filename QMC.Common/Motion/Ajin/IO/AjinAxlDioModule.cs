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
    [Serializable]
    public class AjinAxlDioModule : DioModule
    {
        #region Field
        private AXT_MODULE m_ModuleType;
        #endregion

        #region Constructor
        public AjinAxlDioModule() : base()
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

            IsOpen = true;
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
    }

    //public class AjinAxlDioBuffer : DioBuffer
    //{
    //    #region DioBuffer Members
    //    protected override byte[] OnGetValues()
    //    {
    //        int ret = 0;
    //        byte[] values = new byte[this.Owner.Configuration.OutputCount];

    //        for (int i = 0; i < values.Length; i++)
    //        {
    //            if ((ret = AXD.ReadOutput(this.Owner.Configuration.No, i, ref values[i])) != 0) return values;
    //        }

    //        return values;
    //    }

    //    protected override void OnSetValues(byte[] values)
    //    {
    //        int ret = 0;

    //        for (int i = 0; i < values.Length; i++)
    //        {
    //            if ((ret = AXD.Write(this.Owner.Configuration.No, i, values[i])) != 0) return;
    //        }
    //    }
    //    #endregion
    //}
}
