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
using DecisionsWorkFlow.Database;
using System.Globalization;

namespace DecisionsWorkFlow.Content.Project
{
    public partial class Project : KryptonForm
    {
        private DecisionsWorkFlow.Projects projects;

        private DatabaseContent database = new DatabaseContent();

        private int id;
        public Project(DecisionsWorkFlow.Projects _projects, int _id)
        {
            projects = _projects;
            id = _id;
            InitializeComponent();
        }

        private void Project_Load(object sender, EventArgs e)
        {
            LoadContent();
        }

        private void LoadContent()
        {
            this.Text = "Decisions WorkFlow - Project ( " + database.GetProjectData(id).project_name + " )";
            label2.Text = database.GetProjectData(id).project_name;
            label4.Text = database.GetUserData(database.GetProjectData(id).project_admin).fname + " " + database.GetUserData(database.GetProjectData(id).project_admin).lname;
            label6.Text = database.GetProjectData(id).created_at.ToShortDateString();
            label8.Text = database.GetProjectData(id).project_desc;
            label10.Text = database.CountStudents(id).ToString();
            toolTip1.SetToolTip(label8, label8.Text);
            database.StudentsBySchool(id).ToList().ForEach(s =>
                chart1.Series["Series1"].Points.AddXY(database.GetSchool(s.Key).school_abbr + " - " + s.Count().ToString(), s.Count())
            );
            database.StudentsByNationality(id).ToList().ForEach(s =>
            chart2.Series["Series1"].Points.AddXY(new RegionInfo(s.Key).DisplayName + " - " + s.Count().ToString(), s.Count())
            );
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            DecisionsWorkFlow.Projects pr = new DecisionsWorkFlow.Projects(projects.user);
            pr.Show();
            this.Close();
        }
    }
}
