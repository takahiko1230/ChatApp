using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class Form1 : Form
    {
        private AppIF appif;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            appif = new AppIF();
            if (!appif.Initialize())
            {
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
