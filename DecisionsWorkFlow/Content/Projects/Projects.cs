using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using DecisionsWorkFlow.Content.Projects;
using DecisionsWorkFlow.Tests;

namespace DecisionsWorkFlow
{
    public partial class Projects : KryptonForm
    {
        private List<Project> projects = new List<Project>();

        private string queryTextActive = "";

        private string queryTextTerminated = "";

        private bool defaultText = false;
        public Projects()
        {
            projects.Add(new Project("Construir um Avião", "Os intervenientes neste projeto devem construir um avião", new DateTime(), 1, true));
            projects.Add(new Project("Construir um Carro", "Os intervenientes neste projeto devem construir um carro", new DateTime(), 1, true));
            projects.Add(new Project("Construir um Camião", "Os intervenientes neste projeto devem construir um camião", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Ponte", "Os intervenientes neste projeto devem construir uma Ponte", new DateTime(), 1, true));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            projects.Add(new Project("Construir uma Avionete", "Os intervenientes neste projeto devem construir um avionete", new DateTime(), 2, true));
            projects.Add(new Project("Fazer uma Viagem", "Os intervenientes neste projeto fazer uma viagem", new DateTime(), 1, false));
            InitializeComponent();
        }


        public void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            LoadPanels();
        }

        public void LoadPanels()
        {
            Label labelWarning = new Label();
            labelWarning.Text = "Não foi encontrado nenhum resultado.";
            labelWarning.ForeColor = SystemColors.ActiveCaptionText;
            labelWarning.AutoSize = true;

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();

            var activeCheck = projects.Where(p => p.state == true).Where(p => p.name.ToLower().Contains(queryTextActive.ToLower())).ToList();
            activeCheck.ForEach(project =>
            {
                ProjectInfo userControl = new ProjectInfo(true, project.name, project.description);
                flowLayoutPanel1.Controls.Add(userControl);

            });

            if (activeCheck.Count() == 0)
            {
                flowLayoutPanel1.Controls.Add(labelWarning);
            }

            var terminatedCheck = projects.Where(p => p.state == false).Where(p => p.name.ToLower().Contains(queryTextTerminated.ToLower())).ToList();

            terminatedCheck.ForEach(project =>
            {
                ProjectInfo userControl = new ProjectInfo(false, project.name, project.description);
                flowLayoutPanel2.Controls.Add(userControl);

            });

            if (terminatedCheck.Count() == 0)
            {

                flowLayoutPanel2.Controls.Add(labelWarning);
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

        private void iconButton5_Click(object sender, EventArgs e)
        {
            Projects fr = new Projects();
            fr.Show();
            this.Hide();
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
