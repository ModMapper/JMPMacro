using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;

namespace JMPMacro {
    static class Keyhook {
        #region "API"
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyHookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hhk, int nCode, int wParam, ref KeyData lParam);

        private delegate int KeyHookProc(int nCode, int wParam, ref KeyData lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyData {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x101;
        #endregion


        public delegate void KeyEvent(Keys vkKey, ref bool bNext);
        public static event KeyEvent KeyPressed;
        public static event KeyEvent KeyUnpressed;

        private static readonly KeyHookProc proc;
        private static IntPtr hHook;

        static Keyhook()
            => proc = new KeyHookProc(Callback);

        /// <summary>키보드를 후킹합니다.</summary>
        /// <returns>후킹의 성공 여부입니다.</returns>
        public static bool Hook() {
            if(hHook != IntPtr.Zero) return false;
            IntPtr h = Marshal.GetHINSTANCE(typeof(Keyhook).Module);
            hHook = SetWindowsHookEx(WH_KEYBOARD_LL, proc, h, 0);
            return hHook != IntPtr.Zero;
        }

        /// <summary>키보드 후킹을 해제합니다.</summary>
        /// <returns>후킹 해제의 성공 여부입니다.</returns>
        public static bool Unhook() {
            var ret = UnhookWindowsHookEx(hHook);
            hHook = IntPtr.Zero;
            return ret;
        }

        /// <summary>키보드 후킹 콜백</summary>
        private static int Callback(int nCode, int wParam, ref KeyData lParam) {
            if(0 <= nCode) {
                var next = true;
                if(wParam == WM_KEYDOWN)
                    KeyPressed?.Invoke((Keys)lParam.vkCode, ref next);
                else if(wParam == WM_KEYUP)
                    KeyUnpressed?.Invoke((Keys)lParam.vkCode, ref next);
                if(!next) return 1;  //
            }
            return CallNextHookEx(hHook, nCode, wParam, ref lParam);
        }
    }
}
