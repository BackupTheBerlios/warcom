using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SONStock
{
    public partial class DataGraph : UserControl
    {
        //maximum x and y coordinate to display, when scale = 1
        private int maxX, maxY;
        
        private int xUnit, yUnit;
        private int xValuesCounter = 15;
        //private double[] data= new double[]{123, 139, 15, 434, 645, 432, 340, 731, 342, 450, 801};
        private List<double[]> data = new List<double[]>();

        //scale
        private int scale = 1;
        //pen used to draw axis
        private Pen axisPen = new Pen(Brushes.Black, 2);
        //brush used to draw text under axis
        private Brush axisTextBrush = Brushes.Black;
        private List<Brush> dataBrushes = new List<Brush>();
        //font used to draw axis text
        private Font axisTextFont;
        private Font valuesTextFont;
        //whole control background color
        private Color backgroundColor = Color.White;
        //distance from axis to border of a whole control
        private int xBorder = 5;
        //distance from axis to border of a whole control
        private int yBorder = 5;
        //size of zoomin, zoomout button
        private int zoomControlSize;
        //x-coordinate of a point (0,0) in image coordinate
        private int yDrawingStartValue;
        //y-coordinate of a point (0,0) in image coordinate
        private int xDrawingStartValue;
        //maximum possible scale
        private int maxScale = 256;

        private bool isScaleUserSet = false;
        private Random random = new Random();

        public DataGraph()
        {
            InitializeComponent();
            this.displayArea.BackColor = backgroundColor;

            axisTextFont = new Font("Arial", 8);
            valuesTextFont = new Font("Arial", 7, FontStyle.Italic);

            this.AddData(new double[] { 123, 139, 15, 434, 645, 432, 340, 731, 342, 450, 801 });
            Console.WriteLine("afdonifd");
            this.AddData(new double[] { 523, 149, 155, 46, 56, 904, 40, 431, 742, 350, 501 });
        }

        public void AddData(double[] dataArray)
        {
            if (dataArray == null)
                throw new ArgumentNullException();

            data.Add(dataArray);

            int r = random.Next(255), g = random.Next(255), b = random.Next(255);
            Color c = Color.FromArgb(r, g, b);
            dataBrushes.Add(new SolidBrush(c));
        }

        private void ComputeScale()
        {
            if (data == null || data.Count == 0)
            {
                scale = 1;
                return;
            }

            double m = Double.NegativeInfinity;
            foreach (double[] dataList in data)
            {
                if (dataList != null && dataList.Length > 0)
                {
                    for (int i = 0; i < dataList.Length; i++)
                        if (dataList[i] > m)
                            m = dataList[i];
                }
            }

            scale = (int)Math.Ceiling(m / maxY);
        }

        private void displayArea_Paint(object sender, PaintEventArgs e)
        {
            yDrawingStartValue = this.Height - 4 * yBorder;
            xDrawingStartValue = xBorder;
            maxX = this.displayArea.Width - 2 * xBorder;
            maxY = this.displayArea.Height - 4 * yBorder;

            if (!isScaleUserSet)
                ComputeScale();

            xUnit = (maxX - 10) / xValuesCounter;
            PaintAxis(e.Graphics);
        }

        private void PaintAxis(Graphics graph)
        {
            //osie
            int controlWidth = this.displayArea.Width;
            int controlHeight = this.displayArea.Height;
            Point zeroPoint = new Point(xDrawingStartValue, yDrawingStartValue);
            Point xStopPoint = new Point(controlWidth - xBorder, yDrawingStartValue);
            Point yStopPoint = new Point(xDrawingStartValue, yBorder);
            graph.DrawLine(axisPen, zeroPoint, xStopPoint);
            graph.DrawLine(axisPen, zeroPoint, yStopPoint);
            //groty osi
            Point[] axisEndX = new Point[3];
            Point[] axisEndY = new Point[3];
            axisEndX[0] = xStopPoint;
            axisEndX[1] = new Point(xStopPoint.X - 5, xStopPoint.Y - 5);
            axisEndX[2] = new Point(xStopPoint.X - 5, xStopPoint.Y + 5);
            axisEndY[0] = yStopPoint;
            axisEndY[1] = new Point(yStopPoint.X - 5, yStopPoint.Y + 5);
            axisEndY[2] = new Point(yStopPoint.X + 5, yStopPoint.Y + 5);
            graph.FillPolygon(axisPen.Brush, axisEndX);
            graph.FillPolygon(axisPen.Brush, axisEndY);

            for(int i =0; i<data.Count; i++)
                PaintDataList(graph, zeroPoint, i);

            //podpisy osi
            for (int i = 0; i < maxX / xUnit + 1; i++)
                graph.DrawString(" " + i, axisTextFont, axisTextBrush,
                  new Point(zeroPoint.X - 5 + i * xUnit, zeroPoint.Y));

            graph.DrawString(" " + maxY * scale, axisTextFont, axisTextBrush,
                new Point(yStopPoint.X + 5, yStopPoint.Y));
        }

        public void PaintDataList(Graphics graph, Point zeroPoint, int dataListIndex)
        {
            if (dataListIndex < 0)
                throw new ArgumentException();

            double[] dataList = data[dataListIndex];
            if (dataList == null)
                return;
            Brush brush = dataBrushes[dataListIndex];
            if (brush == null)
                brush = Brushes.Red;

            Pen pen = new Pen(brush);

            Point prev, p = new Point(0, 0);
            for (int i = 0; i < xValuesCounter; i++)
            {
                if (i >= dataList.Length)
                    break;

                if (i == 0)
                    prev = zeroPoint;
                else
                    prev = p;

                p = new Point(zeroPoint.X + i * xUnit, ConvertRealValueToImagePosition(dataList[i]));
                graph.DrawLine(pen, prev, p);
                graph.DrawString(dataList[i].ToString(), valuesTextFont, brush, new Point(p.X + 1, p.Y + 2));
            }
        }


        //converts real coordinates to image coordinates
        public int ConvertRealValueToImagePosition(double yVal)
        {
            double imgY;
            imgY = this.Height - yVal / scale - 4 * yBorder;
            return (int)imgY;
        }

        private void zoomOut_Click(object sender, EventArgs e)
        {
            isScaleUserSet = true;
            if (scale < maxScale)
            {
                scale *= 2;
                if (scale == maxScale)
                    this.zoomOut.Enabled = false;
                this.zoomIn.Enabled = true;
                this.Refresh();
            }
        }

        private void zoomIn_Click(object sender, EventArgs e)
        {
            isScaleUserSet = true;
            if (scale > 1)
            {
                scale /= 2;
                if (scale == 1)
                    this.zoomIn.Enabled = false;
                this.zoomOut.Enabled = true;
                this.Refresh();
            }
        }
    }
}
