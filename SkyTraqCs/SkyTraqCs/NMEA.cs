using System;
using System.Collections.Generic;
using System.Globalization;

namespace SkyTraqCs
{
    static class NMEAHelper
    {
        private static NumberFormatInfo nf;
        private static void InitNumberFormat()
        {
            nf = new NumberFormatInfo();
            nf.NumberDecimalSeparator = ".";
        }

        internal static Int32 GetInt32(string s)
        {
            return int.Parse(s, NumberStyles.Integer);
        }

        internal static Double GetDouble(string s)
        {
            if (nf == null)
            {
                InitNumberFormat();
            }
            if (s.Contains("?")) return 0;
            return double.Parse(s, NumberStyles.Float, nf);
        }

        internal static DateTime GetTime(string s)
        {
            var raw = GetDouble(s);

            var date = DateTime.MinValue;
            int hours, minutes;
            double seconds;
            hours = (int)(raw / 10000);
            raw -= hours * 10000;
            minutes = (int)(raw / 100);
            raw -= minutes * 100;
            seconds = raw;
            date = date.AddHours(hours);
            date = date.AddMinutes(minutes);
            date = date.AddSeconds(seconds);

            return date;
        }

        internal static DateTime GetDate(string s)
        {
            var raw = int.Parse(s);
            int year, month, day;
            day = (int)(raw / 10000);
            raw -= day * 10000;
            month = (int)(raw / 100);
            raw -= month * 100;
            year = 2000 + raw;

            return new DateTime(year, month, day);
        }
    }

    interface INMEAParser
    {
        Dictionary<string, object> Parse(string[] data);
    }

    class NMEAParserGGA : INMEAParser
    {
        public Dictionary<string, object> Parse(string[] data)
        {
            if (data.Length == 14)
            {
                var dict = new Dictionary<string, object>()
                {
                    { "utc_time", NMEAHelper.GetTime(data[0]) },
                    { "latitude", NMEAHelper.GetDouble(data[1]) },
                    { "latitude_hemisphere", data[2] },
                    { "longitude", NMEAHelper.GetDouble(data[3]) },
                    { "longitude_hemisphere", data[4] },
                    { "fix_quality", NMEAHelper.GetInt32(data[5]) },
                    { "number_of_satellites", NMEAHelper.GetInt32(data[6]) },
                    { "horizontal_dilution", NMEAHelper.GetDouble(data[7]) },
                    { "altitude", NMEAHelper.GetDouble(data[8]) },
                    { "altitude_unit", data[9] },
                    { "height_geoid", NMEAHelper.GetDouble(data[10]) },
                    { "height_geoid_unit", data[11] },
                    { "time_since_last_dgps_update", data[12] },
                    { "dgps_station_id_number", data[13] },
                };

                return dict;
            }
            return null;
        }
    }

    class NMEAParserGSA : INMEAParser
    {
        public Dictionary<string, object> Parse(string[] data)
        {
            if (data.Length == 17)
            {
                int i = 0;
                var dict = new Dictionary<string, object>()
                {
                    { "auto_selection", data[i++] },
                    { "fix_dimensions", NMEAHelper.GetInt32(data[i++]) },
                    { "satellite_1", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_2", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_3", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_4", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_5", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_6", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_7", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_8", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_9", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_10", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_11", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "satellite_12", data[i++].Length > 0 ? NMEAHelper.GetInt32(data[i-1]) : -1 },
                    { "pdop", NMEAHelper.GetDouble(data[i++]) },
                    { "hdop", NMEAHelper.GetDouble(data[i++]) },
                    { "vdop", NMEAHelper.GetDouble(data[i++]) },
                };

                return dict;
            }
            return null;
        }
    }

    class NMEAParserRMC : INMEAParser
    {
        public Dictionary<string, object> Parse(string[] data)
        {
            if (data.Length >= 11 && data.Length <= 12)
            {
                var dict = new Dictionary<string, object>()
                {
                    { "utc_time", NMEAHelper.GetTime(data[0]) },
                    { "status", data[1] },
                    { "latitude", NMEAHelper.GetDouble(data[2]) },
                    { "latitude_hemisphere", data[3] },
                    { "longitude", NMEAHelper.GetDouble(data[4]) },
                    { "longitude_hemisphere", data[5] },
                    { "speed ", NMEAHelper.GetDouble(data[6]) },
                    { "track_angle ", NMEAHelper.GetDouble(data[7]) },
                    { "utc_date", NMEAHelper.GetDate(data[8]) },
                    { "magnetic_stuff1", null },
                    { "magnetic_stuff2", null },
                };
                if (data.Length == 12) dict.Add("signal_integrity", data[11]);

                return dict;
            }
            return null;
        }
    }
}