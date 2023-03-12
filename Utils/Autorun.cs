using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExtraHotKeys.Utils
{
    internal class Autorun
    {
        private readonly string _name;
        private readonly string _path;

        public Autorun(string name, string path)
        {
            _name = name;
            _path = path;
        }

        private RegistryKey GetKey(bool writable)
        {
            return Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable);
        }

        public bool IsInstalled()
        {
            return GetKey(false).GetValue(_name) != null;
        }

        public void Install()
        {
            GetKey(true)?.SetValue(_name, _path);
        }

        public void Remove()
        {
            GetKey(true)?.DeleteValue(_name, false);
        }




    }
}
