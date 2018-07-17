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
    public partial class Form1 : Form
    {
        public Algorithm algo;
        public String[,] fromDGV(DataGridView dgv)
        {
            String[,] buffer = new String[dgv.RowCount-1, dgv.ColumnCount];
            for (int i = 0; i < buffer.GetLength(1); i++)
                for (int j = 0; j < buffer.GetLength(0); j++)
                    if (dgv[i, j].Value != null)
                        buffer[j, i] = dgv[i, j].Value.ToString();
                    else buffer[j, i] = "";
            return buffer;
        }
        public bool updated_algo()
        {
            try
            {
                Algorithm algo2 = new Algorithm(fromDGV(dataGridView1), fromDGV(dataGridView2));
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
                return false;
            }
            return true;
        }
        public void update_algo()
        {
            // if ((dataGridView2[0, dataGridView2.RowCount - 2].Value.ToString() == "") || (dataGridView2[0, dataGridView2.RowCount - 2].Equals(e.CellStyle.DataSourceNullValue)))

            if (dataGridView2[0, dataGridView2.RowCount - 2].Value == null)
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.RowCount - 2);
            }

            try
            {
                algo = new Algorithm(fromDGV(dataGridView1), fromDGV(dataGridView2));
                initTrasaFile2();
            }
            catch (Exception ex2)
            {
                if(dataGridView1.Enabled) MessageBox.Show(ex2.Message);
            }
        }
        public Form1()
        {
            InitializeComponent();
            

            initDGV();
            loadFrom(Std0AlgoFilePath);
            //update_algo();

            trackBar1.Minimum = 1;
            trackBar1.Maximum = 10;
            trackBar1.TickFrequency = 1;

            regim_zapuska(0);


            saveFileDialog2.Filter = "TMT files (*.tmt)|*.tmt";//"*.mt";
            saveFileDialog2.FilterIndex = 1;


            saveFileDialog1.Filter = "MT files (*.mt)|*.mt";//"*.mt";
            saveFileDialog1.FilterIndex = 1;
            openFileDialog1.Filter = "MT files (*.mt)|*.mt";//"*.mt";

        }
        public void initDGV()
        {
            //ЛЕНТА
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;
            for (int i = 0; i < 200; i++) this.dataGridView1.Columns.Add(i + "", i + "");
            string[] bf = new string[200];
            string[] bf2 = new string[200];
            //по хорошему из алгоритма надо достать первый символ (нулевой)
            for (int j = 0; j < 200; j++)
            {
                bf[j] = j + 1+"";
                bf2[j] = "_";
            }
            //bf2[0] = "*";
            //bf2[1] = "*";
            //bf2[2] = "*";
            //bf2[3] = "|";
            //bf2[4] = "*";
            //bf2[5] = "*";
            //bf2[6] = "*";
            //bf2[7] = "*";
            this.dataGridView1.Rows.Add(bf);
            this.dataGridView1.Rows.Add(bf2);
            //АЛГОРИТМ
            dataGridView2.ColumnHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;
            //String[,] biff = new String[2, 4] { { "", "*", "|", "_" }, { "1", "", "", "" } };
            //String[,] biff = new String[5, 4] { { "", "*", "|", "_" }, { "1", "*1r", "*2r", "" }, { "2", "*2r", "", "_3l" }, { "3", "_4n", "", "" }, { "4", "", "", "_0n" } };
            //int b1 = biff.GetLength(1);
            //int b0 = biff.GetLength(0);

            //for (int i = 0; i < b0; i++) this.dataGridView2.Columns.Add(biff[i,0], biff[i,0]);
            //for (int i = 0; i < b1; i++)
            //{
            //    string[] bf3 = new string[b0];
            //    for (int j = 0; j < b0; j++) bf3[j] = biff[j, i];
            //    this.dataGridView2.Rows.Add(bf3);
            //}
        }
        public void renderDGV2()
        {
            String[,] buffer = algo.toString();
            for (int i = 0; i < buffer.GetLength(1); i++) this.dataGridView2.Columns.Add(buffer[0, i], buffer[0, i]);
            for (int i = 1; i < buffer.GetLength(1); i++)
                for (int j = 0; j < buffer.GetLength(0); j++) this.dataGridView2.Rows.Add(buffer[j, 0], buffer[j, i]);

            //for (int i = 1; i < buffer.GetLength(0); i++)
            //    for (int j = 0; j < buffer.GetLength(1); j++)
            //        this.dataGridView2.Rows[i].Cells[j].Value = buffer[i, j];
            //this.dataGridView2[i,j] = buffer[i, j];
        } //NOT USED

        private void button5_Click(object sender, EventArgs e)
        {
            if (updated_algo())
            {
                update_algo();
                isPrepering(true);
                regim_zapuska(3);
                try
                {
                    algo.run(dataGridView1, dataGridView2);
                }
                catch (Exception ex)
                {
                    isPrepering(false);
                    regim_zapuska(3);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (updated_algo())
            {
                isPrepering(true);
                regim_zapuska(1);
                try
                {
                    if (!algo.next(dataGridView1, dataGridView2))
                    {
                        isPrepering(false);
                        regim_zapuska(1);
                        update_algo();
                        throw new Exception("Программа успешно выполнена!");
                    }
                }
                catch (Exception ex)
                {
                    isPrepering(false);
                    regim_zapuska(1);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Point cell = dataGridView2.CurrentCellAddress;
            int x = cell.X;
            int y = cell.Y;
            String[,] buffer = fromDGV(dataGridView2);
            if (true)
            {
                String[,] resultBuffer = new String[buffer.GetLength(0), buffer.GetLength(1) + 1];


                for (int j = 0; j < resultBuffer.GetLength(1); j++)
                {
                    //сделать нельзя добавлять первую штуку
                    if (j < x + 1)
                    {
                        for (int i = 0; i < buffer.GetLength(0); i++) resultBuffer[i, j] = buffer[i, j];
                    }
                    else if (j > x + 1) for (int i = 0; i < buffer.GetLength(0); i++) resultBuffer[i, j] = buffer[i, j - 1];
                    else for (int i = 0; i < buffer.GetLength(0); i++) resultBuffer[i, j] = "";
                }
                loadFromMatrix(resultBuffer);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            dataGridView1.Enabled = false;
            clearLenta();
            dataGridView1.Enabled = true;
        }

        private void очиститьЛентуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
            clearLenta();
            dataGridView1.Enabled = true;
        }
        public void clearLenta()
        {
            //по хорошему из алгоритма надо достать первый символ (нулевой)
            for (int j = 0; j < 200; j++)
            {
                this.dataGridView1[j, 0].Value = j + 1 + "";
                this.dataGridView1[j, 1].Value = "_";
                this.dataGridView1[j, 1].Style.BackColor = System.Drawing.Color.White;
            }
            update_algo();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (updated_algo())
            {
                Form2 frm2 = new Form2();
                frm2.Show();
                frm2.FormClosed += (closedSender, closedE) =>
                {
                    enterLeata(frm2.oper1, frm2.oper2);
                };
            }
        }

        private void ввестиДваОперандаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
            frm2.FormClosed += (closedSender, closedE) =>
            {
                enterLeata(frm2.oper1, frm2.oper2);
            };
        }
        public void enterLeata(int i1,int i2)
        {
            if (i1 != 0 || i2 != 0)
            {
                clearLenta();
                for (int i = 0; i < i1; i++) this.dataGridView1[i, 1].Value = this.dataGridView2[0, 1].Value.ToString();//"*"; //По хорошему должен брать первый символ из алфавита
                this.dataGridView1[i1, 1].Value = this.dataGridView2[0, 2].Value.ToString();
                for (int i = i1 + 1; i < i1 + i2 + 1; i++) this.dataGridView1[i, 1].Value = this.dataGridView2[0, 1].Value.ToString();
                update_algo();
            }
            else
            {
                clearLenta();
            }
        }

        private void показатьТрассуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3();
            frm3.Show();
            frm3.updateDGV(TrasaFilePath);
            //frm3.updateDGV(fromDGV(dataGridView1));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3();
            frm3.Show();
            frm3.updateDGV(TrasaFilePath);
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4();
            frm4.Show();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 frm5 = new Form5();
            frm5.Show();
        }

        private void сохранитьТрассуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.Cancel)
                return;
            else
            {
                MessageBox.Show("Файл трассы будет создан после исполнения программы");
                // получаем выбранный файл
                TrasaFilePath = saveFileDialog2.FileName;
                //renderMyAlgoFile(filename);
                //initTrasaFile();
            }
        }

        string TrasaFilePath = @"trasa.tmt";
        public void initTrasaFile2()
        {
            String[,] buffer = fromDGV(dataGridView1);
            String[] buffer_lenta = new String[200];
            for (int i = 0; i < buffer.GetLength(1); i++) buffer_lenta[i] = buffer[1, i];
            using (StreamWriter sw = new StreamWriter(TrasaFilePath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(toString(buffer_lenta));
            }
            //for (int i = 0; i < buffer.GetLength(1); i++) buffer_lenta[i] = buffer[1, i];
            //using (StreamWriter sw = new StreamWriter(TrasaFilePath, true, System.Text.Encoding.Default))
            //{
            //    sw.WriteLine(toString(buffer_lenta));
            //}
        }
        public void initTrasaFile()
        {
            String[,] buffer = fromDGV(dataGridView1);
            String[] buffer_lenta = new String[200];
            //for (int i = 0; i < buffer.GetLength(1); i++) buffer_lenta[i] = buffer[0, i];
            //using (StreamWriter sw = new StreamWriter(TrasaFilePath, false, System.Text.Encoding.Default))
            //{
            //    sw.WriteLine(toString(buffer_lenta));
            //}
            for (int i = 0; i < buffer.GetLength(1); i++) buffer_lenta[i] = buffer[1, i];
            using (StreamWriter sw = new StreamWriter(TrasaFilePath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(toString(buffer_lenta));
            }
        }
        public static String toString(String[] mas_str)
        {
            String buffer = "";
            for (int i = 0; i < mas_str.Length; i++) buffer += mas_str[i] +" ";
            return buffer;
        }
        public static String[] frString(String str)
        {
            Char delimiter = ' ';
            String[] substrings = str.Split(delimiter);
            return substrings;
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!dataGridView1.Enabled)
            {
                initTrasaFile();
                //try
                //{
                //    initTrasaFile();
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
            }
            else
            {
                update_algo();
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                regim_zapuska(1);
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                regim_zapuska(2);
            }
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                regim_zapuska(3);
            }
        }
        public void regim_zapuska(int state)
        {
            switch(state)
            {
                case 1:
                    button3.Enabled = true;
                    trackBar1.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    break;
                case 2:
                    button4.Enabled = true;
                    trackBar1.Enabled = true;
                    button3.Enabled = false;
                    button5.Enabled = false;
                    break;
                case 3:
                    button5.Enabled = true;
                    trackBar1.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    break;
                default:
                    goto case 1;
            }
        }
        public void renderMyAlgoFile(string filename)
        {
            String[,] buffer = fromDGV(dataGridView2);
            String[] buffer_lenta = new String[buffer.GetLength(1)];
            for (int i = 0; i < buffer.GetLength(1); i++) buffer_lenta[i] = buffer[0, i];
            using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(toString(buffer_lenta));
            }
            for (int j = 1; j < buffer.GetLength(0); j++)
            {
                for (int i = 0; i < buffer.GetLength(1); i++) buffer_lenta[i] = buffer[j, i];
                using (StreamWriter sw = new StreamWriter(filename, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(toString(buffer_lenta));
                }
            }
        }
        //string MyAlgoFilePath = @"my_algo.mt";
        private void сохранитьВФайлMTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            renderMyAlgoFile(filename);
        }
        public void loadFromMatrix(String[,] matrix)
        {
            //надо обнулить DGV
            dataGridView2.Columns.Clear();

            String[] buffer = new String[matrix.GetLength(1)];
            for (int i = 0; i < buffer.Length; i++) this.dataGridView2.Columns.Add(matrix[0,i], matrix[0, i]);
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                for (int i = 0; i < buffer.Length; i++) buffer[i] = matrix[j, i];
                this.dataGridView2.Rows.Add(buffer);
            }
            update_algo();
        }
        public void loadFrom(String path)
        {
            //надо обнулить DGV
            dataGridView2.Columns.Clear();
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line = sr.ReadLine();
                String[] buffer = Form1.frString(line);
                for (int i = 0; i < buffer.Length-1; i++) this.dataGridView2.Columns.Add(buffer[i], buffer[i]);
                this.dataGridView2.Rows.Add(buffer);
                while ((line = sr.ReadLine()) != null)
                {
                    buffer = Form1.frString(line);
                    this.dataGridView2.Rows.Add(buffer);
                }
            }
            update_algo();
        }
        string Std1AlgoFilePath = @"std1_algo.mt";
        private void загрузитьСтандартный1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView2.ReadOnly = true;
            button7.Enabled = false;
            button8.Enabled = false;
            loadFrom(Std1AlgoFilePath);
        }
        string Std2AlgoFilePath = @"std2_algo.mt";
        private void загрузитьСтандартный2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView2.ReadOnly = true;
            button7.Enabled = false;
            button8.Enabled = false;
            loadFrom(Std2AlgoFilePath);
        }
        string Std0AlgoFilePath = @"std0_algo.mt";
        private void создатьНовыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView2.ReadOnly = false;
            button7.Enabled = true;
            button8.Enabled = true;
            loadFrom(Std0AlgoFilePath);
        }
        long time_count = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            if (updated_algo())
            {
                isPrepering(true);
                regim_zapuska(2);
                if (timer1.Enabled)
                {
                    button4.Text = "Продолжить";
                    timer1.Stop();
                }
                else
                {
                    button4.Text = "Пауза";
                    timer1.Interval = trackBar1.Value * 500;
                    timer1_Tick(sender, e);
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            int max_time_count = 30000;
            if (time_count < max_time_count)
            {
                time_count += timer1.Interval;
                timer1.Start();
                try
                {
                    if (!algo.next(dataGridView1, dataGridView2))
                    {
                        timer1.Stop();
                        button4.Text = "Запуск";
                        isPrepering(false);
                        regim_zapuska(2);
                        update_algo();
                        time_count = 0;
                        throw new Exception("Программа выполнена!");
                    }
                }
                catch (Exception ex)
                {
                    time_count = 0;
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                timer1.Stop();
                button4.Text = "Запуск";
                isPrepering(false);
                regim_zapuska(2);
                update_algo();
                time_count = 0;
                MessageBox.Show("Превышено время выполнения программы. Ограничение: "+max_time_count);
            }
        }
        public void isPrepering(bool flag)
        {
            if(flag)
            {
                menuStrip1.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                trackBar1.Enabled = false;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                dataGridView1.Enabled = false;
                dataGridView2.Enabled = false;
            }
            else
            {
                menuStrip1.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = true;
                trackBar1.Enabled = true;
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                dataGridView1.Enabled = true;
                dataGridView2.Enabled = true;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Point cell = dataGridView2.CurrentCellAddress;
            int x = cell.X;
            int y = cell.Y;
            String[,] buffer = fromDGV(dataGridView2);
            if (buffer.GetLength(1)>2)
            {
                String[,] resultBuffer = new String[buffer.GetLength(0), buffer.GetLength(1) - 1];


                for (int j = 0; j < resultBuffer.GetLength(1); j++)
                {
                    //сделать нельзя добавлять первую штуку
                    if (j < x)
                    {
                        for (int i = 0; i < buffer.GetLength(0); i++) resultBuffer[i, j] = buffer[i, j];
                    }
                    else if (j >= x) for (int i = 0; i < buffer.GetLength(0); i++) resultBuffer[i, j] = buffer[i, j + 1];
                }
                loadFromMatrix(resultBuffer);
            }
        }
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            update_algo();
        }
        private void загрузитьИзФайлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView2.ReadOnly = false;
            button7.Enabled = true;
            button8.Enabled = true;
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            loadFrom(filename);
        }
    }
}
