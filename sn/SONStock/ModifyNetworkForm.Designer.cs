namespace SONStock
{
    partial class ModifyNetworkForm
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
            this.butCancel = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.newHiddenLayerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.saveStatisticsCheckBox = new System.Windows.Forms.CheckBox();
            this.saveWeightsCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.newHiddenLayerNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(158, 123);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 3;
            this.butCancel.Text = "Anuluj";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(53, 123);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 2;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // newHiddenLayerNumericUpDown
            // 
            this.newHiddenLayerNumericUpDown.Location = new System.Drawing.Point(15, 39);
            this.newHiddenLayerNumericUpDown.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.newHiddenLayerNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.newHiddenLayerNumericUpDown.Name = "newHiddenLayerNumericUpDown";
            this.newHiddenLayerNumericUpDown.Size = new System.Drawing.Size(141, 20);
            this.newHiddenLayerNumericUpDown.TabIndex = 8;
            this.newHiddenLayerNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(12, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 16);
            this.label5.TabIndex = 7;
            this.label5.Text = "Wielkoœæ warstwy ukrytej";
            // 
            // saveStatisticsCheckBox
            // 
            this.saveStatisticsCheckBox.AutoSize = true;
            this.saveStatisticsCheckBox.Location = new System.Drawing.Point(15, 65);
            this.saveStatisticsCheckBox.Name = "saveStatisticsCheckBox";
            this.saveStatisticsCheckBox.Size = new System.Drawing.Size(143, 17);
            this.saveStatisticsCheckBox.TabIndex = 9;
            this.saveStatisticsCheckBox.Text = "Zachowaj statystyki sieci";
            this.saveStatisticsCheckBox.UseVisualStyleBackColor = true;
            // 
            // saveWeightsCheckBox
            // 
            this.saveWeightsCheckBox.AutoSize = true;
            this.saveWeightsCheckBox.Location = new System.Drawing.Point(15, 88);
            this.saveWeightsCheckBox.Name = "saveWeightsCheckBox";
            this.saveWeightsCheckBox.Size = new System.Drawing.Size(122, 17);
            this.saveWeightsCheckBox.TabIndex = 10;
            this.saveWeightsCheckBox.Text = "Zachowaj wagi sieci";
            this.saveWeightsCheckBox.UseVisualStyleBackColor = true;
            // 
            // ModifyNetworkForm
            // 
            this.AcceptButton = this.butOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(292, 175);
            this.ControlBox = false;
            this.Controls.Add(this.saveWeightsCheckBox);
            this.Controls.Add(this.saveStatisticsCheckBox);
            this.Controls.Add(this.newHiddenLayerNumericUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifyNetworkForm";
            this.Text = "Modyfikuj sieæ";
            ((System.ComponentModel.ISupportInitialize)(this.newHiddenLayerNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.NumericUpDown newHiddenLayerNumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox saveStatisticsCheckBox;
        private System.Windows.Forms.CheckBox saveWeightsCheckBox;
    }
}