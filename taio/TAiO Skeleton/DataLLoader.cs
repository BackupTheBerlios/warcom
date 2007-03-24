using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows.Forms;

namespace Taio
{
    class DataLLoader
    {
        private List<Solution> solutions;

        public List<Solution> Solutions
        {
          get { return solutions; }
          set { solutions = value; }
        }

        public void SaveSolution(List<Solution> solutions, List<Rectangle> rectangles)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pliki TAiO (*.tao)|*.tao";
            sfd.Title = "Wybierz plik do zapisu danych";
            if (sfd.ShowDialog() == DialogResult.OK)
            {

            }

        }

        public void OpenFile(ref List<Solution> solutions, ref  List<Rectangle> rectangles)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Pliki TAiO (*.tao)|*.tao";
            ofd.Title = "Wybierz plik zawieraj¹cy prostok¹ty";
            if (ofd.ShowDialog() == DialogResult.OK)
            {

            }
        }

        public List<Rectangle> DrawRectangles(int count, int maxSide)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            Random random = new Random();
            for (int i = 0; i < count; ++i)
                rectangles.Add(new Rectangle(random.Next() % maxSide, random.Next() % maxSide));
            return rectangles;

        }
    }
}
