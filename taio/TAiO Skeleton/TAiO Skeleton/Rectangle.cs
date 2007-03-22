using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace TAiO_Rectangles
{
    class Rectangle
    {
        private Point leftTop;
        private Point rightDown;
        private int sideA;
        private int sideB;
        private List<Rectangle> containedRectangles;
        private Rectangle fatherRectangle;

        #region Constructors
        public Rectangle(int sideA, int sideB, Point leftTop, Rectangle fatherRectangle)
        {
            if (sideA <= 0 || sideB <= 0)
                throw new ArgumentException("Incorrect rectangle side(s)");

            this.leftTop = new Point(leftTop.X, leftTop.Y);
            this.rightDown = new Point(leftTop.X + sideA, leftTop.Y + sideB);

            this.sideA = rightDown.X - leftTop.X;
            this.sideB = rightDown.Y - leftTop.Y;

            this.containedRectangles = new List<Rectangle>();
            this.fatherRectangle = fatherRectangle;
        }

        public Rectangle(int sideA, int sideB, Point leftTop)
            : this(sideA, sideB, leftTop, null) { }

        public Rectangle(int sideA, int sideB) 
            : this(sideA, sideB, new Point(0, 0)) { }

        public Rectangle(Point leftTop, Point rightDown, Rectangle fatherRectangle)
        {
            if (leftTop.X < 0 || leftTop.Y < 0)
                throw new ArgumentException("Incorrect rectangle left-top coordinates");
            if (rightDown.X < 0 || rightDown.Y < 0)
                throw new ArgumentException("Incorrect rectangle right-down coordinates");
            if(leftTop.X >= rightDown.X || leftTop.Y >= rightDown.Y)
                throw new ArgumentException("Incorrect rectangle coordinates");

            this.leftTop = new Point(leftTop.X, leftTop.Y);
            this.rightDown = new Point(rightDown.X, rightDown.Y);

            this.sideA = rightDown.X - leftTop.X;
            this.sideB = rightDown.Y - leftTop.Y;

            this.containedRectangles = new List<Rectangle>();
            this.fatherRectangle = fatherRectangle;
        }

        public Rectangle(Point leftTop, Point rightDown)
            : this(leftTop, rightDown, null) { }
        #endregion

        public bool Covers(Rectangle r)
        {
            if (r.LeftTop.X >= this.leftTop.X && r.RightDown.X <= this.rightDown.X &&
                r.LeftTop.Y >= this.leftTop.Y && r.RightDown.Y <= this.rightDown.Y)
                return true;
            else
                return false;
        }

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
            if(resRDx - resLTx >0 && resRDy- resLTy>0)
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

        public override String ToString()
        {
            return this.LeftTop + ", " + this.RightDown + "; area: " + this.Area;
        }

        #region Accessors
        public Point LeftTop
        {
            get { return leftTop; }
            set { leftTop = value; }
        }

        public Point RightDown
        {
            get { return rightDown; }
            set { rightDown = value; }
        }

        public int SideA
        {
            set { sideA = value; }
            get { return sideA; }
        }

        public int SideB
        {
            set { sideB = value; }
            get { return sideB; }
        }

        public int LongerSide
        {
            get
            {
                if (sideA >= sideB)
                    return sideA;
                else
                    return sideB;
            }
        }

        public int ShorterSide
        {
            get
            {
                if (sideA >= sideB)
                    return sideB;
                else
                    return sideA;
            }
        }

        public int Area
        {
            get { return sideA * sideB; }
        }

        public List<Rectangle> ContainedRectangles
        {
            get { return containedRectangles; }
        }

        public Rectangle FatherRectangle
        {
            get { return fatherRectangle; }
            set { fatherRectangle = value; }
        } 
        #endregion

        public enum Orientation { Horizontal, Vertical }
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