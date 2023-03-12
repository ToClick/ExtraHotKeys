using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtraHotKeys.Utils;
using ExtraHotKeys.Utils;

namespace ExtraHotKeys
{
    [StructLayout(LayoutKind.Sequential)]
    struct Point
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEHOOKSTRUCT
    {
        public Point pt;
        public IntPtr hwnd;
        public uint wHitTestCode;
        public IntPtr dwExtraInfo;
    }



    internal static class Hook
    {
        private static WinAPI.LowLevelKeyboardProc _keysDelegate = KeysCallback;
        private static WinAPI.LowLevelKeyboardProc _mouseDelegate = MouseCallback;

        private static IntPtr _keysPtr = IntPtr.Zero;
        private static IntPtr _mousePtr = IntPtr.Zero;

        private static bool[] _pressed = new bool[short.MaxValue];
        private static StringBuilder _debug = new StringBuilder("");
        public static List<KeyAction> Actions = new List<KeyAction>();

        public static bool Enable = true;



        #region DEBUG
        public static bool DebugPop(out string text)
        {
            text = _debug.ToString();

            if (text.Length == 0)
            {
                return false;
            }

            _debug.Clear();
            return true;
        }

        public static string DebugState()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _pressed.Length; i++)
            {
                if (_pressed[i])
                {
                    sb.AppendFormat("{0} - ", i);
                }
            }

            return sb.ToString();

        }
        #endregion

        public static void Attach()
        {
            _keysPtr = WinAPI.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, _keysDelegate, WinAPI.GetModuleHandle(null), 0);
            _mousePtr = WinAPI.SetWindowsHookEx(HookType.WH_MOUSE_LL, _mouseDelegate, WinAPI.GetModuleHandle(null), 0);
        }

        public static void Detach()
        {
            WinAPI.UnhookWindowsHookEx(_keysPtr);
            WinAPI.UnhookWindowsHookEx(_mousePtr);
        }


        public static List<VKeys> GetPressed()
        {
            var pressed = new List<VKeys>();

            for(int i=0; i<_pressed.Length; i++)
            {
                if (_pressed[i])
                {
                    pressed.Add((VKeys)i);
                }
            }
            

            return pressed;
        }

        public static void RegisterAction(KeyAction action)
        {
            Actions.Add(action);
        }
        private static bool CheckActions()
        {
            try
            {
                foreach (var action in Actions)
                {
                    bool run = true;

                    if (!action.enable)
                    {
                        continue;
                    }
                    
                    for (int i = 0; i < action.keys.Length; i++)
                    {
                        if (!_pressed[action.keys[i]])
                        {
                            run = false;
                        }
                    }

                    if (run)
                    {
                        try
                        {
                            ResetKeys();
                            _debug.AppendFormat("START APP: {0}\n", action.app);

                            Utils.Action.Run(action.app);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        return true;
                    }

                }

                return false;
            }
            catch
            {

                return true;
            }
            finally
            {

            }
        }

        private static bool SetKeyState(short Key, bool IsDown)
        {
            _pressed[Key] = IsDown;

            return Enable && CheckActions();
        }

        private static void ResetKeys()
        {
            for (var i = 0; i < _pressed.Length; i++)
            {
                _pressed[i] = false;
            }
        }

        
        private static IntPtr KeysCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            var keyCode = Marshal.ReadInt32(lParam);
            var wInt = wParam.ToInt32();

            if ((short)VKeys.Back == (short)keyCode)
            {

            }
            else if ((short)VKeys.Snapshot == (short)keyCode)
            {

            }
            else if (wInt == WinAPI.WM_KEYDOWN || wInt == WinAPI.WM_SYSKEYDOWN)
            {
                if (SetKeyState((short)keyCode, true))
                {
                    return (IntPtr)1;
                }
            }
            else if (wInt == WinAPI.WM_KEYUP || wInt == WinAPI.WM_SYSKEYUP)
            {
                if (SetKeyState((short)keyCode, false))
                {
                    return (IntPtr)1;
                }
            } 
            else
            {

            }

            DebugCallback(true, keyCode, wParam, lParam);
            return WinAPI.CallNextHookEx(_keysPtr, code, wParam, lParam);
        }

        private static IntPtr MouseCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (wParam == (IntPtr)0x200 || wParam == (IntPtr)0x20A)
            {
                return WinAPI.CallNextHookEx(_mousePtr, code, wParam, lParam);
            }

            DebugCallback(false, code, wParam, lParam);

            // middle btn press
            if ((IntPtr)WinAPI.WM_MBUTTONDOWN == wParam)
            {
                DebugCallback(false, code, wParam, lParam);
                SetKeyState((short)VKeys.MouseWheel, true);
            }

            // middle btn up
            if ((IntPtr)WinAPI.WM_MBUTTONUP == wParam)
            {
                DebugCallback(false, code, wParam, lParam);
                SetKeyState((short)VKeys.MouseWheel, false);
            } 

            // xbuttons
            if ((IntPtr)WinAPI.WM_XBUTTONDOWN == wParam || (IntPtr)WinAPI.WM_XBUTTONUP == wParam)
            {
                bool isDown = (IntPtr)WinAPI.WM_XBUTTONDOWN == wParam;
                var xkey = VKeys.XBUTTON_1;

                var mhs = Marshal.PtrToStructure<MOUSEHOOKSTRUCT>(lParam);

                if (mhs.hwnd == (IntPtr)0x10000)
                {
                    xkey = VKeys.XBUTTON_1;
                }
                if (mhs.hwnd == (IntPtr)0x20000)
                {
                    xkey = VKeys.XBUTTON_2;
                }

                DebugCallback(false, (short)xkey, wParam, lParam);
                SetKeyState((short)xkey, isDown);
            }


            return WinAPI.CallNextHookEx(_mousePtr, code, wParam, lParam);
        }

        private static void DebugCallback(bool keyBoard, int code, IntPtr wParam, IntPtr lParam)
        {
            string device = (keyBoard ? "KEYS" : "MOUSE");
            string line = string.Format("{0} - {1} {2} [w={3} | l={4}]", device, code.ToString(), code.ToString("X"), wParam.ToString("X"), lParam.ToString("X"));
            _debug.AppendLine(line);
        }
    }
}
 