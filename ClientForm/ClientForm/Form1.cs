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
        //メッセージ数
        private int messagecount = 1;
        //縦方向の文字位置
        int vertical = 0;

        private Form1 form1;
        public Form1 FormOne
        {
            set
            {
                form1 = value;
            }
            get
            {
                return form1;
            }
        }

        TextBox[] textBox;

        public Form1()
        {
            InitializeComponent();
            textBox = new TextBox[100];
            form1 = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            appif = new AppIF();
            if (!(appif.Initialize(this)))
            {
                return;
            }

        }

        private void Send_Click(object sender, EventArgs e)
        {
            //テキストが入力されていないとき抜け出します
            if (WriteText.Text == "")
            {
                Send.Enabled = true;
                return;
            }

            //ボタン押下できないようにする
            Send.Enabled = false;

            //おくれなかったとき
            if (!appif.SendText(WriteText.Text))
            {
                WriteText.Clear();
                Send.Enabled = true;
                return;
            }

            DisplayMessage(WriteText.Text, 1);

            //テキストを送ったら削除する。
            WriteText.Clear();

            Send.Enabled = true;
        }

        public bool DisplayMessage(string word ,int messenger)
        {
            //限界文字数の時に行う操作（後回し）
            if (messagecount == 100)
            {

            }
            //水平方向
            int horizon = 33;

            if (messenger == 1)
            {
                horizon = 200;
            }
            vertical += 27;

            TextBox Word = new TextBox();
            Controls.Add(Word);
            splitContainer1.Panel1.Controls.Add(Word);
            Word.Width = 100;
            Word.Height = 20;
            Word.Multiline = true;
            Word.WordWrap = true;
            Word.Location = new Point(horizon, vertical);
            Word.Name = "Word";
            Word.AutoSize = true;
            Word.TabIndex = 0;
            Word.ReadOnly = true;
            Word.Text = word;


            textBox[messagecount - 1] = Word;
            //メッセージ数のカウント
            messagecount += 1;

            return true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DisplayMessage(WriteText.Text, 1);
            
        }
    }
}
