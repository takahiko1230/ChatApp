using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

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


        //タイマーイベント用ソケット
        private Socket timersocket;
        private IPEndPoint timeripe;
        //タイマーイベント用デリゲートメソッド
        GetData gtd;

        //コンストラクタ
        public ServerSocket()
        {
        }

        public bool AcceptMethod(GetByter gtb,GetData gtd)
        {
            this.gtb = gtb;
            this.gtd = gtd;
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

                if (!TimerInitiailze(socketserver))
                {
                    return false;
                }

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

            int bytesRec = client.Receive(bytes);

            XmlSerializer xs = new XmlSerializer(typeof(Sender));
            MemoryStream mem = new MemoryStream(bytes);
            Sender rev = xs.Deserialize(mem) as Sender;
            mem.Dispose();

            //デリゲート関数を呼び出す
            gtb(rev.msg,rev.id,rev.toid);

            client.Close();

            Accept();
        }

        //タイマーイベント用ソケット作成
        public bool TimerInitiailze(XElement socketserver)
        {
            try
            {
                var timerport = socketserver.Element("TimerPort");
                int timerpot = int.Parse(timerport.Value);

                var address = socketserver.Element("Address");
                IPAddress addre = IPAddress.Parse(address.Value);

                timeripe = new IPEndPoint(addre, timerpot);
                timersocket = new Socket(addre.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public bool TimerBind()
        {
            try
            {
                timersocket.Bind(timeripe);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool TimerListen()
        {
            try
            {
                timersocket.Listen(2);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool TimerAccept()
        {
            try
            {
                //接続要求待機を開始する
                timersocket.BeginAccept(new AsyncCallback(AcceptTimerCallback), timersocket);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //タイマーイベント受信時のコールバック関数
        private void AcceptTimerCallback(System.IAsyncResult ar)
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
                TimerAccept();
                return;
            }

            int bytesRec = client.Receive(bytes);
            string data1 = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            int id = int.Parse(data1);

            //デリゲート関数を呼び出す
            TimerSender ts = gtd(id);
            try
            {
                //データベースから取り出せないときに処理を抜け出す。
                if (ts.sendMsg[0] == null)
                {
                    return;
                }

            }
            catch(Exception e)
            {
                TimerAccept();
                return;
            }
            finally
            {
                XmlSerializer xs = new XmlSerializer(typeof(TimerSender));

                //シリアライズして送信する
                using (MemoryStream mem = new MemoryStream())
                {
                    //シリアライズ
                    xs.Serialize(mem, ts);

                    byte[] serializedDataserializedData = mem.ToArray();

                    client.Send(serializedDataserializedData);

                }

                client.Close();

                TimerAccept();
            }
        }
    }
}
