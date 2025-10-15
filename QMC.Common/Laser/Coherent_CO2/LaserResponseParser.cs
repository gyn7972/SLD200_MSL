using System;
using System.Collections.Generic;
using System.Linq;

namespace QMC.Common.Laser.Coherent_CO2
{
    public static class LaserResponseParser
    {
        private static readonly Dictionary<int, string> FaultMap = new Dictionary<int, string>()
        {
            { 0, "No Fault" },
            { 1, "System Interlock Open" },
            { 2, "Over Temperature" },
            { 3, "VSWR Limit" },
            { 4, "Duty Cycle Limit" },
            { 5, "Shutter Fault" },
            { 6, "RF Power Fault" },
            { 7, "Simmer Fault" },
            { 8, "Cooling Flow Fault" },
            { 9, "Power Supply Fault" },
            { 10, "Unknown Fault" }
        };

        public static ControllerStatus ParseControllerStatus(string response)
        {
            var st = new ControllerStatus { Raw = response };
            try
            {
                var parts = response.Replace("\r", "").Replace("\n", "").Split('|');
                if (parts.Length < 10 || !parts[0].StartsWith("085"))
                    return st;

                st.Voltage48V = ToInt(parts, 3) / 1.0;
                st.LaserTemp = ToInt(parts, 4) / 1.0;
                st.ShutterTemp = ToInt(parts, 5) / 1.0;
                st.PowerWatt = ToInt(parts, 10);

                byte status1 = (byte)ToInt(parts, 7);
                st.Enable = (status1 & (1 << 3)) != 0;
                st.SystemFault = (status1 & (1 << 2)) != 0;
                st.OverTemp = (status1 & (1 << 1)) != 0;
                st.Interlock = (status1 & (1 << 7)) != 0;
            }
            catch { }

            return st;
        }

        public static List<LaserFault> ParseFaultList(string response)
        {
            var list = new List<LaserFault>();
            try
            {
                var parts = response.Replace("\r", "").Replace("\n", "").Split('|');
                foreach (var token in parts)
                {
                    if (int.TryParse(token, out int code) && FaultMap.ContainsKey(code))
                        list.Add(new LaserFault { Code = code, Description = FaultMap[code] });
                }
            }
            catch { }

            return list;
        }

        private static int ToInt(string[] arr, int idx)
        {
            if (idx >= arr.Length) return 0;
            int.TryParse(arr[idx], out int val);
            return val;
        }
    }

    public class ControllerStatus
    {
        public double Voltage48V { get; set; }
        public double LaserTemp { get; set; }
        public double ShutterTemp { get; set; }
        public double PowerWatt { get; set; }
        public bool Enable { get; set; }
        public bool SystemFault { get; set; }
        public bool OverTemp { get; set; }
        public bool Interlock { get; set; }
        public string Raw { get; set; }

        public override string ToString()
        {
            return $"48V={Voltage48V:F1}V, Temp={LaserTemp:F1}°C, Shutter={ShutterTemp:F1}°C, Power={PowerWatt:F1}W, " +
                   $"Enable={Enable}, Fault={SystemFault}, OverTemp={OverTemp}, Interlock={Interlock}";
        }
    }

    public class LaserFault
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public override string ToString() => $"[{Code:D2}] {Description}";
    }
}
