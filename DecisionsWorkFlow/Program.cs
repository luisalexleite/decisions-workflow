using DecisionsWorkFlow.Content.Project;
using DecisionsWorkFlow.Content.Project.Functions;
using DecisionsWorkFlow.Content.Project.Students;
using DecisionsWorkFlow.Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecisionsWorkFlow
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Loading());
        }
    }
}
