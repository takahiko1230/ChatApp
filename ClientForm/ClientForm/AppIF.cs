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
        //コンストラクタ
        public AppIF()
        {

        }

        public bool Initialize()
        {
            socketserver = new SocketServer();
            timerAction = new TimerAction();

            if (!socketserver.Initialize())
            {
                return  false;
            }

            if (socketserver.Connect())
            {
                return false;
            }

            if (!timerAction.Initialize())
            {
                return false;
            }
            timerAction.Timer_.Elapsed += OnTimedEvent;


            return true;
        }

        public bool SendText(string text)
        {
            byte[] msg = Encoding.UTF8.GetBytes(text);
            if (!socketserver.SendMessage(msg))
            {
                return false;
            }

            return true;
        }


        //タイマーイベント
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            
        }
    }
}
