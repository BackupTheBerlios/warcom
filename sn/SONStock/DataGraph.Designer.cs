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
            this.clearContentButton = new System.Windows.Forms.Button();
            this.zoomLabel = new System.Windows.Forms.Label();
            this.colorLabel = new System.Windows.Forms.Label();
            this.changeColor = new System.Windows.Forms.Button();
            this.dataSeriesComboBox = new System.Windows.Forms.ComboBox();
            this.zoomOut = new System.Windows.Forms.Button();
            this.zoomIn = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.valuesLabellingCheckBox = new System.Windows.Forms.CheckBox();
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
            this.displayArea.Size = new System.Drawing.Size(532, 240);
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
            this.splitContainer1.Panel2.Controls.Add(this.valuesLabellingCheckBox);
            this.splitContainer1.Panel2.Controls.Add(this.clearContentButton);
            this.splitContainer1.Panel2.Controls.Add(this.zoomLabel);
            this.splitContainer1.Panel2.Controls.Add(this.colorLabel);
            this.splitContainer1.Panel2.Controls.Add(this.changeColor);
            this.splitContainer1.Panel2.Controls.Add(this.dataSeriesComboBox);
            this.splitContainer1.Panel2.Controls.Add(this.zoomOut);
            this.splitContainer1.Panel2.Controls.Add(this.zoomIn);
            this.splitContainer1.Size = new System.Drawing.Size(649, 240);
            this.splitContainer1.SplitterDistance = 532;
            this.splitContainer1.TabIndex = 3;
            // 
            // clearContentButton
            // 
            this.clearContentButton.Location = new System.Drawing.Point(19, 202);
            this.clearContentButton.Name = "clearContentButton";
            this.clearContentButton.Size = new System.Drawing.Size(75, 23);
            this.clearContentButton.TabIndex = 5;
            this.clearContentButton.Text = "Wyczyœæ";
            this.clearContentButton.UseVisualStyleBackColor = true;
            this.clearContentButton.Click += new System.EventHandler(this.clearContentButton_Click);
            // 
            // zoomLabel
            // 
            this.zoomLabel.Location = new System.Drawing.Point(19, 9);
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(75, 16);
            this.zoomLabel.TabIndex = 4;
            this.zoomLabel.Text = "Zoom";
            this.zoomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // colorLabel
            // 
            this.colorLabel.Location = new System.Drawing.Point(19, 108);
            this.colorLabel.Name = "colorLabel";
            this.colorLabel.Size = new System.Drawing.Size(75, 23);
            this.colorLabel.TabIndex = 4;
            // 
            // changeColor
            // 
            this.changeColor.Location = new System.Drawing.Point(19, 156);
            this.changeColor.Name = "changeColor";
            this.changeColor.Size = new System.Drawing.Size(75, 23);
            this.changeColor.TabIndex = 3;
            this.changeColor.Text = "Zmieñ kolor";
            this.changeColor.UseVisualStyleBackColor = true;
            this.changeColor.Click += new System.EventHandler(this.changeColor_Click);
            // 
            // dataSeriesComboBox
            // 
            this.dataSeriesComboBox.FormattingEnabled = true;
            this.dataSeriesComboBox.Location = new System.Drawing.Point(19, 134);
            this.dataSeriesComboBox.MaxDropDownItems = 18;
            this.dataSeriesComboBox.Name = "dataSeriesComboBox";
            this.dataSeriesComboBox.Size = new System.Drawing.Size(75, 21);
            this.dataSeriesComboBox.TabIndex = 2;
            this.dataSeriesComboBox.Text = "Seria";
            this.dataSeriesComboBox.SelectedIndexChanged += new System.EventHandler(this.dataSeriesComboBox_SelectedIndexChanged);
            // 
            // zoomOut
            // 
            this.zoomOut.Location = new System.Drawing.Point(19, 57);
            this.zoomOut.Name = "zoomOut";
            this.zoomOut.Size = new System.Drawing.Size(75, 23);
            this.zoomOut.TabIndex = 1;
            this.zoomOut.Text = "-";
            this.zoomOut.UseVisualStyleBackColor = true;
            this.zoomOut.Click += new System.EventHandler(this.zoomOut_Click);
            // 
            // zoomIn
            // 
            this.zoomIn.Location = new System.Drawing.Point(19, 28);
            this.zoomIn.Name = "zoomIn";
            this.zoomIn.Size = new System.Drawing.Size(75, 23);
            this.zoomIn.TabIndex = 0;
            this.zoomIn.Text = "+";
            this.zoomIn.UseVisualStyleBackColor = true;
            this.zoomIn.Click += new System.EventHandler(this.zoomIn_Click);
            // 
            // valuesLabellingCheckBox
            // 
            this.valuesLabellingCheckBox.AutoSize = true;
            this.valuesLabellingCheckBox.Checked = true;
            this.valuesLabellingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.valuesLabellingCheckBox.Location = new System.Drawing.Point(8, 88);
            this.valuesLabellingCheckBox.Name = "valuesLabellingCheckBox";
            this.valuesLabellingCheckBox.Size = new System.Drawing.Size(93, 17);
            this.valuesLabellingCheckBox.TabIndex = 6;
            this.valuesLabellingCheckBox.Text = "Etykietuj dane";
            this.valuesLabellingCheckBox.UseVisualStyleBackColor = true;
            this.valuesLabellingCheckBox.CheckedChanged += new System.EventHandler(this.valuesLabellingCheckBox_CheckedChanged);
            // 
            // DataGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DataGraph";
            this.Size = new System.Drawing.Size(649, 240);
            ((System.ComponentModel.ISupportInitialize)(this.displayArea)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox displayArea;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button zoomOut;
        private System.Windows.Forms.Button zoomIn;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button changeColor;
        private System.Windows.Forms.ComboBox dataSeriesComboBox;
        private System.Windows.Forms.Label colorLabel;
        private System.Windows.Forms.Button clearContentButton;
        private System.Windows.Forms.Label zoomLabel;
        private System.Windows.Forms.CheckBox valuesLabellingCheckBox;

    }
}
