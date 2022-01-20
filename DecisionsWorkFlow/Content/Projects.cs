using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace DecisionsWorkFlow
{
    public partial class Form1 : KryptonForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void kryptonGroup2_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            ToggleOcurringProjects(label1.Text == "Projetos a Decorrer ►");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ToggleOcurringProjects(label2.Text == "Projetos Terminados ▼");
        }

        private void ToggleOcurringProjects(bool open)
        {
            if (open == true)
            {
                label1.Text = "Projetos a Decorrer ▼";
                label2.Text = "Projetos Terminados ►";
                kryptonGroup2.Hide();
                kryptonGroup1.Show();
                label2.Top = 652;
            } else
            {
                label1.Text = "Projetos a Decorrer ►";
                label2.Text = "Projetos Terminados ▼";
                kryptonGroup2.Top = 264;
                kryptonGroup2.Show();
                kryptonGroup1.Hide();
                label2.Top = 222;
            }
        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
