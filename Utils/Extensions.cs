using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtraHotKeys.Utils
{
    public static class VkeysExtensions
    {
        public static string ToKeyString(this VKeys Key)
        {
            FieldInfo fi = Key.GetType().GetField(Key.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return Key.ToString();
        }



        public static string ToActionString(this VKeys[] Array)
        {
            if (Array == null || Array.Length == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            List<string> lines = new List<string>();
            VKeys[] priority = new VKeys[] {
                VKeys.LeftControl, VKeys.RightControl,
                VKeys.Shift, VKeys.LeftShift, VKeys.RightShift,
                VKeys.Space, VKeys.MouseWheel, VKeys.Tab,
            };

            foreach (VKeys key in Array)
            {

                sb.AppendFormat("{0} + ", key.ToKeyString());
            }

            sb.Remove(sb.Length - 3, 3);
            return sb.ToString();
        }

        public static string ToActionString(this List<VKeys> Array)
        {
            StringBuilder sb = new StringBuilder();
            List<string> lines = new List<string>();
            VKeys[] priority = new VKeys[] {
                VKeys.LeftControl, VKeys.RightControl,
                VKeys.Shift, VKeys.LeftShift, VKeys.RightShift,
                VKeys.Space, VKeys.MouseWheel, VKeys.Tab,
            };

            foreach (VKeys key in Array)
            {

                sb.AppendFormat("{0} + ", key.ToKeyString());
            }

            sb.Remove(sb.Length - 3, 3);
            return sb.ToString();
        }

        public static VKeys[] ParseFromAction(this string Text)
        {
            List<VKeys> keys = new List<VKeys>();

            string[] lines = Text.Split(new string[] { " + " }, StringSplitOptions.None);


            foreach (var line in lines)
            {
                foreach (VKeys name in Enum.GetValues(typeof(VKeys)))
                {
                    if (name.ToKeyString() == line)
                    {
                        keys.Add(name);
                    }
                }
            }

            return keys.ToArray();
        }
    }

    public static class Extensions
    {
        public static int TagInt(this Control control)
        {
            if (control == null || control.Tag == null)
            {
                return -1;
            }

            if (int.TryParse(control.Tag.ToString(), out var value))
            {
                return value;
            }

            return -1;
        }


        public static void AbortSafe(this Thread thread)
        {
            try
            {
                thread.Abort();
                thread?.Abort();
            }
            catch
            {

            }
        }

        public static Thread StartBackground(this Thread thread)
        {
            thread.IsBackground = true;
            thread.Start();

            return thread;
        }
    }
}
