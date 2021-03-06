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
        //prawy gorny rog prostokata
        private DoublePoint realValue = new DoublePoint();
        //rzeczywist wspolrzedna myszki
        private DoublePoint realMousePosition = new DoublePoint();
        //prostokat do wysietlenia
        private Taio.Rectangle rectangle;
        //lista prostokatow skladajaca sie na prostokat zlozony
        private List<Taio.Rectangle> extractedRectangles;
        private int maxX, maxY;
        private double scale = 0.25;
        private double minScale = 0.125 / 4;
        //czy mozna rysowac
        private bool canDraw = true;
        private Pen axisPen = new Pen(Brushes.Black, 2);
        private Brush axisTextBrush = Brushes.Black;
        private Font axisTextFont;
        private Color backgroundColor = Color.White;
        private int xBorder = 5;
        private int yBorder = 5;
        private int zoomControlSize;
        private int yDrawingStartValue;
        private int xDrawingStartValue;
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

        #region rect_info
        //probuje popbrac informacje o prostokacie wskazywanym przez kursor
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

        //probuje popbrac informacje o prostokacie ze zlozonoego prostokata wskazywanego przez kursor
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
        #endregion

        #region drawing
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

        //rysuje prostokat
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

        //rysuje zlozony prostokat
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
        #endregion

        //wyswietla wspolrzedne myszki sprawdzajac czy mieszcza sie na wykresie
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

        //konweruje rzeczywiste wspolrzedne myszki do wspolrzednych kontrolki
        private DoublePoint ConvertRealPostionToImagePosition(DoublePoint realPos)
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
                    Rescale(value.LongerSide);
                    if (value.ContainedRectangles.Count == 0)
                    {
                        realValue = new DoublePoint(value.RightDown.X, value.RightDown.Y);
                        this.xTextBox.Enabled = true;
                        this.yTextBox.Enabled = true;
                        canDraw = true;
                    }
                    else
                    {
                        rectangle = value;
                        this.xTextBox.Enabled = false;
                        this.yTextBox.Enabled = false;
                        canDraw = false;
                    }
                }
            }
        }
        #endregion

        //przeskalowuje prostokat aby miescil sie na ekranie
        private void Rescale(int maxSide)
        {
            if (this.autoScaleCheckBox.Checked)
            {
                int max = (this.maxX < this.maxY) ? maxX : maxY;
                if (maxSide > max * scale)
                    while (maxSide > max * scale && scale <= maxScale)
                        scale *= 2;
                if (maxSide < max * scale / 2)
                    while (maxSide < max * scale / 2 && scale >= minScale)
                        scale /= 2;
                if (scale > minScale)
                    this.zoomOut.Enabled = true;
                if (scale < maxScale)
                    this.zoomIn.Enabled = true;
            }
        }

        //dobiera kolory dla prostokatow
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

        //seeks complex rectangle and extracts all simple rectangles
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

        #region public_methods
        //ustawia wybrany prostokat na przodzie
        public void SelectRectangleByNumber(int number)
        {
            if (extractedRectangles != null)
            {
                int index = -1;
                for (int i = 0; i < extractedRectangles.Count; ++i)
                    if (extractedRectangles[i].Number == number)
                    {
                        index = i;
                        break;
                    }
                if (index != -1)
                {
                    Taio.Rectangle rect = extractedRectangles[index];
                    extractedRectangles.RemoveAt(index);
                    extractedRectangles.Add(rect);
                }
            }
        }

        public void ChangeColor()
        {
            this.label1.BackColor = Taio.Properties.Settings.Default.color;
            this.label2.BackColor = Taio.Properties.Settings.Default.color;
            this.label3.BackColor = Taio.Properties.Settings.Default.color;
        }

        //ustawia ustawienia kontrolki na domyslne
        public void Clear()
        {
            this.rectangle = null;
            this.realValue = new DoublePoint();
            this.canDraw = true;
            this.xTextBox.Enabled = true;
            this.yTextBox.Enabled = true;
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
        #endregion

        #region DoublePoint
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
        #endregion

        #region events
        private void displayArea_MouseMove(object sender, MouseEventArgs e)
        {
            bool validMousePos = DisplayMousePosition(e.X, e.Y);
            if (validMousePos && e.Button == MouseButtons.Left && canDraw)
            {
                this.realValue = realMousePosition;
                this.xTextBox.Text = ((int)realValue.X).ToString();
                this.yTextBox.Text = ((int)realValue.Y).ToString();
            }
            TrySetRectangleInfo();
            this.Refresh();
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

        private void autoScaleCheckBox_Click(object sender, EventArgs e)
        {
            if (this.autoScaleCheckBox.Checked)
            {
                if (rectangle != null)
                    Rescale(rectangle.LongerSide);
                else
                    Rescale((int)Math.Max(this.realValue.X, this.realValue.Y));
            }
        }

        private void xTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateTextBoxes();
        }

        private void ValidateTextBoxes()
        {
            int val;
            if (Int32.TryParse(xTextBox.Text, out val) && val > 0)
            {
                this.realValue = new DoublePoint(val, realValue.Y);
                int max = (int)Math.Max(realValue.X, realValue.Y);
                this.Rescale(max);
                this.Refresh();
            }
            if (Int32.TryParse(yTextBox.Text, out val) && val > 0)
            {
                this.realValue = new DoublePoint(realValue.X, val);
                int max = (int)Math.Max(realValue.X, realValue.Y);
                this.Rescale(max);
                this.Refresh();

            }
        }

        private void yTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateTextBoxes();
        }
        #endregion
    }
}
