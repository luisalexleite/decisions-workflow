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
using DecisionsWorkFlow.Content.Project;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace DecisionsWorkFlow.Content.Projects
{
    public partial class ProjectInfo : UserControl
    {
        private bool type;
        private string name;
        private string desc;
        private int id;
        private DecisionsWorkFlow.Projects projects;

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

        public ProjectInfo(bool _type, string _name, string _desc, int _id, DecisionsWorkFlow.Projects _projects)
        {
            type= _type;
            name= _name;
            desc = _desc;
            id = _id;
            projects = _projects;
            InitializeComponent();
            this.BorderStyle = BorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            this.Click += UserControl1_Click;
            label1.Click += UserControl1_Click;
            label2.Click += UserControl1_Click;
            label1.Text = name;
            toolTip1.SetToolTip(label1, name);
            label2.Text = desc;
            toolTip1.SetToolTip(label2, desc);

            if (type)
            {
                this.BackColor = SystemColors.ActiveCaptionText;
            } else
            {
                this.BackColor = Color.DarkGray;
            }
        }

        private void OpenProjectWindow()
        {
            projects.Hide();
            Project.Project project = new Project.Project(projects, id);
            project.Size = projects.Size;
            project.Show();
        }

        private void UserControl1_Click(object sender, EventArgs e)
        {
            OpenProjectWindow();
        }
    }
}
