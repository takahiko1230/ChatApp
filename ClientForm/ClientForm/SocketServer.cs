using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ClientForm
{
    class SocketServer
    {
        //送信用ソケット
        private Socket socket;
        private IPEndPoint ipe;

        //送信用クラス
        Sender sender;

        //タイマーイベント用ソケット
        private Socket timerSocket;
        private IPEndPoint timeipe;

        //自分のid
        private int id;
        //相手のid
        private int toid;

        public Socket timersocket
        {
            set
            {
                timerSocket = value;
            }
            get
            {
                return timerSocket;
            }
        }

        public SocketServer()
        {

        }
        public bool Initialize()
        {
            try
            {
                XDocument xml = XDocument.Load(System.IO.Directory.GetCurrentDirectory() + "\\ClientForm.xml");
                XElement socketserver = xml.Element("SocketServer");

                //設定ファイルから自分のIDを取得する
                var iD = socketserver.Element("ID");
                id = int.Parse(iD.Value);

                //設定ファイルから送信先のIDを取得する
                var toId = socketserver.Element("ToID");
                toid = int.Parse(toId.Value);

                //送信用クラス作成
                sender = new Sender(id,toid);

                var port = socketserver.Element("Port");
                int pot = int.Parse(port.Value);

                var address = socketserver.Element("Address");
                IPAddress addre = IPAddress.Parse(address.Value);

                ipe = new IPEndPoint(addre, pot);

                //タイマー用のソケットを初期化
                if (!TimerInitialize(socketserver))
                {
                    return false;
                }

                return true;

            }catch(Exception e)
            {
                return false;
            }
        }

        //ソケットを作成する。
        public bool CreateSocket()
        {
            socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //レシーブする際のタイムアウト値を設定
            socket.ReceiveTimeout = 5000;

            if (socket == null)
            {
                return false;
            }
            return true;
        } 

        public bool Connect()
        {
            try
            {
                socket.Connect(ipe);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SendMessage(string text)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Sender));
            try
            {
                sender.msg = text;

                using (MemoryStream mem = new MemoryStream())
                {
                    //シリアライズ
                    xs.Serialize(mem, sender);

                    byte[] serializedDataserializedData = mem.ToArray();

                    socket.Send(serializedDataserializedData);

                }

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool TimerInitialize(XElement socketserver)
        {
            try
            {
                //タイマーイベント用のソケットを初期化
                var timerport = socketserver.Element("TimerPort");
                int timerPort = int.Parse(timerport.Value);

                //ipアドレスを取得
                var address = socketserver.Element("Address");
                IPAddress addre = IPAddress.Parse(address.Value);

                timeipe = new IPEndPoint(addre, timerPort);

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        //ソケットを作成する。
        public bool CreateTimerSocket()
        {
            timerSocket = new Socket(timeipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //レシーブする際のタイムアウト値を設定
            timerSocket.ReceiveTimeout = 5000;

            if (timerSocket == null)
            {
                return false;
            }
            return true;
        }

        public bool TimerConnect()
        {
            try
            {
                timerSocket.Connect(timeipe);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SendTimerMessage(Form1 f1,AppIF appif)
        {
            try
            {
                string str = id.ToString();
                byte[] msg = Encoding.UTF8.GetBytes(str);
                timerSocket.Send(msg);

                //受信用のバイト列作成
                byte[] bytes = new byte[1024];
                //送信後受信待機
                int byteRec = timerSocket.Receive(bytes);
                //受信データをデシリアライズする
                XmlSerializer xs = new XmlSerializer(typeof(TimerSender));
                MemoryStream mem = new MemoryStream(bytes);
                TimerSender ts = xs.Deserialize(mem) as TimerSender;

                f1.Invoke(new Action<TimerSender>(appif.DisplayWord), ts);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
