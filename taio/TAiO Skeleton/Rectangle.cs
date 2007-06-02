using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Taio
{
    /// <summary>
    /// Klasa reprezentuj¹ca prostok¹t
    /// </summary>
    class Rectangle : IComparable<Rectangle>
    {
        protected Point leftTop;
        protected Point rightDown;
        private List<Rectangle> containedRectangles;
        private Rectangle parentRectangle;
        private Color color;
        private int number;

        private static int counter = 1;


        #region Konstruktory
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideA">D³ugoœæ boku poziomego</param>
        /// <param name="sideB">D³ugoœæ boku pionowego</param>
        /// <param name="leftTop">Lewy górny wierzcho³ek</param>
        /// <param name="number">Numer porz¹dkowy</param>
        /// <param name="parentRectangle">Prostok¹t zawieraj¹cy nowo tworzony prostok¹t</param>
        public Rectangle(int sideA, int sideB, Point leftTop, int number, Rectangle parentRectangle)
        {
            if (sideA <= 0 || sideB <= 0)
                throw new ArgumentException("Incorrect rectangle side(s)");

            this.leftTop = new Point(leftTop.X, leftTop.Y);
            this.rightDown = new Point(leftTop.X + sideA, leftTop.Y + sideB);

            this.containedRectangles = new List<Rectangle>();
            this.number = number;
            this.parentRectangle = parentRectangle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideA">D³ugoœæ boku poziomego</param>
        /// <param name="sideB">D³ugoœæ boku pionowego</param>
        /// <param name="leftTop">Lewy górny wierzcho³ek</param>
        /// <param name="number">Numer porz¹dkowy</param>
        public Rectangle(int sideA, int sideB, Point leftTop, int number)
            : this(sideA, sideB, leftTop, number, null) { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideA">D³ugoœæ boku poziomego</param>
        /// <param name="sideB">D³ugoœæ boku pionowego</param>
        /// <param name="leftTop">Lewy górny wierzcho³ek</param>
        public Rectangle(int sideA, int sideB, Point leftTop)
            : this(sideA, sideB, leftTop, counter++) { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideA">D³ugoœæ boku poziomego</param>
        /// <param name="sideB">D³ugoœæ boku pionowego</param>
        public Rectangle(int sideA, int sideB)
            : this(sideA, sideB, new Point(0, 0)) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftTop">Lewy górny wierzcho³ek</param>
        /// <param name="rightDown">Prawy dolny wierzcho³ek</param>
        /// <param name="number">Numer porz¹dkowy</param>
        /// <param name="parentRectangle">Prostok¹t zawieraj¹cy nowo tworzony prostok¹t</param>
        public Rectangle(Point leftTop, Point rightDown, int number, Rectangle parentRectangle)
        {
            if (leftTop.X < 0 || leftTop.Y < 0)
                throw new ArgumentException("Incorrect rectangle left-top coordinates");
            if (rightDown.X < 0 || rightDown.Y < 0)
                throw new ArgumentException("Incorrect rectangle right-down coordinates");
            if (leftTop.X >= rightDown.X || leftTop.Y >= rightDown.Y)
                throw new ArgumentException("Incorrect rectangle coordinates");

            this.leftTop = new Point(leftTop.X, leftTop.Y);
            this.rightDown = new Point(rightDown.X, rightDown.Y);

            this.containedRectangles = new List<Rectangle>();
            this.parentRectangle = parentRectangle;

            this.number = number;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftTop">Lewy górny wierzcho³ek</param>
        /// <param name="rightDown">Prawy dolny wierzcho³ek</param>
        /// <param name="number">Numer porz¹dkowy</param>
        public Rectangle(Point leftTop, Point rightDown, int number)
            : this(leftTop, rightDown, number, null) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftTop">Lewy górny wierzcho³ek</param>
        /// <param name="rightDown">Prawy dolny wierzcho³ek</param>
        public Rectangle(Point leftTop, Point rightDown)
            : this(leftTop, rightDown, counter++) { }
        #endregion

        #region Geometria
        /// <summary>
        /// Czy prostok¹t pokrywa prostok¹t r
        /// </summary>
        /// <param name="r">/param>
        /// <returns></returns>
        public bool Covers(Rectangle r)
        {
            if (r.LeftTop.X >= this.leftTop.X && r.RightDown.X <= this.rightDown.X &&
                r.LeftTop.Y >= this.leftTop.Y && r.RightDown.Y <= this.rightDown.Y)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Znajduje przeciêcie dwóch prostok¹tów
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Rectangle IntersectionRect(Rectangle rect)
        {
            if (rect == null)
                throw new ArgumentNullException();

            Rectangle l, r, d, u; //left, right, down, up
            if (this.LeftTop.X <= rect.LeftTop.X)
            {
                l = this;
                r = rect;
            }
            else
            {
                l = rect;
                r = this;
            }

            if (this.LeftTop.Y <= rect.LeftTop.Y)
            {
                u = this;
                d = rect;
            }
            else
            {
                u = rect;
                d = this;
            }

            if (l.RightDown.X <= r.LeftTop.X || u.RightDown.Y <= d.LeftTop.Y)
                return null;
            if (this.Covers(rect))
                return new Rectangle(rect.LeftTop, rect.RightDown);
            if (rect.Covers(this))
                return new Rectangle(this.leftTop, this.rightDown);

            int resLeftTopX, resLeftTopY, resRightDownX, resRightDownY;
            resLeftTopX = r.LeftTop.X;
            resLeftTopY = d.LeftTop.Y;

            if (r.RightDown.X <= l.RightDown.X)
                resRightDownX = r.RightDown.X;
            else
                resRightDownX = l.RightDown.X;
            if (d.RightDown.Y <= u.RightDown.Y)
                resRightDownY = d.RightDown.Y;
            else
                resRightDownY = u.RightDown.Y;


            return new Rectangle(new Point(resLeftTopX, resLeftTopY), new Point(resRightDownX, resRightDownY));
        }

        /// <summary>
        /// Znajduje ró¿nicê prostok¹ta i prostok¹ta rect
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public List<Rectangle> Subtract(Rectangle rect)
        {
            List<Rectangle> results = new List<Rectangle>();
            Rectangle intersection = this.IntersectionRect(rect);
            if (intersection == null)
            {
                results.Add(this);
                return results;
            }

            if (rect.Covers(this))
                return results; // puste

            int resLTx, resLTy, resRDx, resRDy;

            resLTx = this.LeftTop.X;
            resLTy = this.LeftTop.Y;
            resRDx = intersection.LeftTop.X;
            resRDy = this.RightDown.Y;
            if (resRDx - resLTx > 0 && resRDy - resLTy > 0)
                results.Add(new Rectangle
                    (new Point(resLTx, resLTy), new Point(resRDx, resRDy)));

            resLTx = intersection.LeftTop.X;
            resLTy = this.LeftTop.Y;
            resRDx = intersection.RightDown.X;
            resRDy = intersection.LeftTop.Y;
            if (resRDx - resLTx > 0 && resRDy - resLTy > 0)
                results.Add(new Rectangle
                    (new Point(resLTx, resLTy), new Point(resRDx, resRDy)));

            resLTx = intersection.LeftTop.X;
            resLTy = intersection.RightDown.Y;
            resRDx = intersection.RightDown.X;
            resRDy = this.RightDown.Y;
            if (resRDx - resLTx > 0 && resRDy - resLTy > 0)
                results.Add(new Rectangle
                    (new Point(resLTx, resLTy), new Point(resRDx, resRDy)));

            resLTx = intersection.RightDown.X;
            resLTy = this.LeftTop.Y;
            resRDx = this.RightDown.X;
            resRDy = this.RightDown.Y;
            if (resRDx - resLTx > 0 && resRDy - resLTy > 0)
                results.Add(new Rectangle
                    (new Point(resLTx, resLTy), new Point(resRDx, resRDy)));

            return results;
        }
        #endregion

        #region Edycja
        /// <summary>
        /// Zmieñ rozmiary prostok¹ta (przesuñ prawy dolny wierzcho³ek, nie zmieniaj¹c lewego górnego)
        /// </summary>
        /// <param name="rightDown"></param>
        /// <returns></returns>
        public Rectangle Resize(Point rightDown)
        {
            if (rightDown.X < 0 || rightDown.Y < 0 || rightDown.X <= this.leftTop.X || rightDown.Y <= this.leftTop.Y)
                throw new ArgumentException("Incorrect right-down coordinates");

            this.rightDown = rightDown;

            return this;
        }

        /// <summary>
        /// Zmieñ rozmiary prostok¹ta (przesuñ prawy dolny wierzcho³ek, nie zmieniaj¹c lewego górnego)
        /// </summary>
        /// <param name="sideA"></param>
        /// <param name="sideB"></param>
        /// <returns>Resized rectangle</returns>
        private Rectangle Resize(int sideA, int sideB)
        {
            if (sideA <= 0 || sideB <= 0)
                throw new ArgumentException("Incorrect side");

            int rdx = this.leftTop.X + sideA;
            int rdy = this.leftTop.Y + sideB;
            this.rightDown = new Point(rdx, rdy);

            return this;
        }

        /// <summary>
        /// Obróæ prostok¹t (zamieñ sideA i sideB, nie zmieniaj¹c lewego górnego wierzcho³ka)
        /// </summary>
        /// <returns>Rotated rectangle</returns>
        public Rectangle Rotate()
        {
            //Resize(new Point(this.leftTop.X + this.SideB, this.leftTop.Y + this.SideA));

            Point oldLT = new Point(leftTop.X, leftTop.Y);
            Move(new Point(0, 0));
            Resize(new Point(rightDown.Y, rightDown.X));
            Move(oldLT);

            foreach (Rectangle r in containedRectangles)
                r.RotateChild();

            return this;
        }

        private void RotateChild()
        {
            Point transp = new Point(rightDown.Y, leftTop.X);
            Move(new Point(0, 0));
            Point nLT = new Point(-RightDown.Y, LeftTop.X);
            Point nRD = new Point(-LeftTop.Y, RightDown.X);

            nLT.X += transp.X;
            nLT.Y += transp.Y;

            nRD.X += transp.X;
            nRD.Y += transp.Y;

            Move(nLT);
            Resize(nRD);

            foreach (Rectangle r in containedRectangles)
                r.RotateChild();
        }

        /// <summary>
        /// Przesuñ prostok¹t
        /// </summary>
        /// <param name="leftTop"></param>
        /// <returns></returns>
        public Rectangle Move(Point leftTop)
        {
            if (leftTop.X < 0 || leftTop.Y < 0)
                throw new ArgumentException("Incorrect left-top vertex coordinates");

            int xTransposition = leftTop.X - this.leftTop.X;
            int yTransposition = leftTop.Y - this.leftTop.Y;

            this.leftTop = leftTop;
            this.rightDown.X += xTransposition;
            this.rightDown.Y += yTransposition;

            return this;
        }

        /// <summary>
        /// Ustaw prostok¹t-rodzica
        /// </summary>
        /// <param name="parentRectangle"></param>
        public void SetParentRectangle(Rectangle parentRectangle)
        {
            this.parentRectangle = parentRectangle;
            parentRectangle.containedRectangles.Add(this);
        }
        #endregion

        #region Akcesory
        /// <summary>
        /// Lewy górny wierzcho³ek
        /// </summary>
        public Point LeftTop
        {
            get { return leftTop; }
            //set { leftTop = value; }
        }

        /// <summary>
        /// Prawy dolny wierzcho³ek
        /// </summary>
        public Point RightDown
        {
            get { return rightDown; }
            //set { rightDown = value; }
        }

        /// <summary>
        /// Bok "poziomy"
        /// </summary>
        public int SideA
        {
            get { return this.rightDown.X - this.leftTop.X; ; }
        }

        /// <summary>
        /// Bok "pionowy"
        /// </summary>
        public int SideB
        {
            get { return this.rightDown.Y - this.leftTop.Y; ; }
        }

        /// <summary>
        /// D³u¿szy bok
        /// </summary>
        public int LongerSide
        {
            get
            {
                if (SideA >= SideB)
                    return SideA;
                else
                    return SideB;
            }
        }

        /// <summary>
        /// Krótszy bok
        /// </summary>
        public int ShorterSide
        {
            get
            {
                if (SideA >= SideB)
                    return SideB;
                else
                    return SideA;
            }
        }

        /// <summary>
        /// Pole
        /// </summary>
        public int Area
        {
            get { return SideA * SideB; }
        }

        /// <summary>
        /// Zawierane prostok¹ty
        /// </summary>
        public List<Rectangle> ContainedRectangles
        {
            get { return containedRectangles; }
        }

        /// <summary>
        /// Prostok¹t-rodzic
        /// </summary>
        public Rectangle ParentRectangle
        {
            get { return parentRectangle; }
        }

        /// <summary>
        /// Kolor
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Numer
        /// </summary>
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public Orientation RectangleOrientation
        {
            get
            {
                if (SideA >= SideB)
                    return Orientation.Horizontal;
                else
                    return Orientation.Vertical;
            }
        }
        #endregion

        #region Porównywanie

        public int CompareTo(Rectangle rhs)
        {
            return this.number.CompareTo(rhs.number);
        }

        public int CompareTo(Rectangle rhs, RectangleComparer.ComparisonType compType)
        {
            switch (compType)
            {
                case RectangleComparer.ComparisonType.Area:
                    return this.Area.CompareTo(rhs.Area);
                case RectangleComparer.ComparisonType.LongerSide:
                    return this.LongerSide.CompareTo(rhs.LongerSide);
                case RectangleComparer.ComparisonType.Number:
                    return this.Number.CompareTo(rhs.Number);
            }
            return 0;
        }

        public static RectangleComparer GetComparer()
        {
            return new RectangleComparer();
        }

        public class RectangleComparer : IComparer<Rectangle>
        {
            private Rectangle.RectangleComparer.ComparisonType comparison;

            public enum ComparisonType
            {
                Area,
                LongerSide,
                Number
            };

            public int GetHashCode(Rectangle r)
            {
                return r.GetHashCode();
            }

            public int Compare(Rectangle lhs, Rectangle rhs)
            {
                return lhs.CompareTo(rhs, this.comparison);
            }

            public bool Equals(Rectangle lhs, Rectangle rhs)
            {
                return this.Compare(lhs, rhs) == 0;
            }

            public Rectangle.RectangleComparer.ComparisonType Comparison
            {
                get
                {
                    return comparison;
                }

                set
                {
                    comparison = value;
                }
            }
        }

        #endregion

        public override String ToString()
        {
            return "#" + this.number + ": " + this.LeftTop + ", " + this.RightDown + "; area: " + this.Area;
        }

        /// <summary>
        /// Orientacja prostok¹ta
        /// </summary>
        public enum Orientation
        {
            /// <summary>
            /// SideA >= SideB
            /// </summary>
            Horizontal,
            /// <summary>
            /// SideB > SideB
            /// </summary>
            Vertical
        }


    }
}