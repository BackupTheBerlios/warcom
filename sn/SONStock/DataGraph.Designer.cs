namespace SONStock
{
    partial class DataGraph
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.displayArea = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.zoomIn = new System.Windows.Forms.Button();
            this.zoomOut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.displayArea)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // displayArea
            // 
            this.displayArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayArea.Location = new System.Drawing.Point(0, 0);
            this.displayArea.Name = "displayArea";
            this.displayArea.Size = new System.Drawing.Size(536, 296);
            this.displayArea.TabIndex = 0;
            this.displayArea.TabStop = false;
            this.displayArea.Paint += new System.Windows.Forms.PaintEventHandler(this.displayArea_Paint);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.displayArea);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.zoomOut);
            this.splitContainer1.Panel2.Controls.Add(this.zoomIn);
            this.splitContainer1.Size = new System.Drawing.Size(653, 296);
            this.splitContainer1.SplitterDistance = 536;
            this.splitContainer1.TabIndex = 3;
            // 
            // zoomIn
            // 
            this.zoomIn.Location = new System.Drawing.Point(19, 49);
            this.zoomIn.Name = "zoomIn";
            this.zoomIn.Size = new System.Drawing.Size(75, 23);
            this.zoomIn.TabIndex = 0;
            this.zoomIn.Text = "+";
            this.zoomIn.UseVisualStyleBackColor = true;
            this.zoomIn.Click += new System.EventHandler(this.zoomIn_Click);
            // 
            // zoomOut
            // 
            this.zoomOut.Location = new System.Drawing.Point(19, 90);
            this.zoomOut.Name = "zoomOut";
            this.zoomOut.Size = new System.Drawing.Size(75, 23);
            this.zoomOut.TabIndex = 1;
            this.zoomOut.Text = "-";
            this.zoomOut.UseVisualStyleBackColor = true;
            this.zoomOut.Click += new System.EventHandler(this.zoomOut_Click);
            // 
            // DataGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DataGraph";
            this.Size = new System.Drawing.Size(653, 296);
            ((System.ComponentModel.ISupportInitialize)(this.displayArea)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox displayArea;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button zoomOut;
        private System.Windows.Forms.Button zoomIn;

    }
}
