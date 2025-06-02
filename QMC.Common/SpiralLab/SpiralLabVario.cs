using System;
using System.Numerics;
using SpiralLab.Sirius;

namespace QMC.Common.Parts
{
    public class SpiralLabVario : Part
    {
        public bool IsInitialized { get; private set; } = false;

        #region Properties
        public IRtc Rtc { get; private set; }
        public IRtc3D Rtc3D { get; private set; }
        public string ScannerName { get; private set; }
        #endregion

        public SpiralLabVario(string name, IRtc rtcInstance) : base(name)
        {
            this.ScannerName = name;
            this.Rtc = rtcInstance ?? throw new ArgumentNullException(nameof(rtcInstance));
            this.Rtc3D = rtcInstance as IRtc3D ?? throw new InvalidCastException("IRtc 인스턴스는 IRtc3D를 구현해야 합니다.");

            IsInitialized = true;
        }

        public override int Create()
        {
            return base.Create();
        }

        public bool MoveZAbsolute(float zMm)
        {
            return Rtc3D.CtlMove(new Vector3(0, 0, zMm));
        }

        public bool SetZOffset(float zOffset)
        {
            return Rtc3D.CtlZOffset(zOffset);
        }

        public bool SetZDefocus(float zDefocus)
        {
            return Rtc3D.CtlZDefocus(zDefocus);
        }

        public float GetCurrentZOffset() => Rtc3D.ZOffset;
        public float GetCurrentZDefocus() => Rtc3D.ZDefocus;

        public bool LoadZTable(float coefA, float coefB, float coefC)
        {
            return Rtc3D.CtlLoadZTable(coefA, coefB, coefC);
        }

        public bool ReadZTableFromCorrectionFile(string filePath, out double coefA, out double coefB, out double coefC)
        {
            return Rtc3D.CtlCorrectionReadABC(filePath, out coefA, out coefB, out coefC);
        }

        public void ListZOffset(float zOffset)
        {
            Rtc3D.ListZOffset(zOffset);
        }

        public void ListZDefocus(float zDefocus)
        {
            Rtc3D.ListZDefocus(zDefocus);
        }

        public void ListJump3D(float x, float y, float z, float rampFactor = 1.0f)
        {
            Rtc3D.ListJump3D(x, y, z, rampFactor);
        }

        public void ListMark3D(float x, float y, float z, float rampFactor = 1.0f)
        {
            Rtc3D.ListMark3D(x, y, z, rampFactor);
        }

        public void ListArc3D(float cx, float cy, float cz, float sweepAngle, float rampFactor = 1.0f)
        {
            Rtc3D.ListArc3D(cx, cy, cz, sweepAngle, rampFactor);
        }

        public void PrintStatus()
        {
            Console.WriteLine($"[Rtc3D] ZOffset : {Rtc3D.ZOffset:F3} mm");
            Console.WriteLine($"[Rtc3D] ZDefocus : {Rtc3D.ZDefocus:F3} mm");
            Console.WriteLine($"[Rtc3D] KZFactor : {Rtc3D.KZFactor:F3} bits/mm");
            Console.WriteLine($"[Rtc3D] Focal Length : {Rtc3D.FLength:F3} mm");
        }
    }
}
