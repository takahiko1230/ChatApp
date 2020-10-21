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
        private Socket sck;
        private IPEndPoint ipe;
        public SocketServer()
        {

        }
        public bool Initialize()
        {
            XDocument xml = XDocument.Load(System.IO.Directory.GetCurrentDirectory()+ "\\ClientForm.xml");
            XElement socketserver = xml.Element("SocketServer");
            var port = socketserver.Element("Port");
            int pot = int.Parse(port.Value);

            var address= socketserver.Element("Address");
            IPAddress addre= IPAddress.Parse(address.Value);

            ipe = new IPEndPoint(addre,pot);
            sck = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            return true;
        }

        public bool Connect()
        {
            try
            {
                sck.Connect(ipe);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
