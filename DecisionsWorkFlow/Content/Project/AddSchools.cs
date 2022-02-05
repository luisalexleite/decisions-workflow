using ComponentFactory.Krypton.Toolkit;
using DecisionsWorkFlow.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecisionsWorkFlow.Content.Project
{
    public partial class AddSchools : KryptonForm
    {
        private EntitySet<schools> schoolList = new EntitySet<schools>();

        private DatabaseContent database = new DatabaseContent();

        private DatabaseDataContext db = new DatabaseDataContext();
        public AddSchools()
        {
            InitializeComponent();
        }

        private void AddSchools_Load(object sender, EventArgs e)
        {
            database.GetSchoolList().ForEach(s =>listBox2.Items.Add(s.school_name + " (" + s.school_abbr + ")"));
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (kryptonTextBox2.Text != "" && kryptonTextBox3.Text != "")
            {
                if (schoolList.ToList().Find(sl => sl.school_name.Equals(kryptonTextBox2.Text) || sl.school_abbr.Equals(kryptonTextBox3.Text)) == null)
                {
                    schoolList.Add(new schools()
                    {
                        school_name = kryptonTextBox2.Text,
                        school_abbr = kryptonTextBox3.Text,
                    });
                    kryptonTextBox2.Text = "";
                    kryptonTextBox3.Text = "";
                    LoadAttributes();
                }
                else
                {
                    MessageBox.Show("Não deve incluir atributos com mesmo conjunto nome/abreviação.");
                }
            }
            else
            {
                MessageBox.Show("Deve preencher tanto o nome como a abreviação para adicionar um atributo.");
            }
        }

        private void LoadAttributes()
        {
            listBox1.Items.Clear();
            schoolList.ToList().ForEach(s => listBox1.Items.Add(s.school_name + " (" + s.school_abbr + ")"));
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            if (schoolList.Count > 0)
            {
                if (listBox1.SelectedIndex >= 0) { 
                    schoolList.RemoveAt(listBox1.SelectedIndex);
                    LoadAttributes();
                }
            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            bool check = true;
            schoolList.ToList().ForEach(sl =>
                { 
                    if (db.schools.Where(s => s.school_abbr == sl.school_abbr).Count() > 0)
                    {
                        check = false;
                        return;
                    }
                
                }
            );
            if (check == false)
            {
                MessageBox.Show("Já existem uma ou mais escolas na base de dados.");
            }
            else if (schoolList.Count == 0)
            {
                MessageBox.Show("Deve adicionar pelo menos uma escola.");
            }
            else
            {
                db.schools.InsertAllOnSubmit(schoolList);
                db.SubmitChanges();
                this.Close();
            }
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
