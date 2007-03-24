using System.Windows.Forms;
using System.Drawing;
using System;

namespace SONStock
{
    partial class MatrixPreview
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

        public MatrixPreview(string labelText, double[,] matrix)
        {
            this.labelText = labelText;
            BuildControl(matrix);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(3, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(27, 13);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Title";
            // 
            // MatrixPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.titleLabel);
            this.Name = "MatrixPreview";
            this.Size = new System.Drawing.Size(221, 203);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private string labelText;
        private double[,] matrix;
        private TextBox[,] tb;
        private int matrixSizeX, matrixSizeY;
        private System.Windows.Forms.Label titleLabel;

        private int cellWidth = 50;

        public int CellWidth
        {
            get { return cellWidth; }
            set 
            {
                if (value > 0)
                    cellWidth = value;
                else
                    cellWidth = 50;
            }
        }

        private int widthShift = 5;
        private int cellHeight = 30;
        private int heightShift = 18;

        public void BuildControl(double[] matrix)
        {
            if(matrix == null)
                throw new ArgumentNullException();

            this.matrix = new double[1, matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
                this.matrix[0, i] = matrix[i];

            BuildControl(this.matrix);
        }

        public void BuildControl(double[,] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            this.matrix = matrix;
            this.matrixSizeX = matrix.GetUpperBound(0) + 1;
            this.matrixSizeY = matrix.GetUpperBound(1) + 1;

            if (tb == null || tb.GetUpperBound(0) != matrixSizeX || tb.GetUpperBound(1) != matrixSizeY)
            {
                if (tb != null)
                    foreach (TextBox tbox in tb)
                        this.Controls.Remove(tbox);
                tb = new TextBox[matrixSizeX, matrixSizeY];
            }

            for (int i = 0; i < matrixSizeX; i++)
                for (int j = 0; j < matrixSizeY; j++)
                {
                    TextBox temp = tb[i, j] = new TextBox();
                    temp.SetBounds(i * cellWidth + widthShift, j * cellHeight + heightShift, cellWidth, cellHeight);
                    
                    temp.Text = Math.Round(matrix[i, j],2).ToString();
                    temp.Visible = true;
                    temp.TabIndex = j * matrixSizeX + i;
                    this.Controls.Add(temp);
                }

            Point edge = tb[matrixSizeX - 1, matrixSizeY - 1].Location;
            this.SetBounds(this.Location.X, this.Location.Y, edge.X + cellWidth + 160, edge.Y + cellHeight + 100);
        }

        public string Title
        {
            get { return titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        /*public double[,] Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }*/
    }
}
