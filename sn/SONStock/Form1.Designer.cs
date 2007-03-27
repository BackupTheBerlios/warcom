namespace SONStock
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.networkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.networkMatrixPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.learnNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.estimateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDataFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDataFromManyFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addDataToExistingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.performEstimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.wyczyœæDaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGraph1 = new SONStock.DataGraph();
            this.exitValuesMatrixPreview = new SONStock.MatrixPreview();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.networkToolStripMenuItem,
            this.estimateToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(697, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // networkToolStripMenuItem
            // 
            this.networkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveNetworkToolStripMenuItem,
            this.loadNetworkToolStripMenuItem,
            this.toolStripSeparator1,
            this.networkMatrixPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.learnNetworkToolStripMenuItem});
            this.networkToolStripMenuItem.Name = "networkToolStripMenuItem";
            this.networkToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.networkToolStripMenuItem.Text = "&Sieæ";
            // 
            // saveNetworkToolStripMenuItem
            // 
            this.saveNetworkToolStripMenuItem.Name = "saveNetworkToolStripMenuItem";
            this.saveNetworkToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveNetworkToolStripMenuItem.Text = "Zapisz sieæ";
            this.saveNetworkToolStripMenuItem.Click += new System.EventHandler(this.saveNetworkToolStripMenuItem_Click);
            // 
            // loadNetworkToolStripMenuItem
            // 
            this.loadNetworkToolStripMenuItem.Name = "loadNetworkToolStripMenuItem";
            this.loadNetworkToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadNetworkToolStripMenuItem.Text = "Wczytaj sieæ";
            this.loadNetworkToolStripMenuItem.Click += new System.EventHandler(this.loadNetworkToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // networkMatrixPreviewToolStripMenuItem
            // 
            this.networkMatrixPreviewToolStripMenuItem.Name = "networkMatrixPreviewToolStripMenuItem";
            this.networkMatrixPreviewToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.networkMatrixPreviewToolStripMenuItem.Text = "Zobacz macierze wag";
            this.networkMatrixPreviewToolStripMenuItem.Click += new System.EventHandler(this.networkMatrixPreviewToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(184, 6);
            // 
            // learnNetworkToolStripMenuItem
            // 
            this.learnNetworkToolStripMenuItem.Name = "learnNetworkToolStripMenuItem";
            this.learnNetworkToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.learnNetworkToolStripMenuItem.Text = "Ucz sieæ";
            this.learnNetworkToolStripMenuItem.Click += new System.EventHandler(this.learnNetworkToolStripMenuItem_Click);
            // 
            // estimateToolStripMenuItem
            // 
            this.estimateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDataFromFileToolStripMenuItem,
            this.loadDataFromManyFilesToolStripMenuItem,
            this.addDataToExistingToolStripMenuItem,
            this.toolStripSeparator3,
            this.performEstimationToolStripMenuItem,
            this.toolStripSeparator4,
            this.wyczyœæDaneToolStripMenuItem});
            this.estimateToolStripMenuItem.Name = "estimateToolStripMenuItem";
            this.estimateToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.estimateToolStripMenuItem.Text = "&Dane";
            // 
            // loadDataFromFileToolStripMenuItem
            // 
            this.loadDataFromFileToolStripMenuItem.Name = "loadDataFromFileToolStripMenuItem";
            this.loadDataFromFileToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.loadDataFromFileToolStripMenuItem.Text = "Wczytaj dane z pliku";
            this.loadDataFromFileToolStripMenuItem.Click += new System.EventHandler(this.loadDataFromFileToolStripMenuItem_Click);
            // 
            // loadDataFromManyFilesToolStripMenuItem
            // 
            this.loadDataFromManyFilesToolStripMenuItem.Name = "loadDataFromManyFilesToolStripMenuItem";
            this.loadDataFromManyFilesToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.loadDataFromManyFilesToolStripMenuItem.Text = "Wczytaj dane z wielu plików";
            this.loadDataFromManyFilesToolStripMenuItem.Click += new System.EventHandler(this.loadDataFromManyFilesToolStripMenuItem_Click);
            // 
            // addDataToExistingToolStripMenuItem
            // 
            this.addDataToExistingToolStripMenuItem.CheckOnClick = true;
            this.addDataToExistingToolStripMenuItem.Enabled = false;
            this.addDataToExistingToolStripMenuItem.Name = "addDataToExistingToolStripMenuItem";
            this.addDataToExistingToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.addDataToExistingToolStripMenuItem.Text = "Dodaj dane do wczytanych";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(215, 6);
            // 
            // performEstimationToolStripMenuItem
            // 
            this.performEstimationToolStripMenuItem.Enabled = false;
            this.performEstimationToolStripMenuItem.Name = "performEstimationToolStripMenuItem";
            this.performEstimationToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.performEstimationToolStripMenuItem.Text = "Prognozuj";
            this.performEstimationToolStripMenuItem.Click += new System.EventHandler(this.performEstimationToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(215, 6);
            // 
            // wyczyœæDaneToolStripMenuItem
            // 
            this.wyczyœæDaneToolStripMenuItem.Name = "wyczyœæDaneToolStripMenuItem";
            this.wyczyœæDaneToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.wyczyœæDaneToolStripMenuItem.Text = "Wyczyœæ dane";
            this.wyczyœæDaneToolStripMenuItem.Click += new System.EventHandler(this.clearDataToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.settingsToolStripMenuItem.Text = "&Ustawienia";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // dataGraph1
            // 
            this.dataGraph1.Location = new System.Drawing.Point(12, 38);
            this.dataGraph1.Name = "dataGraph1";
            this.dataGraph1.Size = new System.Drawing.Size(673, 236);
            this.dataGraph1.TabIndex = 6;
            // 
            // exitValuesMatrixPreview
            // 
            this.exitValuesMatrixPreview.BackColor = System.Drawing.SystemColors.Control;
            this.exitValuesMatrixPreview.CellWidth = 100;
            this.exitValuesMatrixPreview.Location = new System.Drawing.Point(268, 280);
            this.exitValuesMatrixPreview.Name = "exitValuesMatrixPreview";
            this.exitValuesMatrixPreview.Size = new System.Drawing.Size(292, 146);
            this.exitValuesMatrixPreview.TabIndex = 5;
            this.exitValuesMatrixPreview.Title = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 431);
            this.Controls.Add(this.dataGraph1);
            this.Controls.Add(this.exitValuesMatrixPreview);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "SONStock";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem networkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveNetworkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadNetworkToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem networkMatrixPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem learnNetworkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem estimateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDataFromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addDataToExistingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem performEstimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDataFromManyFilesToolStripMenuItem;

        private SettingsForm settingsForm = new SettingsForm();
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem wyczyœæDaneToolStripMenuItem;
        private MatrixPreview exitValuesMatrixPreview;
        private DataGraph dataGraph1;
    }
}

