using ComponentFactory.Krypton.Toolkit;
using DecisionsWorkFlow.Database;
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
    public partial class Functions : KryptonForm
    {
        private DatabaseContent database = new DatabaseContent();

        private Project projectForm;

        public int project;

        private string queryText = "";

        private bool defaultText = false;

        public Functions(int _project, Project _projectForm)
        {
            project = _project;
            projectForm = _projectForm;
            InitializeComponent();
        }

        private void Functionscs_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
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

            var activeCheck = database.GetFunctionList(project, queryText, comboBox1.SelectedIndex);

            activeCheck.ForEach(function =>
            {
                FunctionInfo userControl = new FunctionInfo(function.func_name, function.func_desc, function.id, this);
                flowLayoutPanel1.Controls.Add(userControl);

            });

            if (activeCheck.Count() == 0)
            {
                flowLayoutPanel1.Controls.Add(NoResults());
            }
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
                kryptonTextBox1.Text = "Escreva uma palavra ou expressão que descreva a função (Ex: Gestor)";
            };
        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {

            if (defaultText == false)
            {
                queryText = kryptonTextBox1.Text;
            }
            else
            {
                queryText = "";
            }

            defaultText = false;
            LoadPanels();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPanels();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            AddFunction function = new AddFunction(project);
            function.ShowDialog();
            LoadPanels();
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            projectForm.Show();
            this.Hide();
        }

        private void Functions_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
