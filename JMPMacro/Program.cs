using System.Threading;
using System.Windows.Forms;

namespace JMPMacro {
    class Program {
        static bool ExitProgram;

        static void Main(string[] args) {
            //키보드 후킹
            Keyhook.KeyPressed += KeyEvent;
            Keyhook.Hook();
            while(!ExitProgram) {
                Application.DoEvents(); //메세지 대기
                Thread.Sleep(0x20);     //스레드 대기
            }
        }
        
        static void KeyEvent(Keys vkKey, ref bool ret) {
            switch(vkKey) {
            case Keys.MediaPlayPause:
                ret = !Player.Play();   //재생/일시정지
                if(!ret) ExitProgram = true;
                break;
            case Keys.MediaNextTrack:
                ret = !Player.Next();   //다음 트랙 재생
                if(!ret) ExitProgram = true;
                break;
            case Keys.MediaPreviousTrack:
                ret = !Player.Prev();   //이전 트랙 재생
                if(!ret) ExitProgram = true;
                break;
            case Keys.MediaStop:
                Keyhook.Unhook();       //정지 (프로그램 종료)
                ret = ExitProgram = true;
                break;
            }
        }
    }
}
