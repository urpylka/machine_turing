using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace machine_turing
{
    public partial class Form2 : Form
    {
        public int oper1 = 0;
        public int oper2 = 0;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            oper1 = (int)numericUpDown1.Value;
            oper2 = (int)numericUpDown2.Value;
            this.Close();
        }
    }
}
