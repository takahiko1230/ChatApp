using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServer
{
    public partial class Form1 : Form
    {
        private ServerIf serverIf;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serverIf = new ServerIf();
            if (!serverIf.Initialilze())
            {
                return;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serverIf.StartServer())
            {
                return;
            }
        }
    }
}
