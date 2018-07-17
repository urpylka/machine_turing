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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        //public void updateDGV(String path)
        //{
        //    using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
        //    {
        //        string line = sr.ReadLine();
        //        String[] buffer = Form1.frString(line);
        //        for (int i = 0; i < buffer.Length; i++) this.dataGridView1.Columns.Add(buffer[i], buffer[i]);
        //        this.dataGridView1.Rows.Add(buffer);
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            buffer = Form1.frString(line);
        //            this.dataGridView1.Rows.Add(buffer);
        //        }
        //    }
        //}
        public void updateDGV(String infoPathFile)
        {
            //string infoPathFile = @"info.txt";
            StreamReader sr = new StreamReader(infoPathFile, Encoding.UTF8);
            string text = sr.ReadToEnd();
            richTextBox1.AppendText(text);
        }
        public String[,] biff;
        //public void updateDGV(String[,] biff2)
        //{
        //    biff = biff2;
        //    int b1 = biff.GetLength(1);
        //    int b0 = biff.GetLength(0);

        //    for (int i = 0; i < b1; i++) this.dataGridView1.Columns.Add(biff[0,i], biff[0,i]);
        //    for (int i = 0; i < b0; i++)
        //    {
        //        string[] bf3 = new string[b1];
        //        for (int j = 0; j < b1; j++) bf3[j] = biff[i, j];
        //        this.dataGridView1.Rows.Add(bf3);
        //    }
        //}
        private void Form3_Load(object sender, EventArgs e)
        {
            //dataGridView1.ColumnHeadersVisible = false;
            //dataGridView1.RowHeadersVisible = false;
        }
    }
}
