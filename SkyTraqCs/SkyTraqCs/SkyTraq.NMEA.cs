using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SkyTraqCs
{
    partial class SkyTraq
    {
        public string GetNMEAMessage()
        {
            return serialPort.ReadLine();
        }

        public Dictionary<string, object> ParseNMEAMessage(string message)
        {
            if (!message.StartsWith("$")) return null;
            message = message.Substring(1);
            if (message[message.Length - 3] == '*')
            {
                var checksumstr = message.Substring(message.Length - 2);
                var checksum = Convert.ToUInt16(checksumstr, 16);
                message = message.Remove(message.Length - 3);
                var sum = 0;
                foreach (var c in message.ToCharArray())
                {
                    sum ^= c;
                }
                if (checksum != sum)
                {
                    Debug.WriteLine("Checksum failure; expected: 0x{0:x2}, got 0x{1:x2}", checksum, sum);
                    return null;
                }
            }

            var talker_id = message.Substring(0, 2); message = message.Substring(2);
            var message_type = message.Substring(0, 3); message = message.Substring(3);

            if (!message.StartsWith(","))
            {
                Debug.WriteLine("Message doesn't seem to have items...?");
                return null;
            }
            message = message.Substring(1);
            var message_items = message.Split(',');

            var nmea_parsers = new Dictionary<string, INMEAParser>()
            {
                { "GGA", new NMEAParserGGA() }, // Fix information
                { "GSA", new NMEAParserGSA() }, // Overall Satellite data
                { "RMC", new NMEAParserRMC() }, // recommended minimum data for gps
                { "VTG", null}, // Vector track an Speed over the Ground
            };

            if (nmea_parsers.ContainsKey(message_type))
            {
                if (nmea_parsers[message_type] == null) return null;
                return nmea_parsers[message_type].Parse(message_items);
            }

            return null;
        }
    }
}