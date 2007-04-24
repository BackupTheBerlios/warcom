namespace SONStock
{
    partial class SettingsForm
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
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.estMethodComboBox = new System.Windows.Forms.ComboBox();
            this.estTimeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.epsTextBox = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.entryLayerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.hiddenLayerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.useTechnicalAnalysisCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.estTimeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entryLayerNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hiddenLayerNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(103, 202);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 0;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.OKButtton_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(208, 202);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Anuluj";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Metoda przewidywania:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Czas przewidywania";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(12, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Dok³adnoœæ uczenia";
            // 
            // estMethodComboBox
            // 
            this.estMethodComboBox.FormattingEnabled = true;
            this.estMethodComboBox.Items.AddRange(new object[] {
            "Sieæ Elmana",
            "ARIMA/GARCH"});
            this.estMethodComboBox.Location = new System.Drawing.Point(208, 20);
            this.estMethodComboBox.Name = "estMethodComboBox";
            this.estMethodComboBox.Size = new System.Drawing.Size(141, 21);
            this.estMethodComboBox.TabIndex = 5;
            this.estMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.estMethodComboBox_SelectedIndexChanged);
            // 
            // estTimeNumericUpDown
            // 
            this.estTimeNumericUpDown.Location = new System.Drawing.Point(208, 80);
            this.estTimeNumericUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.estTimeNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.estTimeNumericUpDown.Name = "estTimeNumericUpDown";
            this.estTimeNumericUpDown.Size = new System.Drawing.Size(141, 20);
            this.estTimeNumericUpDown.TabIndex = 6;
            this.estTimeNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // epsTextBox
            // 
            this.epsTextBox.Location = new System.Drawing.Point(208, 142);
            this.epsTextBox.Name = "epsTextBox";
            this.epsTextBox.Size = new System.Drawing.Size(141, 20);
            this.epsTextBox.TabIndex = 7;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // entryLayerNumericUpDown
            // 
            this.entryLayerNumericUpDown.Location = new System.Drawing.Point(208, 47);
            this.entryLayerNumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.entryLayerNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.entryLayerNumericUpDown.Name = "entryLayerNumericUpDown";
            this.entryLayerNumericUpDown.Size = new System.Drawing.Size(141, 20);
            this.entryLayerNumericUpDown.TabIndex = 6;
            this.entryLayerNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(12, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Iloœæ danych historycznych:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(12, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "Wielkoœæ warstwy ukrytej";
            // 
            // hiddenLayerNumericUpDown
            // 
            this.hiddenLayerNumericUpDown.Location = new System.Drawing.Point(208, 109);
            this.hiddenLayerNumericUpDown.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.hiddenLayerNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hiddenLayerNumericUpDown.Name = "hiddenLayerNumericUpDown";
            this.hiddenLayerNumericUpDown.Size = new System.Drawing.Size(141, 20);
            this.hiddenLayerNumericUpDown.TabIndex = 6;
            this.hiddenLayerNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(12, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Analiza techniczna";
            // 
            // useTechnicalAnalysisCheckBox
            // 
            this.useTechnicalAnalysisCheckBox.AutoSize = true;
            this.useTechnicalAnalysisCheckBox.Location = new System.Drawing.Point(208, 170);
            this.useTechnicalAnalysisCheckBox.Name = "useTechnicalAnalysisCheckBox";
            this.useTechnicalAnalysisCheckBox.Size = new System.Drawing.Size(76, 17);
            this.useTechnicalAnalysisCheckBox.TabIndex = 8;
            this.useTechnicalAnalysisCheckBox.Text = "W³¹czona";
            this.useTechnicalAnalysisCheckBox.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 253);
            this.ControlBox = false;
            this.Controls.Add(this.useTechnicalAnalysisCheckBox);
            this.Controls.Add(this.epsTextBox);
            this.Controls.Add(this.hiddenLayerNumericUpDown);
            this.Controls.Add(this.entryLayerNumericUpDown);
            this.Controls.Add(this.estTimeNumericUpDown);
            this.Controls.Add(this.estMethodComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsForm";
            this.Text = "Ustawienia";
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.estTimeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entryLayerNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hiddenLayerNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox estMethodComboBox;
        private System.Windows.Forms.NumericUpDown estTimeNumericUpDown;
        private System.Windows.Forms.TextBox epsTextBox;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.NumericUpDown entryLayerNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown hiddenLayerNumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox useTechnicalAnalysisCheckBox;
        private System.Windows.Forms.Label label6;
    }
}