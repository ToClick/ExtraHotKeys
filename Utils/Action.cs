using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExtraHotKeys.Utils
{
    internal class Action
    {
        #region IMPORTS


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);


        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, IntPtr dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);


        [DllImport("Kernel32.dll")]
        static extern uint QueryFullProcessImageName(IntPtr hProcess, uint flags, StringBuilder text, out uint size);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        #region ENUMS

        public enum ListModules : uint
        {
            LIST_MODULES_DEFAULT = 0x0,
            LIST_MODULES_32BIT = 0x01,
            LIST_MODULES_64BIT = 0x02,
            LIST_MODULES_ALL = 0x03
        }

        [Flags]
        enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }
        #endregion

        public static void Run(string app)
        {
            if (GetWindowByPath(app, out IntPtr hWnd))
            {
                ActivateWindow(hWnd);
                return;
            }

            Process.Start(app);
        }

        private static void ActivateWindow(IntPtr hWnd)
        {
            const int restore = 9;
            
            if (hWnd == GetForegroundWindow())
            {
                return;
            }
            
            if (IsIconic(hWnd))
            {
                ShowWindow(hWnd, restore);
            }

            SetForegroundWindow(hWnd);
        }

        private static bool GetWindowByPath(string path, out IntPtr appWnd)
        {
            var result = IntPtr.Zero;
            appWnd = IntPtr.Zero;

            EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                GetWindowThreadProcessId(hWnd, out IntPtr processId);

                if (hWnd == (IntPtr)0x0000000000020448)
                {

                }

                IntPtr hProcess = OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VMRead, false, processId);

                if (hProcess == IntPtr.Zero)
                {
                    return true;
                }

                uint nChars = 256;
                var buffer = new StringBuilder((int)nChars);

                if (QueryFullProcessImageName(hProcess, 0, buffer, out nChars) != 0)
                {
                    if (CompareApps(path, buffer.ToString()))
                    {
                        result = hWnd;
                        return false;
                    }

                    return true;
                }
                
                return true;
            }, IntPtr.Zero);

            appWnd = result;
            return result != IntPtr.Zero;
        }

        private static bool CompareApps(string srcPath, string procPath)
        {
            if (srcPath == procPath)
            {
                return true;
            }
            
            string srcDir = Path.GetDirectoryName(srcPath);
            string prtDir = srcDir + "\\app\\";

            return procPath.Contains(prtDir);
        }

    }
}
