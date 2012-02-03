using System;

namespace SkyTraqCs
{
    internal class SkyTraqPackage
    {
        internal ushort length;
        internal byte[] data;
        internal byte checksum;

        internal SkyTraqPackage(ushort length)
        {
            this.length = length;
            this.data = new byte[this.length];
        }

        internal SkyTraqPackage(byte message_id, byte[] data)
        {
            this.length = (ushort)(data.Length + 1);
            this.data = new byte[this.length];
            this.data[0] = message_id;
            Array.Copy(data, 0, this.data, 1, data.Length);
        }

        internal SkyTraqPackage(params byte[] data)
        {
            this.length = (ushort)data.Length;
            this.data = data;
        }

        private byte CalculateChecksum()
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
            return this.checksum == this.CalculateChecksum();
        }

        internal byte[] GetRawPackage()
        {
            var raw = new byte[7 + this.length];

            int j = 0;
            raw[j++] = 0xa0;
            raw[j++] = 0xa1;
            raw[j++] = (byte)((this.length >> 8) & 0xFF);
            raw[j++] = (byte)(this.length & 0xFF);
            for (int i = 0; i < this.length; ++i)
            {
                raw[j++] = this.data[i];
            }
            raw[j++] = this.CalculateChecksum();
            raw[j++] = 0x0d;
            raw[j++] = 0x0a;

            return raw;
        }
    }
}
