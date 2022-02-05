using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DecisionsWorkFlow.Database;
using ProjectoSAD.ManageProjects;

namespace DecisionsWorkFlow.Content.Project.Functions
{
    public partial class Attributes : KryptonForm
    {
        private int function;
        private int index;
        private DatabaseContent database = new DatabaseContent();
        private List<DatabaseContent.CompareAttributes> attributeList;
        public Attributes(int _function)
        {
            function = _function;
            attributeList = database.GetCompareAttributes(function);
            InitializeComponent();
        }

        private void Attributes_Load(object sender, EventArgs e)
        {
            label7.Text = attributeList.ElementAt(0).attr1;
            label8.Text = attributeList.ElementAt(0).attr2;
            trackBar1.Value = attributeList.ElementAt(0).val;
            label10.Text = "1 de " + attributeList.Count;
            iconButton3.Enabled = false;
            if (attributeList.Count == 1)
            {
                iconButton2.Enabled = false;
                iconButton1.Text = "Concluir";
            }
            ChangeDescription();
        }

        private void nextIndex()
        {
            if (attributeList.Count <= index + 2)
            {
                iconButton1.Text = "Concluir";
                iconButton2.Enabled = false;
            }
            attributeList.ElementAt(index).val = trackBar1.Value;
            index++;
            label7.Text = attributeList.ElementAt(index).attr1;
            label8.Text = attributeList.ElementAt(index).attr2;
            trackBar1.Value = attributeList.ElementAt(index).val;
            label10.Text = index + 1 + " de " + attributeList.Count;
            iconButton3.Enabled = true;
            ChangeDescription();
        }

        private void prevIndex()
        {
            if (index - 1 <= 0)
            {
                iconButton3.Enabled = false;

            }

            attributeList.ElementAt(index).val = trackBar1.Value;
            index--;
            label7.Text = attributeList.ElementAt(index).attr1;
            label8.Text = attributeList.ElementAt(index).attr2;
            trackBar1.Value = attributeList.ElementAt(index).val;
            label10.Text = index + 1 + " de " + attributeList.Count;
            iconButton1.Text = "Próximo";
            iconButton2.Enabled = true;
            ChangeDescription();
        }

        private string GetDescriptionText()
        {
            switch (trackBar1.Value)
            {
                case 1:
                    return " O atributo " + attributeList.ElementAt(index).attr1 + " é extremamente mais importante que o atributo " + attributeList.ElementAt(index).attr2 + ".";
                case 2:
                    return " O atributo " + attributeList.ElementAt(index).attr1 + " tem muita mais importância que o atributo " + attributeList.ElementAt(index).attr2 + ".";
                case 3:
                    return " O atributo " + attributeList.ElementAt(index).attr1 + " tem mais importância que o atributo " + attributeList.ElementAt(index).attr2 + ".";
                case 4:
                    return " O atributo " + attributeList.ElementAt(index).attr1 + " tem pouca mais importância que o atributo " + attributeList.ElementAt(index).attr2 + ".";
                case 5:
                    return " Os atributos " + attributeList.ElementAt(index).attr1 + " e " + attributeList.ElementAt(index).attr2 + " têm a mesma importância.";
                case 6:
                    return " O atributo " + attributeList.ElementAt(index).attr2 + " tem pouca mais importância que o atributo " + attributeList.ElementAt(index).attr1 + ".";
                case 7:
                    return " O atributo " + attributeList.ElementAt(index).attr2 + " tem mais importância que o atributo " + attributeList.ElementAt(index).attr1 + ".";
                case 8:
                    return " O atributo " + attributeList.ElementAt(index).attr2 + " tem muita mais importância que o atributo " + attributeList.ElementAt(index).attr1 + ".";
                case 9:
                    return " O atributo " + attributeList.ElementAt(index).attr2 + "  é extremamente mais importante que o atributo " + attributeList.ElementAt(index).attr1 + ".";
                default: return null;
            }
        }

        private string GetDescriptionText(int _index)
        {
            switch (attributeList.ElementAt(_index).val)
            {
                case 1:
                    return " O atributo " + attributeList.ElementAt(_index).attr1 + " é extremamente mais importante que o atributo " + attributeList.ElementAt(_index).attr2 + ".";
                case 2:
                    return " O atributo " + attributeList.ElementAt(_index).attr1 + " tem muita mais importância que o atributo " + attributeList.ElementAt(_index).attr2 + ".";
                case 3:
                    return " O atributo " + attributeList.ElementAt(_index).attr1 + " tem mais importância que o atributo " + attributeList.ElementAt(_index).attr2 + ".";
                case 4:
                    return " O atributo " + attributeList.ElementAt(_index).attr1 + " tem pouca mais importância que o atributo " + attributeList.ElementAt(_index).attr2 + ".";
                case 5:
                    return " Os atributos " + attributeList.ElementAt(_index).attr1 + " e " + attributeList.ElementAt(_index).attr2 + " têm a mesma importância.";
                case 6:
                    return " O atributo " + attributeList.ElementAt(_index).attr2 + " tem pouca mais importância que o atributo " + attributeList.ElementAt(_index).attr1 + ".";
                case 7:
                    return " O atributo " + attributeList.ElementAt(_index).attr2 + " tem mais importância que o atributo " + attributeList.ElementAt(_index).attr1 + ".";
                case 8:
                    return " O atributo " + attributeList.ElementAt(_index).attr2 + " tem muita mais importância que o atributo " + attributeList.ElementAt(_index).attr1 + ".";
                case 9:
                    return " O atributo " + attributeList.ElementAt(_index).attr2 + "  é extremamente mais importante que o atributo " + attributeList.ElementAt(_index).attr1 + ".";
                default: return null;
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (index < attributeList.Count - 1)
            {
                nextIndex();
            }
            else
            {
                attributeList.ElementAt(index).val = trackBar1.Value;

                string history = "";

                for (int i = 0; i < attributeList.Count; i++)
                {
                    history += "\n" + GetDescriptionText(i);
                }

                DialogResult dialogResult = MessageBox.Show(history, "Deseja manter estas decisões?", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    ManageProjects manageProjects  = new ManageProjects((int)database.GetProjectByFunction(function).project_id, function);

                    int[] arr = attributeList.Select(al => al.val).ToArray();
                    float[] normalizedVal = new float[arr.Length];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        normalizedVal[i] = manageProjects.normalizeValueWeights(arr[i]);
                    }

                    if (database.GetProjectByFunction(function).weight_set) {
                        database.UpdateFunctionWeights(function, manageProjects.weightsMatrix(normalizedVal));
                    }
                    else { 
                        database.SetFunctionWeights(function, manageProjects.weightsMatrix(normalizedVal));
                    }
                    Close();
                }

            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            prevIndex();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            nextIndex();
        }

        private void ChangeDescription()
        {
            label1.Text = GetDescriptionText();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ChangeDescription();
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
