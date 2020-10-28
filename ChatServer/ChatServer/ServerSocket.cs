using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChatServer
{
    class ServerSocket
    {
        //初期設定ソケット
        private Socket socket;
        //アクセプトソケット
        private Socket accSocket;
        //IPアドレスとポートの組み合わせ
        private IPEndPoint ipe;

        //アクセプト後メソッド
        GetByter gtb;


        //コンストラクタ
        public ServerSocket()
        {
        }

        public bool AcceptMethod(GetByter gtb)
        {
            this.gtb = gtb;
            return true;
        }

        public bool Initialize()
        {
            try
            {
                XDocument xml = XDocument.Load(System.IO.Directory.GetCurrentDirectory() + "\\ChatServer.xml");
                XElement socketserver = xml.Element("SocketServer");
                var port = socketserver.Element("Port");
                int pot = int.Parse(port.Value);

                var address = socketserver.Element("Address");
                IPAddress addre = IPAddress.Parse(address.Value);

                ipe = new IPEndPoint(addre, pot);
                socket = new Socket(addre.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Bind()
        {
            try
            {
                socket.Bind(ipe);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Listen()
        {
            try
            {
                socket.Listen(2);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Accept()
        {
            try
            {
                //接続要求待機を開始する
                socket.BeginAccept(new AsyncCallback(AcceptCallback), socket);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //アクセプトしたときに呼ばれるコールバック
        private void AcceptCallback(System.IAsyncResult ar)
        {
            byte[] bytes = new byte[1024];
            //サーバーSocketの取得
            Socket server = (Socket)ar.AsyncState;

            //接続要求を受け入れる
            Socket client = null;
            try
            {
                //クライアントSocketの取得
                client = server.EndAccept(ar);
            }
            catch
            {
                Accept();
                return;
            }
            int bytesRec = socket.Receive(bytes);

            string data1 = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            client.Close();

            Accept();
        }
    }
}
