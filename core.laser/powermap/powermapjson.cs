using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using QMC.Core;
using Newtonsoft.Json;

using Category = System.String;
using SetWatt = System.Double;
using MeasureWatt = System.Double;

namespace QMC.Core.Laser
{
    [JsonObject]
    public sealed class PowerMapJson : IPowerMap
    {
        public uint Index { get; set; }
        public string FileName { get; set; }
        public string XName { get; set; }
        public double XGap { get; set; }
        public Dictionary<Category, Dictionary<SetWatt, MeasureWatt>> Data { get; set; }
        
        [JsonIgnore]
        public object Tag { get; set; }


        public PowerMapJson()
        {
            this.Data = new Dictionary<Category, Dictionary<SetWatt, MeasureWatt>>();
        }

        public PowerMapJson(uint index, string mapFileName, string xName)
            : this()
        {
            this.Index = index;
            this.FileName = mapFileName;
            this.XName = xName;
        }

        public void Clear()
        {
            this.Data?.Clear();
        }
        public void Clear(Category category)
        {
            if (this.Data.ContainsKey(category))
                this.Data?.Remove(category);
        }
        public bool Update(Category category, double x, double detectedWatt)
        {
            var data = new Dictionary<SetWatt, MeasureWatt>();
            
            if (!this.Data.ContainsKey(category))
                this.Data.Add(category, data);
            else
                this.Data.TryGetValue(category, out data);

            if (!data.ContainsKey(x))
                data.Add(x, detectedWatt);
            else
                data[x] = detectedWatt;

            return true;
        }
        public bool Query(Category category, double x, out double watt)
        {
            watt = 0;
            var data = new Dictionary<SetWatt, MeasureWatt>();

            if (!this.Data.ContainsKey(category))
            {
                //Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"{this.Index} : target category is not exist: {category}");
                return false;
            }
            else
                this.Data.TryGetValue(category, out data);

            if (data.ContainsKey(x))
            {
                data.TryGetValue(x, out watt);
                return true;
            }
            else
            {
                //Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"{this.Index} : target x is not exist: {x}");
                return false;
            }
        }
        public bool MaxDetectedPower(Category category, out double detectedWatt)
        {
            detectedWatt = 0;
            try
            {
                this.Data.TryGetValue(category, out Dictionary<SetWatt, MeasureWatt> data);
                detectedWatt = data.Values.Max();
                return true;
            }
            catch(Exception ex)
            {
                //Logger.Log(Logger.Module.Laser, ex);
                return false;
            }
        }
        public bool MinMaxX(Category category, out double minX, out double maxX)
        {
            minX = maxX = 0;
            try
            {
                this.Data.TryGetValue(category, out Dictionary<SetWatt, MeasureWatt> data);
                maxX = data.Keys.Max();
                minX = data.Keys.Min();
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Log(Logger.Module.Laser, ex);
                return false;
            }
        }
        public bool Lookup(Category category, double targetWatt, out double x)
        {
            x = 0;
            double intervalMinWatt = double.MaxValue;
            double targetTolowX = double.MaxValue, targetTohighX = double.MinValue;
            double targetTolowWatt = 0, targetTohighWatt = 0;
            try
            {
                this.Data.TryGetValue(category, out Dictionary<SetWatt, MeasureWatt> data);
                var sortedData = data.OrderBy(X => X.Key);
                double minWatt = data.Values.Min();
                double maxWatt = data.Values.Max();

                foreach (var d in sortedData)
                {
                    double selectWatt = d.Value;
                    double intervalWatt = Math.Abs(selectWatt - targetWatt);
                    if (intervalWatt == 0)
                    {
                        x = d.Key;
                        return true;
                    }
                    else if (intervalWatt < intervalMinWatt)
                    {
                        intervalMinWatt = intervalWatt;
                        if(d.Value == data.Values.Last())
                        {
                            targetTohighX = d.Key;
                            targetTohighWatt = d.Value;
                            break;
                        }
                        targetTolowX = d.Key;
                        targetTolowWatt = d.Value;
                    }
                    else if (intervalWatt > intervalMinWatt)
                    {
                        targetTohighX = d.Key;
                        targetTohighWatt = d.Value;
                        break;
                    }
                }
                if (minWatt > targetWatt || maxWatt < targetWatt)
                {
                    //Logger.Log(Logger.Module.Laser, Logger.Type.Error, $"{this.Index}: powermap look up failed. target: {targetWatt:F3} W");
                    return false;
                }
                x = (targetWatt - targetTolowWatt) * ((targetTohighX - targetTolowX) / (targetTohighWatt - targetTolowWatt)) + targetTolowX;
                return true;
            }
            catch (ArgumentException ex)
            {
                //Logger.Log(Logger.Module.Laser, ex);
                return false;
            }
        }
    }
}
