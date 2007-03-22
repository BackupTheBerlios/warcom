using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Kontrolka_do_TAiO
{
    public partial class RectDisplay : UserControl
    {
        //upper right corner of rectangle - used only when creating one
        private Point realValue = Point.Empty;
        //mouse position in real coordinate
        private Point realMousePosition = Point.Empty;
        //rectangle to display
        private Taio.Rectangle rectangle;
       //list of extracted rectangles from complex rectangle
        private List<Taio.Rectangle> extractedRectangles;
        //maximum x and y coordinate to display, when scale = 1
        private int maxX, maxY;
        //scale
        private int scale = 1;
        //information whether user can create new rectangle
        private bool canDraw = true;
        //pen used to draw axis
        private Pen axisPen = new Pen(Brushes.Black, 2);
        //brush used to draw text under axis
        private Brush axisTextBrush = Brushes.Black;
        //font used to draw axis text
        private Font axisTextFont; 
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
        private int indexOfRectangleToDisplay;

        public RectDisplay()
        {
            InitializeComponent();
            this.displayArea.BackColor = backgroundColor;
            this.zoomIn.BackColor = backgroundColor;
            this.zoomOut.BackColor = backgroundColor;
            this.mousePosition.BackColor = backgroundColor;
            this.splitContainer.Panel1.BackColor = backgroundColor;
            this.splitContainer.Panel2.BackColor = backgroundColor;
            this.rectInfo.BackColor = backgroundColor;
            axisTextFont = new Font("Arial", 6);
        }

        private void displayArea_Paint(object sender, PaintEventArgs e)
        {
            zoomControlSize = this.zoomIn.Height;
            yDrawingStartValue = this.Height - 4 * yBorder;
            xDrawingStartValue = xBorder;
            maxX = this.displayArea.Width - 2 * xBorder;
            maxY = this.displayArea.Height - 4 * yBorder;
            if (!realValue.IsEmpty || rectangle != null)
                DrawRectangle(e.Graphics);
            PaintAxis(e.Graphics);
        }

        //draw rectangle
        private void DrawRectangle(Graphics graph)
        {
            if (rectangle != null)
                DrawComplexRectangle(graph);
            else
            {
                Point endPoint = ConvertRealPostionToImagePosition(realValue);
                graph.FillRectangle(Brushes.Red, xDrawingStartValue, endPoint.Y, endPoint.X - xDrawingStartValue,
                    yDrawingStartValue - endPoint.Y);
                graph.DrawRectangle(new Pen(GetColorInversion(Color.Red), 2), xDrawingStartValue, endPoint.Y, endPoint.X - xDrawingStartValue,
                    yDrawingStartValue - endPoint.Y);
            }
        }

        //tries to get information about rectangle which is pointed by mouse pointer
        private void TrySetRectangleInfo()
        {
            string textToDisplay = "";
            if (realMousePosition != Point.Empty)
            {
                if (realMousePosition.X >= 0 && realMousePosition.X <= realValue.X &&
                    realMousePosition.Y >= 0 && realMousePosition.Y <= realValue.Y)
                    textToDisplay = "(0,0) - (" + realValue.X + "," + realValue.Y + ")";
                else if (rectangle != null)
                    textToDisplay = TrySetComplexRectangleInfo(realMousePosition.X,
                        realMousePosition.Y);
            }
            rectInfo.Text = textToDisplay;
        }

        //tries to get information about rectangle from complex rectangle which is pointed by mouse pointer
        private String TrySetComplexRectangleInfo(int rX, int rY)
        {
            String textToDisplay = "";
            for (int i = 0; i < extractedRectangles.Count; ++i)
            {
                Taio.Rectangle rectangle = extractedRectangles[i];
                if (rectangle.ContainedRectangles.Count == 0 && rectangle.LeftTop.X <= rX
                    && rectangle.RightDown.X >= rX && rectangle.LeftTop.Y >= rY && rectangle.RightDown.Y <= rY)
                {
                    textToDisplay += "(" + rectangle.LeftTop.X + "," + rectangle.RightDown.Y + ") - (" +
                        rectangle.RightDown.X + "," + rectangle.LeftTop.Y + ")\n";
                    indexOfRectangleToDisplay = i;
                }
            }
            return textToDisplay;
        }

        //draw complex rectangle
        private void DrawComplexRectangle(Graphics graph)
        {
            for(int i=0;i<extractedRectangles.Count;++i)
            {
                Taio.Rectangle rectangle = extractedRectangles[i];
                Point leftTop = ConvertRealPostionToImagePosition(rectangle.LeftTop);
                Point rightDown = ConvertRealPostionToImagePosition(rectangle.RightDown);
                graph.FillRectangle(new SolidBrush(rectangle.Color), leftTop.X, leftTop.Y ,
                    rightDown.X - leftTop.X, rightDown.Y - leftTop.Y);
                graph.DrawRectangle(new Pen(new SolidBrush(GetColorInversion(rectangle.Color)), 2),
                    leftTop.X, leftTop.Y, rightDown.X - leftTop.X, rightDown.Y - leftTop.Y);
            }
        }

        private Color GetColorInversion(Color color)
        {
            return Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
        }

     //draw axis
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
            axisEndX[1] = new Point(xStopPoint.X - 5, xStopPoint.Y-5);
            axisEndX[2] = new Point(xStopPoint.X - 5, xStopPoint.Y+5);
            axisEndY[0] = yStopPoint;
            axisEndY[1] = new Point(yStopPoint.X - 5, yStopPoint.Y + 5);
            axisEndY[2] = new Point(yStopPoint.X + 5, yStopPoint.Y + 5);
            graph.FillPolygon(axisPen.Brush, axisEndX);
            graph.FillPolygon(axisPen.Brush, axisEndY);
            //podpisy osi
            graph.DrawString("(0,0)", axisTextFont, axisTextBrush, new Point(zeroPoint.X - 5, zeroPoint.Y));
            graph.DrawString("(" + maxX * scale + ",0)", axisTextFont, axisTextBrush, 
                new Point(xStopPoint.X - 45, xStopPoint.Y));
            graph.DrawString("(0," + maxY * scale + ")", axisTextFont, axisTextBrush, 
                new Point(yStopPoint.X + 5, yStopPoint.Y));
        }

        private void displayArea_MouseMove(object sender, MouseEventArgs e)
        {
            bool validMousePos = DisplayMousePosition(e.X, e.Y);
            if (validMousePos && e.Button == MouseButtons.Left && canDraw)
                this.realValue = realMousePosition;
            TrySetRectangleInfo();
            this.Refresh();
        }

        //display mouse coordinates in real coordinates, and check whether this coordinates are above axis
        private bool DisplayMousePosition(int mX, int mY)
        {
            bool validPosition = true;
            int x = mX - xBorder;
            int y = this.displayArea.Height - mY - 3 * yBorder;
            if (x < xDrawingStartValue - xBorder)
            {
                x = 0;
                validPosition = false;
            }
            else if (x > maxX)
            {
                x = maxX;
                validPosition = false;
            }
            else if (y < 0)
            {
                y = 0;
                validPosition = false;
            }
            else if (y > maxY)
            {
                y = maxY;
                validPosition = false;
            }
            x *= scale;
            y*=scale;
            if (validPosition)
                realMousePosition = new Point(x, y);
            else
                realMousePosition = Point.Empty;
            this.mousePosition.Text = "(X, Y) = (" + x + ", " + y + ")";
            return validPosition;
        }

        //converts real coordinates to image coordinates
        public Point ConvertRealPostionToImagePosition(Point realPos)
        {
            double imgX, imgY;
            imgX = realPos.X / scale + xBorder;
            imgY = this.Height - realPos.Y / scale - 4 * yBorder;
            return new Point((int)imgX, (int)imgY);
        }

        #region Accessors
        [Description("Background color of the whole control"), Category("Appearance")] 
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        [Description("Font used to display text under axis"), Category("Appearance")] 
        public Font AxisTextFont
        {
            get { return axisTextFont; }
            set { axisTextFont = value; }
        }

        [Description("Brush used to display text"), Category("Appearance")] 
        public Brush AxisTextBrush
        {
            get { return axisTextBrush; }
            set { axisTextBrush = value; }
        }

        [Description("Distance from a border of a control"), Category("Appearance")] 
        public int YBorder
        {
            get { return yBorder; }
            set { yBorder = value; }
        }

        [Description("Information whether user can draw own rectangle"), Category("General")] 
        public bool CanDraw
        {
            get { return canDraw; }
            set { canDraw = value; }
        }

        [Description("Distance from a border of a control"), Category("Appearance")] 
        public int XBorder
        {
            get { return xBorder; }
            set { xBorder = value; }
        }

        [Description("Pen used to draw axis"), Category("Appearance")] 
        public Pen AxisPen
        {
            get { return axisPen; }
            set { axisPen = value; }
        }

        [Description("Rectangle created by a user, or rectangle which should be draw on a control"), Category("General")] 
        internal Taio.Rectangle Rectangle
        {
            get
            {
                int shorterSide = Math.Min(realValue.X, realValue.Y);
                int longerSide = Math.Max(realValue.X, realValue.Y);
                if (shorterSide == 0 || longerSide == 0)
                    throw new ArgumentException("Zero lenght side!");
                rectangle = new Taio.Rectangle(longerSide, shorterSide);
                return rectangle;
            }
            set
            {
                if (value.ContainedRectangles.Count == 0)
                {
                    realValue = new Point(value.RightDown.X, value.RightDown.Y);
                }
                else
                {
                    extractedRectangles = new List<Taio.Rectangle>();
                    ExtractRectangles(value);
                    SetColors();
                    rectangle = value;
                    canDraw = false;
                }
            }
        }
        #endregion

        private void SetColors()
        {
            int MYSTERIOUS_VALUE = 2 << 24;
            int numberOfRectangles = extractedRectangles.Count;
            int step = (MYSTERIOUS_VALUE - 1) / numberOfRectangles;
            Random rand = new Random();
            int current = rand.Next(MYSTERIOUS_VALUE);
            for (int i = 0; i < extractedRectangles.Count; ++i, current = (current + step) % (MYSTERIOUS_VALUE))
            {
                extractedRectangles[i].Color = Color.FromArgb((current >> 16) % 256, 
                    (current >> 8) % 256, current % 256);
            }

        }

        private void ExtractRectangles(Taio.Rectangle rectangle)
        {
            if (rectangle.ContainedRectangles.Count == 0)
                extractedRectangles.Add(rectangle);
            else
                for (int i = 0; i < rectangle.ContainedRectangles.Count; ++i)
                    ExtractRectangles(rectangle.ContainedRectangles[i]);
        }

        private void zoomOut_Click(object sender, EventArgs e)
        {
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
            if (scale > 1)
            {
                scale /= 2;
                if (scale == 1)
                    this.zoomIn.Enabled = false;
                this.zoomOut.Enabled = true;
                this.Refresh();
            }
        }

        private void displayArea_MouseClick(object sender, MouseEventArgs e)
        {
            if (rectangle != null)
            {
                Taio.Rectangle rect = extractedRectangles[indexOfRectangleToDisplay];
                extractedRectangles.RemoveAt(indexOfRectangleToDisplay);
                extractedRectangles.Add(rect);
            }
        }
    }
}
