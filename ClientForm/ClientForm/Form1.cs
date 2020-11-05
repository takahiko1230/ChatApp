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
        //どちらが送信したか
        private bool messenger;
        //縦方向の文字位置
        int vertical = 27;

        TextBox[] textBox;

        public Form1()
        {
            InitializeComponent();
            textBox = new TextBox[100];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            appif = new AppIF();
            if (!appif.Initialize())
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

            //テキストを送ったら削除する。
            WriteText.Clear();

            Send.Enabled = true;
        }

        //public bool DisplayMessage(string word)
        //{
        //    //限界文字数の時に行う操作（後回し）
        //    if (messagecount == 100)
        //    {

        //    }
        //    //水平方向
        //    int horizon = 51;

        //    if (messenger)
        //    {
        //        horizon = 558;
        //        vertical += 16;
        //    }

        //    TextBox Word = new TextBox();
        //    Controls.Add(Word);
        //    splitContainer1.Panel1.Controls.Add(Word);
        //    Word.Multiline = true;
        //    Word.AutoSize = true;
        //    Word.Location = new Point(horizon, vertical);
        //    Word.Anchor = (AnchorStyles)Right;
        //    Word.Name = "Word";
        //    Word.AutoSize = true;
        //    Word.TabIndex = 0;
        //    Word.Anchor = AnchorStyles.Top;
        //    Word.Anchor = AnchorStyles.Right;
        //    Word.Text = word;


        //    textBox[messagecount - 1] = Word;
        //    //メッセージ数のカウント
        //    messagecount += 1;

        //    return true;
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    messenger = true;
        //    DisplayMessage(WriteText.Text);
        //    WriteText.Text = "";
        //}
    }
}
