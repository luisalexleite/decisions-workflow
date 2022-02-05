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
    public partial class AddFunction : KryptonForm
    {
        DatabaseDataContext db = new DatabaseDataContext();

        int id;

        public AddFunction(int _id)
        {
            id = _id;
            InitializeComponent();
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            if (kryptonTextBox1.Text == "")
            {
                MessageBox.Show("Deve atribuir um nome à função.");
            } else if (db.functions.Where(s => s.project_id == id && s.func_name.ToLower() == kryptonTextBox1.Text.ToLower()).Count() > 0)
            {
                MessageBox.Show("Já existe uma fun~ção com esse nome");
            }
            else
            {
                functions function = new functions()
                {
                    func_name = kryptonTextBox1.Text,
                    func_desc = kryptonRichTextBox1.Text,
                    project_id = id,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                db.functions.InsertOnSubmit(function);
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
