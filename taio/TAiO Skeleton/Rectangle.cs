using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Taio
{
    class Rectangle
    {
        private Point leftTop;
        private Point rightDown;
        //private int sideA;
        //private int sideB;
        private List<Rectangle> containedRectangles;
        private Rectangle parentRectangle;
        private Color color;
        private int number;
        private static int counter = 1;


        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideA">"Top/bottom" side's length</param>
        /// <param name="sideB">"Left/right" side's length</param>
        /// <param name="leftTop">Left-top rectangle's vertex</param>
        /// <param name="parentRectangle">Rectangle which contains new rectangle</param>
        public Rectangle(int sideA, int sideB, Point leftTop, Rectangle parentRectangle)
        {
            if (sideA <= 0 || sideB <= 0)
                throw new ArgumentException("Incorrect rectangle side(s)");

            this.leftTop = new Point(leftTop.X, leftTop.Y);
            this.rightDown = new Point(leftTop.X + sideA, leftTop.Y + sideB);

            //this.sideA = rightDown.X - leftTop.X;
            //this.sideB = rightDown.Y - leftTop.Y;

            this.containedRectangles = new List<Rectangle>();
            this.parentRectangle = parentRectangle;

            this.number = counter++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideA">"Top/bottom" side's length</param>
        /// <param name="sideB">"Left/right" side's length</param>
        /// <param name="leftTop">LeftTop rectangle's vertex</param>
        public Rectangle(int sideA, int sideB, Point leftTop)
            : this(sideA, sideB, leftTop, null) { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideA">"Top/bottom" side's length</param>
        /// <param name="sideB">"Left/right" side's length</param>
        public Rectangle(int sideA, int sideB) 
            : this(sideA, sideB, new Point(0, 0)) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftTop">Left-top rectangle's vertex</param>
        /// <param name="rightDown">Right-down rectangle's vertex</param>
        /// <param name="parentRectangle">Rectangle which contains new rectangle</param>
        public Rectangle(Point leftTop, Point rightDown, Rectangle parentRectangle)
        {
            if (leftTop.X < 0 || leftTop.Y < 0)
                throw new ArgumentException("Incorrect rectangle left-top coordinates");
            if (rightDown.X < 0 || rightDown.Y < 0)
                throw new ArgumentException("Incorrect rectangle right-down coordinates");
            if(leftTop.X >= rightDown.X || leftTop.Y >= rightDown.Y)
                throw new ArgumentException("Incorrect rectangle coordinates");

            this.leftTop = new Point(leftTop.X, leftTop.Y);
            this.rightDown = new Point(rightDown.X, rightDown.Y);

            //this.sideA = rightDown.X - leftTop.X;
            //this.sideB = rightDown.Y - leftTop.Y;

            this.containedRectangles = new List<Rectangle>();
            this.parentRectangle = parentRectangle;

            this.number = counter++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftTop">Left-top rectangle's vertex</param>
        /// <param name="rightDown">Right-down rectangle's vertex</param>
        public Rectangle(Point leftTop, Point rightDown)
            : this(leftTop, rightDown, null) { }
        #endregion

        #region Geometry
        /// <summary>
        /// Whether rectangle covers the rectangle r
        /// </summary>
        /// <param name="r">Rectangle to check</param>
        /// <returns>True if the rectangle r is covered, else if it does not</returns>
        public bool Covers(Rectangle r)
        {
            if (r.LeftTop.X >= this.leftTop.X && r.RightDown.X <= this.rightDown.X &&
                r.LeftTop.Y >= this.leftTop.Y && r.RightDown.Y <= this.rightDown.Y)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Find the intersection of two rectangles
        /// </summary>
        /// <param name="rect">Rectangle to intersect</param>
        /// <returns>Intersection rectangle or null if intersection is empty</returns>
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
        /// Find the subtraction of two rectangles
        /// </summary>
        /// <param name="rect">Rectangle to subtract</param>
        /// <returns>List of rectangles from subtraction</returns>
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

        #region Edition
        /// <summary>
        /// Resize rectangle (move right-down vertex, left-top vertex remains unchanged)
        /// </summary>
        /// <param name="rightDown">New right-down vertex coordinates</param>
        /// <returns>Resized rectangle</returns>
        public Rectangle Resize(Point rightDown)
        {
            if (rightDown.X < 0 || rightDown.Y < 0 || rightDown.X <= this.leftTop.X || rightDown.Y <= this.leftTop.Y)
                throw new ArgumentException("Incorrect right-down coordinates");

            this.rightDown = rightDown;
            //this.sideA = this.rightDown.X - this.leftTop.X;
            //this.sideB = this.rightDown.Y - this.leftTop.Y;

            return this;
        }

        /// <summary>
        /// Resize rectangle (move right-down vertex, left-top vertex remains unchanged)
        /// </summary>
        /// <param name="sideA">New sideA length</param>
        /// <param name="sideB">New sideB length</param>
        /// <returns>Resized rectangle</returns>
        public Rectangle Resize(int sideA, int sideB)
        {
            if (sideA <= 0 || sideB <= 0)
                throw new ArgumentException("Incorrect side");

            int rdx = this.leftTop.X + sideA;
            int rdy = this.leftTop.Y + sideB;
            this.rightDown = new Point(rdx, rdy);

            //this.sideA = sideA;
            //this.sideB = sideB;

            return this;
        }

        /// <summary>
        /// Rotate rectangle (swap sideA and sideB, leftTop remains unchanged)
        /// </summary>
        /// <returns>Rotated rectangle</returns>
        public Rectangle Rotate()
        {
            Resize(new Point(this.leftTop.X + this.SideB, this.leftTop.Y + this.SideA));
            return this;
        }

        /// <summary>
        /// Moves rectangle so that the left-top vertices lay in the given leftTop
        /// </summary>
        /// <param name="leftTop">New leftTop position</param>
        /// <returns>Moved rectangle</returns>
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
        /// Set parent for the rectangle
        /// </summary>
        /// <param name="parentRectangle">New parentRectangle</param>
        public void SetParentRectangle(Rectangle parentRectangle)
        {
            this.parentRectangle = parentRectangle;
            parentRectangle.containedRectangles.Add(this);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Rectangle's left-top vertex
        /// </summary>
        public Point LeftTop
        {
            get { return leftTop; }
            //set { leftTop = value; }
        }

        /// <summary>
        /// Rectangle's right-down vertex
        /// </summary>
        public Point RightDown
        {
            get { return rightDown; }
            //set { rightDown = value; }
        }

        /// <summary>
        /// Rectangle's top/bottom side length
        /// </summary>
        public int SideA
        {
            get { return this.rightDown.X - this.leftTop.X; ; }
        }

        /// <summary>
        /// Rectangle's left/right side length
        /// </summary>
        public int SideB
        {
            get { return this.rightDown.Y - this.leftTop.Y; ; }
        }

        /// <summary>
        /// Rectangle's longer side length
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
        /// Rectangle's shorter side length
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
        /// Rectangle area
        /// </summary>
        public int Area
        {
            get { return SideA * SideB; }
        }

        /// <summary>
        /// Contained rectangles list
        /// </summary>
        public List<Rectangle> ContainedRectangles
        {
            get { return containedRectangles; }
        }

        /// <summary>
        /// Rectangle containing given rectangle
        /// </summary>
        public Rectangle ParentRectangle
        {
            get { return parentRectangle; }
        }

        /// <summary>
        /// Rectangle's color
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Rectangle's number
        /// </summary>
        public int Number
        {
            get { return number; }
        }
	
        #endregion


        public override String ToString()
        {
            return /*"#"+this.number + ": "+ */ this.LeftTop + ", " + this.RightDown + "; area: " + this.Area;
        }

        /// <summary>
        /// Rectangle's orientation
        /// </summary>
        public enum Orientation
        { 
            /// <summary>
            /// Rectangle's SideA is not shorten than SideB
            /// </summary>
            Horizontal,
            /// <summary>
            /// Rectangle's SideB is not shorten than SideB
            /// </summary>
            Vertical
        }
    }
}

#region arch
/*public void ChangeShorterSide(int newSide)
        {
            if (newSide <= 0)
                throw new ArgumentException("Incorrect rectangle side");

            if (newSide > sideA)
            {
                sideB = sideA;
                sideA = newSide;
            }
            else
                this.sideB = newSide;
        }

        public void ChangeLongerSide(int newSide)
        {
            if (newSide <= 0)
                throw new ArgumentException("Incorrect rectangle side");

            if (newSide < sideB)
            {
                sideA = sideB;
                sideB = newSide;
            }
            else
                sideA = newSide;
        }
 * 
 * public Rectangle(int sideA, int sideB, Rectangle fatherRectangle)
        {
            if (sideA <= 0 || sideB <= 0)
                throw new ArgumentException("Incorrect rectangle side(s)");

            if (sideA >= sideB)
            {
                this.sideA = sideA;
                this.sideB = sideB;
            }
            else
            {
                this.sideA = sideB;
                this.sideB = sideA;
            }

            this.leftTop = new Point(0,0);
            this.rightDown = new Point(sideA, sideB);

            this.containedRectangles = new List<Rectangle>();
            this.fatherRectangle = fatherRectangle;
        }
 */
#endregion