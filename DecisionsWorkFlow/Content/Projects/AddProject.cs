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
using ComponentFactory.Krypton.Toolkit;
using DecisionsWorkFlow.Database;

namespace DecisionsWorkFlow.Content.Projects
{

    public partial class AddProject : KryptonForm
    {
        private int user;

        private EntitySet<attributes> attributeList = new EntitySet<attributes>();

        private DatabaseContent database = new DatabaseContent();

        private DatabaseDataContext db = new DatabaseDataContext();

        public AddProject(int _user)
        {
            user = _user;
            InitializeComponent();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (kryptonTextBox2.Text != "" && kryptonTextBox3.Text != "")
            {
                if (attributeList.ToList().Find(al => al.attr_name.Equals(kryptonTextBox2.Text.ToUpper()) || al.attr_abbr.Equals(kryptonTextBox3.Text.ToUpper())) == null)
                {
                    attributeList.Add(new attributes
                    {
                        attr_name = kryptonTextBox2.Text.ToUpper(),
                        attr_abbr = kryptonTextBox3.Text.ToUpper(),
                    });
                    kryptonTextBox2.Text = "";
                    kryptonTextBox3.Text = "";
                    LoadAttributes();
                } else
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
            attributeList.ToList().ForEach(a => listBox1.Items.Add(a.attr_name + " (" + a.attr_abbr + ")"));
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            if (attributeList.Count > 0) {
                if (listBox1.SelectedIndex >= 0)
                {
                    attributeList.RemoveAt(listBox1.SelectedIndex);
                    LoadAttributes();
                }
            }
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            if (database.CheckExistentProject(kryptonTextBox1.Text))
            {
                MessageBox.Show("Já existe um projeto com esse nome.");
            }
            else
            {
                if (kryptonTextBox1.Text == "")
                {
                    MessageBox.Show("Deve atribuir um nome ao projeto.");
                }
                else if (attributeList.Count < 2)
                {
                    MessageBox.Show("O número de atributos deve ser superior ou igual a dois (por motivos de comparação dos mesmos)");
                }
                else
                {
                    projects project = new projects()
                    {
                        project_name = kryptonTextBox1.Text,
                        project_desc = kryptonRichTextBox1.Text,
                        attributes = attributeList,
                        project_admin = user,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now,
                    };

                    db.projects.InsertOnSubmit(project);
                    db.SubmitChanges();
                    this.Close();

                }
            }
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddProject_Load(object sender, EventArgs e)
        {

        }
    }
}
