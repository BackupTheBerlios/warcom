using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Taio
{
    public partial class MainWindow : Form
    {
        
        private List<Rectangle> rectangles;
        private List<Solution> solutions = new List<Solution>();
        private int count;
        private DataLoader dataLoader = new DataLoader();
        
        public MainWindow()
        {
            count = 10;
            rectangles = new List<Rectangle>();
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

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
            //@TODO
            //pobraæ od u¿ytkownika ile ma losowaæ prostok¹tów i najwiêkszy bok
            this.rectangles = this.dataLoader.RandomRectangles(10, 20);
            this.solutions.Clear();
        }

        private void preciseSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                count += 10;
                rectangles.Add(rect);
                viewRectangle(rect, addRectangleToTreeView(rect));
            }
        }

        private void removeRectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeRectangleFromTreeView();
        }
        #endregion

        #region Lista prostok¹tów
        // dodawany prostok¹t do listy prostok¹tów
        private TreeNode addRectangleToTreeView(Rectangle rect)
        {
            TreeNode node = new TreeNode();
            int count = rectangles.Count;
            node.Text = count + " prostok¹t [" + rect.SideB +
                            ", " + rect.SideA + ", " + rect.Area + "]";
           
            this.rectanglesTreeView.Nodes.Add(node);

            return node;
        }

        // prostok¹t usuwany z listy prostok¹tów
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

        // aktualizacja indeksów prostok¹tów na liœcie, po usuniêciu prostok¹ta
        private void indexChange(int indexOfRemoved)
        {
            for (IEnumerator it = this.rectanglesTreeView.Nodes.GetEnumerator(); it.MoveNext(); )
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

        // uaktualnia prostok¹t wyœwietlany w kontrolce prostok¹ta
        private void rectanglesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int index = -1;

            if (this.rectanglesTreeView.SelectedNode != null)
                index = this.rectanglesTreeView.SelectedNode.Index;

            if (index >= 0)
            {
                viewRectangle(rectangles[index], this.rectanglesTreeView.SelectedNode);                
            }
            else
                viewRectangle(null, null);            
        }
        #endregion        

        #region Kontrolka prostok¹ta
        // wyœwietlany prostok¹t w kontrolce
        private void viewRectangle(Rectangle rect, TreeNode node)
        {
            //this.rectanglesTreeView.SelectedNode = node;
            this.rectangleViewer.Rectangle = rect;
            //this.rectangleViewer.Node = node;
            this.rectangleViewer.Refresh();
        }
        #endregion        

    }
}