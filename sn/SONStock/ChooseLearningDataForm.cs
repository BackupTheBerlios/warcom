using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SONStock
{
    public partial class ChooseLearningDataForm : Form
    {
        private SortedDictionary<DateTime, double> learningData;
        private int initialDateIndex = -1;
        private int learningDataSize = 0;

        public ChooseLearningDataForm()
        {
            InitializeComponent();
        }

        public void LoadData(SortedDictionary<DateTime, double> learningData)
        {
            this.learningData = learningData;
            this.initialDateComboBox.Items.Clear();
            foreach (DateTime dt in learningData.Keys)
            {
                this.initialDateComboBox.Items.Add(dt);
                this.learningDataSizeNumericUpDown.Minimum = 1;
                this.learningDataSizeNumericUpDown.Maximum = learningData.Count;
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (this.initialDateComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Wybierz pocz¹tkow¹ datê zestawu!");
                return;
            }

            initialDateIndex = this.initialDateComboBox.SelectedIndex;
            learningDataSize = (int) this.learningDataSizeNumericUpDown.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void initialDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.learningDataSizeNumericUpDown.Maximum = learningData.Count - this.initialDateComboBox.SelectedIndex;
        }

        public int InitialDateIndex
        {
            get { return initialDateIndex; }
        }

        public int LearningDataSetSize
        {
            get { return learningDataSize; }
        }

        
    }
}