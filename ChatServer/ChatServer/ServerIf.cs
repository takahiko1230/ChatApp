using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public delegate void GetByter(string s,int i,int k);
    public delegate void Display(string m);
    public delegate TimerSender GetData(int m);
    class ServerIf
    {
        private ServerSocket serverSocket;
        private ConnectDB connectDB;
        private Form1 form1;

        public ServerIf()
        {

        }

        public bool Initialilze(Form1 form)
        {
            serverSocket = new ServerSocket();
            connectDB = new ConnectDB();
            //デリゲート登録
            GetByter gtb = GetByter;
            GetData gtd = GetDatar;

            form1 = form;

            //サーバークラスを初期化する
            if (!serverSocket.Initialize())
            {
                return false;
            }


            //アクセプトしたときに返すメソッドをソケット側に定義
            if (!serverSocket.AcceptMethod(gtb,gtd))
            {
                return false;
            }

            //データベース接続クラスを初期化する
            if (!connectDB.Initialize())
            {
                return false;
            }

            return true;
        }

        //チャット受信用のソケット
        public bool StartServer()
        {
            if (!serverSocket.Bind())
            {
                return false;
            }
            
            if(!serverSocket.Listen())
            {
                return false;
            }

            if (!serverSocket.Accept()) 
            {
                return false;
            }
            return true;
        }

        //定期的な受信用ソケット
        public bool StartTimerServer()
        {
            if (!serverSocket.TimerBind())
            {
                return false;
            }

            if (!serverSocket.TimerListen())
            {
                return false;
            }

            if (!serverSocket.TimerAccept())
            {
                return false;
            }
            return true;
        }
        //DBへつなげる
        public void GetByter(string word,int id,int toid)
        {
            //データベースへ送る
            if(!connectDB.AddDB(word, id, toid))
            {
                return;
            }

            form1.Invoke(new Action<string>(this.DisplayWord), word);
        }

        public void DisplayWord(string ss)
        {
            form1.textbox1.Text = ss;
        }

        public TimerSender GetDatar(int id)
        {
            List<string> list = connectDB.GetMsg(id);
            TimerSender ts = new TimerSender();
            ts.sendMsg = list;
            
            return ts;
        }
    }
}
