using ComponentFactory.Krypton.Toolkit;
using DecisionsWorkFlow.Database;
using ProjectoSAD.ManageProjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecisionsWorkFlow.Content.Project.Functions
{
    public partial class Function : KryptonForm
    {
        DatabaseContent database = new DatabaseContent();
        ManageProjects manageProjects = new ManageProjects(1,1);
        public Function()
        {
            InitializeComponent();
        }

        private void Function_Load(object sender, EventArgs e)
        {
            manageProjects.getStudentData().ForEach(x =>
                dataGridView1.Rows.Add(x.Name, x.School, x.SchoolNumber, x.Nationality, x.DwfPoints, x.SaatyIndex)
            );
        }
    }
}
