using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientForm
{
    class AppIF
    {
        private SocketServer socketserver;
        //コンストラクタ
        public AppIF()
        {

        }

        public bool Initialize()
        {
            socketserver = new SocketServer();
            if (!socketserver.Initialize())
            {
                return  false;
            }

            if (socketserver.Connect())
            {
                return false;
            }

            return true;
        }
    }
}
