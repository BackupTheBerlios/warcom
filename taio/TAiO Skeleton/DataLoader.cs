using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace Taio
{
    class DataLoader
    {
        private StringBuilder log = new StringBuilder();


        #region File operation
        public void SaveData(List<Solution> solutions, List<Rectangle> rectangles)
        {
            this.log = new StringBuilder();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pliki TAiO (*.taio;*.tao)|*.taio;*.tao|Wszystkie pliki (*.*)|*.*";
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

        public void OpenData(ref List<Solution> solutions, ref  List<Rectangle> rectangles, out bool clearLists)
        {
            this.log = new StringBuilder();
            clearLists = false;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Pliki TAiO (*.taio;*.tao)|*.taio;*.tao|Wszystkie pliki (*.*)|*.*";
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
                            if (!this.CheckCorrect(ref solutions, rectangles))
                            {
                                this.log.AppendLine("Wczytane rozwi¹zania nie s¹ poprawne");
                                Debug.WriteLine("Wczytane rozwi¹zania nie s¹ poprawne");
                            }
                            foreach (Solution s in solutions)
                                s.Info = info;
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
                        if (str.StartsWith("#"))
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
        #endregion

        #region read data from strings
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
                        {
                            int l = Int32.Parse(x1[i].Value) <= Int32.Parse(x2[i].Value)?
                                Int32.Parse(x1[i].Value) : Int32.Parse(x2[i].Value);
                            int rt = Int32.Parse(x1[i].Value) <= Int32.Parse(x2[i].Value) ?
                                Int32.Parse(x2[i].Value) : Int32.Parse(x1[i].Value);
                            int d = Int32.Parse(y1[i].Value) <= Int32.Parse(y2[i].Value) ?
                                Int32.Parse(y1[i].Value) : Int32.Parse(y2[i].Value);
                            int g = Int32.Parse(y1[i].Value) <= Int32.Parse(y2[i].Value) ?
                                Int32.Parse(y2[i].Value) : Int32.Parse(y1[i].Value);
                            //g += d;
                            //rt += l;
                            Rectangle rect = new Rectangle(new Point(l, d), new Point(rt, g));
                            if (rects.Count == 0 ||
                                    rect.LeftTop.X <= rects[0].LeftTop.X && rect.LeftTop.Y <= rects[0].LeftTop.Y)
                                rects.Insert(0, rect);
                            else
                                rects.Add(rect);
                        }
                        if (rects.Count > 0 && (rects[0].LeftTop.X != 0 || rects[0].LeftTop.Y != 0))
                            for (int i = rects.Count - 1; i >= 0; --i)
                                rects[i] = rects[i].Move(new Point(rects[i].LeftTop.X - rects[0].LeftTop.X,
                                                                rects[i].LeftTop.Y - rects[0].LeftTop.Y));
                        RectangleContainer rc = new RectangleContainer();
                        rc.InsertRectangles(rects);
                        if (!rc.IsCorrectRectangle)
                        {
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
        #endregion

        #region append data to file
        private void AppendRectangle(Rectangle rect, TextWriter wr)
        {
            if (rect.ContainedRectangles == null || rect.ContainedRectangles.Count == 0)
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
                if (solutions != null && solutions.Count > 0 && solutions[0].Info != null)
                    wr.WriteLine(solutions[0].Info);
                else
                    wr.WriteLine("Plik stworzony przez grupê Aproksumuj¹cych z Wilkami:)))");
                foreach (Solution s in solutions)
                    if (s.Tag.StartsWith("AW") && (s.Info == null || s.Info == ""))
                        wr.WriteLine("Czas wykonania algorytmu " + s.Tag +
                            " wynosi³: " + s.Ts);
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
        #endregion

        #region is data correct?
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
                        break;
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
            if (rect.ContainedRectangles != null && rect.ContainedRectangles.Count > 0)
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
                        rect.Number = rectangles[i].Number;
                        return true;
                    }
                return false;
            }
            return true;
        }
        #endregion

        public List<Rectangle> RandomRectangles(int count, int maxSide, int minSide)
        {
            if (minSide < 1)
                minSide = 1;
            if (maxSide < minSide)
            {
                int tmp = minSide;
                minSide = maxSide;
                maxSide = minSide;
            }
            List<Rectangle> rectangles = new List<Rectangle>();
            Random random = new Random();
            for (int i = 0; i < count; ++i)
                if(maxSide!=minSide)
                rectangles.Add(new Rectangle(random.Next() % (maxSide-minSide+1) + minSide,
                    random.Next() % (maxSide-minSide+1) + minSide));
                else
                    rectangles.Add(new Rectangle(minSide, minSide));
            return rectangles;
        }

        public string GetLog
        {
            get { return this.log == null ? "" : this.log.ToString(); }
        }
    }
}
