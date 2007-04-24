using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SONStock
{
    public partial class SettingsForm : Form
    {
        private double eps;
        //TODO: dodac obsluge metody przewidywania (enum + Properties....)
        private int estimationMethod;
        private int estimationTime;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OKButtton_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                this.errorProvider1.SetError(this.epsTextBox, "");
                Properties.Settings.Default.eps = Double.Parse(this.epsTextBox.Text);
                Properties.Settings.Default.estimationTime = (int)this.estTimeNumericUpDown.Value;
                Properties.Settings.Default.hiddenLayerSize = (int)this.hiddenLayerNumericUpDown.Value;
                Properties.Settings.Default.entryLayerSize = (int)this.entryLayerNumericUpDown.Value;
                Properties.Settings.Default.estimationMethod = this.estMethodComboBox.SelectedIndex;
                Properties.Settings.Default.useTechnicalAnalysis = this.useTechnicalAnalysisCheckBox.Checked;
                Properties.Settings.Default.Save();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateData()
        {
            double tempEps;
            if (Double.TryParse(this.epsTextBox.Text, out tempEps))
            {
                this.errorProvider1.SetError(this.epsTextBox, "");
                return true;
            }

            this.errorProvider1.SetError(this.epsTextBox, "Incorrect value (double expected).");
            return false;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            eps = Properties.Settings.Default.eps;
            estimationTime = Properties.Settings.Default.estimationTime;

            this.estMethodComboBox.SelectedIndex = Properties.Settings.Default.estimationMethod;
            this.epsTextBox.Text = eps.ToString();
            this.estTimeNumericUpDown.Value = estimationTime;
            this.entryLayerNumericUpDown.Value = Properties.Settings.Default.entryLayerSize;
            this.hiddenLayerNumericUpDown.Value = Properties.Settings.Default.hiddenLayerSize;
            this.useTechnicalAnalysisCheckBox.Checked = Properties.Settings.Default.useTechnicalAnalysis;
 
        }

        private void estMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}