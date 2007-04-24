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
        private int min;

        public int Min
        {
            get { return min; }
            set { min = value; }
        }


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
            this.BackColor = Properties.Settings.Default.color;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateData()
        {
            int no;
            bool error = false;
            if (Int32.TryParse(this.CountTextBox.Text, out no) && no > 0)
            {
                this.Count = no;
                this.errorProvider1.SetError(this.CountTextBox, "");
            }
            else
            {
                error = true;
                this.errorProvider1.SetError(this.CountTextBox, "Niepoprawne wartoœci (liczby naturalne > 0 wymagane).");
            }
            if (Int32.TryParse(this.MaxSideTextBox.Text, out no) && no > 0)
            {
                this.Max = no;
                this.errorProvider1.SetError(this.MaxSideTextBox, "");
            }
            else
            {
                error = true;
                this.errorProvider1.SetError(this.MaxSideTextBox, "Niepoprawne wartoœci (liczby naturalne > 0 wymagane).");
            }
            if (Int32.TryParse(this.MinSideTextBox.Text, out no) && no > 0)
            {
                this.Min = no;
                this.errorProvider1.SetError(this.MinSideTextBox, "");
            }
            else
            {
                error = true;
                this.errorProvider1.SetError(this.MinSideTextBox, "Niepoprawne wartoœci (liczby naturalne > 0 wymagane).");
            }
            return error;
        }
    }
}