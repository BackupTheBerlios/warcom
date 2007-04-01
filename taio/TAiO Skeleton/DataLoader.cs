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
    class DataLoader
    {
        private StringBuilder log = new StringBuilder();

        public void SaveData(List<Solution> solutions, List<Rectangle> rectangles)
        {
            this.log = new StringBuilder();
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
                            this.log.AppendLine("Plik istnieje, ale z innymi danymi, a u¿ytkownik go nie chcia³ nadpisaæ");
                            MessageBox.Show("W takim razie nie robiê nic:)");
                            return;
                        }
                    }
                    else
                    {
                        this.DeleteOldSolutions(sfd.FileName, solutions);
                        this.AppendSolutions(sfd.FileName, solutions);
                        return;
                    }
                }
                this.AppendSolutions(sfd.FileName, solutions, rectangles);
            }
        }

        // zmienna clearLists mówi czy listy zosta³y wyczyszczone
        public void OpenData(ref List<Solution> solutions, ref  List<Rectangle> rectangles, out bool clearLists)
        {
            this.log = new StringBuilder();
            clearLists = false;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Pliki TAiO (*.tao)|*.tao";
            ofd.Title = "Wybierz plik zawieraj¹cy prostok¹ty";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                clearLists = true;
                rectangles.Clear();
                solutions.Clear();
                this.LoadSolutions(ofd.FileName, ref solutions, ref  rectangles);
            }
        }

        private void LoadSolutions(string fileName, ref List<Solution> solutions, ref  List<Rectangle> rectangles)
        {
            String str = null;
            using (TextReader rd = new StreamReader(fileName))
            {
                string wzorzec = "##\r\n(?<info>(.|\r|\n)*?)##\r\n(?<input>([0-9]*,[0-9]*\r\n)*?)" +
                    "##(\r\n)?(#(?<result>(.{0,4}\r\n([0-9]*,[0-9]*,[0-9]*,[0-9]*(\r\n)?)*)?))*";
                Regex wyr = new Regex(wzorzec, RegexOptions.IgnoreCase);
                if ((str = rd.ReadToEnd()) != null)
                {
                    try
                    {
                        str = str.Replace(" ", "");
                        Match elem = wyr.Match(str);
                        if (elem.Success)
                        {
                            string info = elem.Groups["info"].Value;
                            CaptureCollection result = elem.Groups["result"].Captures;
                            string input = elem.Groups["input"].Value;
                            rectangles = this.ReadInput(input);
                            solutions = this.ReadResult(result);
                            this.CheckCorrect(ref solutions, rectangles);
                        }
                        else
                        {
                            this.log.AppendLine("B³ad w danych w pliku - z³y format");
                            MessageBox.Show("B³¹d w danych w pliku - z³y format");
                        }
                    }
                    catch (Exception ex)
                    {
                        this.log.AppendLine("B³¹d w danych w pliku");
                        Debug.WriteLine("B³¹d: " + ex.Message + "\n\n" + ex.StackTrace);
                    }
                }
            }
        }

        private void DeleteOldSolutions(string filename, List<Solution> solutions)
        {
            if (solutions.Count == 0)
                return;
            StringBuilder strbld = new StringBuilder();
            for (int i = 1; i < solutions.Count; ++i)
                strbld.Append("(#" + solutions[i].Tag + ")|");
            strbld.Append("(#" + solutions[0].Tag + ")");
            Regex wyr = new Regex(strbld.ToString(), RegexOptions.IgnoreCase);
            strbld = new StringBuilder();
            string str;
            using (TextReader rd = new StreamReader(filename))
            {
                int counter = 0;
                bool del = false;
                while ((str = rd.ReadLine()) != null)
                {
                    if (str.StartsWith("##"))
                        counter++;
                    if (counter == 3)
                    {
                        if(str.StartsWith("#"))
                            del = false;
                        Match elem = wyr.Match(str);
                        if (elem.Success)
                            del = true;
                        if (!del)
                            strbld.AppendLine(str);
                    }
                    else
                        strbld.AppendLine(str);
                }
            }
            this.WriteString(filename, strbld.ToString());
        }

        private void WriteString(string filename, string data)
        {
            using (TextWriter wr = new StreamWriter(filename))
            {
                wr.Write(data);
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
                        str = str.Replace(" ", "");
                        int noOfComa = str.IndexOf(',');
                        int sideA = Int32.Parse(str.Substring(0, noOfComa));
                        int sideB = Int32.Parse(str.Substring(noOfComa + 1, str.Length - noOfComa - 1));
                        counter++;
                        rectangles.Add(new Rectangle(sideA, sideB));
                    }
                }
                catch (Exception ex)
                {
                    // to siê nie kompilowa³o - overlord_1984
                    // this.log.AppendLine("B³¹d przy wczytywaniu {0} prostok¹ta", counter.ToString());
                    this.log.AppendLine("B³¹d przy wczytywaniu " + counter + " prostok¹ta");
                    Debug.WriteLine("B³¹d przy wczytywaniu {0} prostok¹ta", counter.ToString());
                    throw ex;
                }
            }
            return rectangles;
        }

        private List<Solution> ReadResult(CaptureCollection result)
        {
            List<Solution> solutions = new List<Solution>();
            string wzorzec = "(?<tag>.{0,4}?)\r\n((?<x1>([0-9]*)?),(?<y1>([0-9]*)?),(?<x2>([0-9]*)?),(?<y2>([0-9]*)?)(\r\n)?)*";
            Regex wyr = new Regex(wzorzec, RegexOptions.IgnoreCase);
            for (int k = 0; k < result.Count; ++k)
            {
                string r = result[k].Value;
                Match elem = wyr.Match(r);
                if (elem.Success)
                {
                    string tag = "";
                    try
                    {
                        tag = elem.Groups["tag"].Value;
                        CaptureCollection x1 = elem.Groups["x1"].Captures;
                        CaptureCollection x2 = elem.Groups["x2"].Captures;
                        CaptureCollection y1 = elem.Groups["y1"].Captures;
                        CaptureCollection y2 = elem.Groups["y2"].Captures;
                        List<Rectangle> rects = new List<Rectangle>();
                        for (int i = 0; i < x1.Count; ++i)
                            rects.Add(new Rectangle(new System.Drawing.Point(Int32.Parse(x1[i].Value), Int32.Parse(y1[i].Value)),
                                                    new System.Drawing.Point(Int32.Parse(x2[i].Value), Int32.Parse(y2[i].Value))));
                        
                        //TODO tutaj nie dzia³a kontener tak jak nale¿y
                        RectangleContainer rc = new RectangleContainer();
                        rc.InsertRectangles(rects);
                        if (!rc.IsCorrectRectangle)
                        {
                            // to siê nie kompilowa³o - overlord_1984
                            // this.log.AppendLine("Powsta³ nie prostok¹t dla tagu: {0}", tag);
                            this.log.AppendLine("Powsta³ nie prostok¹t dla tagu: " + tag);
                            Debug.WriteLine("Powsta³ nie prostok¹t dla tagu: {0}", tag);
                        }
                        else
                            solutions.Add(new Solution(tag, rc.MaxCorrectRect));
                    }
                    catch (Exception ex)
                    {
                        this.log.AppendLine("B³¹d w tagu " + tag);
                        Debug.WriteLine("B³¹d w tagu " + tag + " : " + ex.Message + "\n\n" + ex.StackTrace);
                    }
                }
            }
            return solutions;
        }

        private void AppendRectangle(Rectangle rect, TextWriter wr)
        {
            if (rect.ContainedRectangles == null || rect.ContainedRectangles.Count==0)
                wr.WriteLine(rect.LeftTop.X + "," + rect.LeftTop.Y + "," +
                    rect.RightDown.X + "," + rect.RightDown.Y);
            else
            {
                for (int i = 0; i < rect.ContainedRectangles.Count; ++i)
                    this.AppendRectangle(rect.ContainedRectangles[i], wr);
            }
        }

        private void AppendSolutions(string fileName, List<Solution> solutions, List<Rectangle> rectangles)
        {
            using (TextWriter wr = new StreamWriter(fileName))
            {
                wr.WriteLine("##");
                wr.WriteLine("Plik stworzony przez grupê Aproksumuj¹cych z Wilkami:)))");
                wr.WriteLine("##");
                for (int i = 0; i < rectangles.Count; ++i)
                    wr.WriteLine(rectangles[i].SideA + "," + rectangles[i].SideB);
                wr.WriteLine("##");
            }
            this.AppendSolutions(fileName, solutions);
        }

        private void AppendSolutions(string fileName, List<Solution> solutions)
        {
            using (TextWriter wr = new StreamWriter(fileName, true))
            {
                for (int i = 0; i < solutions.Count; ++i)
                {
                    Solution s = solutions[i];
                    wr.WriteLine("#" + s.Tag);
                    this.AppendRectangle(s.Rectangle, wr);
                }
            }
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

        private bool CheckCorrect(ref List<Solution> solutions, List<Rectangle> rectangles)
        {
            bool flag = true;
            for (int i = 0; i < solutions.Count; ++i)
            {
                bool[] flags = new bool[rectangles.Count];
                if (!this.CheckCorrect(solutions[i].Rectangle, rectangles, ref flags))
                {
                    flag = false;
                    solutions[i].Correct = false;
                }
            }
            return flag;
        }

        private bool CheckCorrect(Rectangle rect, List<Rectangle> rectangles, ref bool[] flags)
        {
            if (rect == null)
                return true;
            if (rect.ContainedRectangles != null)
            {
                for (int i = 0; i < rect.ContainedRectangles.Count; ++i)
                    if (!this.CheckCorrect(rect.ContainedRectangles[i], rectangles, ref flags))
                        return false;
            }
            else
            {
                for (int i = 0; i < rectangles.Count; ++i)
                    if (!flags[i] &&
                        ((rect.SideA == rectangles[i].SideA && rect.SideB == rectangles[i].SideB) ||
                        (rect.SideB == rectangles[i].SideA && rect.SideA == rectangles[i].SideB)))
                    {
                        flags[i] = true;
                        return true;
                    }
            }
            return false;
        }

        public List<Rectangle> RandomRectangles(int count, int maxSide)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            Random random = new Random();
            for (int i = 0; i < count; ++i)
                rectangles.Add(new Rectangle(random.Next() % maxSide + 1, random.Next() % maxSide + 1));
            return rectangles;
        }

        public string GetLog
        {
            get { return this.log == null ? "" : this.log.ToString(); }
        }
    }
}
