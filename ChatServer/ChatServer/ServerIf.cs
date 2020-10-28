using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public delegate void GetByter(string s);
    class ServerIf
    {
        private ServerSocket serverSocket;
        private ConnectDB connectDB;

        public ServerIf()
        {

        }

        public bool Initialilze()
        {
            serverSocket = new ServerSocket();
            connectDB = new ConnectDB();

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

            return true;
        }

        //DBへつなげる
        public void getByter(string byter)
        {

        }
    }
}
