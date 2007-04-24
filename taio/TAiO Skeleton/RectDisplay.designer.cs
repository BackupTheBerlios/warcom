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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.zoomIn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.zoomOut = new System.Windows.Forms.Button();
            this.rectInfo = new System.Windows.Forms.ListView();
            this.autoScaleCheckBox = new System.Windows.Forms.CheckBox();
            this.xTextBox = new System.Windows.Forms.TextBox();
            this.yTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.displayArea)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.label2.Location = new System.Drawing.Point(1, 67);
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
            this.label1.Location = new System.Drawing.Point(0, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Informacje              ";
            // 
            // mousePosition
            // 
            this.mousePosition.AutoSize = true;
            this.mousePosition.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.mousePosition.Location = new System.Drawing.Point(3, 93);
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
            this.splitContainer.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer.Size = new System.Drawing.Size(491, 345);
            this.splitContainer.SplitterDistance = 349;
            this.splitContainer.TabIndex = 13;
            // 
            // displayArea
            // 
            this.displayArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.displayArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayArea.Location = new System.Drawing.Point(0, 0);
            this.displayArea.Name = "displayArea";
            this.displayArea.Size = new System.Drawing.Size(345, 341);
            this.displayArea.TabIndex = 0;
            this.displayArea.TabStop = false;
            this.displayArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.displayArea_MouseMove);
            this.displayArea.Paint += new System.Windows.Forms.PaintEventHandler(this.displayArea_Paint);
            this.displayArea.MouseClick += new System.Windows.Forms.MouseEventHandler(this.displayArea_MouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.yTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.xTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.autoScaleCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.zoomIn);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.zoomOut);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.mousePosition);
            this.splitContainer1.Panel1MinSize = 165;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rectInfo);
            this.splitContainer1.Size = new System.Drawing.Size(134, 341);
            this.splitContainer1.SplitterDistance = 165;
            this.splitContainer1.TabIndex = 15;
            // 
            // zoomIn
            // 
            this.zoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.zoomIn.FlatAppearance.BorderSize = 0;
            this.zoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.zoomIn.Image = global::Taio.Properties.Resources.arrow_up_32;
            this.zoomIn.Location = new System.Drawing.Point(0, 0);
            this.zoomIn.Name = "zoomIn";
            this.zoomIn.Size = new System.Drawing.Size(40, 40);
            this.zoomIn.TabIndex = 9;
            this.zoomIn.UseVisualStyleBackColor = true;
            this.zoomIn.Click += new System.EventHandler(this.zoomIn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Blue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(0, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "o prostokatach:       ";
            // 
            // zoomOut
            // 
            this.zoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.zoomOut.FlatAppearance.BorderSize = 0;
            this.zoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.zoomOut.Image = global::Taio.Properties.Resources.arrow_down_32;
            this.zoomOut.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.zoomOut.Location = new System.Drawing.Point(49, 0);
            this.zoomOut.Name = "zoomOut";
            this.zoomOut.Size = new System.Drawing.Size(40, 40);
            this.zoomOut.TabIndex = 8;
            this.zoomOut.UseVisualStyleBackColor = true;
            this.zoomOut.Click += new System.EventHandler(this.zoomOut_Click);
            // 
            // rectInfo
            // 
            this.rectInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rectInfo.Location = new System.Drawing.Point(0, 0);
            this.rectInfo.Name = "rectInfo";
            this.rectInfo.Size = new System.Drawing.Size(134, 172);
            this.rectInfo.TabIndex = 0;
            this.rectInfo.UseCompatibleStateImageBehavior = false;
            this.rectInfo.View = System.Windows.Forms.View.List;
            // 
            // autoScaleCheckBox
            // 
            this.autoScaleCheckBox.AutoSize = true;
            this.autoScaleCheckBox.Checked = true;
            this.autoScaleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoScaleCheckBox.Location = new System.Drawing.Point(3, 46);
            this.autoScaleCheckBox.Name = "autoScaleCheckBox";
            this.autoScaleCheckBox.Size = new System.Drawing.Size(104, 17);
            this.autoScaleCheckBox.TabIndex = 15;
            this.autoScaleCheckBox.Text = "Auto skalowanie";
            this.autoScaleCheckBox.UseVisualStyleBackColor = true;
            this.autoScaleCheckBox.Click += new System.EventHandler(this.autoScaleCheckBox_Click);
            // 
            // xTextBox
            // 
            this.xTextBox.Location = new System.Drawing.Point(28, 109);
            this.xTextBox.Name = "xTextBox";
            this.xTextBox.Size = new System.Drawing.Size(35, 20);
            this.xTextBox.TabIndex = 16;
            this.xTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.xTextBox_KeyPress);
            // 
            // yTextBox
            // 
            this.yTextBox.Location = new System.Drawing.Point(95, 109);
            this.yTextBox.Name = "yTextBox";
            this.yTextBox.Size = new System.Drawing.Size(35, 20);
            this.yTextBox.TabIndex = 17;
            this.yTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.yTextBox_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "X=";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(69, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Y=";
            // 
            // RectDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.MinimumSize = new System.Drawing.Size(50, 50);
            this.Name = "RectDisplay";
            this.Size = new System.Drawing.Size(491, 345);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.displayArea)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView rectInfo;
        private System.Windows.Forms.CheckBox autoScaleCheckBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox yTextBox;
        private System.Windows.Forms.TextBox xTextBox;
    }
}
