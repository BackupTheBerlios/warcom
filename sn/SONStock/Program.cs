using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SONStock
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Garch.GarchModel g = new Garch.GarchModel(1,1,0.1);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}