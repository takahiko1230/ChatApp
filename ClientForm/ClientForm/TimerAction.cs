using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClientForm
{
    class TimerAction
    {
        private Timer timer;

        //コンストラクタ
        public TimerAction()
        {

        }
        //セッター、ゲッター
        public Timer Timer_
        {
            set
            {
                timer = value;
            }
            get
            {
                return timer;
            }
        }

        public bool Initialize()
        {
            // Create a timer and set a two second interval.
            timer = new Timer();
            timer.Interval = 1000;
           return true;
        }
    }
}
