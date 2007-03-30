using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Taio
{
    public partial class ArgumentDialog : Form
    {
        private int max;
        private int count;


        public int Max
        {
            get { return max; }
            set { max = value; }
        }
        

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public ArgumentDialog()
        {
            InitializeComponent();
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                this.errorProvider1.SetError(this.CountTextBox, "");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateData()
        {
            int max, count;
            if (Int32.TryParse(this.MaxSideTextBox.Text,out max) && 
                Int32.TryParse(this.CountTextBox.Text, out count))
            {
                if (max>0 && count>0)
                {
                    this.Max = max;
                    this.Count = count;
                    this.errorProvider1.SetError(this.CountTextBox, "");
                    return true;
                }
            }

            this.errorProvider1.SetError(this.CountTextBox, "Niepoprawne wartoœci (liczby naturalne > 0 wymagane).");
            return false;
        }
    }
}