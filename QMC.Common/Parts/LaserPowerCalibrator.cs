using QMC.Common;
using SpiralLab.Sirius;
//using SpiralLab.Sirius2.Laser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



//namespace QMC.Process.WorkStage.Parts
namespace QMC.Common.Parts
{
    public class LaserPowerCalibrator : Part
    {
        public ScannerParameter Scanner { get; set; }

        public LaserPowerCalibrator(string strName) : base(strName)
        {
        }

        public int EnableLaserOn(bool bOn)
        {
            int ret = 0;

            if (Scanner.Laser == null || Scanner.RTC == null)
            {
                //Error
                return -1;
            }

            if ((ret = Scanner.Forward()) != 0) return ret;
                
            if(bOn)
            {
                Scanner.RTC.CtlLaserOn();
                
            }
            else
            {
                Scanner.RTC.CtlLaserOff();
            }

            return ret;
        }

        public int SetLaserPower(double dPower)
        {
            int ret = 0;

            IPowerControl powerControl = Scanner.Laser as IPowerControl;
            //ILaserPowerControl powerControl = Scanner.Laser as ILaserPowerControl;            //  Sirius2
            if(powerControl == null)
            {
                return -1;
            }

            powerControl.CtlPower((float)dPower, "ETCHING");

            return ret;
        }

        public int SetRepRate(double dFrequency, double dPulseWidth)
        {
            int ret = 0;
            if (Scanner.Laser == null || Scanner.RTC == null)
            {
                //Error
                return -1;
            }

            Scanner.RTC.CtlFrequency((float)dFrequency, (float)dPulseWidth);

            return ret;
        }
        public Task<int> BeginVerifyPower(double dPower)
        {
            return Task.Factory.StartNew(() =>
            {
                return VerifyPower(dPower);
            });
        }
        public int VerifyPower(double dPower)
        {
            int ret = 0;
            bool success = false;            
            var powerControl = Scanner.Laser as IPowerControl;
            //var powerControl = Scanner.Laser as ILaserPowerControl;                   //  Sirius2
            if (null == powerControl)
                return -1;

            if ((ret = Scanner.Forward()) != 0) return ret;

            float targetWatt = (float)dPower;
            Scanner.EnableLaserEmission(true);
            //Scanner.PowerMeter.CtlStop();
            //Scanner.PowerMeter.CtlClear();
            Thread.Sleep(1000);

            if (!powerControl.CtlPower(targetWatt, "ETCHING"))
                return -10;
            //laser.OperatingPower = currentWatt;
            Log.Write(this, $"LASER TARGET POWER= {targetWatt}");

            //laser on (WARNING !!!)
            //if (!Scanner.RTC.CtlLaserOn())
            if (!Scanner.RTC.CtlLaserOn())
                return -11;
            Log.Write(this, "WARNING !!! LASER IS ON ...");

            //for preheating 
            Thread.Sleep(5 * 1000);

            /*//start measurement
            if(!Scanner.PowerMeter.CtlStart())
                return -12;*/

            //during 5 secs
            Thread.Sleep(5 * 1000);

            /*//stop measurement
            if (!Scanner.PowerMeter.CtlStop())
                return -13;*/

            //laser off
            //if (!Scanner.RTC.CtlLaserOff())
            if (!Scanner.RTC.CtlLaserOff())
                return -14;

            // average power (watt)
            Scanner.EnableLaserEmission(false);

            /*var avgWatt = Scanner.PowerMeter.Data.Average();
            Log.Write(this, $"LASER AVG POWER= {avgWatt}");
            Log.Write(this, $"LASER IS OFF");
            var deviationWatt = Math.Abs(targetWatt - avgWatt);

            //+-0.2W 이내 출력 오차를 성공으로 판단
            success &= deviationWatt < Scanner.Config.VerifyRange;*/

            if(!success)
            {
                //Fail
                ret = -1;
            }

            return ret;
        }
        public Task<int> RunPowerCalibration(double dStart, double dEnd, double dStep)
        {
            return Task.Factory.StartNew(() =>
            {
                int ret = 0;

                ret = RunPowerCalibrationSync(dStart, dEnd, dStep);

                return ret;
            });
        }
        public int RunPowerCalibrationSync(double dStart, double dEnd, double dStep)
        {
            int ret = 0;

            var powerControl = Scanner.Laser as IPowerControl;
            //var powerControl = Scanner.Laser as ILaserPowerControl;                   //  Sirius2
            if (null == powerControl)
                return -1;

            if ((ret = Scanner.Forward()) != 0) return ret;

            //Scanner.PowerMeter.CtlStop();
            //Scanner.PowerMeter.CtlClear();
            Scanner.Map.Clear();

            bool success = true;
            float maxWatt = (float)(dEnd > Scanner.Laser.MaxPowerWatt ? Scanner.Laser.MaxPowerWatt : dEnd);
            float increaseWatt = (float)dStep;
            float currentWatt = (float)dStart;

            //Scanner.Map.XName = "Watt";
            //Scanner.Map.XGap = currentWatt;
            //Scanner.Map.Name = "Watt";                                //  2025. 01. 02.  SCH : 왜 안될까?
            //Scanner.Map.XGap = currentWatt;
            //IPGLaserYLPN laser = Scanner.Laser as IPGLaserYLPN;

            Scanner.EnableLaserEmission(true);

            while (currentWatt <= maxWatt)
            {
                success &= powerControl.CtlPower(currentWatt);
                //laser.OperatingPower = currentWatt;
                Log.Write(this, $"LASER TARGET POWER= {currentWatt}");

                //laser on (WARNING !!!)
                //success &= Scanner.RTC.CtlLaserOn();
                success &= Scanner.RTC.CtlLaserOn();
                Log.Write(this, "WARNING !!! LASER IS ON ...");

                //for preheating 
                Thread.Sleep(5 * 1000);

                //start measurement
                //success &= Scanner.PowerMeter.CtlStart();

                //during 5 secs
                Thread.Sleep(5 * 1000);

                //stop measurement
                //success &= Scanner.PowerMeter.CtlStop();

                //laser off
                //success &= Scanner.RTC.CtlLaserOff();
                success &= Scanner.RTC.CtlLaserOff();

                // average power (watt)
                /*var avgWatt = Scanner.PowerMeter.Data.Average();
                Log.Write(this, $"LASER AVG POWER= {avgWatt}");
                Log.Write(this, $"LASER IS OFF");
                success &= Scanner.Map.Update("ETCHING", currentWatt, avgWatt);*/

                //Scanner.PowerMeter.CtlClear();
                currentWatt += increaseWatt;
                if (!success)
                    break;
            }

            Scanner.EnableLaserEmission(false);

            if (!success)
            {
                ret = -10;
            }
            else
            {
                ret = Scanner.SavePowerMap();
            }

            return ret;
        }
    }
}
