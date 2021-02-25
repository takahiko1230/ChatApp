using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientForm
{
    class AppIF
    {
        private SocketServer socketserver;
        private TimerAction timerAction;

        private AppIF appif;

        private Form1 f1;

        //コンストラクタ
        public AppIF()
        {

        }

        public bool Initialize(Form1 f1)
        {
            socketserver = new SocketServer();
            timerAction = new TimerAction();

            if (!socketserver.Initialize())
            {
                return  false;
            }

            if (!timerAction.Initialize())
            {
                return false;
            }

            //タイマーイベントをセット
            timerAction.Timer_.Elapsed += OnTimedEvent;

            appif =this;

            this.f1 = f1;

            return true;
        }

        public bool SendText(string text)
        {
            if (!socketserver.CreateSocket())
            {
                return false;
            }

            if (!socketserver.Connect())
            {
                return false;
            }

            if (!socketserver.SendMessage(text))
            {
                return false;
            }

            return true;
        }


        //タイマーイベント（定期的にサーバーとつなげる）
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (!socketserver.CreateTimerSocket())
            {
                return;
            }

            if (!socketserver.TimerConnect())
            {
                return;
            }

            if (!socketserver.SendTimerMessage(f1,appif))
            {
                return ;
            }
        }

        public void DisplayWord(TimerSender ts)
        {
            foreach(string word in ts.sendMsg)
            {
                f1.DisplayMessage(word, 0);
            }
        }
    }
}
