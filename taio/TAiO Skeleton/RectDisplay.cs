using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Kontrolka_do_TAiO
{
    //UWAGA!!!!!!!!!
    //Jedyny sposob aby ustawic lub pobrac prostokat z kontrolki jest zastosowanie
    //wlasciwosci Rectangle, kazdy inny sposob jest niedozwolony, 
    //REFERENCJA DO PROSTOKATA WEWNATRZ KONTROLKI I TA UDOSTEPNIONA NA ZEWNATRZ TO NIE JEST
    //TA SAMA REFERENCJA 
    public partial class RectDisplay : UserControl
    {
        //upper right corner of rectangle - used only when creating one
        private DoublePoint realValue = new DoublePoint();
        //mouse position in real coordinate
        private DoublePoint realMousePosition = new DoublePoint();
        //rectangle to display
        private Taio.Rectangle rectangle;
        //list of extracted rectangles from complex rectangle
        private List<Taio.Rectangle> extractedRectangles;
        //maximum x and y coordinate to display, when scale = 1
        private int maxX, maxY;
        //scale
        private double scale = 0.25;
        private double minScale = 0.125 / 4;
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
        private int alfa = 175;

        public delegate void RectangleClickHandler(int rectId);
        public event RectangleClickHandler RectangleClicked;


        public RectDisplay()
        {
            InitializeComponent();
            this.displayArea.BackColor = backgroundColor;
            this.zoomIn.BackColor = backgroundColor;
            this.zoomOut.BackColor = backgroundColor;
            this.mousePosition.BackColor = backgroundColor;
            this.splitContainer.Panel1.BackColor = backgroundColor;
            this.splitContainer.Panel2.BackColor = backgroundColor;
            axisTextFont = new Font("Arial", 8);
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
            else if (!realValue.IsEmpty)
            {
                DoublePoint endPoint = ConvertRealPostionToImagePosition(realValue);
                Color color = Color.FromArgb(alfa, 123, 231, 132);
                graph.FillRectangle(new SolidBrush(color), xDrawingStartValue, (int)endPoint.Y,
                    (int)endPoint.X - xDrawingStartValue, yDrawingStartValue - (int)endPoint.Y);
                graph.DrawRectangle(new Pen(GetColorInversion(color), 2), xDrawingStartValue,
                    (int)endPoint.Y, (int)endPoint.X - xDrawingStartValue,
                    yDrawingStartValue - (int)endPoint.Y);
            }
        }

        //tries to get information about rectangle which is pointed by mouse pointer
        private void TrySetRectangleInfo()
        {
            rectInfo.Items.Clear();
            if (!realMousePosition.IsEmpty)
            {
                if (realMousePosition.X >= 0 && realMousePosition.X <= (int)realValue.X &&
                    realMousePosition.Y >= 0 && realMousePosition.Y <= (int)realValue.Y)
                    rectInfo.Items.Add("(0,0) - (" + (int)realValue.X + "," + (int)realValue.Y + ")");
                else if (rectangle != null)
                    TrySetComplexRectangleInfo(realMousePosition.X,
                        realMousePosition.Y);
            }
        }

        //tries to get information about rectangle from complex rectangle which is pointed by mouse pointer
        private void TrySetComplexRectangleInfo(float rX, float rY)
        {
            for (int i = 0; i < extractedRectangles.Count; ++i)
            {
                Taio.Rectangle rectangle = extractedRectangles[i];
                if (rectangle.ContainedRectangles.Count == 0 && rectangle.LeftTop.X <= rX
                    && rectangle.RightDown.X >= rX && rectangle.LeftTop.Y <= rY &&
                    rectangle.RightDown.Y >= rY)
                {
                    rectInfo.Items.Add("(" + rectangle.LeftTop.X + "," + rectangle.LeftTop.Y + ") - (" +
                        rectangle.RightDown.X + "," + rectangle.RightDown.Y + ")");
                    indexOfRectangleToDisplay = i;
                }
            }
        }

        //draw complex rectangle
        private void DrawComplexRectangle(Graphics graph)
        {
            for (int i = 0; i < extractedRectangles.Count; ++i)
            {
                Taio.Rectangle rectangle = extractedRectangles[i];
                DoublePoint leftTop = ConvertRealPostionToImagePosition(rectangle.LeftTop);
                DoublePoint rightDown = ConvertRealPostionToImagePosition(rectangle.RightDown);
                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graph.FillRectangle(new SolidBrush(rectangle.Color), (int)leftTop.X, (int)rightDown.Y,
                    (int)rightDown.X - (int)leftTop.X, (int)leftTop.Y - (int)rightDown.Y);
                graph.DrawRectangle(new Pen(new SolidBrush(GetColorInversion(rectangle.Color)), 2),
                    (int)leftTop.X, (int)rightDown.Y, (int)rightDown.X - (int)leftTop.X,
                    (int)leftTop.Y - (int)rightDown.Y);
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
            axisEndX[1] = new Point(xStopPoint.X - 5, xStopPoint.Y - 5);
            axisEndX[2] = new Point(xStopPoint.X - 5, xStopPoint.Y + 5);
            axisEndY[0] = yStopPoint;
            axisEndY[1] = new Point(yStopPoint.X - 5, yStopPoint.Y + 5);
            axisEndY[2] = new Point(yStopPoint.X + 5, yStopPoint.Y + 5);
            graph.FillPolygon(axisPen.Brush, axisEndX);
            graph.FillPolygon(axisPen.Brush, axisEndY);
            //podpisy osi
            graph.DrawString("(0 ; 0)", axisTextFont, axisTextBrush, new Point(zeroPoint.X - 5, zeroPoint.Y));
            graph.DrawString("(" + (int)(maxX * scale) + " ; 0)", axisTextFont, axisTextBrush,
                new Point(xStopPoint.X - 45, xStopPoint.Y));
            graph.DrawString("(0 ; " + (int)(maxY * scale) + ")", axisTextFont, axisTextBrush,
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
            float x = mX - xBorder;
            float y = this.displayArea.Height - mY - 3 * yBorder;
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
            x = (float)(x * scale);
            y = (float)(y * scale);
            if (validPosition)
                realMousePosition = new DoublePoint(x, y);
            else
                realMousePosition = new DoublePoint();
            this.mousePosition.Text = "(X, Y) = (" + (int)x + ", " + (int)y + ")";
            return validPosition;
        }

        //converts real coordinates to image coordinates
        public DoublePoint ConvertRealPostionToImagePosition(DoublePoint realPos)
        {
            float imgX, imgY;
            imgX = (float)((int)realPos.X / scale + xBorder);
            imgY = (float)(this.Height - (int)realPos.Y / scale - 4 * yBorder);
            return new DoublePoint(imgX, imgY);
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

        [Description("Rectangle created by a user, or rectangle which should be draw on a control"),
        Category("General")]
        internal Taio.Rectangle Rectangle
        {
            get
            {
                int sideA = (int)realValue.X;
                int sideB = (int)realValue.Y;
                if (sideA == 0 || sideB == 0)
                    return null;
                return new Taio.Rectangle(sideA, sideB);
            }
            set
            {
                if (value != null)
                {
                    rectangle = null;
                    extractedRectangles = new List<Taio.Rectangle>();
                    ExtractRectangles(value);
                    SetColors();
                    int max = (this.maxX < this.maxY) ? maxX : maxY;
                    if (value.LongerSide > max * scale)
                        while (value.LongerSide > max * scale && scale <=maxScale)
                            scale *= 2;
                    if (value.LongerSide < max * scale / 2)
                        while (value.LongerSide < max * scale / 2 && scale >= minScale)
                            scale /= 2;
                    if (value.ContainedRectangles.Count == 0)
                    {
                        realValue = new DoublePoint(value.RightDown.X, value.RightDown.Y);
                        canDraw = true;
                    }
                    else
                    {
                        rectangle = value;
                        canDraw = false;
                    }
                }
            }
        }
        #endregion

        private void SetColors()
        {
            int MYSTERIOUS_VALUE = 2 << 24;
            int numberOfRectangles = extractedRectangles.Count;
            int step = (MYSTERIOUS_VALUE - 1) / (numberOfRectangles + 1);
            Random rand = new Random();
            int current = rand.Next(MYSTERIOUS_VALUE);
            for (int i = 0; i < extractedRectangles.Count; ++i, current = (current + step) % (MYSTERIOUS_VALUE))
            {
                extractedRectangles[i].Color = Color.FromArgb(alfa, (current >> 16) % 256,
                    (current >> 8) % 256, current % 256);
            }
        }

        private void ExtractRectangles(Taio.Rectangle rectangle)
        {
            if (rectangle.ContainedRectangles.Count == 0)
            {
                Taio.Rectangle rec = new Taio.Rectangle(rectangle.SideA, rectangle.SideB, rectangle.LeftTop);
                rec.Number = rectangle.Number;
                extractedRectangles.Add(rec);
            }
            else
                for (int i = 0; i < rectangle.ContainedRectangles.Count; ++i)
                    ExtractRectangles(rectangle.ContainedRectangles[i]);
        }

        private void zoomOut_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void zoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void displayArea_MouseClick(object sender, MouseEventArgs e)
        {
            if (rectangle != null && this.rectInfo.Items.Count > 0)
            {
                Taio.Rectangle rect = extractedRectangles[indexOfRectangleToDisplay];
                RectangleClicked(rect.Number);
                extractedRectangles.RemoveAt(indexOfRectangleToDisplay);
                extractedRectangles.Add(rect);
            }
        }

        public void Clear()
        {
            this.rectangle = null;
            this.realValue = new DoublePoint();
            this.Refresh();
        }

        public void ZoomIn()
        {
            if (scale >= minScale)
            {
                scale /= 2;
                if (scale == minScale)
                    this.zoomIn.Enabled = false;
                this.zoomOut.Enabled = true;
                this.Refresh();
            }
        }

        public void ZoomOut()
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

        public class DoublePoint
        {
            float x, y;
            bool isEmpty;

            public bool IsEmpty
            {
                get { return isEmpty; }
            }

            public DoublePoint()
            {
                isEmpty = true;
            }

            public DoublePoint(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            public float Y
            {
                get { return y; }
            }

            public float X
            {
                get { return x; }
            }

            public static implicit operator DoublePoint(Point f)
            {
                return new DoublePoint((float)f.X, (float)f.Y);
            }
        }
    }
}
