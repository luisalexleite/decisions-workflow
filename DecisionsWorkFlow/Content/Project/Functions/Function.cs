using ComponentFactory.Krypton.Toolkit;
using DecisionsWorkFlow.Database;
using LiveCharts.WinForms;
using ProjectoSAD.ManageProjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DecisionsWorkFlow.Content.Project.Functions
{
    public partial class Function : KryptonForm
    {
        DatabaseContent database = new DatabaseContent();
        DatabaseDataContext db = new DatabaseDataContext();
        ManageProjects manageProjects;
        Functions functions;
        private string queryText = "";
        private bool defaultText = false;
        private int id;

        public Function(int _id, Functions _functions)
        {
            id = _id;
            functions = _functions;
            InitializeComponent();
        }

        private void GetColorLabel(double num, int index)
        {
            double x = (double)1/manageProjects.getStudentData().Count();
            if (num > x + 0.05)
            {
                dataGridView1.Rows[index].Cells[5].Style.ForeColor = System.Drawing.Color.Green;
            }
            else if (num < x + 0.05) { dataGridView1.Rows[index].Cells[5].Style.ForeColor = System.Drawing.Color.Red; }
            else { dataGridView1.Rows[index].Cells[5].Style.ForeColor = System.Drawing.Color.YellowGreen; }
        }

        private void DwfPoints(double num, int index)
        {
            if (num > 55)
            {
                dataGridView1.Rows[index].Cells[4].Style.ForeColor = System.Drawing.Color.Green;
            } else if (num < 45) {
                dataGridView1.Rows[index].Cells[4].Style.ForeColor = System.Drawing.Color.Red;
            } else
            {
                dataGridView1.Rows[index].Cells[4].Style.ForeColor = System.Drawing.Color.YellowGreen;
            }
        }

        private void Function_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox2.Items.Add("Todas");
            comboBox3.Items.Add("Todas");

            database.GetSchoolList().ForEach(school =>
            comboBox2.Items.Add(school.school_name));

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            List<string> countryList = new List<string>();
            foreach (CultureInfo culture in cultures)
            {
                try { 
                    RegionInfo region = new RegionInfo(culture.LCID);
                    countryList.Add(region.DisplayName);
                } catch
                { }
            }

            countryList.Distinct().OrderBy(cl => cl).ToList().ForEach(country =>
            comboBox3.Items.Add(country));

            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            LoadPanels(true);
        }

        private void LoadPanels(bool check)
        {
            
            IEnumerable<Aluno> queryTextStudents;
            manageProjects = new ManageProjects((int)database.GetProjectByFunction(id).project_id, id);
            dataGridView1.Rows.Clear();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    queryTextStudents = manageProjects.getStudentData().Where(mp => mp.Name.ToLower().Contains(queryText.ToLower())).OrderByDescending(mp => mp.SaatyIndex);
                    break;
                case 1:
                    queryTextStudents = manageProjects.getStudentData().Where(mp => mp.Name.ToLower().Contains(queryText.ToLower())).OrderByDescending(mp => mp.DwfPoints);
                    break;
                case 2:
                    queryTextStudents = manageProjects.getStudentData().Where(mp => mp.Name.ToLower().Contains(queryText.ToLower())).OrderBy(mp => mp.Name);
                    break;
                case 3:
                    queryTextStudents = manageProjects.getStudentData().Where(mp => mp.Name.ToLower().Contains(queryText.ToLower())).OrderByDescending(mp => mp.Name);
                    break;
                default:
                    queryTextStudents = manageProjects.getStudentData();
                    break;
            }

            if (comboBox2.SelectedIndex != 0 && comboBox2.SelectedIndex != -1)
            {
               queryTextStudents = queryTextStudents.Where(mp => mp.School.Equals(comboBox2.SelectedItem.ToString()));
            }

            if (comboBox3.SelectedIndex != 0 && comboBox3.SelectedIndex != -1)
            {
                queryTextStudents = queryTextStudents.Where(mp => mp.Nationality.Equals(comboBox3.SelectedItem.ToString()));
            }

            int i = 0;
            queryTextStudents.ToList().ForEach(x => {
             dataGridView1.Rows.Add(x.Name, x.School, x.SchoolNumber, x.Nationality, x.DwfPoints, x.SaatyIndex);
                if (check) { 
                DwfPoints(x.DwfPoints, i);
                GetColorLabel(x.SaatyIndex, i);
                i++;
                }
            });
            chart2.Controls.Clear();
            chart2.Series.Clear();
            chart2.Series.Add("Series1");
            chart2.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            chart2.Series["Series1"].IsValueShownAsLabel = true;
            db.functions_attributes.Where(fa => fa.func_id == id).ToList().ForEach(s =>
                chart2.Series["Series1"].Points.AddXY(s.attributes.attr_name, Math.Round((double)s.attr_weight * 100, 2))
             ) ;

            label8.Text = "Ponto Forte: " + database.getPoints((int)db.functions.Where(f => f.id == id).FirstOrDefault().project_id)[0].ToUpperInvariant();
            label9.Text = "Ponto Fraco: " + database.getPoints((int)db.functions.Where(f => f.id == id).FirstOrDefault().project_id)[1].ToUpperInvariant();
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
            LoadPanels(true);
        }

        private void kryptonTextBox1_Leave(object sender, EventArgs e)
        {
            if (kryptonTextBox1.Text.Equals(""))
            {
                defaultText = true;
                kryptonTextBox1.Text = "Escreva o nome de uma pessoa (Ex: João)";
            };
        }

        private void kryptonTextBox1_Enter(object sender, EventArgs e)
        {
            kryptonTextBox1.Text = "";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPanels(true);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPanels(true);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPanels(true);
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            Attributes attr = new Attributes(id);
            attr.ShowDialog();
            LoadPanels(true);
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            functions.Show();
            this.Hide();
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Function_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

