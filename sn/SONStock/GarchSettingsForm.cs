using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SONStock
{
    public partial class GarchSettingsForm : Form
    {
        private int x, y;
        private int p;

        public int P
        {
            get { return p; }
        }
        private int q;

        public int Q
        {
            get { return q; }
        }
        private double gamma;

        public double Gamma
        {
            get { return gamma; }
        }
        private double[] theta;

        public double[] Theta
        {
            get { return theta; }
        }


        public GarchSettingsForm()
        {
            InitializeComponent();
            this.pNumericUpDown.Value =p= 1;
            this.qNumericUpDown.Value= q = 1;
            x = this.thetaLabel.Right;
            y = this.thetaLabel.Bottom;
            for (int i = 0; i < 3; ++i)
            {
                TextBox tb = new TextBox();
                if (x + 20 + this.gammaTextBox.Width >= this.Width)
                {
                    x = 0;
                    y += 15;
                }
                tb.Left = x+20;
                tb.Top = y;
                tb.Width = this.gammaTextBox.Width;
                x += tb.Width;
                tb.Text = "0,1";
                tb.Name = "theta" + i;
                this.Controls.Add(tb);
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.gamma = Double.Parse(this.gammaTextBox.Text);
                this.theta = new double[p + q + 1];
                for (int i = 0; i < p + q + 1; ++i)
                    this.theta[i] = Double.Parse(((TextBox)this.Controls["theta" + i]).Text);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Niepoprawne dane");
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void pNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.pNumericUpDown.Value < 1)
                    this.pNumericUpDown.Value = 1;
                else
                {
                    this.ChangeControls();
                }
            }
            catch (Exception)
            {
                this.pNumericUpDown.Value = 1;
            }
        }

        private void ChangeControls()
        {
            try
            {
                if (P < this.pNumericUpDown.Value || Q < this.qNumericUpDown.Value)
                {
                    p=(int)this.pNumericUpDown.Value;
                    q=(int)this.qNumericUpDown.Value;
                    TextBox tb = new TextBox();
                    if (x + 20 + this.gammaTextBox.Width >= this.Width)
                    {
                        x = 0;
                        y += 20;
                    }
                    tb.Left = x + 20;
                    tb.Top = y;
                    tb.Width = this.gammaTextBox.Width;
                    x += tb.Width;
                    tb.Text = "0,1";
                    tb.Name = "theta" + P+Q;
                    this.Controls.Add(tb);
                }
                else
                {
                    p = (int)this.pNumericUpDown.Value;
                    q = (int)this.qNumericUpDown.Value;
                    this.Controls.RemoveAt(this.Controls.Count-1);
                    x = this.Controls[this.Controls.Count - 1].Right-10;
                    y = this.Controls[this.Controls.Count - 1].Top;
                }

            }
            catch (Exception)
            {
            }
        }

        private void qNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.qNumericUpDown.Value < 0)
                    this.qNumericUpDown.Value = 1;
                else
                {
                    this.ChangeControls();
                }
            }
            catch (Exception)
            {
                this.qNumericUpDown.Value = 1;
            }
        }
    }
}