using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecisionsWorkFlow.Start
{
    public partial class Loading : KryptonForm
    {
        int maximum = 50;
        int total = 0;

        public Loading()
        {
            InitializeComponent();
        }

        private void Loading_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            

            if (total == maximum)
            {
                var welcome = new Login();
                this.Hide();
                welcome.Show(); 
                timer1.Stop();
            }
            else if (total == (int)maximum/ 2)
            {
                label1.Text = "A mudar o aspeto da aplicação...";
                total += 1;
            }
            else if (total == (int)maximum - 10)
            {
                label1.Text = "A preparar os retoques finais...";
                total += 1;
            }
            else { total += 1; }
        }

        private void Loading_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
