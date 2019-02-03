using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace JMPMacro {
    class Program {
        static bool ExitProgram;
        static Mutex InstanceMutex;

        static void Main(string[] args) {
            //중복 실행 검사
            const string MutexName = "JMPMacroInstance";
            InstanceMutex = new Mutex(true, MutexName, out bool isNew);
            if(!isNew) return;

            //키보드 후킹
            Keyhook.KeyPressed += KeyEvent;
            Keyhook.Hook();
            while(!ExitProgram) {
                Application.DoEvents(); //메세지 대기
                Thread.Sleep(0x20);     //스레드 대기
            }
        }
        
        static void KeyEvent(Keys vkKey, ref bool bNext) {
            switch(vkKey) {
            case Keys.MediaPlayPause:
                bNext = !Player.Play();   //재생/일시정지
                if(bNext) ExitProgram = true;
                break;
            case Keys.MediaNextTrack:
                bNext = !Player.Next();   //다음 트랙 재생
                if(bNext) ExitProgram = true;
                break;
            case Keys.MediaPreviousTrack:
                bNext = !Player.Prev();   //이전 트랙 재생
                if(bNext) ExitProgram = true;
                break;
            case Keys.MediaStop:
                Keyhook.Unhook();       //정지 (프로그램 종료)
                bNext = ExitProgram = true;
                break;
            }
        }
    }
}
