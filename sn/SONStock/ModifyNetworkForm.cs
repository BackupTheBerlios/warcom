using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SONStock
{
    public partial class ModifyNetworkForm : Form
    {
        private int newHiddenLayerSize;
        private bool saveStatistics = false;
        private bool saveWeights = true;

        public ModifyNetworkForm()
        {
            InitializeComponent();
            this.newHiddenLayerNumericUpDown.Value = Properties.Settings.Default.hiddenLayerSize;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            newHiddenLayerSize = (int) newHiddenLayerNumericUpDown.Value;
            saveStatistics = this.saveStatisticsCheckBox.Checked;
            saveWeights = this.saveWeightsCheckBox.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public int NewHiddenLayerSize
        {
            get { return newHiddenLayerSize; }
        }

        public bool SaveStatistics
        {
            get { return saveStatistics; }
        }

        public bool SaveWeights
        {
            get { return saveWeights; }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}