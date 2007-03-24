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
            this.butOK = new System.Windows.Forms.Button();
            this.hiddenExitMatrixPreview = new SONStock.MatrixPreview();
            this.contextHiddenMatrixPreview = new SONStock.MatrixPreview();
            this.entryHiddenMatrixPreview = new SONStock.MatrixPreview();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(318, 376);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 1;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // hiddenExitMatrixPreview
            // 
            this.hiddenExitMatrixPreview.BackColor = System.Drawing.SystemColors.Control;
            this.hiddenExitMatrixPreview.CellWidth = 50;
            this.hiddenExitMatrixPreview.Location = new System.Drawing.Point(26, 253);
            this.hiddenExitMatrixPreview.Name = "hiddenExitMatrixPreview";
            this.hiddenExitMatrixPreview.Size = new System.Drawing.Size(367, 105);
            this.hiddenExitMatrixPreview.TabIndex = 4;
            this.hiddenExitMatrixPreview.Title = "Wagi miêdzy warstw¹ ukryt¹ a warstw¹ wyjœciow¹";
            // 
            // contextHiddenMatrixPreview
            // 
            this.contextHiddenMatrixPreview.BackColor = System.Drawing.SystemColors.Control;
            this.contextHiddenMatrixPreview.CellWidth = 50;
            this.contextHiddenMatrixPreview.Location = new System.Drawing.Point(26, 149);
            this.contextHiddenMatrixPreview.Name = "contextHiddenMatrixPreview";
            this.contextHiddenMatrixPreview.Size = new System.Drawing.Size(367, 98);
            this.contextHiddenMatrixPreview.TabIndex = 3;
            this.contextHiddenMatrixPreview.Title = "Wagi miêdzy warstw¹ kontekstow¹ a warstw¹ ukryt¹";
            // 
            // entryHiddenMatrixPreview
            // 
            this.entryHiddenMatrixPreview.BackColor = System.Drawing.SystemColors.Control;
            this.entryHiddenMatrixPreview.CellWidth = 50;
            this.entryHiddenMatrixPreview.Location = new System.Drawing.Point(26, 12);
            this.entryHiddenMatrixPreview.Name = "entryHiddenMatrixPreview";
            this.entryHiddenMatrixPreview.Size = new System.Drawing.Size(367, 119);
            this.entryHiddenMatrixPreview.TabIndex = 2;
            this.entryHiddenMatrixPreview.Title = "Wagi miêdzy wejœciem a warstw¹ ukryt¹";
            // 
            // MatrixPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 406);
            this.ControlBox = false;
            this.Controls.Add(this.hiddenExitMatrixPreview);
            this.Controls.Add(this.contextHiddenMatrixPreview);
            this.Controls.Add(this.entryHiddenMatrixPreview);
            this.Controls.Add(this.butOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MatrixPreviewForm";
            this.Text = "Macierze wag po³¹czeñ miêdzy warstwami";
            this.ResumeLayout(false);

        }

        #endregion

        /*private double[,] entryHiddenWeights;
        private double[,] contextHiddenWeights;
        private double[,] hiddenExitWeights;*/
        //private MatrixPreview entryHiddenPreview, contextHiddenPreview, hiddenExitPreview;
        private System.Windows.Forms.Button butOK;
        private MatrixPreview entryHiddenMatrixPreview;
        private MatrixPreview contextHiddenMatrixPreview;
        private MatrixPreview hiddenExitMatrixPreview;
    }
}