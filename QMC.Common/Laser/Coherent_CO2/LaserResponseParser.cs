using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Laser.Coherent_CO2
{
    public static class LaserResponseParser
    {
        public static string ParseStatusResponse(byte[] response)
        {
            if (response == null || response.Length < 4 || response[0] != 0x02 || response[1] != 0xB1)
                return "Invalid Status Response";

            byte statusByte = response[3]; // Example: 0x00 = OK, 0x01 = Laser On, 0xFF = Fault (varies by model)
            string description;
            switch (statusByte)
            {
                case 0x00: description = "Laser OFF"; break;
                case 0x01: description = "Laser ON"; break;
                case 0x02: description = "Standby"; break;
                case 0xFF: description = "Fault Active"; break;
                default: description = $"Unknown Status (0x{statusByte:X2})"; break;
            }

            return $"Status: {description}";
        }

        public static string ParseTemperatureResponse(byte[] response)
        {
            if (response == null || response.Length < 5 || response[0] != 0x02 || response[1] != 0xB1)
                return "Invalid Temperature Response";

            byte highByte = response[3];
            byte lowByte = response[4];
            int rawValue = (highByte << 8) | lowByte;

            double temperature = rawValue / 100.0; // Assuming unit = 0.01 deg C
            return $"Temperature: {temperature:F2} °C";
        }

        public static string ParseFaultResponse(byte[] response)
        {
            if (response == null || response.Length < 5 || response[0] != 0x02 || response[1] != 0xB1)
                return "Invalid Fault Response";

            byte faultCode = response[3];
            string description;

            switch (faultCode)
            {
                case 0x00: description = "No Fault"; break;
                case 0x01: description = "Interlock Open"; break;
                case 0x02: description = "Over Temp"; break;
                case 0x03: description = "RF Fault"; break;
                case 0x04: description = "Laser Tube Error"; break;
                case 0xFF: description = "Unknown Fault"; break;
                default: description = $"Fault Code: 0x{faultCode:X2}"; break;
            }

            return description;
        }

        public static bool ValidateChecksum(byte[] response)
        {
            if (response == null || response.Length < 2)
                return false;

            byte sum = 0;
            for (int i = 0; i < response.Length - 1; i++)
                sum ^= response[i];

            return sum == response[response.Length - 1];
        }
    }
}
