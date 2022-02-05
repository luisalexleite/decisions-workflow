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
using DecisionsWorkFlow.Content.Project.Functions;
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

        private void ClearControls()
        {
            chart1.Titles.Clear();
            chart2.Titles.Clear();
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart1.Series.Add("Series1");
            chart2.Series.Add("Series1");
            chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            chart1.Series["Series1"].IsValueShownAsLabel = true;
            chart2.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            chart2.Series["Series1"].IsValueShownAsLabel = true;
        }
        private void LoadContent()
        {
            ClearControls();
            if (database.GetProjectData(id).project_admin != projects.user)
            {
                iconButton1.Enabled = false;
                iconButton6.Enabled = false;
            }
            else
            {
                iconButton6.Enabled=false;
            }

            if (database.GetProjectData(id).terminated == true )
            {
                iconButton1.Text = "Restaurar Projeto";
                iconButton2.Enabled = false;
                iconButton3.Enabled = false;
                iconButton5.Enabled = false;
                iconButton6.Enabled = false;
            } else
            {
                iconButton1.Text = "Terminar Projeto";
                iconButton2.Enabled = true;
                iconButton3.Enabled = true;
                iconButton5.Enabled = true;
            }

            if (database.CountStudents(id) < 2)
            {
                iconButton5.Enabled = false;
            }

            this.Text = "Decisions WorkFlow - Projeto ( " + database.GetProjectData(id).project_name + " )";
            label2.Text = database.GetProjectData(id).project_name;
            label4.Text = database.GetUserData(database.GetProjectData(id).project_admin).fname + " " + database.GetUserData(database.GetProjectData(id).project_admin).lname;
            label6.Text = database.GetProjectData(id).created_at.ToShortDateString();
            label8.Text = database.GetProjectData(id).project_desc;
            label10.Text = database.CountStudents(id).ToString();
            toolTip1.SetToolTip(label8, label8.Text);
            if (database.CountStudents(id) > 0) { 
                database.StudentsBySchool(id).ToList().ForEach(s =>
                    chart1.Series["Series1"].Points.AddXY(database.GetSchool(s.Key).school_abbr + " - " + s.Count().ToString(), s.Count())
                );
                database.StudentsByNationality(id).ToList().ForEach(s =>
                chart2.Series["Series1"].Points.AddXY(new RegionInfo(s.Key).DisplayName + " - " + s.Count().ToString(), s.Count())
                );
            }
            else
            {
                chart1.Titles.Add("Nenhum aluno registado.");
                chart2.Titles.Add("Nenhum aluno registado.");
            }
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            DecisionsWorkFlow.Projects pr = new DecisionsWorkFlow.Projects(projects.user);
            pr.Show();
            this.Hide();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            Students.Students st = new Students.Students(id, this);
            st.Show();
            this.Hide();
        }

        public void OnOpen()
        {
            LoadContent();
            this.Show();
        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            LoadContent();
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Functions.Functions func = new Functions.Functions(id,this);
            func.Show();
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            AddSchools addSchools = new AddSchools();
            addSchools.ShowDialog();
            LoadContent();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            database.TerminateProject(id);
            LoadContent();
        }

        private void Project_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
