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

        private TextBox text1;


        public TextBox textbox1
        {
            set
            { 
                text1 = value; 
            }
            get
            {
                return text1;
            }
        }

        public Form1()
        {
            InitializeComponent();
            text1 = textBox1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serverIf = new ServerIf();
            if (!serverIf.Initialilze(this))
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
