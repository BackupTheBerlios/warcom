using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SONStock
{
    public partial class MatrixPreviewForm : Form
    {
        public MatrixPreviewForm()
        {
            InitializeComponent();
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}