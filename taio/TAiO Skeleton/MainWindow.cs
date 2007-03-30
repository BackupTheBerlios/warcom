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
            InitializeComponent();
            //testDrawingComplexRects();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void threadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("W�tek zako�czony");
            this.algorithm = null;
        }

        private void startThread(object sender, DoWorkEventArgs e)
        {
            Debug.WriteLine("W�tek rozpocz�ty");
            if (this.algorithm != null)
                this.algorithm.ComputeMaximumRectangle(this.rectangles);
        }

        #region Menu
        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void DrawToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataLoader.OpenData(ref this.solutions, ref this.rectangles);
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
                this.rectangles = this.dataLoader.RandomRectangles(ad.Count,ad.Max);
                this.solutions.Clear();
            }
        }

        private void preciseSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.algorithm = new Algorithm0();
            bw.RunWorkerAsync();
            //Thread.Sleep(500);
            //Debug.WriteLine("Dobra w�tek chyba ju� dzia�a, a ja si� budze po drzemce");
        }

        private void algorithm1SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void algorithm2SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void algorithm3SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void authorsHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void programHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        #region Lista prostok�t�w
        // dodawany prostok�t do listy prostok�t�w
        private TreeNode addRectangleToTreeView(Rectangle rect)
        {
            TreeNode node = new TreeNode();
            int count = rectangles.Count;
            node.Text = count + " prostok�t [" + rect.SideA +
                            ", " + rect.SideB + ", " + rect.Area + "]";
           
            this.rectanglesTreeView.Nodes.Add(node);

            return node;
        }

        // prostok�t usuwany z listy prostok�t�w
        private void removeRectangleFromTreeView()
        {
            int index = -1;

            if (this.rectanglesTreeView.SelectedNode != null)
                index = this.rectanglesTreeView.SelectedNode.Index;

            if (index >= 0)
            {
                rectangles.RemoveAt(index);                
                this.rectanglesTreeView.Nodes.RemoveAt(index);
                if (this.rectanglesTreeView.Nodes.Count == 0)
                    viewRectangle(null, null);
                indexChange(index);
            }
        }

        // aktualizacja indeks�w prostok�t�w na li�cie, po usuni�ciu prostok�ta
        private void indexChange(int indexOfRemoved)
        {
            for (IEnumerator it = this.rectanglesTreeView.Nodes.GetEnumerator(); it.MoveNext(); )
            {
                TreeNode node = (TreeNode)it.Current;
                if (node.Index >= indexOfRemoved)
                {
                    Rectangle rect = (Rectangle)rectangles[node.Index];
                    node.Text = node.Index + 1 + " prostok�t [" + rect.SideA +
                            ", " + rect.SideB + ", " + rect.Area + "]";
                }
            }
        }

        #endregion        

        #region Kontrolka prostok�ta
        // wy�wietlany prostok�t w kontrolce
        private void viewRectangle(Rectangle rect, TreeNode node)
        {
            this.rectangleViewer.Rectangle = rect;
            this.rectangleViewer.Refresh();
        }
        #endregion        

        private void acceptChangebutton_Click(object sender, EventArgs e)
        {
            int index = -1;
            if (this.rectanglesTreeView.SelectedNode != null)
                index = this.rectanglesTreeView.SelectedNode.Index;
            if (index >= 0)
            {
                Taio.Rectangle rect = this.rectangleViewer.Rectangle;
                if (rect != null)
                {
                    rectangles[index] = rect;
                    this.rectanglesTreeView.SelectedNode.Text = (index + 1) + " prostok�t [" + rect.SideA +
                            ", " + rect.SideB + ", " + rect.Area + "]";
                }
            }
        }

        private void rectangleViewer_RectangleClicked(int rectId)
        {
            if (rectId >= 0)
            {
                if (rectId <= this.rectanglesTreeView.Nodes.Count)
                {
                    int i = 0;
                    for (i = 0; i < this.rectangles.Count; ++i)
                        if (rectangles[i].Number == rectId)
                            break;
                    if (i < this.rectangles.Count)
                    {
                        this.rectanglesTreeView.SelectedNode = this.rectanglesTreeView.Nodes[i];
                        this.rectanglesTreeView.Refresh();
                    }
                }
            }
        }

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

        private void rectanglesTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int index = -1;
            if (this.rectanglesTreeView.SelectedNode != null)
                index = e.Node.Index;
            if (index >= 0)
            {
                viewRectangle(rectangles[index], this.rectanglesTreeView.SelectedNode);
            }
            else
                viewRectangle(null, null);  
        }

    }
}