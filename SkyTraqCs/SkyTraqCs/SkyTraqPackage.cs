using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyTraqCs
{
    internal class SkyTraqPackage
    {
        internal ushort length;
        internal byte[] data;
        internal byte checksum;

        internal SkyTraqPackage()
        {
            this.length = 0;
            this.data = null;
        }
        internal SkyTraqPackage(ushort length)
        {
            this.Resize(length);
        }

        internal void Resize(ushort length)
        {
            this.length = length;
            this.data = new byte[this.length];
        }

        internal byte CalculateSkyTraqChecksum()
        {
            byte c = 0;
            for (int i = 0; i < length; ++i)
            {
                c = (byte)(c ^ data[i]);
            }
            return c;
        }

        internal bool CheckChecksum()
        {
            return this.checksum == this.CalculateSkyTraqChecksum();
        }
    }
    public class SkyTraqConfig
    {
        public UInt32 log_wr_ptr;
        public UInt16 sectors_left;
        public UInt16 total_sectors;
        public UInt32 max_time;
        public UInt32 min_time;
        public UInt32 max_distance;
        public UInt32 min_distance;
        public UInt32 max_speed;
        public UInt32 min_speed;
        public byte datalog_enable;
        public byte log_fifo_mode;
        public byte agps_enabled;
        public UInt16 agps_hours_left;

        internal SkyTraqConfig(byte[] p)
        {
            log_wr_ptr = BitConverter.ToUInt32(p, 1);
            sectors_left = BitConverter.ToUInt16(p, 5);
            total_sectors = BitConverter.ToUInt16(p, 7);
            max_time = BitConverter.ToUInt32(p, 9);
            min_time = BitConverter.ToUInt32(p, 13);
            max_distance = BitConverter.ToUInt32(p, 17);
            min_distance = BitConverter.ToUInt32(p, 21);
            max_speed = BitConverter.ToUInt32(p, 25);
            min_speed = BitConverter.ToUInt32(p, 29);
            datalog_enable = p[33];
            log_fifo_mode = p[34];
        }
    }

    struct SkyTraqCommand
    {
        internal static readonly byte SKYTRAQ_COMMAND_SYSTEM_RESTART = 0x01,
        SKYTRAQ_COMMAND_QUERY_SOFTWARE_VERSION = 0x02,
        SKYTRAQ_COMMAND_QUERY_SOFTWARE_CRC = 0x03,
        SKYTRAQ_COMMAND_SET_FACTORY_DEFAULTS = 0x04,
        SKYTRAQ_COMMAND_CONFIGURE_SERIAL_PORT = 0x05,
        SKYTRAQ_COMMAND_CONFIGURE_NMEA_MESSAGE = 0x08,
        SKYTRAQ_COMMAND_CONFIGURE_MESSAGE_TYPE = 0x09,
        SKYTRAQ_COMMAND_GET_CONFIG = 0x17,
        SKYTRAQ_COMMAND_WRITE_CONFIG = 0x18,
        SKYTRAQ_COMMAND_ERASE = 0x19,
        SKYTRAQ_COMMAND_READ_SECTOR = 0x1b,
        SKYTRAQ_COMMAND_GET_EPHERMERIS = 0x30,
        SKYTRAQ_COMMAND_SET_EPHEMERIS = 0x31,
        SKYTRAQ_COMMAND_READ_AGPS_STATUS = 0x34,
        SKYTRAQ_COMMAND_SEND_AGPS_DATA = 0x35,
        SKYTRAQ_RESPONSE_SOFTWARE_VERSION = 0x80,
        SKYTRAQ_RESPONSE_SOFTWARE_CRC = 0x81,
        SKYTRAQ_RESPONSE_ACK = 0x83,
        SKYTRAQ_RESPONSE_NACK = 0x84,
        SKYTRAQ_RESPONSE_EPHEMERIS_DATA = 0xb1;
    }
}
