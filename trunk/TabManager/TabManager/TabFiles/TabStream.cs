using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TabManager.TabFiles
{
    static class TabStream
    {
        public static int PeekByte(this Stream s)
        {
            int b = s.ReadByte();
            s.Position--;
            return b;
        }

        public static uint LE_ReadUInt32(this Stream s)
        {
            long ret = ((s.ReadByte() & 0xFF) << 0) | ((s.ReadByte() & 0xFF) << 8) | ((s.ReadByte() & 0xFF) << 16) | ((s.ReadByte() & 0xFF) << 24);
            return (uint)(ret & 0xFFFFFFFF);
        }
        public static int LE_ReadInt32(this Stream s)
        {
            return ((s.ReadByte() & 0xFF) << 0) | ((s.ReadByte() & 0xFF) << 8) | ((s.ReadByte() & 0xFF) << 16) | ((s.ReadByte() & 0xFF) << 24);
        }

        public static ushort LE_ReadUInt16(this Stream s)
        {
            return (ushort)((((s.ReadByte() & 0xFF) << 0) | ((s.ReadByte() & 0xFF) << 8)) & 0xFFFF);
        }
        public static short LE_ReadInt16(this Stream s)
        {
            return (short)((((s.ReadByte() & 0xFF) << 0) | ((s.ReadByte() & 0xFF) << 8)) & 0xFFFF);
        }

        public static byte LE_ReadUInt8(this Stream s)
        {
            return (byte)(s.ReadByte() & 0xFF);
        }
        public static sbyte LE_ReadInt8(this Stream s)
        {
            return (sbyte)(s.ReadByte() & 0xFF);
        }

        public static string ReadStringWithLength(this Stream s)
        {
            var str = new StringBuilder();
            int l = s.ReadByte();
            while (l-- > 0)
            {
                str.Append((char)s.ReadByte());
            }
            return str.ToString();
        }

        public static string GPReadString(this Stream s)
        {
            var x = s.LE_ReadUInt32();
            if (x == 1)
            {
                s.ReadByte();
                return null;
            }
            return s.ReadStringWithLength();
        }
    }
}
