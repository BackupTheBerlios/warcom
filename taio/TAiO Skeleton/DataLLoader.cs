using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

namespace Taio
{
    class DataLLoader
    {
        public void SaveSolution(List<Solution> solutions, List<Rectangle> rectangles)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pliki TAiO (*.tao)|*.tao";
            sfd.Title = "Wybierz plik do zapisu danych";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(sfd.FileName))
                {
                    List<Solution> s = new List<Solution>();
                    List<Rectangle> r = new List<Rectangle>();
                    this.LoadSolutions(sfd.FileName, ref s, ref  r);
                    if (!this.CheckData(r, rectangles))
                    {
                        if (MessageBox.Show("Dane wejœciowe z programu oraz dane z pliku nie s¹ identyczne" +
                                "\nCzy podmieniæ plik?", "Informacja", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Delete(sfd.FileName);
                        }
                        else
                        {
                            MessageBox.Show("W takim razie nie robiê nic:)");
                            return;
                        }
                    }
                }
                this.AppendSolutions(sfd.FileName, solutions, rectangles);
            }

        }

        public void OpenFile(ref List<Solution> solutions, ref  List<Rectangle> rectangles)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Pliki TAiO (*.tao)|*.tao";
            ofd.Title = "Wybierz plik zawieraj¹cy prostok¹ty";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.LoadSolutions(ofd.FileName, ref solutions, ref  rectangles);
            }
        }

        private void LoadSolutions(string fileName, ref List<Solution> solutions, ref  List<Rectangle> rectangles)
        {
            using (TextReader rd = new StreamReader(fileName))
            {
                String str = null;
                string wzorzec = "##\r\n(?<info>(.|\r|\n)*?)##\r\n(?<input>([0-9]*,[0-9]*\r\n)*)" +
                    "##(\r\n)?(#(?<result>(.){0,4}\r\n([0-9]*,[0-9]*,[0-9]*,[0-9]*(\r\n)?)*))*";
                Regex wyr = new Regex(wzorzec, RegexOptions.IgnoreCase);
                if ((str = rd.ReadToEnd()) != null)
                {
                    try
                    {
                        str = str.Trim();
                        Match elem = wyr.Match(str);
                        if (elem.Success)
                        {
                            string info = elem.Groups["info"].Value;
                            string result = elem.Groups["result"].Value;
                            string input = elem.Groups["input"].Value;
                            rectangles = this.ReadInput(input);
                            solutions = this.ReadResult(result);
                            if (!this.CheckCorrect(solutions, rectangles))
                                solutions.Clear();
                        }
                        else
                        {
                            MessageBox.Show("B³¹d w danych w pliku - z³y format");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("B³¹d: " + ex.Message + "\n\n" + ex.StackTrace);
                    }
                }
            }
        }

        private List<Rectangle> ReadInput(string input)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            using (TextReader rd = new StringReader(input))
            {
                int counter = 0;
                try
                {
                    string str;
                    while ((str = rd.ReadLine()) != null)
                    {
                        str = str.Trim();
                        int noOfComa = str.IndexOf(',');
                        int sideA = Int32.Parse(str.Substring(0, noOfComa));
                        int sideB = Int32.Parse(str.Substring(noOfComa+1, str.Length - noOfComa-1));
                        counter++;
                        rectangles.Add(new Rectangle(sideA, sideB));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("B³¹d przy wczytywaniu {0} prostok¹ta", counter.ToString());
                    throw ex;
                }
            }
            return rectangles;
        }

        private List<Solution> ReadResult(string result)
        {
            List<Solution> solutions = new List<Solution>();
            //TODO wczytaj rozwi¹zania
            return solutions;
        }

        private void AppendSolutions(string fileName, List<Solution> solutions, List<Rectangle> rectangles)
        {
            //TODO zapisz do pliku
        }

        private bool CheckData(List<Rectangle> r1, List<Rectangle> r2)
        {
            if (r1 == null || r2 == null || r1.Count != r2.Count)
                return false;
            bool flag = false;
            bool[] flags = new bool[r2.Count];
            for (int i = 0; i < r1.Count; ++i)
            {
                flag = false;
                for (int j = 0; j < r2.Count; ++j)
                    if (!flags[j] && (r1[i].SideA == r2[j].SideA && r1[i].SideB == r2[j].SideB ||
                        r1[i].SideB == r2[j].SideA && r1[i].SideA == r2[j].SideB))
                    {
                        flags[j] = true;
                        flag = true;
                    }
            }
            return flag;
        }

        private bool CheckCorrect(List<Solution> solutions, List<Rectangle> rectangles)
        {
            //TODO sprawdziæ czy w którymœ solutions nie ma przypadkiem prostk¹ta spoza recatangles
            return true;
        }

        public List<Rectangle> RandomRectangles(int count, int maxSide)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            Random random = new Random();
            for (int i = 0; i < count; ++i)
                rectangles.Add(new Rectangle(random.Next() % maxSide, random.Next() % maxSide));
            return rectangles;

        }
    }
}
