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

namespace Taio
{
    public partial class MainWindow : Form
    {

        private List<Rectangle> rectangles;
        private List<Solution> solutions = new List<Solution>();
        private DataLoader dataLoader = new DataLoader();
        private IAlgorithm algorithm;
        private BackgroundWorker bw;

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
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void threadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int index = -1;

            Debug.WriteLine("W¹tek zakoñczony");
            if (this.algorithm.GetRectangle() == null)
            {
                this.algorithm = null;
                this.EnableMenu(true);
                return;
            }

            Solution s = new Solution(this.algorithm.GetTag(), this.algorithm.GetRectangle());
            for (int i = 0; i < this.solutions.Count; ++i)
            {
                if (this.solutions[i].Tag == s.Tag)
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
            this.algorithm = null;
            this.EnableMenu(true);
        }

        private void startThread(object sender, DoWorkEventArgs e)
        {
            Debug.WriteLine("W¹tek rozpoczêty");
            if (this.algorithm != null)
                this.algorithm.ComputeMaximumRectangle(this.rectangles);
        }

        private void EnableMenu(bool flag)
        {
            this.openFileToolStripMenuItem.Enabled = flag;
            this.preciseSolutionToolStripMenuItem.Enabled = flag;
            this.randomRectanglesToolStripMenuItem.Enabled = flag;
            this.saveFileToolStripMenuItem.Enabled = flag;
            this.saveSolutionFileToolStripMenuItem.Enabled = flag;
            this.newFileToolStripMenuItem.Enabled = flag;
            this.newRectanglesToolStripMenuItem.Enabled = flag;
            this.algorithm1SolutionToolStripMenuItem.Enabled = flag;
            this.algorithm2SolutionToolStripMenuItem.Enabled = flag;
            this.algorithm3SolutionToolStripMenuItem.Enabled = flag;
        }

        #region Menu
        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.rectangleViewer.Clear();
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

        private void saveSolutionFileToolStripMenuItem_Click(object sender, EventArgs e)
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

        }

        private void randomRectanglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArgumentDialog ad = new ArgumentDialog();
            if (ad.ShowDialog() == DialogResult.OK)
            {
                this.rectangles = this.dataLoader.RandomRectangles(ad.Count, ad.Max);
                this.solutions.Clear();
                this.rectanglesTreeView.Nodes[0].Nodes.Clear();
                this.rectanglesTreeView.Nodes[1].Nodes.Clear();
                addRectanglesOnlyToView();
                addSolutionsOnlyToView();
            }
        }

        private void preciseSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EnableMenu(false);
            this.algorithm = new Algorithm0();
            bw.RunWorkerAsync();
        }

        private void algorithm1SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EnableMenu(false);
            this.algorithm = new Algorithm1();
            bw.RunWorkerAsync();
        }

        private void algorithm2SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EnableMenu(false);
            this.algorithm = new Algorithm2();
            bw.RunWorkerAsync();
        }

        private void algorithm3SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
        private void startAlgorithm_Click(object sender, EventArgs e)
        {

        }

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
                        viewRectangle(null, null);
                    indexChange(index);
                }
                else if (this.rectanglesTreeView.SelectedNode.Parent.Name.Equals("Solutions"))
                {
                    solutions.RemoveAt(index);
                    this.rectanglesTreeView.Nodes[1].Nodes.RemoveAt(index);
                    if (this.rectanglesTreeView.Nodes[1].Nodes.Count == 0)
                        viewRectangle(null, null);
                }
            }
        }

        // na razie zostawi³em t¹ metodê metodê
        // wyœwietlany odpowiedni prostok¹t z listy
        private void rectanglesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool isSolution = false;     // zmienna mówi, z której listy ma byæ wyœwietlony prostok¹t
            int index = -1;
            if (this.rectanglesTreeView.SelectedNode != null)
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
                    viewRectangle(rectangles[index], this.rectanglesTreeView.SelectedNode);
            }
            else
                viewRectangle(null, null);
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
                    viewRectangle(rectangles[index], this.rectanglesTreeView.SelectedNode);
            }
            else
                viewRectangle(null, null);
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
                if (i < this.rectangles.Count)
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

            Algorithm1 al = new Algorithm1();
            Rectangle res = al.ComputeMaximumRectangle(rr);
            System.Console.WriteLine(res.SideA + " " + res.SideB);
        }

        #endregion
    }
}