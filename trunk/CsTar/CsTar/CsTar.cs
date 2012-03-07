using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace jawsper
{
    public class CsTar : IDisposable
    {
        private Stream s;
        private List<TarFile> files = new List<TarFile>();

        public CsTar(Stream stream)
        {
            s = stream;
        }

        public void Dispose()
        {
            if (s is Stream) s.Dispose();
        }

        public static string DisplayFileSize(ulong size)
        {
            var dsize = (double)size;
            int n = 0;
            var endings = new string[] { "", "Ki", "Mi", "Gi", "Ti", "Ei", "Zi", "Yi" };
            while (n < endings.Length && dsize > 1024.0)
            {
                n++;
                dsize /= 1024.0;
            }
            return String.Format("{0:0.00} {1}B", dsize, endings[n]);
        }

        private static string _gnu_long_name = null;
        private TarFile ReadFile()
        {
            try
            {
                var buff = new byte[512];
                s.Read(buff, 0, buff.Length);
                var f = new TarFile(buff);
                if (f.Valid)
                {
                    if (_gnu_long_name != null)
                    {
                        f.Name = _gnu_long_name;
                        _gnu_long_name = null;
                    }
                    switch (f.Type)
                    {
                        case 'L':
                            var file = ReadFile(f.Size);
                            _gnu_long_name = Encoding.ASCII.GetString(file, 0, file.Length);
                            return f;
                    }
                    if (f.Size > 0)
                    {
                        /*
                         * '0' or (ASCII NUL)	Normal file
                         * '1'	Hard link
                         * '2'	Symbolic link
                         * '3'	Character special
                         * '4'	Block special
                         * '5'	Directory
                         * '6'	FIFO
                         * '7'	Contiguous file
                         * 'g'	global extended header with meta data (POSIX.1-2001)
                         * 'x'	extended header with meta data for the next file in the archive (POSIX.1-2001)
                         * 'A'–'Z'	Vendor specific extensions (POSIX.1-1988)
                         * All other values	reserved for future standardization
                         * */
                        if (f.Type == 0 || f.Type == '0')
                        {
                            SeekFile(f.Size);
                        }
                        else
                        {
                            var file = ReadFile(f.Size);
                            string s1 = Encoding.ASCII.GetString(file, 0, file.Length);
                            int a = 0;
                        }
                    }
                    files.Add(f);

                    return f;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private byte[] ReadFile(ulong fsize)
        {
            var buff = new byte[fsize];
            s.Read(buff, 0, buff.Length);
            long seek = 0;
            if (fsize % 512 != 0)
            {
                seek = (512 - ((long)fsize) % 512);
                if (s.CanSeek) s.Seek(seek, SeekOrigin.Current);
                else for (int i = 0; i < seek; i++) s.ReadByte();
            }

            return buff;
        }

        private void SeekFile(ulong fsize)
        {
            long seek = ((long)fsize);
            if (seek % 512 != 0) seek += (512 - (((long)fsize) % 512));
            Debug.Assert(seek % 512 == 0);
            if (s.CanSeek) s.Seek(seek, SeekOrigin.Current);
            else for (int i = 0; i < seek; i++) s.ReadByte();
        }

        public void ReadHeaders()
        {
            TarFile f;
            do
            {
                f = ReadFile();
                if (f is TarFile)
                {
                    //Console.WriteLine("File: {0}", f.ToString());
                }
            } while (f != null);
            Console.WriteLine("I have {0} entries", files.Count);
        }
    }

    internal class TarFile
    {
        internal TarFile(byte[] buff)
        {
            this.Valid = buff[0] != 0;
            if (!Valid) return;
            this.Name = GetString(buff, 0, 100);
            var str = GetString(buff, 124, 12).Trim((char)0x20);
            this.Size = str.Length == 0 ? 0 : Convert.ToUInt64(str, 8);
            this.Type = Convert.ToChar(GetString(buff, 156, 1));
            this.LinkedName = GetString(buff, 157, 100);

            this.PrefixName = GetString(buff, 345, 155);

            if (Type != '0')
            {
                int a = 0;
            }
        }

        private string GetString(byte[] buff, int index, int count)
        {
            return Encoding.ASCII.GetString(buff, index, count).TrimEnd('\0');
        }

        internal bool Valid { get; private set; }
        internal string Name { get; set; }
        internal string LinkedName { get; private set; }
        internal string PrefixName { get; private set; }
        internal char Type { get; private set; }
        internal ulong Size { get; private set; }

        public override string ToString()
        {
            return String.Format("{{{0}; {1}B; {2}}}", Name, Size, Type);
        }
    }
}
