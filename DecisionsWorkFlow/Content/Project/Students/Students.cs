using ComponentFactory.Krypton.Toolkit;
using DecisionsWorkFlow.Database;
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

namespace DecisionsWorkFlow.Content.Project.Students
{
    public partial class Students : KryptonForm
    {
        private int id;

        private Project project;

        private DatabaseContent database = new DatabaseContent();
        public Students(int _id, Project _project)
        {
            id = _id;
            project = _project;
            InitializeComponent();
        }

        private void Students_Load(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void LoadTable()
        {
            dataGridView1.Columns.Clear();
            if (database.CountStudents(id) > 0) { 
            dataGridView1.Columns.Add("name", "Nome do Aluno");
            dataGridView1.Columns.Add("school", "Instituição de Ensino");
            dataGridView1.Columns.Add("schoolId", "Nº de Identificação do Aluno");
            dataGridView1.Columns.Add("nationality", "Nacionalidade");
            int i = 3;
            database.GetProjectData(id).attributes.ToList().ForEach(a => {
                i++;
                dataGridView1.Columns.Add("attr" + i, a.attr_abbr);
                dataGridView1.Columns[i].ToolTipText = a.attr_name;
            }
            );
            dataGridView1.Columns.Add("buttonRemove", "");
            dataGridView1.Columns.Add("buttonEdit", "");

            int j;
            database.GetStudents(id).ToList().ForEach(s =>
            {
                j = 3;
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[0].Value = s.student_name;
                row.Cells[1].Value = s.schools.school_name;
                row.Cells[2].Value = s.student_id;
                row.Cells[3].Value = new RegionInfo(s.national_code).DisplayName;
                database.GetStudentAtributes(s.id).ToList().ForEach(sa =>
                {
                    j++;
                    row.Cells.Add(new DataGridViewTextBoxCell());
                    row.Cells[j].Value = sa.Value;
                });
                row.Cells.Add(new DataGridViewButtonCell());
                row.Cells[++j].Value = "Alterar Classificações";
                row.Cells.Add(new DataGridViewButtonCell());
                row.Cells[++j].Value = "Remover";

                dataGridView1.Rows.Add(row);
            });
            }
            else
            {
                label6.Visible = true;
                label6.Text = "Nenhum aluno registado";
            }
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            this.Close();
            project.OnOpen();
        }
    }
}
