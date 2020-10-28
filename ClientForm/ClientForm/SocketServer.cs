using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LoggerClass;

namespace ClientForm
{
    class SocketServer
    {
        //ソケット
        private Socket socket;
        private IPEndPoint ipe;
        public SocketServer()
        {

        }
        public bool Initialize()
        {
            string methodName = "Initialize()";
            try
            {

                Logger.WriteLine("SocketServer"+methodName+"Start");

                XDocument xml = XDocument.Load(System.IO.Directory.GetCurrentDirectory() + "\\ClientForm.xml");
                XElement socketserver = xml.Element("SocketServer");
                var port = socketserver.Element("Port");
                int pot = int.Parse(port.Value);

                var address = socketserver.Element("Address");
                IPAddress addre = IPAddress.Parse(address.Value);

                ipe = new IPEndPoint(addre, pot);
                socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                //レシーブする際のタイムアウト値を設定
                socket.ReceiveTimeout = 5000;

                return true;
            }catch(Exception e)
            {
                Logger.WriteLine("SocketServer" + methodName + "Error");
                return false;
            }
        }

        //ソケットのコネクト
        public bool Connect()
        {
            string methodName = "Connect()";
            try
            {
                socket.Connect(ipe);
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("SocketServer" + methodName + "Error");
                return false;
            }
        }


        //メッセージをサーバーへ送信
        public bool SendMessage(byte[] msg)
        {
            string methodName = "SendMessage()";
            try
            {
                socket.Send(msg);
                return true;
            }
            catch
            {
                Logger.WriteLine("SocketServer" + methodName + "Error");
                return false;
            }
        }
    }
}
