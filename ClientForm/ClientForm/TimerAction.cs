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
            timer.Interval = 5000;
            // Have the timer fire repeated events (true is the default)
            timer.AutoReset = true;
            // Start the timer
            timer.Enabled = true;
            return true;
        }
    }
}
