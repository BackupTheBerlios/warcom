namespace Kontrolka_do_TAiO
{
    partial class RectDisplay
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mousePosition = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.displayArea = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rectInfo = new System.Windows.Forms.Label();
            this.zoomIn = new System.Windows.Forms.Button();
            this.zoomOut = new System.Windows.Forms.Button();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.displayArea)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Blue;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(0, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Pozycja:                 ";
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Blue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(0, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Informacje              ";
            // 
            // mousePosition
            // 
            this.mousePosition.AutoSize = true;
            this.mousePosition.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.mousePosition.Location = new System.Drawing.Point(3, 75);
            this.mousePosition.Name = "mousePosition";
            this.mousePosition.Size = new System.Drawing.Size(69, 13);
            this.mousePosition.TabIndex = 10;
            this.mousePosition.Text = "(X, Y) = (0, 0)";
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.displayArea);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.label3);
            this.splitContainer.Panel2.Controls.Add(this.rectInfo);
            this.splitContainer.Panel2.Controls.Add(this.zoomIn);
            this.splitContainer.Panel2.Controls.Add(this.label2);
            this.splitContainer.Panel2.Controls.Add(this.zoomOut);
            this.splitContainer.Panel2.Controls.Add(this.mousePosition);
            this.splitContainer.Panel2.Controls.Add(this.label1);
            this.splitContainer.Size = new System.Drawing.Size(442, 300);
            this.splitContainer.SplitterDistance = 300;
            this.splitContainer.TabIndex = 13;
            // 
            // displayArea
            // 
            this.displayArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.displayArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayArea.Location = new System.Drawing.Point(0, 0);
            this.displayArea.Name = "displayArea";
            this.displayArea.Size = new System.Drawing.Size(296, 296);
            this.displayArea.TabIndex = 0;
            this.displayArea.TabStop = false;
            this.displayArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.displayArea_MouseMove);
            this.displayArea.Paint += new System.Windows.Forms.PaintEventHandler(this.displayArea_Paint);
            this.displayArea.MouseClick += new System.Windows.Forms.MouseEventHandler(this.displayArea_MouseClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Blue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(-2, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "o prostokatach:       ";
            // 
            // rectInfo
            // 
            this.rectInfo.AutoSize = true;
            this.rectInfo.Location = new System.Drawing.Point(3, 146);
            this.rectInfo.Name = "rectInfo";
            this.rectInfo.Size = new System.Drawing.Size(0, 13);
            this.rectInfo.TabIndex = 13;
            // 
            // zoomIn
            // 
            this.zoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.zoomIn.Enabled = false;
            this.zoomIn.FlatAppearance.BorderSize = 0;
            this.zoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.zoomIn.Image = global::Taio.Properties.Resources.arrow_up_32;
            this.zoomIn.Location = new System.Drawing.Point(3, 3);
            this.zoomIn.Name = "zoomIn";
            this.zoomIn.Size = new System.Drawing.Size(40, 40);
            this.zoomIn.TabIndex = 9;
            this.zoomIn.UseVisualStyleBackColor = true;
            this.zoomIn.Click += new System.EventHandler(this.zoomIn_Click);
            // 
            // zoomOut
            // 
            this.zoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.zoomOut.FlatAppearance.BorderSize = 0;
            this.zoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.zoomOut.Image = global::Taio.Properties.Resources.arrow_down_32;
            this.zoomOut.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.zoomOut.Location = new System.Drawing.Point(49, 3);
            this.zoomOut.Name = "zoomOut";
            this.zoomOut.Size = new System.Drawing.Size(40, 40);
            this.zoomOut.TabIndex = 8;
            this.zoomOut.UseVisualStyleBackColor = true;
            this.zoomOut.Click += new System.EventHandler(this.zoomOut_Click);
            // 
            // RectDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.MinimumSize = new System.Drawing.Size(50, 50);
            this.Name = "RectDisplay";
            this.Size = new System.Drawing.Size(442, 300);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.displayArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox displayArea;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button zoomIn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button zoomOut;
        private System.Windows.Forms.Label mousePosition;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label rectInfo;
        private System.Windows.Forms.Label label3;
    }
}
