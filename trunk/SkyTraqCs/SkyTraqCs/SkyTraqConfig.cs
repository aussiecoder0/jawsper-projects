using System;

namespace SkyTraqCs
{
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

        private static void cpy(byte[] dst, int pos, byte[] src)
        {
            int j = pos;
            for (int i = src.Length - 1; i >= 0; --i)
            {
                dst[j++] = src[i];
            }
        }
        internal SkyTraqPackage GetWritePackage()
        {
            var buff = new byte[26];
            cpy(buff, 1, BitConverter.GetBytes(max_time));
            cpy(buff, 5, BitConverter.GetBytes(min_time));
            cpy(buff, 9, BitConverter.GetBytes(max_distance));
            cpy(buff, 13, BitConverter.GetBytes(min_distance));
            cpy(buff, 17, BitConverter.GetBytes(max_speed));
            cpy(buff, 21, BitConverter.GetBytes(min_speed));
            buff[25] = datalog_enable;
            buff[26] = log_fifo_mode;

            return new SkyTraqPackage(SkyTraqCommand.SKYTRAQ_COMMAND_WRITE_CONFIG, buff);
        }
    }
}