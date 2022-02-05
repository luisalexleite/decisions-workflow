using ComponentFactory.Krypton.Toolkit;
using DecisionsWorkFlow.Database;
using ExcelLibrary.SpreadSheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Globalization;
using System.IO;
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

        private DatabaseDataContext db = new DatabaseDataContext();

        private string queryText = "";

        private bool defaultText = false;

        public Students(int _id, Project _project)
        {
            id = _id;
            project = _project;
            InitializeComponent();
        }

        private void Students_Load(object sender, EventArgs e)
        {
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
            LoadTable();
        }

        private void LoadTable()
        {
            dataGridView1.Rows.Clear();
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
            database.GetStudents(id).Where(s => s.student_name.Contains(queryText)).ToList().ForEach(s =>
            {
                j = 3;
                RegionInfo regionInfo = new RegionInfo(s.national_code);
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[0].Value = s.student_name;
                row.Cells[1].Value = s.schools.school_name;
                row.Cells[2].Value = s.student_id;
                row.Cells[3].Value = regionInfo.DisplayName;
                
                database.GetStudentAtributes(s.id).ToList().ForEach(sa =>
                {
                    j++;
                    row.Cells.Add(new DataGridViewTextBoxCell());
                    row.Cells[j].Value = sa.Value;
                });
                row.Cells.Add(GenererateButton(s.id));
                dataGridView1.Rows.Add(row);
            });
                label6.Visible = false;
            }
            else
            {
                label6.Visible = true;
                label6.Text = "Nenhum aluno registado";
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].GetType().Name == "DataGridViewButtonCell")
            {
                DeleteStudent(Int32.Parse(((DataGridViewButtonCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]).Tag.ToString()));
            }
        }

        private DataGridViewButtonCell GenererateButton(int id)
        {
            DataGridViewButtonCell dataGridViewButtonCell = new DataGridViewButtonCell();
            dataGridViewButtonCell.Value = "Remover";
            dataGridViewButtonCell.Tag = id;
            return dataGridViewButtonCell;

        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            project.OnOpen();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
                Workbook workbook = new Workbook(); 
                Worksheet worksheet = new Worksheet("Lista de Alunos"); 
                worksheet.Cells[0, 0] = new Cell("Nome do Aluno");
                worksheet.Cells[0, 1] = new Cell("Instituição de Ensino");
                worksheet.Cells[0, 2] = new Cell("Nº de Identificação de Aluno");
                worksheet.Cells[0, 3] = new Cell("Nacionalidade");
                int i = 3;
                database.GetProjectData(id).attributes.ToList().ForEach(a => {
                    i++;
                    worksheet.Cells[0, i] = new Cell(a.attr_name);
                }
            );

            workbook.Worksheets.Add(worksheet);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Sheet File| *.xls";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                workbook.Save(saveFileDialog1.FileName);
            }

        }

        private void DeleteStudent(int _id)
        {
            try { 
                database.deleteStudent(_id);
                MessageBox.Show("Aluno removido com sucesso.");
                LoadTable();
            } catch (Exception ex)
            {
                    MessageBox.Show(ex.Message);
            }
        }

        private bool checkValid(string validate)
        {
            int result = 0;
            try
            {
                result = Int32.Parse(validate);
            } catch
            {
                return false;
            }

            if (result > 0 && result < 6)
            {
                return true;
            }
            return false;
        }

        private bool getCells(string file)
        {
            Workbook book = Workbook.Load(file);

            Worksheet sheet = book.Worksheets[0];
            
            int i = 1;

            EntitySet<students> students = new EntitySet<students>();

            while (true)
            {
                int j = 4;
                bool check = true;

                students student = new students();
                var name = sheet.Cells[i, 0];
                var school = sheet.Cells[i, 1];
                var schoolId = sheet.Cells[i, 2];
                var nationality = sheet.Cells[i, 3];

                database.GetProjectData(id).attributes.ToList().ForEach(a =>
                    {
                        if (sheet.Cells[i, j].IsEmpty)
                        {
                            check = false;
                            return;
                        }
                        j++;
                    }
                );

                if (!name.IsEmpty
                && !school.IsEmpty
                && !schoolId.IsEmpty
                && !nationality.IsEmpty
                && check) 
                { 
                    try
                    {
                        new RegionInfo(nationality.StringValue);
                    }
                    catch
                    {
                        MessageBox.Show("Nacionalidade inválida.");
                        return false;
                    }
                    var dbcheck = db.students.Where(s => (s.project_id == id) && s.schools.school_abbr.Equals(school.StringValue) && s.student_id.Equals(schoolId.StringValue));
                    if (dbcheck.Count() > 0) {
                        EntitySet<students_attributes> studentsAttributes = new EntitySet<students_attributes>();
                        var studentCheck = dbcheck.FirstOrDefault();

                        studentCheck.student_name = name.StringValue;
                        studentCheck.national_code = nationality.StringValue;

                        int k = 4;
                        bool valid = true;
                        database.GetProjectData(id).attributes.ToList().ForEach(a => {
                            if (!checkValid(sheet.Cells[i, k].StringValue))
                            {
                                valid = false;
                                return;
                            }
                            studentCheck.students_attributes[k - 4].attr_value = Int32.Parse(sheet.Cells[i, k].StringValue);
                            k++;
                        });
                        

                        if (!valid)
                        {
                            MessageBox.Show("Classificação Inválida.");
                            return false;
                        }

                    } else
                    {
                        if (database.GetSchoolList().Where(s => s.school_abbr.Equals(school.StringValue)).Count() == 0) {
                            MessageBox.Show("Escola não existe no sistema.");
                            return false;
                        } else
                        {
                            student.student_name = name.StringValue;
                            student.school_id = database.GetSchoolList().Where(s => s.school_abbr.Equals(school.StringValue)).FirstOrDefault().id;
                            student.student_id = schoolId.StringValue;
                            student.national_code = nationality.StringValue;
                            student.project_id = id;

                            int k = 4;
                            bool valid = true;
                            database.GetProjectData(id).attributes.ToList().ForEach(a => {
                                if (!checkValid(sheet.Cells[i, k].StringValue)) { 
                                    valid = false;
                                    return; }
                                student.students_attributes.Add(new students_attributes { attr_id = a.id, attr_value = Int32.Parse(sheet.Cells[i, k].StringValue) });
                                k++;
                            });
                            if (!valid)
                            {
                                MessageBox.Show("Classificação Inválida.");
                                return false;
                            }
                            if (students.Where(s => s.school_id == student.school_id && s.student_id == student.student_id).Count() == 0) { 
                            students.Add(student);
                            }
                        }

                    }
                } else
                {
                    db.students.InsertAllOnSubmit(students);
                    db.SubmitChanges();
                    return true;
                }
                i++;
            }
        }

        private bool validateSheetFormat(string file)
        {
            try { 
                Workbook.Load(file);
            } catch(Exception ex) {
                    MessageBox.Show(ex.ToString());
            }

            Workbook book = Workbook.Load(file);
            Worksheet sheet = book.Worksheets[0];

            bool check = true;

            int i = 3;
            database.GetProjectData(id).attributes.ToList().ForEach(a =>
                {
                    i++;
                    if (a.attr_name.Equals(sheet.Cells[0, i])) {
                        check = false;
                        return;
                    }
                }
            );

            if (sheet.Cells[0,0].StringValue.Equals("Nome do Aluno") 
                && sheet.Cells[0,1].StringValue.Equals("Instituição de Ensino") 
                && sheet.Cells[0,2].StringValue.Equals("Nº de Identificação de Aluno") 
                && sheet.Cells[0,3].StringValue.Equals("Nacionalidade") 
                && check)
            {
                return true;
            } else
            {
                MessageBox.Show("Formatação do ficheiro errada.");
                return false;
            }
            
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog saveFileDialog2 = new OpenFileDialog();

            saveFileDialog2.Filter = "Sheet File| *.xls";
            saveFileDialog2.FilterIndex = 2;
            saveFileDialog2.RestoreDirectory = true;

            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                if(validateSheetFormat(saveFileDialog2.FileName) && getCells(saveFileDialog2.FileName))
                {
                    MessageBox.Show("Dados inseridos com sucesso.");
                    LoadTable();
                };
            }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            LoadTable();
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
                kryptonTextBox1.Text = "Escreva o nome de uma pessoa (Ex: João)";
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
            LoadTable();
        }

        private void Students_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
