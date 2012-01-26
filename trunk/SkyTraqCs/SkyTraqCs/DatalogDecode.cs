using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Globalization;

/*

    Control program for SkyTraq GPS data logger.

    Copyright (C) 2008  Jesper Zedlitz, jesper@zedlitz.de
    Copyright (C) 2012  Jasper Seidel, conversion to C#

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111 USA

 */

namespace SkyTraqCs
{
    public static class DatalogDecode
    {
        public static long ProcessBuffer(StreamWriter sw, byte[] buffer, long first_timestamp)
        {
            int offset = 0;
            long time = 0;
            int ecef_x = 0, ecef_y = 0, ecef_z = 0, speed = 0;
            double latitude = 0.0, longitude = 0.0, height = 0.0;
            //int tagged_entry = 0;
            long last_timestamp = first_timestamp;

            Debug.WriteLine("Processing {0} bytes", buffer.Length);

            sw.WriteLine("<!-- next-sector -->");

            while (offset < buffer.Length)
            {
                if ((buffer[offset] & 0x40) != 0)
                {
                    // long entry
                    DecodeLongEntry(buffer, offset, ref time, ref ecef_x, ref ecef_y, ref ecef_z, ref speed);
                    EcefToGeo(ecef_x, ecef_y, ecef_z, ref longitude, ref latitude, ref height);
                    //if ((buffer[offset] & 0x20) != 0) tagged_entry = 1;

                    if (last_timestamp > 0 && (time > (last_timestamp + 36001)))
                    {
                        /* start a new track segment if the time difference between
                        two points is more than one hour */
                        sw.Write("</trgseg>\n<trkseg>\n");
                    }
                    OutputGpxTrkPoint(sw, time, latitude, longitude, height, speed);
                    offset += 18;
                }
                else if (buffer[offset] == 0x80)
                {
                    // short entry
                    DecodeShortEntry(buffer, offset, ref time, ref ecef_x, ref ecef_y, ref ecef_z, ref speed);
                    EcefToGeo(ecef_x, ecef_y, ecef_z, ref longitude, ref latitude, ref height);

                    if (last_timestamp > 0 && (time > (last_timestamp + 36001)))
                    {
                        /* start a new track segment if the time difference between
                        two points is more than one hour */
                        sw.Write("</trgseg>\n<trkseg>\n");
                    }
                    OutputGpxTrkPoint(sw, time, latitude, longitude, height, speed);
                    offset += 8;
                }
                else
                {
                    // search for valid entry
                    offset++;
                }

                last_timestamp = time;
            }

            return last_timestamp;
        }

        private static void DecodeShortEntry(byte[] buffer, int offset, ref long time, ref int ecef_x, ref int ecef_y, ref int ecef_z, ref int speed)
        {
            int dt, dx, dy, dz;
            speed = buffer[offset + 1];
            dt = (buffer[offset + 2] << 8) + buffer[offset + 3];
            dx = (buffer[offset + 4] << 2) + ((buffer[offset + 5] >> 6) & 0x03);
            dy = (buffer[offset + 5] & 0x3f) + (((buffer[offset + 6] >> 4) & 0x0f) << 6);
            dz = ((buffer[offset + 6] & 0x03) << 8) + buffer[offset + 7];

            if (dx >= 512) dx = 511 - dx;
            if (dy >= 512) dy = 511 - dy;
            if (dz >= 512) dz = 511 - dz;

            time += dt;
            ecef_x += dx;
            ecef_y += dy;
            ecef_z += dz;
        }

        private static void DecodeLongEntry(byte[] buffer, int offset, ref long time, ref int ecef_x, ref int ecef_y, ref int ecef_z, ref int speed)
        {
            int wno, tow;

            speed = buffer[offset + 1];
            wno = (buffer[offset + 3] + ((buffer[offset + 2] & 0x0F) << 8)) + 1024;
            tow = (buffer[offset + 4] << 12) + (buffer[offset + 5] << 4) + ((buffer[offset + 2] >> 4) & 0x0F);
            ecef_x = buffer[offset + 7] + (buffer[offset + 6] << 8) + (buffer[offset + 9] << 16) + (buffer[offset + 8] << 24);
            ecef_y = buffer[offset + 11] + (buffer[offset + 10] << 8) + (buffer[offset + 13] << 16) + (buffer[offset + 12] << 24);
            ecef_z = buffer[offset + 15] + (buffer[offset + 14] << 8) + (buffer[offset + 17] << 16) + (buffer[offset + 16] << 24);

            time = GspTimeToTimestamp(wno, tow);

        }

        private static void EcefToGeo(int X, int Y, int Z, ref double longitude, ref double latitude, ref double height)
        {
            double a, f, b, e2, ep2, r2, r, E2, F, G, c, s, P, Q, ro, tmp, U, V, zo, h, phi, lambda;

            a = 6378137.0; /* earth semimajor axis in meters */
            f = 1 / 298.257223563; /* reciprocal flattening */
            b = a * (1 - f); /* semi-minor axis */

            e2 = 2 * f - f * f; /* first eccentricity squared */
            ep2 = f * (2 - f) / ((1 - f) * (1 - f)); /* second eccentricity squared */

            r2 = (double)X * (double)X + (double)Y * (double)Y;
            r = Math.Sqrt(r2);
            E2 = a * a - b * b;
            F = 54 * b * b * Z * Z;
            G = r2 + (1 - e2) * Z * Z - e2 * E2;
            c = (e2 * e2 * F * r2) / (G * G * G);
            s = Math.Pow((1 + c + Math.Sqrt(c * c + 2 * c)), 1 / 3);
            P = F / (3 * (s + 1 / s + 1) * (s + 1 / s + 1) * G * G);
            Q = Math.Sqrt(1 + 2 * e2 * e2 * P);
            ro = -(e2 * P * r) / (1 + Q) + Math.Sqrt((a * a / 2) * (1 + 1 / Q) - ((1 - e2) * P * Z * Z) / (Q * (1 + Q)) - P * r2 / 2);
            tmp = (r - e2 * ro) * (r - e2 * ro);
            U = Math.Sqrt(tmp + Z * Z);
            V = Math.Sqrt(tmp + (1 - e2) * Z * Z);
            zo = (b * b * Z) / (a * V);

            h = U * (1 - b * b / (a * V));
            phi = Math.Atan((Z + ep2 * zo) / r);
            lambda = Math.Atan2(Y, X);

            longitude = lambda * 180 / Math.PI;
            latitude = phi * 180 / Math.PI;
            height = h;
        }

        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }


        private static void OutputGpxTrkPoint(StreamWriter sw, long unix_time, double latitude, double longitude, double height, int speed)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            var dt = ConvertFromUnixTimestamp(unix_time);
            sw.Write(" <trkpt lat=\"{0}\" lon=\"{1}\"><ele>{2}</ele><time>{3:s}</time><!--unixtime:{5}--><speed>{4}</speed></trkpt>\n",
                latitude.ToString(nfi), longitude.ToString(nfi),
                height.ToString(nfi), dt, speed, unix_time
                );
        }

        private static long GspTimeToTimestamp(int wno, int tow)
        {
            return 604800L * (long)wno + 315964800L + (long)tow;
        }
    }
}
