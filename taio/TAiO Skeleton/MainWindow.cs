using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Taio.Algorithms;
using System.Diagnostics;
using System.IO;
using System.Resources;

namespace Taio
{
    public partial class MainWindow : Form
    {

        private List<Rectangle> rectangles;
        private List<Solution> solutions = new List<Solution>();
        private DataLoader dataLoader = new DataLoader();
        private IAlgorithm algorithm;
        private BackgroundWorker bw;
        private DateTime dt;
        private String text;
        private char[] param = "\n".ToCharArray();
        
        public MainWindow()
        {
            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(startThread);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadCompleted);
            rectangles = new List<Rectangle>();
            solutions = new List<Solution>();
            InitializeComponent();

            //testDrawingComplexRects();
            //testAlgorihm1();
            //testAlgorithm2();
            //this.testAlgoritm0();
            this.ChangeColor();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private int countArea()
        {
            int result = 0;

            foreach (Rectangle rect in rectangles)
                result += rect.Area;

            return result;
        }

        private void threadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int index = -1;

            Debug.WriteLine("W¹tek zakoñczony");
            if (this.algorithm.GetRectangle() == null)
            {
                MessageBox.Show("b³¹d lub stop");
                this.algorithm = null;
                this.EnableMenu(true);
                return;
            }

            Solution s = new Solution(this.algorithm.GetTag(), this.algorithm.GetRectangle());
            s.Ts = DateTime.Now.Subtract(dt);
            for (int i = 0; i < this.solutions.Count; ++i)
            {
                if (this.solutions[i].Tag == s.Tag && i < this.rectanglesTreeView.Nodes[1].Nodes.Count)
                {
                    index = i;
                    this.rectanglesTreeView.SelectedNode = this.rectanglesTreeView.Nodes[1].Nodes[i];
                    removeRectangleFromTreeView();
                }
            }

            if (index == -1)
                index = this.solutions.Count;

            addSolution(s);
            this.rectanglesTreeView.SelectedNode = this.rectanglesTreeView.Nodes[1].Nodes[index];
            TreeNodeMouseClickEventArgs eventArg = new TreeNodeMouseClickEventArgs(this.rectanglesTreeView.SelectedNode,
                MouseButtons.Left, 1, 0, 0);
            rectanglesTreeView_NodeMouseClick(this, eventArg);
            this.rectanglesTreeView.Refresh();
            this.algorithm = null;
            this.EnableMenu(true);
            //MessageBox.Show("Gdzieœ to wypada³oby wyœwietliæ, czas wykonania= " +  s.Ts.ToString());
            text += "Suma wszystkich prostok¹tów:  " + countArea() + "\n";
            text += "Pole wyliczonego prostok¹ta:     " + s.Rectangle.Area + "\n";
            text += "Czas wykonania:                          " + s.Ts.ToString();
            output.Lines = text.Split(param);
        }

        private void startThread(object sender, DoWorkEventArgs e)
        {
            Debug.WriteLine("W¹tek rozpoczêty");
            dt = DateTime.Now;
            if (this.algorithm != null)
            {
                this.algorithm.ComputeMaximumRectangle(this.rectangles);
            }
        }

        private void ChangeColor()
        {
            this.BackColor = Properties.Settings.Default.color;
            this.menuStrip1.BackColor = Properties.Settings.Default.color;
            foreach (ToolStripMenuItem tsmi in this.menuStrip1.Items)
                foreach (ToolStripItem tsi in tsmi.DropDownItems)
                {
                    tsi.BackColor = this.BackColor;
                }
            this.rectanglesContextMenuStrip.BackColor = this.BackColor;
            this.rectangleViewer.ChangeColor();
            this.toolStripSeparator1.BackColor = this.BackColor;
        }

        private void EnableMenu(bool flag)
        {
            this.openFileToolStripMenuItem.Enabled = flag;
            this.preciseSolutionToolStripMenuItem.Enabled = flag;
            this.randomRectanglesToolStripMenuItem.Enabled = flag;
            this.saveFileToolStripMenuItem.Enabled = flag;
            this.newFileToolStripMenuItem.Enabled = flag;
            this.newRectanglesToolStripMenuItem.Enabled = flag;
            this.algorithm1SolutionToolStripMenuItem.Enabled = flag;
            this.algorithm2SolutionToolStripMenuItem.Enabled = flag;
            this.algorithm3SolutionToolStripMenuItem.Enabled = flag;
        }

        #region Menu
        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // czyszczone listy
            this.rectanglesTreeView.Nodes[0].Nodes.Clear();
            this.rectanglesTreeView.Nodes[1].Nodes.Clear();            
            this.rectangles.Clear();
            this.solutions.Clear();
            this.rectangleViewer.Clear();
            this.rectanglesTreeView.Nodes[0].Nodes.Clear();
            this.rectanglesTreeView.Nodes[1].Nodes.Clear();
        }

        private void colorChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.BackColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.color = this.colorDialog1.Color;
                Properties.Settings.Default.Save();
                this.ChangeColor();
                this.Invalidate();
            }
        }

        private void DrawToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool clearLists;
            this.dataLoader.OpenData(ref this.solutions, ref this.rectangles, out clearLists);
            if (clearLists)
            {
                this.rectanglesTreeView.Nodes[0].Nodes.Clear();
                this.rectanglesTreeView.Nodes[1].Nodes.Clear();
                addRectanglesOnlyToView();
                addSolutionsOnlyToView();
            }
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataLoader.SaveData(this.solutions, this.rectangles);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.algorithm != null)
            {
                if (MessageBox.Show("Czy napewno chcesz przerwaæ dzia³anie programu?",
                    "Zakoñczenie", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        this.algorithm.StopThread();
                        Thread.Sleep(500);
                        if (this.bw.IsBusy)
                            Debug.WriteLine("No to coœ siê nie zamkn¹³ jeszcze");
                    }
                    catch (Exception) { }
                }
                else
                    return;
            }
            Application.Exit();
        }

        private void newRectanglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.rectanglesTreeView.SelectedNode = null;
            viewRectangle(null, null);
            this.rectangleViewer.Clear();
        }

        private void randomRectanglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArgumentDialog ad = new ArgumentDialog();
            if (ad.ShowDialog() == DialogResult.OK)
            {
                this.rectangles = this.dataLoader.RandomRectangles(ad.Count, ad.Max, ad.Min);
                this.solutions.Clear();
                this.rectanglesTreeView.Nodes[0].Nodes.Clear();
                this.rectanglesTreeView.Nodes[1].Nodes.Clear();
                addRectanglesOnlyToView();
                addSolutionsOnlyToView();
            }
        }

        private void randomAddRectanglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArgumentDialog ad = new ArgumentDialog();
            if (ad.ShowDialog() == DialogResult.OK)
            {
                this.rectangles.AddRange(this.dataLoader.RandomRectangles(ad.Count, ad.Max, ad.Min));
                this.rectanglesTreeView.Nodes[0].Nodes.Clear();
                this.rectanglesTreeView.Nodes[1].Nodes.Clear();
                addRectanglesOnlyToView();
                addSolutionsOnlyToView();
            }
        }

        private void preciseSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = "Algorytm dok³adny:\n";
            this.EnableMenu(false);
            //this.algorithm = new Algorithm0();
            this.algorithm = new Algorithm0v2();
            //this.algorithm = new Algorithm0v1();
            bw.RunWorkerAsync();
        }

        private void algorithm1SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = "Algorytm aproksymuj¹cy 1:\n";
            this.EnableMenu(false);
            this.algorithm = new Algorithm1Mod();
            bw.RunWorkerAsync();
        }

        private void algorithm2SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = "Algorytm aproksymuj¹cy 2:\n";
            this.EnableMenu(false);
            this.algorithm = new Algorithm2();
            bw.RunWorkerAsync();
        }

        private void algorithm3SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = "Algorytm aproksymuj¹cy 3:\n";
            this.EnableMenu(false);
            this.algorithm = new Algorithm3();
            bw.RunWorkerAsync();
        }

        private void authorsHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void programHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // szuka w bin/Debug
            FileInfo fileInfo = new FileInfo("Resources/help.pdf");

            if (fileInfo.Exists)
                System.Diagnostics.Process.Start("Resources/help.pdf");
            else
            {
                // szuka w Resources w Taio Skeleton a nie w bin/Debug
                fileInfo = new FileInfo("../../Resources/help.pdf");
                if (fileInfo.Exists)
                    System.Diagnostics.Process.Start("../../Resources/help.pdf");
                else
                    MessageBox.Show("Brak pliku pomocy help.pdf w katalogu Resources", "Informacja", MessageBoxButtons.OK);
            }
        }
        #endregion

        #region Menu kontektstowe
        private void addRectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle rect = rectangleViewer.Rectangle;
            if (rect != null)
            {
                rectangles.Add(rect);
                viewRectangle(rect, addRectangleToTreeView(rect));
            }
        }

        private void removeRectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeRectangleFromTreeView();
        }
        #endregion

        #region Przyciski
        private void stopAlgorithm_Click(object sender, EventArgs e)
        {
            if (this.algorithm != null)
                this.algorithm.StopThread();
        }
        #endregion

        #region Lista prostok¹tów
        // dodawany prostok¹t do listy prostok¹tów
        private TreeNode addRectangleToTreeView(Rectangle rect)
        {
            TreeNode node = new TreeNode();
            int count = rectangles.Count;
            node.Text = count + " prostok¹t [" + rect.SideA +
                            ", " + rect.SideB + ", " + rect.Area + "]";

            // dodawanie do listy prostok¹tów 
            this.rectanglesTreeView.Nodes[0].Nodes.Add(node);
            this.rectanglesTreeView.SelectedNode = this.rectanglesTreeView.Nodes[0].Nodes[count-1];
            TreeNodeMouseClickEventArgs eventArg = new TreeNodeMouseClickEventArgs(this.rectanglesTreeView.SelectedNode,
                MouseButtons.Left, 1, 0, 0);
            rectanglesTreeView_NodeMouseClick(this, eventArg);
            this.rectanglesTreeView.Refresh();
            
            return node;
        }

        // prostok¹t usuwany z listy prostok¹tów
        private void removeRectangleFromTreeView()
        {
            int index = -1;

            if (this.rectanglesTreeView.SelectedNode != null && this.rectanglesTreeView.SelectedNode.Parent != null)
                index = this.rectanglesTreeView.SelectedNode.Index;

            if (index >= 0)
            {
                if (this.rectanglesTreeView.SelectedNode.Parent.Name.Equals("Rectangles"))
                {
                    rectangles.RemoveAt(index);
                    this.rectanglesTreeView.Nodes[0].Nodes.RemoveAt(index);
                    if (this.rectanglesTreeView.Nodes[0].Nodes.Count == 0)
                        this.rectangleViewer.Clear();
                    indexChange(index);
                }
                else if (this.rectanglesTreeView.SelectedNode.Parent.Name.Equals("Solutions"))
                {
                    solutions.RemoveAt(index);
                    this.rectanglesTreeView.Nodes[1].Nodes.RemoveAt(index);
                    if (this.rectanglesTreeView.Nodes[1].Nodes.Count == 0)
                        this.rectangleViewer.Clear(); 
                }

                TreeNodeMouseClickEventArgs eventArg = new TreeNodeMouseClickEventArgs(this.rectanglesTreeView.SelectedNode,
                                MouseButtons.Left, 1, 0, 0);
                rectanglesTreeView_NodeMouseClick(this, eventArg);
                this.rectanglesTreeView.Refresh();
            }
        }
                
        private void rectanglesTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            bool isSolution = false;     // zmienna mówi, z której listy ma byæ wyœwietlony prostok¹t
            int index = -1;
            if (e.Node != null)
            {
                if (e.Node.Parent != null && e.Node.Parent.Name.Equals("Rectangles"))
                    index = e.Node.Index;
                else if (e.Node.Parent != null && e.Node.Parent.Name.Equals("Solutions"))
                {
                    index = e.Node.Index;
                    isSolution = true;
                }                
            }
            if (index >= 0)
            {
                if (isSolution && solutions[index].Correct)
                    viewRectangle(solutions[index].Rectangle, this.rectanglesTreeView.SelectedNode);
                else
                {
                    Keys keysMod = Control.ModifierKeys;
                    if (keysMod == Keys.Control)
                    {
                        this.rectangleViewer.SelectRectangleByNumber(rectangles[index].Number);
                        this.rectangleViewer.Refresh();
                    }
                    else
                        viewRectangle(rectangles[index], this.rectanglesTreeView.SelectedNode);
                }
            }
            else
                this.rectangleViewer.Clear(); 
        }

        // aktualizacja indeksów prostok¹tów na liœcie, po usuniêciu prostok¹ta
        private void indexChange(int indexOfRemoved)
        {
            for (IEnumerator it = this.rectanglesTreeView.Nodes[0].Nodes.GetEnumerator(); it.MoveNext(); )
            {
                TreeNode node = (TreeNode)it.Current;
                if (node.Index >= indexOfRemoved)
                {
                    Rectangle rect = (Rectangle)rectangles[node.Index];
                    node.Text = node.Index + 1 + " prostok¹t [" + rect.SideA +
                            ", " + rect.SideB + ", " + rect.Area + "]";
                }
            }
        }

        private void rectangleViewer_RectangleClicked(int rectId)
        {
            if (rectId >= 0)
            {
                int i = 0;
                for (i = 0; i < this.rectangles.Count; ++i)
                    if (rectangles[i].Number == rectId)
                        break;
                if (i < this.rectangles.Count && i < this.rectanglesTreeView.Nodes[0].Nodes.Count)
                {
                    this.rectanglesTreeView.SelectedNode = this.rectanglesTreeView.Nodes[0].Nodes[i];
                    this.rectanglesTreeView.Refresh();
                }
            }
        }

        // dodawanie prostok¹tów tylko do widoku na kontrolce
        private void addRectanglesOnlyToView()
        {
            int count = 1;
            for (IEnumerator it = this.rectangles.GetEnumerator(); it.MoveNext(); )
            {
                TreeNode node = new TreeNode();
                Rectangle rect = (Rectangle)it.Current;
                node.Text = count + " prostok¹t [" + rect.SideA +
                            ", " + rect.SideB + ", " + rect.Area + "]";
                this.rectanglesTreeView.Nodes[0].Nodes.Add(node);
                count++;
            }
        }

        #endregion

        #region Lista rozwi¹zañ
        // dodawane rozwi¹zanie do listy rozwi¹zañ 
        private void addSolution(Solution newSolution)
        {
            if (newSolution != null)
            {
                solutions.Add(newSolution);
                TreeNode node = new TreeNode();
                Rectangle nsolRect = newSolution.Rectangle;
                String descr = "";
                if (nsolRect != null)
                    descr = " (" + nsolRect.SideA + ", " + nsolRect.SideB + ", " + nsolRect.Area + ")";
                node.Text = newSolution.Tag + descr;
                this.rectanglesTreeView.Nodes[1].Nodes.Add(node);
            }
        }


        // dodawanie rozwi¹zañ tylko do widoku na kontrolce
        private void addSolutionsOnlyToView()
        {
            for (IEnumerator it = this.solutions.GetEnumerator(); it.MoveNext(); )
            {
                TreeNode node = new TreeNode();
                Solution s = (Solution)it.Current;
                if (s == null)
                    continue;
                Rectangle sRect = s.Rectangle;
                String descr = "";
                if (sRect != null)
                    descr = " (" + sRect.SideA + ", " + sRect.SideB + ", " + sRect.Area + ")";
                node.Text = s.Tag + descr;
                this.rectanglesTreeView.Nodes[1].Nodes.Add(node);
            }
        }
        #endregion

        #region Kontrolka prostok¹ta
        // wyœwietlany prostok¹t w kontrolce
        private void viewRectangle(Rectangle rect, TreeNode node)
        {
            this.rectangleViewer.Rectangle = rect;
            this.rectangleViewer.Refresh();
        }

        private void acceptChangebutton_Click(object sender, EventArgs e)
        {
            int index = -1;
            // zmiany tylko dla prostok¹tów
            if (this.rectanglesTreeView.SelectedNode != null && this.rectanglesTreeView.SelectedNode.Parent != null
                && this.rectanglesTreeView.SelectedNode.Parent.Name.Equals("Rectangles"))
                index = this.rectanglesTreeView.SelectedNode.Index;
            if (index >= 0)
            {
                Taio.Rectangle rect = this.rectangleViewer.Rectangle;
                if (rect != null)
                {
                    rectangles[index] = rect;
                    this.rectanglesTreeView.SelectedNode.Text = (index + 1) + " prostok¹t [" + rect.SideA +
                            ", " + rect.SideB + ", " + rect.Area + "]";
                }
            }
        }

        #endregion

        #region Testy
        private void testDrawingComplexRects()
        {
            Rectangle t1 = new Rectangle(100, 200);
            Rectangle t2 = new Rectangle(100, 200);
            RectangleContainer rc = new RectangleContainer();
            rc.InsertRectangle(t1, Rectangle.Orientation.Vertical);
            rc.InsertRectangle(t2, new Point(90, 0), Rectangle.Orientation.Vertical);
            Rectangle t3 = rc.MaxCorrectRect;
            rectangles.Add(t1);
            addRectangleToTreeView(t1);
            rectangles.Add(t2);
            addRectangleToTreeView(t2);
            rectangleViewer.Rectangle = t3;
            rectangleViewer.Refresh();
        }

        private void testAlgorithm2()
        {
            Rectangle[] rects1 = new Rectangle[]{
                new Rectangle(19, 44),
                new Rectangle(20, 12),
                new Rectangle(15, 42),
                new Rectangle(16, 4),
                new Rectangle(35, 44),
                new Rectangle(23, 3),
                new Rectangle(24, 18),
                new Rectangle(8, 46),
                new Rectangle(38, 9),
                new Rectangle(13, 11)
            };

            Rectangle[] rects2 = new Rectangle[]{
                new Rectangle(50, 40),
                new Rectangle(30, 20),
                new Rectangle(20, 10),
                new Rectangle(50, 10),
                new Rectangle(51, 1)
            };

            Rectangle[] rects = rects1;
            rectangles.AddRange(rects);

            Algorithm2 al = new Algorithm2();
            Rectangle res = al.ComputeMaximumRectangle(rectangles);
            foreach (Rectangle r in res.ContainedRectangles)
                addRectangleToTreeView(r);
            addSolution(new Solution(al.GetTag(), res));
        } 

        private void testAlgorihm1()
        {
            Rectangle r1 = new Rectangle(10, 12);
            List<Rectangle> rr = new List<Rectangle>();
            rr.Add(r1);

            Algorithm1Mod al = new Algorithm1Mod();
            Rectangle res = al.ComputeMaximumRectangle(rr);
            System.Console.WriteLine(res.SideA + " " + res.SideB);
        }


        private void testAlgoritm0()
        {
            BackgroundWorker bwTest = new BackgroundWorker();
            bwTest.DoWork += new DoWorkEventHandler(startAlg0);
            bwTest.ProgressChanged += new ProgressChangedEventHandler(infoAlg0);
            bwTest.RunWorkerAsync();
        }

        private void infoAlg0(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Visible = true;
            this.progressBar1.Value = e.ProgressPercentage;
            this.progressBar1.Invalidate();           
        }

        private void startAlg0(object sender, DoWorkEventArgs e)
        {
            this.progressBar1.Maximum = 10000;
            for (int i = 0; i < 10000; ++i)
            {
                List<Rectangle> rects=null;
                if(i%100-1==0)
                    rects = this.dataLoader.RandomRectangles(5, 3, 1);
                else
                if (i % 10 - 1 == 0)
                    rects = this.dataLoader.RandomRectangles(5, 2, 1);
                else
                    rects = this.dataLoader.RandomRectangles(4, 4, 1);
                IAlgorithm alg0 = new Algorithm0();
                DateTime dt = DateTime.Now;
                alg0.ComputeMaximumRectangle(rects);
                Solution s1 = new Solution(alg0.GetTag(), alg0.GetRectangle());
                s1.Ts = DateTime.Now.Subtract(dt);
                IAlgorithm alg1 = new Algorithm0v1();
                dt = DateTime.Now;
                alg1.ComputeMaximumRectangle(rects);
                Solution s2 = new Solution(alg1.GetTag(), alg1.GetRectangle());
                s2.Ts = DateTime.Now.Subtract(dt);
                IAlgorithm alg2 = new Algorithm0v2();
                dt = DateTime.Now;
                alg2.ComputeMaximumRectangle(rects);
                Solution s3 = new Solution(alg2.GetTag(), alg2.GetRectangle());
                s3.Ts = DateTime.Now.Subtract(dt);
                if (s1.Rectangle.Area != s2.Rectangle.Area ||
                    s1.Rectangle.Area != s3.Rectangle.Area 
                    ||s3.Rectangle.Area != s2.Rectangle.Area
                    )
                {
                    List<Solution> s = new List<Solution>();
                    s.Add(s1);
                    s.Add(s2);
                    s.Add(s3);
                    this.dataLoader.AppendSolutions(@"C:\Documents and Settings\Jakub\Pulpit\projekty\testy\test" 
                        + i + ".taio",s, rects);
                }
                BackgroundWorker bw1 = (BackgroundWorker)sender;
                bw1.WorkerReportsProgress = true;
                bw1.ReportProgress(i+1);
            }
        }
        #endregion
    }
}