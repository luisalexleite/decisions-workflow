using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using DecisionsWorkFlow.Content.Projects;
using DecisionsWorkFlow.Database;

namespace DecisionsWorkFlow
{
    public partial class Projects : KryptonForm
    {
        private DatabaseContent database = new DatabaseContent();

        public int user;

        private string queryTextActive = "";

        private string queryTextTerminated = "";

        private bool defaultText = false;

        public Projects(int _user)
        {
            user = _user;
            InitializeComponent();
        }


        public void Form1_Load(object sender, EventArgs e)
        {
            string username = database.GetUserData(user)?.fname + " " + database.GetUserData(user)?.lname;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            this.Text = "Decisions WorkFlow - Projetos (" + username + ")";
            label3.Text = "| " +  username;
            LoadPanels();
        }

        private Label NoResults()
        {
            Label labelWarning = new Label();
            labelWarning.Text = "Não foi encontrado nenhum resultado.";
            labelWarning.ForeColor = SystemColors.ActiveCaptionText;
            labelWarning.AutoSize = true;

            return labelWarning;
        }

        public void LoadPanels()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();

            var activeCheck = database.GetProjectList(user, false, queryTextActive, comboBox1.SelectedIndex);

            activeCheck.ForEach(project =>
            {
                ProjectInfo userControl = new ProjectInfo(true, project.project_name, project.project_desc, project.id, this);
                flowLayoutPanel1.Controls.Add(userControl);

            });

            if (activeCheck.Count() == 0)
            {
                flowLayoutPanel1.Controls.Add(NoResults());
            }

            var terminatedCheck = database.GetProjectList(user, true, queryTextTerminated, comboBox2.SelectedIndex);

            terminatedCheck.ForEach(project =>
            {
                ProjectInfo userControl = new ProjectInfo(false, project.project_name, project.project_desc, project.id, this);
                flowLayoutPanel2.Controls.Add(userControl);

            });

            if (terminatedCheck.Count() == 0)
            {
                flowLayoutPanel2.Controls.Add(NoResults());
            }
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
                label2.Top = 640;
                kryptonGroup2.Hide();
                kryptonGroup1.Show();
                LoadPanels();
            } else
            {
                label1.Text = "Projetos a Decorrer ►";
                label2.Text = "Projetos Terminados ▼";
                label2.Top = 224;
                kryptonGroup2.Top = 264;
                kryptonGroup2.Show();
                kryptonGroup1.Hide();
                LoadPanels();
            }
        }

        private void Reload()
        {
            Projects pr = new Projects(user);
            pr.Show();
            this.Hide();
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void kryptonTextBox1_Enter(object sender, EventArgs e)
        {
            kryptonTextBox1.Text = "";
        }

        private void kryptonTextBox1_Leave(object sender, EventArgs e)
        {
            if (kryptonTextBox1.Text.Equals(""))
            {
                defaultText = true;
                kryptonTextBox1.Text = "Escreva uma palavra ou expressão que descreva o projeto (Ex: Eramus)";
            };
        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (defaultText == false)
            {
                queryTextActive = kryptonTextBox1.Text;
            }
            else
            {
                queryTextActive = "";
            }

            defaultText = false;
            LoadPanels();
        }

        private void kryptonTextBox2_Enter(object sender, EventArgs e)
        {
            kryptonTextBox2.Text = "";
        }

        private void kryptonTextBox2_Leave(object sender, EventArgs e)
        {
            if (kryptonTextBox2.Text.Equals(""))
            {
                defaultText = true;
                kryptonTextBox2.Text = "Escreva uma palavra ou expressão que descreva o projeto (Ex: Eramus)";
            };
        }

        private void kryptonTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (defaultText == false)
            {
                queryTextTerminated = kryptonTextBox2.Text;
            } else
            {
                queryTextTerminated = "";
            }
            defaultText = false;
            LoadPanels();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPanels();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPanels();
        }

        public void OnOpen()
        {
            Reload();
        }
    }
}
