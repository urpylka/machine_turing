using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace machine_turing
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();

            webBrowser1.Navigate(new Uri("file:///C:/Users/SMIRART/Documents/Visual%20Studio%202015/Projects/machine_turing/machine_turing/bin/Debug/info/info.html"));
            //webBrowser1.Url = System.Uri("info/info.html");

            //string infoPathFile = @"inf.txt";
            //StreamReader sr = new StreamReader(infoPathFile, Encoding.UTF8);
            //string text = sr.ReadToEnd();
            //richTextBox1.AppendText(text);

            //richTextBox1.Text = System.IO.ReadAllText(infoPathFile);
        }


        private void richTextBox1_LinkClicked(Object sender, LinkClickedEventArgs e)
        {
            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "LinkText", e.LinkText);
            messageBoxCS.AppendLine();
            System.Diagnostics.Process.Start(e.LinkText);
            //MessageBox.Show(messageBoxCS.ToString(), "LinkClicked Event");
        }
    }
}
