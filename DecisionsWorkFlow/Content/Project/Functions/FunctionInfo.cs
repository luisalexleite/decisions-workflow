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
    public partial class FunctionInfo : UserControl
    {
        private string name;
        private string desc;
        private int id;
        private Functions functions;
        private DatabaseContent database = new DatabaseContent();

        /// <summary>
        /// User Control must be round
        /// </summary>

        private int radius = 12;
        [DefaultValue(12)]
        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                this.RecreateRegion();
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void RecreateRegion()
        {
            var bounds = ClientRectangle;
            //using (var path = GetRoundRectagle(bounds, this.Radius))
            //    this.Region = new Region(path);

            //Better round rectangle
            this.Region = Region.FromHrgn(CreateRoundRectRgn(bounds.Left, bounds.Top,
                bounds.Right, bounds.Bottom, Radius, radius));
            this.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.RecreateRegion();
        }

        public FunctionInfo(string _name, string _desc, int _id, Functions _functions)
        {
            name = _name;
            desc = _desc;
            id = _id;
            functions = _functions;
            InitializeComponent();
        }

        private void FunctionInfo_Load(object sender, EventArgs e)
        {
            this.Click += FunctionInfo_Click;
            label1.Click += FunctionInfo_Click;
            label2.Click += FunctionInfo_Click;
            label1.Text = name;
            toolTip1.SetToolTip(label1, name);
            label2.Text = desc;
            toolTip1.SetToolTip(label2, desc);
        }

        private void OpenProjectWindow()
        {
            if (database.GetProjectByFunction(id).weight_set)
            {
                Function function = new Function(id, functions);
                functions.Hide();
                function.Show();
            }
            else
            {
                Attributes attr = new Attributes(id);
                attr.ShowDialog();
                Function function = new Function(id, functions);
                functions.Hide();
                function.Show();
            }
        }

        private void FunctionInfo_Click(object sender, EventArgs e)
        {
            OpenProjectWindow();
        }
    }
}
