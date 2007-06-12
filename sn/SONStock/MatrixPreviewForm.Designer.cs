namespace SONStock
{
    partial class MatrixPreviewForm
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

        public MatrixPreviewForm(double[,] entryHiddenWeights, double[,] contextHiddenWeights, double[,] hiddenExitWeights):this()
        {
            if(entryHiddenWeights != null)
                entryHiddenMatrixPreview.BuildControl(entryHiddenWeights);
            if(contextHiddenWeights != null)
                contextHiddenMatrixPreview.BuildControl(contextHiddenWeights);
            if (hiddenExitMatrixPreview != null)
                hiddenExitMatrixPreview.BuildControl(hiddenExitWeights);

        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hiddenExitMatrixPreview = new SONStock.MatrixPreview();
            this.contextHiddenMatrixPreview = new SONStock.MatrixPreview();
            this.entryHiddenMatrixPreview = new SONStock.MatrixPreview();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // hiddenExitMatrixPreview
            // 
            this.hiddenExitMatrixPreview.BackColor = System.Drawing.SystemColors.Control;
            this.hiddenExitMatrixPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.hiddenExitMatrixPreview.CellWidth = 50;
            this.hiddenExitMatrixPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hiddenExitMatrixPreview.Location = new System.Drawing.Point(0, 0);
            this.hiddenExitMatrixPreview.Name = "hiddenExitMatrixPreview";
            this.hiddenExitMatrixPreview.Size = new System.Drawing.Size(422, 118);
            this.hiddenExitMatrixPreview.TabIndex = 4;
            this.hiddenExitMatrixPreview.Title = "Wagi miêdzy warstw¹ ukryt¹ a warstw¹ wyjœciow¹";
            // 
            // contextHiddenMatrixPreview
            // 
            this.contextHiddenMatrixPreview.BackColor = System.Drawing.SystemColors.Control;
            this.contextHiddenMatrixPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.contextHiddenMatrixPreview.CellWidth = 50;
            this.contextHiddenMatrixPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contextHiddenMatrixPreview.Location = new System.Drawing.Point(0, 0);
            this.contextHiddenMatrixPreview.Name = "contextHiddenMatrixPreview";
            this.contextHiddenMatrixPreview.Size = new System.Drawing.Size(422, 114);
            this.contextHiddenMatrixPreview.TabIndex = 3;
            this.contextHiddenMatrixPreview.Title = "Wagi miêdzy warstw¹ kontekstow¹ a warstw¹ ukryt¹";
            // 
            // entryHiddenMatrixPreview
            // 
            this.entryHiddenMatrixPreview.BackColor = System.Drawing.SystemColors.Control;
            this.entryHiddenMatrixPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.entryHiddenMatrixPreview.CellWidth = 50;
            this.entryHiddenMatrixPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entryHiddenMatrixPreview.Location = new System.Drawing.Point(0, 0);
            this.entryHiddenMatrixPreview.Name = "entryHiddenMatrixPreview";
            this.entryHiddenMatrixPreview.Size = new System.Drawing.Size(422, 166);
            this.entryHiddenMatrixPreview.TabIndex = 2;
            this.entryHiddenMatrixPreview.Title = "Wagi miêdzy wejœciem a warstw¹ ukryt¹";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.hiddenExitMatrixPreview);
            this.splitContainer1.Size = new System.Drawing.Size(422, 406);
            this.splitContainer1.SplitterDistance = 284;
            this.splitContainer1.TabIndex = 6;
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
            this.splitContainer2.Panel1.Controls.Add(this.entryHiddenMatrixPreview);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.contextHiddenMatrixPreview);
            this.splitContainer2.Size = new System.Drawing.Size(422, 284);
            this.splitContainer2.SplitterDistance = 166;
            this.splitContainer2.TabIndex = 0;
            // 
            // MatrixPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 406);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatrixPreviewForm";
            this.ShowInTaskbar = false;
            this.Text = "Macierze wag po³¹czeñ miêdzy warstwami";
            this.TopMost = true;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /*private double[,] entryHiddenWeights;
        private double[,] contextHiddenWeights;
        private double[,] hiddenExitWeights;*/
        //private MatrixPreview entryHiddenPreview, contextHiddenPreview, hiddenExitPreview;
        private MatrixPreview entryHiddenMatrixPreview;
        private MatrixPreview contextHiddenMatrixPreview;
        private MatrixPreview hiddenExitMatrixPreview;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}