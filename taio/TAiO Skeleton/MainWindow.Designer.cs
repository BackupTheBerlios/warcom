namespace Taio
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Prostok¹ty");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Rozwi¹zania");
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rectanglesTreeView = new System.Windows.Forms.TreeView();
            this.rectanglesContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRectangleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeRectangleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.startAlgorithm = new System.Windows.Forms.Button();
            this.stopAlgorithm = new System.Windows.Forms.Button();
            this.acceptChangebutton = new System.Windows.Forms.Button();
            this.addRectbutton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSolutionFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rectanglesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newRectanglesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomRectanglesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preciseSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithm1SolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithm2SolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithm3SolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.programHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.rectangleViewer = new Kontrolka_do_TAiO.RectDisplay();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.rectanglesContextMenuStrip.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rectanglesTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(769, 414);
            this.splitContainer1.SplitterDistance = 255;
            this.splitContainer1.TabIndex = 1;
            // 
            // rectanglesTreeView
            // 
            this.rectanglesTreeView.ContextMenuStrip = this.rectanglesContextMenuStrip;
            this.rectanglesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rectanglesTreeView.Location = new System.Drawing.Point(0, 0);
            this.rectanglesTreeView.Name = "rectanglesTreeView";
            treeNode1.Name = "Rectangles";
            treeNode1.Text = "Prostok¹ty";
            treeNode1.ToolTipText = "Lista prostok¹tów";
            treeNode2.Name = "Solutions";
            treeNode2.Text = "Rozwi¹zania";
            treeNode2.ToolTipText = "Lista rozwi¹zañ";
            this.rectanglesTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.rectanglesTreeView.Size = new System.Drawing.Size(255, 414);
            this.rectanglesTreeView.TabIndex = 0;
            this.toolTip.SetToolTip(this.rectanglesTreeView, "Lista prostok¹tów");
            this.rectanglesTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.rectanglesTreeView_NodeMouseClick);
            // 
            // rectanglesContextMenuStrip
            // 
            this.rectanglesContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRectangleToolStripMenuItem,
            this.removeRectangleToolStripMenuItem});
            this.rectanglesContextMenuStrip.Name = "rectanglesContextMenuStrip";
            this.rectanglesContextMenuStrip.Size = new System.Drawing.Size(219, 48);
            // 
            // addRectangleToolStripMenuItem
            // 
            this.addRectangleToolStripMenuItem.Name = "addRectangleToolStripMenuItem";
            this.addRectangleToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.addRectangleToolStripMenuItem.Text = "Dodaj prostok¹t";
            this.addRectangleToolStripMenuItem.Click += new System.EventHandler(this.addRectangleToolStripMenuItem_Click);
            // 
            // removeRectangleToolStripMenuItem
            // 
            this.removeRectangleToolStripMenuItem.Name = "removeRectangleToolStripMenuItem";
            this.removeRectangleToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.removeRectangleToolStripMenuItem.Text = "Usuñ prostok¹t/rozwi¹zanie";
            this.removeRectangleToolStripMenuItem.Click += new System.EventHandler(this.removeRectangleToolStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.rectangleViewer);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.startAlgorithm);
            this.splitContainer2.Panel2.Controls.Add(this.stopAlgorithm);
            this.splitContainer2.Panel2.Controls.Add(this.acceptChangebutton);
            this.splitContainer2.Panel2.Controls.Add(this.addRectbutton);
            this.splitContainer2.Size = new System.Drawing.Size(510, 414);
            this.splitContainer2.SplitterDistance = 338;
            this.splitContainer2.SplitterWidth = 8;
            this.splitContainer2.TabIndex = 1;
            // 
            // startAlgorithm
            // 
            this.startAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startAlgorithm.Location = new System.Drawing.Point(349, 16);
            this.startAlgorithm.Name = "startAlgorithm";
            this.startAlgorithm.Size = new System.Drawing.Size(68, 23);
            this.startAlgorithm.TabIndex = 1;
            this.startAlgorithm.Text = "Start";
            this.startAlgorithm.UseVisualStyleBackColor = true;
            this.startAlgorithm.Click += new System.EventHandler(this.startAlgorithm_Click);
            // 
            // stopAlgorithm
            // 
            this.stopAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopAlgorithm.Location = new System.Drawing.Point(423, 16);
            this.stopAlgorithm.Name = "stopAlgorithm";
            this.stopAlgorithm.Size = new System.Drawing.Size(75, 23);
            this.stopAlgorithm.TabIndex = 2;
            this.stopAlgorithm.Text = "Stop";
            this.stopAlgorithm.UseVisualStyleBackColor = true;
            this.stopAlgorithm.Click += new System.EventHandler(this.stopAlgorithm_Click);
            // 
            // acceptChangebutton
            // 
            this.acceptChangebutton.Location = new System.Drawing.Point(117, 16);
            this.acceptChangebutton.Name = "acceptChangebutton";
            this.acceptChangebutton.Size = new System.Drawing.Size(113, 23);
            this.acceptChangebutton.TabIndex = 1;
            this.acceptChangebutton.Text = "Akceptuj zmiany";
            this.acceptChangebutton.UseVisualStyleBackColor = true;
            this.acceptChangebutton.Click += new System.EventHandler(this.acceptChangebutton_Click);
            // 
            // addRectbutton
            // 
            this.addRectbutton.Location = new System.Drawing.Point(3, 16);
            this.addRectbutton.Name = "addRectbutton";
            this.addRectbutton.Size = new System.Drawing.Size(108, 23);
            this.addRectbutton.TabIndex = 0;
            this.addRectbutton.Text = "Dodaj prostok¹t";
            this.addRectbutton.UseVisualStyleBackColor = true;
            this.addRectbutton.Click += new System.EventHandler(this.addRectangleToolStripMenuItem_Click);
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.AutoToolTip = true;
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileToolStripMenuItem,
            this.openFileToolStripMenuItem,
            this.saveFileToolStripMenuItem,
            this.saveSolutionFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(34, 20);
            this.fileMenuItem.Text = "Plik";
            this.fileMenuItem.ToolTipText = "Plik";
            // 
            // newFileToolStripMenuItem
            // 
            this.newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            this.newFileToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.newFileToolStripMenuItem.Text = "Nowy";
            this.newFileToolStripMenuItem.Click += new System.EventHandler(this.newFileToolStripMenuItem_Click);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.openFileToolStripMenuItem.Text = "Otwórz";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // saveFileToolStripMenuItem
            // 
            this.saveFileToolStripMenuItem.Name = "saveFileToolStripMenuItem";
            this.saveFileToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveFileToolStripMenuItem.Text = "Zapisz";
            this.saveFileToolStripMenuItem.Click += new System.EventHandler(this.saveFileToolStripMenuItem_Click);
            // 
            // saveSolutionFileToolStripMenuItem
            // 
            this.saveSolutionFileToolStripMenuItem.Name = "saveSolutionFileToolStripMenuItem";
            this.saveSolutionFileToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveSolutionFileToolStripMenuItem.Text = "Zapisz rozwi¹zanie";
            this.saveSolutionFileToolStripMenuItem.Click += new System.EventHandler(this.saveSolutionFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(171, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.exitToolStripMenuItem.Text = "Wyjœcie";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // rectanglesToolStripMenuItem
            // 
            this.rectanglesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newRectanglesToolStripMenuItem,
            this.randomRectanglesToolStripMenuItem});
            this.rectanglesToolStripMenuItem.Name = "rectanglesToolStripMenuItem";
            this.rectanglesToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.rectanglesToolStripMenuItem.Text = "Prostok¹ty";
            // 
            // newRectanglesToolStripMenuItem
            // 
            this.newRectanglesToolStripMenuItem.Name = "newRectanglesToolStripMenuItem";
            this.newRectanglesToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.newRectanglesToolStripMenuItem.Text = "Nowy prostok¹t";
            this.newRectanglesToolStripMenuItem.Click += new System.EventHandler(this.newRectanglesToolStripMenuItem_Click);
            // 
            // randomRectanglesToolStripMenuItem
            // 
            this.randomRectanglesToolStripMenuItem.Name = "randomRectanglesToolStripMenuItem";
            this.randomRectanglesToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.randomRectanglesToolStripMenuItem.Text = "Generowanie losowe";
            this.randomRectanglesToolStripMenuItem.Click += new System.EventHandler(this.randomRectanglesToolStripMenuItem_Click);
            // 
            // solutionToolStripMenuItem
            // 
            this.solutionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preciseSolutionToolStripMenuItem,
            this.algorithm1SolutionToolStripMenuItem,
            this.algorithm2SolutionToolStripMenuItem,
            this.algorithm3SolutionToolStripMenuItem});
            this.solutionToolStripMenuItem.Name = "solutionToolStripMenuItem";
            this.solutionToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.solutionToolStripMenuItem.Text = "Rozwi¹zanie";
            // 
            // preciseSolutionToolStripMenuItem
            // 
            this.preciseSolutionToolStripMenuItem.Name = "preciseSolutionToolStripMenuItem";
            this.preciseSolutionToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.preciseSolutionToolStripMenuItem.Text = "Algorytm dok³adny";
            this.preciseSolutionToolStripMenuItem.Click += new System.EventHandler(this.preciseSolutionToolStripMenuItem_Click);
            // 
            // algorithm1SolutionToolStripMenuItem
            // 
            this.algorithm1SolutionToolStripMenuItem.Name = "algorithm1SolutionToolStripMenuItem";
            this.algorithm1SolutionToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.algorithm1SolutionToolStripMenuItem.Text = "Algorytm aproksymuj¹cy 1";
            this.algorithm1SolutionToolStripMenuItem.Click += new System.EventHandler(this.algorithm1SolutionToolStripMenuItem_Click);
            // 
            // algorithm2SolutionToolStripMenuItem
            // 
            this.algorithm2SolutionToolStripMenuItem.Name = "algorithm2SolutionToolStripMenuItem";
            this.algorithm2SolutionToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.algorithm2SolutionToolStripMenuItem.Text = "Algorytm aproksymuj¹cy 2";
            this.algorithm2SolutionToolStripMenuItem.Click += new System.EventHandler(this.algorithm2SolutionToolStripMenuItem_Click);
            // 
            // algorithm3SolutionToolStripMenuItem
            // 
            this.algorithm3SolutionToolStripMenuItem.Name = "algorithm3SolutionToolStripMenuItem";
            this.algorithm3SolutionToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.algorithm3SolutionToolStripMenuItem.Text = "Algorytm aproksymuj¹cy 3";
            this.algorithm3SolutionToolStripMenuItem.Click += new System.EventHandler(this.algorithm3SolutionToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programHelpToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.helpToolStripMenuItem.Text = "Pomoc";
            // 
            // programHelpToolStripMenuItem
            // 
            this.programHelpToolStripMenuItem.Name = "programHelpToolStripMenuItem";
            this.programHelpToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.programHelpToolStripMenuItem.Text = "Plik pomocy";
            this.programHelpToolStripMenuItem.Click += new System.EventHandler(this.programHelpToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.rectanglesToolStripMenuItem,
            this.solutionToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(769, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // rectangleViewer
            // 
            this.rectangleViewer.AxisTextFont = new System.Drawing.Font("Arial", 8F);
            this.rectangleViewer.BackgroundColor = System.Drawing.Color.White;
            this.rectangleViewer.CanDraw = true;
            this.rectangleViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rectangleViewer.Location = new System.Drawing.Point(0, 0);
            this.rectangleViewer.MinimumSize = new System.Drawing.Size(50, 50);
            this.rectangleViewer.Name = "rectangleViewer";
            this.rectangleViewer.Size = new System.Drawing.Size(510, 338);
            this.rectangleViewer.TabIndex = 0;
            this.rectangleViewer.XBorder = 5;
            this.rectangleViewer.YBorder = 5;
            this.rectangleViewer.RectangleClicked += new Kontrolka_do_TAiO.RectDisplay.RectangleClickHandler(this.rectangleViewer_RectangleClicked);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 438);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainWindow";
            this.Text = "TAiO prostok¹ty";
            this.toolTip.SetToolTip(this, "toolTip");
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.rectanglesContextMenuStrip.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView rectanglesTreeView;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip rectanglesContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addRectangleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeRectangleToolStripMenuItem;
        private Kontrolka_do_TAiO.RectDisplay rectangleViewer;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button acceptChangebutton;
        private System.Windows.Forms.Button addRectbutton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSolutionFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rectanglesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newRectanglesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem randomRectanglesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preciseSolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem algorithm1SolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem algorithm2SolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem algorithm3SolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem programHelpToolStripMenuItem;
        private System.Windows.Forms.Button stopAlgorithm;
        private System.Windows.Forms.Button startAlgorithm;
    }
}