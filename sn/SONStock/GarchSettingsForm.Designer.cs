namespace SONStock
{
    partial class GarchSettingsForm
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
            this.gammaTextBox = new System.Windows.Forms.TextBox();
            this.qNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.butCancel = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.thetaLabel = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.qNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // gammaTextBox
            // 
            this.gammaTextBox.Location = new System.Drawing.Point(303, 20);
            this.gammaTextBox.Name = "gammaTextBox";
            this.gammaTextBox.Size = new System.Drawing.Size(68, 20);
            this.gammaTextBox.TabIndex = 10;
            this.gammaTextBox.Text = "0,1";
            // 
            // qNumericUpDown
            // 
            this.qNumericUpDown.Location = new System.Drawing.Point(156, 12);
            this.qNumericUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.qNumericUpDown.Name = "qNumericUpDown";
            this.qNumericUpDown.Size = new System.Drawing.Size(67, 20);
            this.qNumericUpDown.TabIndex = 9;
            this.qNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.qNumericUpDown.ValueChanged += new System.EventHandler(this.qNumericUpDown_ValueChanged);
            // 
            // pNumericUpDown
            // 
            this.pNumericUpDown.Location = new System.Drawing.Point(48, 12);
            this.pNumericUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.pNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pNumericUpDown.Name = "pNumericUpDown";
            this.pNumericUpDown.Size = new System.Drawing.Size(66, 20);
            this.pNumericUpDown.TabIndex = 8;
            this.pNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pNumericUpDown.ValueChanged += new System.EventHandler(this.pNumericUpDown_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(12, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "p";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(134, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "q";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(243, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "gamma";
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(204, 42);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 15;
            this.butCancel.Text = "Anuluj";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(99, 42);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 14;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // thetaLabel
            // 
            this.thetaLabel.AutoSize = true;
            this.thetaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.thetaLabel.Location = new System.Drawing.Point(12, 71);
            this.thetaLabel.Name = "thetaLabel";
            this.thetaLabel.Size = new System.Drawing.Size(37, 16);
            this.thetaLabel.TabIndex = 16;
            this.thetaLabel.Text = "theta";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // GarchSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 273);
            this.Controls.Add(this.thetaLabel);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.gammaTextBox);
            this.Controls.Add(this.qNumericUpDown);
            this.Controls.Add(this.pNumericUpDown);
            this.Name = "GarchSettingsForm";
            this.Text = "Ustawienia modelu Garch GJR";
            ((System.ComponentModel.ISupportInitialize)(this.qNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox gammaTextBox;
        private System.Windows.Forms.NumericUpDown qNumericUpDown;
        private System.Windows.Forms.NumericUpDown pNumericUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Label thetaLabel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}