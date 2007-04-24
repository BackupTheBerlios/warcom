namespace SONStock
{
    partial class ChooseLearningDataForm
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
            this.initialDateComboBox = new System.Windows.Forms.ComboBox();
            this.learningDataSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.learningDataSizeNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(157, 124);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "Anuluj";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(52, 124);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 4;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // initialDateComboBox
            // 
            this.initialDateComboBox.FormattingEnabled = true;
            this.initialDateComboBox.Location = new System.Drawing.Point(29, 28);
            this.initialDateComboBox.Name = "initialDateComboBox";
            this.initialDateComboBox.Size = new System.Drawing.Size(224, 21);
            this.initialDateComboBox.TabIndex = 6;
            this.initialDateComboBox.Text = "Wybierz pocz¹tkow¹ datê zestawu";
            this.initialDateComboBox.SelectedIndexChanged += new System.EventHandler(this.initialDateComboBox_SelectedIndexChanged);
            // 
            // learningDataSizeNumericUpDown
            // 
            this.learningDataSizeNumericUpDown.Location = new System.Drawing.Point(29, 82);
            this.learningDataSizeNumericUpDown.Name = "learningDataSizeNumericUpDown";
            this.learningDataSizeNumericUpDown.Size = new System.Drawing.Size(224, 20);
            this.learningDataSizeNumericUpDown.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "WprowadŸ iloœæ danych w zestawie";
            // 
            // ChooseLearningDataForm
            // 
            this.AcceptButton = this.butOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(286, 166);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.learningDataSizeNumericUpDown);
            this.Controls.Add(this.initialDateComboBox);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseLearningDataForm";
            this.Text = "Wybierz dane do uczenia sieci";
            ((System.ComponentModel.ISupportInitialize)(this.learningDataSizeNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.ComboBox initialDateComboBox;
        private System.Windows.Forms.NumericUpDown learningDataSizeNumericUpDown;
        private System.Windows.Forms.Label label1;
    }
}