using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KOCharp
{
    public class INIReader
    {
        string Path = string.Empty;
        public INIReader(string path)
        {
            this.Path = path;
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public string Read(string Section, string key)
        {
            string Default = String.Empty;
            StringBuilder StrBuild = new StringBuilder(256);
            GetPrivateProfileString(Section, key, Default, StrBuild, 255, Path);
            byte[] bytes = Encoding.Default.GetBytes(StrBuild.ToString());
            return Encoding.UTF8.GetString(bytes);
        }

        public string GetString(string Section, string key)
        {
            return Read(Section, key);
        }

        public int GetInt(string Section, string key)
        {
            return int.Parse(Read(Section, key));
        }

        public short GetShort(string Section, string key)
        {
            return short.Parse(Read(Section, key));
        }

        public byte GetByte(string Section, string key)
        {
            return byte.Parse(Read(Section, key));
        }
    }
}
