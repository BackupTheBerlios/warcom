using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Taio
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //CompareWithBest();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        

        #region Tests
        public static void CompareWithBest()
        {
            string path = @"C:\Documents and Settings\Marcin\Pulpit\tests\3x5";
            string[] files = Directory.GetFiles(path);
            List<Rectangle> rects = new List<Rectangle>();
            List<Solution> sols = new List<Solution>();
            Algorithms.Algorithm1Mod alg = new Taio.Algorithms.Algorithm1Mod();
            TextWriter tw = new StreamWriter(path + "\\result.csv", true);
            for (int i = 0; i < files.Length; i++)
            {
                DataLoader dl = new DataLoader();
                dl.LoadSolutions(files[i], ref sols, ref rects, false);
                int bestArea = sols[0].Rectangle.Area;
                Rectangle rect = alg.ComputeMaximumRectangle(rects);
                int apsArea = 0;
                if (rect != null)
                    apsArea = rect.Area;
                tw.WriteLine(bestArea + ";" + apsArea);

            }
            tw.Close();
        }        
        #endregion
    }
}
