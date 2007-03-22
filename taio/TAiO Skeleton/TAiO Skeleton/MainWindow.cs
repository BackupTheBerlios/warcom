using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TAiO_Rectangles
{
    public partial class MainWindow : Form
    {
        private ArrayList rectangles;

        public MainWindow()
        {
            rectangles = new ArrayList();
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        #region Menu
        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveSolutionFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
            Rectangle rect = new Rectangle(20, 10);
            rectangles.Add(rect);
            addRectangleToTreeView(rect);
        }

        private void removeRectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeRectangleFromTreeView();
        }
        #endregion

        #region Lista prostokątów
        // dodawany prostokąt do listy prostokątów
        private void addRectangleToTreeView(Rectangle rect)
        {
            TreeNode node = new TreeNode();
            int count = rectangles.Count;
            node.Text = count + " prostokąt [" + rect.LongerSide +
                            ", " + rect.ShorterSide + ", " + rect.Area + "]";
           
            this.rectanglesTreeView.Nodes.Add(node);
        }

        // prostokąt usuwany z listy prostokątów
        private void removeRectangleFromTreeView()
        {
            int index = this.rectanglesTreeView.SelectedNode.Index;
            rectangles.RemoveAt(index);
            this.rectanglesTreeView.Nodes.RemoveAt(index);
        }
        #endregion        
    }
}