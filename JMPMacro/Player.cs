using System.Runtime.InteropServices;
using System;

namespace JMPMacro {
    static class Player {
        #region "API"
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private const int WM_COMMAND = 0x0111;
        #endregion

        //지니뮤직 창 정보
        private const string Caption = "지니뮤직";
        private const string Class = "#32770";

        //지니뮤직 메뉴 파라메터
        //private const int wMute = 0x271E; //음소거
        private const int wPlay = 0x180011C7; //0x2723; //재생 명령
        private const int wNext = 0x180011C8; //0x2724; //다음 명령
        private const int wPrev = 0x180011C6; //0x2722; //이전 명령
        
        public static bool Play()
            => Command(GetHandle(), wPlay);

        public static bool Next()
            => Command(GetHandle(), wNext);

        public static bool Prev()
            => Command(GetHandle(), wPrev);

        private static IntPtr GetHandle()
            => FindWindow(Class, Caption);

        private static bool Command(IntPtr hwnd, int Param) {
            if(hwnd == IntPtr.Zero) return false;
            return PostMessage(hwnd, WM_COMMAND, Param, 0);
        }
    }
}
