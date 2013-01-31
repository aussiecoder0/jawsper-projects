using System.IO;
using System.Text;

namespace TabManager.TabFiles
{
    public class TabStream : FileStream
    {
        public TabStream(string path, FileMode mode)
            : base(path, mode)
        {
        }
        public int PeekByte()
        {
            int b = ReadByte();
            Position--;
            return b;
        }

        public uint LE_ReadUInt32()
        {
            long ret = ((ReadByte() & 0xFF) << 0) | ((ReadByte() & 0xFF) << 8) | ((ReadByte() & 0xFF) << 16) | ((ReadByte() & 0xFF) << 24);
            return (uint)(ret & 0xFFFFFFFF);
        }
        public int LE_ReadInt32()
        {
            return ((ReadByte() & 0xFF) << 0) | ((ReadByte() & 0xFF) << 8) | ((ReadByte() & 0xFF) << 16) | ((ReadByte() & 0xFF) << 24);
        }

        public ushort LE_ReadUInt16()
        {
            return (ushort)((((ReadByte() & 0xFF) << 0) | ((ReadByte() & 0xFF) << 8)) & 0xFFFF);
        }
        public short LE_ReadInt16()
        {
            return (short)((((ReadByte() & 0xFF) << 0) | ((ReadByte() & 0xFF) << 8)) & 0xFFFF);
        }

        public byte LE_ReadUInt8()
        {
            return (byte)(ReadByte() & 0xFF);
        }
        public sbyte LE_ReadInt8()
        {
            return (sbyte)(ReadByte() & 0xFF);
        }

        public string ReadStringWithLength()
        {
            var str = new StringBuilder();
            int l = ReadByte();
            while (l-- > 0)
            {
                str.Append((char)ReadByte());
            }
            return str.ToString();
        }

        public string GPReadString()
        {
            var x = LE_ReadUInt32();
            if (x == 1)
            {
                ReadByte();
                return null;
            }
            return ReadStringWithLength();
        }
    }
}
