using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public delegate void GetByter(string s);
    public delegate void Display(string m);
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

            form1 = form;

            //サーバークラスを初期化する
            if (!serverSocket.Initialize())
            {
                return false;
            }

            GetByter gtb = getByter;

            //アクセプトしたときに返すメソッドをソケット側に定義
            if (!serverSocket.AcceptMethod(gtb))
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

        //DBへつなげる
        public void getByter(string byter)
        {
            form1.Invoke(new Action<string>(this.DisplayWord), byter);
        }

        public void DisplayWord(string ss)
        {
            form1.textbox1.Text = ss;
        }
    }
}
