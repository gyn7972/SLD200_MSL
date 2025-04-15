using QMC.Common;
using QMC.Common.Motion.Ajin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class PressureSensor : Part
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public static double PressureValue { get; set; }
        public DateTime LastUpdated { get; set; }
        public double PressureScale { get; set; }

        public static LowPassFilter lowPassFilter;
        private static Task _task;
        public PressureSensor(int sensorId, string sensorName)
            : base(sensorName)
        {
            SensorId = sensorId;
            SensorName = sensorName;
            PressureValue = 0.0;
            LastUpdated = DateTime.Now;
            if(_task == null)
            {
                _task = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(2000);
                    double dValue = 0.0;
                    while (true)
                    {
                        try
                        {
                            dValue = ReadPressure();
                            if (dValue != 0.0)
                            {
                                UpdatePressure(dValue);
                            }

                            Thread.Sleep(1);
                        }
                        catch (Exception ex)
                        {
                            // Handle exception
                        }
                    }

                });
            }
            
        }

        private double ReadPressure()
        {
            double dValue = 0.0;
            AXA.AxaiSwReadVoltage(SensorId, ref dValue);
            return dValue;
        }

        public void UpdatePressure(double newPressure)
        {
            if (lowPassFilter == null)
            {
                lowPassFilter = new LowPassFilter(0.001, newPressure);
            }
            
            PressureValue = lowPassFilter.Filter(newPressure); ;
            LastUpdated = DateTime.Now;
        }
    }
    public class LowPassFilter
    {
        private double _cutoffFrequence;
        private double _previousValue;
        public LowPassFilter(double cutoffFrequence,double prevalue)
        {
            CutoffFrequence = cutoffFrequence;
            _previousValue = prevalue;
        }

        public double CutoffFrequence { get => _cutoffFrequence; set => _cutoffFrequence = value; }

        public double Filter(double newValue)
        {
            double filteredValue = CutoffFrequence * newValue + (1 - CutoffFrequence) * _previousValue;
            _previousValue = filteredValue;
            //Console.WriteLine(filteredValue.ToString()); 
            return filteredValue;
        }
    }
}
