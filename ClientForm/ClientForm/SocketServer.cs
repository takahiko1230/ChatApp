using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            try
            {
                XDocument xml = XDocument.Load(System.IO.Directory.GetCurrentDirectory() + "\\ClientForm.xml");
                XElement socketserver = xml.Element("SocketServer");
                var port = socketserver.Element("Port");
                int pot = int.Parse(port.Value);

                var address = socketserver.Element("Address");
                IPAddress addre = IPAddress.Parse(address.Value);

                ipe = new IPEndPoint(addre, pot);

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

        public bool SendMessage(byte[] msg)
        {
            try
            {
                socket.Send(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
