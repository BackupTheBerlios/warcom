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
        #region Zmienne klasy
        /// <summary>
        /// Lista prostok�t�w.
        /// </summary>
        private List<Rectangle> rectangles;
        /// <summary>
        /// Lista rozwi�za�.
        /// </summary>
        private List<Solution> solutions = new List<Solution>();
        /// <summary>
        /// Obiekt do �adowania danych.
        /// </summary>
        private DataLoader dataLoader = new DataLoader();
        /// <summary>
        /// Interfejs do algorytm�w.
        /// </summary>
        private IAlgorithm algorithm;
        private BackgroundWorker bw;
        private DateTime dt;
        private String text;
        private char[] param = "\n".ToCharArray();
        #endregion

        /// <summary>
        /// Konstruktor okna.
        /// </summary>
        public MainWindow()
        {
            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(startThread);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(threadCompleted);
            rectangles = new List<Rectangle>();
            solutions = new List<Solution>();
            InitializeComponent();
            this.ChangeColor();
        }

        #region Funkcje klasy
        /// <summary>
        /// Funkcja wylicza sum� p�l wszystkich prostok�t�w.
        /// </summary>
        /// <returns>suma p�l</returns>
        private int countArea()
        {
            int result = 0;

            foreach (Rectangle rect in rectangles)
                result += rect.Area;

            return result;
        }

        /// <summary>
        /// Funkcja wywo�ywana przy zako�czeniu w�tku.
        /// </summary>
        /// <param name="sender">obiekt wysy�aj�cy ��danie zako�czenia</param>
        /// <param name="e">parametry</param>
        private void threadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int index = -1;
            RectTreeNode tempNode = null;

            Debug.WriteLine("W�tek zako�czony");
            if (this.algorithm.GetRectangle() == null)
            {
                MessageBox.Show("Rozwi�zanie zerowe");
                this.algorithm = null;
                this.EnableMenu(true);
                return;
            }

            Solution s = new Solution(this.algorithm.GetTag(), this.algorithm.GetRectangle());
            
            // sprawdzane, czy dane rozwi�zanie ju� wyst�pi�o
            for (int i = 0; i < this.solutions.Count; ++i)
            {
                if (this.solutions[i].Tag == s.Tag && i < this.rectanglesTreeView.Nodes[1].Nodes.Count)
                {
                    index = i;
                    tempNode = (RectTreeNode)this.rectanglesTreeView.Nodes[1].Nodes[i];
                    break;
                }
            }

            if (index == -1)
                index = this.solutions.Count;

            if (tempNode != null)
            {
                this.rectanglesTreeView.SelectedNode = tempNode;
                removeRectangleFromTreeView();
            }

            // dodawanie rozwi�zanie
            addSolution(s, index);
            this.rectanglesTreeView.SelectedNode = this.rectanglesTreeView.Nodes[1].Nodes[index];
            TreeNodeMouseClickEventArgs eventArg = new TreeNodeMouseClickEventArgs(this.rectanglesTreeView.SelectedNode,
                MouseButtons.Left, 1, 0, 0);

            s.Ts = DateTime.Now.Subtract(dt);
            text += "Suma p�l wszystkich prostok�t�w:  " + countArea() + "\n";
            text += "Pole wyliczonego prostok�ta:            " + s.Rectangle.Area + "\n";
            text += "Czas wykonania:                                " + s.Ts.ToString();
            ((RectTreeNode)this.rectanglesTreeView.SelectedNode).InfoOutput = text;
            rectanglesTreeView_NodeMouseClick(this, eventArg);
            this.rectanglesTreeView.Refresh();
            this.algorithm = null;
            this.EnableMenu(true);           
        }

        /// <summary>
        /// Funkcja rozpoczynaj�ca w�tek.
        /// </summary>
        /// <param name="sender">obiekt ropoczynaj�cy w�tek</param>
        /// <param name="e">parametry</param>
        private void startThread(object sender, DoWorkEventArgs e)
        {
            Debug.WriteLine("W�tek rozpocz�ty");
            dt = DateTime.Now;
            if (this.algorithm != null)
            {
                this.algorithm.ComputeMaximumRectangle(this.rectangles);
            }
        }

        /// <summary>
        /// Funkcja do zmieninia koloru.
        /// </summary>
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

        /// <summary>
        /// Funkcja uaktyniaj�ca/deaktywuj�ca menu.
        /// </summary>
        /// <param name="flag">zmienna m�wi, czy menu ma by� aktywowane/deaktywowane</param>
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
        #endregion

        #region Menu
        /// <summary>
        /// Nowy projekt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // czyszczone listy
            this.rectanglesTreeView.Nodes[0].Nodes.Clear();
            this.rectanglesTreeView.Nodes[1].Nodes.Clear();            
            this.rectangles.Clear();
            this.solutions.Clear();
            this.rectanglesTreeView.Nodes[0].Nodes.Clear();
            this.rectanglesTreeView.Nodes[1].Nodes.Clear();
            viewRectangle(null, null);
        }

        /// <summary>
        /// Zmieniane kolory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Dane wczytywane zgodnie z formatem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool clearLists;
            this.dataLoader.OpenData(ref this.solutions, ref this.rectangles, out clearLists, false);
            if (clearLists)
            {
                this.rectanglesTreeView.Nodes[0].Nodes.Clear();
                this.rectanglesTreeView.Nodes[1].Nodes.Clear();
                addRectanglesOnlyToView();
                addSolutionsOnlyToView();
                viewRectangle(null, null);                                
            }
        }

        /// <summary>
        /// Dane zapisywane zgodnie z formatem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.dataLoader.SaveData(this.solutions, this.rectangles, false);
        }

        /// <summary>
        /// Dane wczytywane niezgodnie z formatem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool clearLists;
            this.dataLoader.OpenData(ref this.solutions, ref this.rectangles, out clearLists, true);
            if (clearLists)
            {
                this.rectanglesTreeView.Nodes[0].Nodes.Clear();
                this.rectanglesTreeView.Nodes[1].Nodes.Clear();
                addRectanglesOnlyToView();
                addSolutionsOnlyToView();
                viewRectangle(null, null);               
            }
        }

        /// <summary>
        /// Dane zapisywane niezgodnie z formatem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataLoader.SaveData(this.solutions, this.rectangles, true);
        }

        /// <summary>
        /// Wyj�cie z programu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.algorithm != null)
            {
                if (MessageBox.Show("Czy napewno chcesz przerwa� dzia�anie programu?",
                    "Zako�czenie", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        this.algorithm.StopThread();
                        Thread.Sleep(500);
                        if (this.bw.IsBusy)
                            Debug.WriteLine("W�tek jeszcze nie zako�czony");
                    }
                    catch (Exception) { }
                }
                else
                    return;
            }
            Application.Exit();
        }

        /// <summary>
        /// Dodawany nowy prostok�t.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newRectanglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.rectanglesTreeView.SelectedNode = null;
            viewRectangle(null, null);            
        }

        /// <summary>
        /// Losowane prostok�ty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                viewRectangle(null, null);
            }
        }

        /// <summary>
        /// Dodowane losowe prostok�ty do listy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                viewRectangle(null, null);
            }
        }

        /// <summary>
        /// Uruchamiany algorytm dok�adny.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void preciseSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = "Algorytm dok�adny:\n";
            this.EnableMenu(false);
            this.algorithm = new Algorithm0v2();
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Uruchamiany algorytm aproksymuj�cy 1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void algorithm1SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = "Algorytm aproksymuj�cy 1:\n";
            this.EnableMenu(false);
            this.algorithm = new Algorithm1Mod();
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Uruchamiany algorytm aproksymuj�cy 2.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void algorithm2SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = "Algorytm aproksymuj�cy 2:\n";
            this.EnableMenu(false);
            this.algorithm = new Algorithm2();
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Uruchamiany algorytm aproksymuj�cy 3.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void algorithm3SolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = "Algorytm aproksymuj�cy 3:\n";
            this.EnableMenu(false);
            this.algorithm = new Algorithm3();
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Uruchamiana pomoc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Dodawany nowy prostok�t.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addRectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle rect = rectangleViewer.Rectangle;
            if (rect != null)
            {
                rectangles.Add(rect);
                viewRectangle(rect, addRectangleToTreeView(rect));
            }
        }

        /// <summary>
        /// Usuwany prostok�t z listy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeRectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeRectangleFromTreeView();
        }
        #endregion

        #region Przyciski
        /// <summary>
        /// Stop aktualnie wykonywanego algorytmu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopAlgorithm_Click(object sender, EventArgs e)
        {
            if (this.algorithm != null)
                this.algorithm.StopThread();
        }
        #endregion

        #region Lista prostok�t�w
        /// <summary>
        ///  Dodawany prostok�t do wy�wietlanej listy prostok�t�w.
        /// </summary>
        /// <param name="rect">nowy prostok�t</param>
        /// <returns>nowo utworzony w�ze�</returns>
        private RectTreeNode addRectangleToTreeView(Rectangle rect)
        {
            RectTreeNode node = new RectTreeNode();
            int count = rectangles.Count;
            node.Text = count + " prostok�t [" + rect.SideA +
                            ", " + rect.SideB + ", " + rect.Area + "]";

            // dodawanie do listy prostok�t�w 
            this.rectanglesTreeView.Nodes[0].Nodes.Add(node);
            this.rectanglesTreeView.SelectedNode = this.rectanglesTreeView.Nodes[0].Nodes[count-1];
            TreeNodeMouseClickEventArgs eventArg = new TreeNodeMouseClickEventArgs(this.rectanglesTreeView.SelectedNode,
                MouseButtons.Left, 1, 0, 0);
            rectanglesTreeView_NodeMouseClick(this, eventArg);
            this.rectanglesTreeView.Refresh();
            
            return node;
        }

        /// <summary>
        /// Prostok�t usuwany z wy�wietlanej listy prostok�t�w.
        /// </summary>
        private void removeRectangleFromTreeView()
        {
            int index = -1;

            if (this.rectanglesTreeView.SelectedNode != null && this.rectanglesTreeView.SelectedNode.Parent != null)
                index = this.rectanglesTreeView.SelectedNode.Index;

            if (index >= 0)
            {
                // sprawdzana lista prostok�t�w
                if (this.rectanglesTreeView.SelectedNode.Parent.Name.Equals("Rectangles"))
                {
                    rectangles.RemoveAt(index);
                    this.rectanglesTreeView.Nodes[0].Nodes.RemoveAt(index);
                    if (this.rectanglesTreeView.Nodes[0].Nodes.Count == 0)
                        viewRectangle(null, null);
                    indexChange(index);
                }
                // sprawdzana lista rozwi�za�
                else if (this.rectanglesTreeView.SelectedNode.Parent.Name.Equals("Solutions"))
                {
                    solutions.RemoveAt(index);
                    this.rectanglesTreeView.Nodes[1].Nodes.RemoveAt(index);
                    if (this.rectanglesTreeView.Nodes[1].Nodes.Count == 0)
                        viewRectangle(null, null);
                }

                TreeNodeMouseClickEventArgs eventArg = new TreeNodeMouseClickEventArgs(this.rectanglesTreeView.SelectedNode,
                                MouseButtons.Left, 1, 0, 0);
                rectanglesTreeView_NodeMouseClick(this, eventArg);
                this.rectanglesTreeView.Refresh();
            }
        }
                
        /// <summary>
        /// Metoda obs�uguj�ca zdarzenie klikni�cia na jeden z w�z��w listy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rectanglesTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            bool isSolution = false;     // zmienna m�wi, z kt�rej listy ma by� wy�wietlony prostok�t
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
                    viewRectangle(solutions[index].Rectangle, (RectTreeNode)e.Node);
                else
                {
                    Keys keysMod = Control.ModifierKeys;
                    if (keysMod == Keys.Control)
                    {
                        this.rectangleViewer.SelectRectangleByNumber(rectangles[index].Number);
                        this.rectangleViewer.Refresh();
                    }
                    else
                        viewRectangle(rectangles[index], (RectTreeNode)e.Node);
                }
            }
            else
                viewRectangle(null, null);
        }

        /// <summary>
        /// Aktualizacja indeks�w prostok�t�w na li�cie, po usuni�ciu prostok�ta.
        /// </summary>
        /// <param name="indexOfRemoved"></param>
        private void indexChange(int indexOfRemoved)
        {
            for (IEnumerator it = this.rectanglesTreeView.Nodes[0].Nodes.GetEnumerator(); it.MoveNext(); )
            {
                RectTreeNode node = (RectTreeNode)it.Current;
                if (node.Index >= indexOfRemoved)
                {
                    Rectangle rect = (Rectangle)rectangles[node.Index];
                    node.Text = node.Index + 1 + " prostok�t [" + rect.SideA +
                            ", " + rect.SideB + ", " + rect.Area + "]";
                }
            }
        }

        /// <summary>
        /// Metoda pozwala wy�wietli� prostok�t z listy, kt�ry zosta� u�yty w rozwi�zaniu.
        /// </summary>
        /// <param name="rectId">indeks prostok�ta</param>
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

        /// <summary>
        /// Dodawanie prostok�t�w tylko do widoku na kontrolce.
        /// </summary>
        private void addRectanglesOnlyToView()
        {
            int count = 1;
            for (IEnumerator it = this.rectangles.GetEnumerator(); it.MoveNext(); )
            {
                RectTreeNode node = new RectTreeNode();
                Rectangle rect = (Rectangle)it.Current;
                node.Text = count + " prostok�t [" + rect.SideA +
                            ", " + rect.SideB + ", " + rect.Area + "]";
                this.rectanglesTreeView.Nodes[0].Nodes.Add(node);
                count++;
            }
        }
        #endregion

        #region Lista rozwi�za�
        /// <summary>
        /// Dodawane rozwi�zanie do listy rozwi�za�.
        /// </summary>
        /// <param name="newSolution">nowe rozwi�zanie</param>
        private void addSolution(Solution newSolution)
        {
            if (newSolution != null)
            {
                solutions.Add(newSolution);
                RectTreeNode node = new RectTreeNode();
                Rectangle nsolRect = newSolution.Rectangle;
                String descr = "";
                if (nsolRect != null)
                    descr = " (" + nsolRect.SideA + ", " + nsolRect.SideB + ", " + nsolRect.Area + ")";
                node.Text = newSolution.Tag + descr;
                this.rectanglesTreeView.Nodes[1].Nodes.Add(node);
            }
        }

        /// <summary>
        /// Dodawane rozwi�zanie do listy rozwi�za� w odpowiednie miejsce.
        /// </summary>
        /// <param name="newSolution">nowe rozwi�zanie</param>
        /// <param name="index">wskazane miejsce</param>
        private void addSolution(Solution newSolution, int index)
        {
            if (newSolution != null && index >= 0)
            {
                solutions.Insert(index, newSolution);
                RectTreeNode node = new RectTreeNode();
                Rectangle nsolRect = newSolution.Rectangle;
                String descr = "";
                if (nsolRect != null)
                    descr = " (" + nsolRect.SideA + ", " + nsolRect.SideB + ", " + nsolRect.Area + ")";
                node.Text = newSolution.Tag + descr;
                this.rectanglesTreeView.Nodes[1].Nodes.Insert(index, node);
            }
        }

        /// <summary>
        /// Dodawanie rozwi�za� tylko do widoku na kontrolce.
        /// </summary>
        private void addSolutionsOnlyToView()
        {
            for (IEnumerator it = this.solutions.GetEnumerator(); it.MoveNext(); )
            {
                RectTreeNode node = new RectTreeNode();
                Solution s = (Solution)it.Current;
                if (s == null)
                    continue;
                Rectangle sRect = s.Rectangle;
                String descr = "";
                if (sRect != null)
                    descr = " (" + sRect.SideA + ", " + sRect.SideB + ", " + sRect.Area + ")";
                node.Text = s.Tag + descr;
                this.rectanglesTreeView.Nodes[1].Nodes.Add(node);

                text = "Algorytm " + s.Tag + ":\n";
                text += "Suma p�l wszystkich prostok�t�w:  " + countArea() + "\n";
                text += "Pole wyliczonego prostok�ta:            " + s.Rectangle.Area + "\n";
                node.InfoOutput = text;
            }
        }
        #endregion

        #region Kontrolka prostok�ta
        /// <summary>
        /// Wy�wietlany prostok�t w kontrolce.
        /// </summary>
        /// <param name="rect">dany prostok�t</param>
        /// <param name="node">odpowiadaj�cy prostok�towi w�ze� listy</param>
        private void viewRectangle(Rectangle rect, RectTreeNode node)
        {
            this.rectangleViewer.Rectangle = rect;
            this.rectangleViewer.Refresh();

            if(rect == null)
                this.rectangleViewer.Clear(); 

            if (node == null)
            {
                text = "";
                output.Lines = text.Split(param);
            }
            else
                output.Lines = node.InfoOutput.Split(param);
        }

        /// <summary>
        /// Akceptacja zmian rozmiaru prostok�ta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void acceptChangebutton_Click(object sender, EventArgs e)
        {
            int index = -1;
            // zmiany tylko dla prostok�t�w
            if (this.rectanglesTreeView.SelectedNode != null && this.rectanglesTreeView.SelectedNode.Parent != null
                && this.rectanglesTreeView.SelectedNode.Parent.Name.Equals("Rectangles"))
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
        #endregion
                
        #region Klasa RectTreeNode
        protected class RectTreeNode : TreeNode
        {
            #region Zmienne klasy
            /// <summary>
            /// Informacje dla okna output.
            /// </summary>
            string infoOutput;
            #endregion

            #region Konstruktory
            /// <summary>
            /// Konstruktor bezparametrowy.
            /// </summary>
            public RectTreeNode() : base() 
            {
                infoOutput = "";
            }

            /// <summary>
            /// Konstruktor jednoparametrowy
            /// </summary>
            /// <param name="text">wy�wietlany tekst danego w�z�a</param>
            public RectTreeNode(string text) : base(text)
            {
                infoOutput = "";
            }

            /// <summary>
            /// Konstruktor dwuparametrowy
            /// </summary>
            /// <param name="text">wy�wietlany tekst danego w�z�a</param>
            /// <param name="infoOutput">tekst kontrolki output</param>
            public RectTreeNode(string text, string infoOutput)
                : base(text)
            {
                this.infoOutput = infoOutput;
            }
            #endregion

            #region Akcesory do zmiennych
            /// <summary>
            /// Informacje dla okna output.
            /// </summary>
            public string InfoOutput
            {
                get { return infoOutput; }
                set { infoOutput = value; }
            }
            #endregion
        }
        #endregion               
    }
}