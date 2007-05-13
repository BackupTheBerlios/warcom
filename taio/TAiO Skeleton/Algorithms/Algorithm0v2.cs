using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

namespace Taio.Algorithms
{
    class Algorithm0v2 : IAlgorithm
    {
        private string tag = "AW02";
        private Rectangle rect;
        private bool stop;

        public Rectangle ComputeMaximumRectangle(List<Rectangle> rectangles)
        {
            try
            {
                this.rect = this.ComputeRectangles(rectangles);
            }
            catch (Exception) { }
            return this.rect;
        }

        private Rectangle ComputeRectangles(List<Rectangle> rectangles)
        {
            if (rectangles == null)
                throw new ArgumentNullException("Illegal argument");
            if (rectangles.Count == 0)
                throw new ArgumentException("Illegal argument");
            List<Rectangle> rects = new List<Rectangle>();
            foreach (Rectangle r in rectangles)
                rects.Add(r);
            int maxArea = this.ComputeMaximumArea(rects);
            Rectangle maxRect = this.FindGoodRectangleWithMaxArea(rects);
            while (!stop && maxArea > 0)
            {
                List<SetCoverEntry> tab = this.ReductionTask(ref maxArea, rects);
                if (maxRect != null && maxArea <= maxRect.Area)
                {
                    this.rect = maxRect;
                    return this.rect;
                }
                for (int i = 0; i < tab.Count; ++i)
                {
                    SetCoverEntry s = tab[i];
                    this.rect = this.SetCover(s.LongerSide, s.ShorterSide, s.Rects);
                    if (this.rect != null)
                        return this.rect;
                }
                --maxArea;
            }
            this.rect = maxRect;
            return this.rect;
        }

        //Tutaj wielka dupa wydajnoœciowa:)
        private Rectangle SetCover(int a, int b, List<Rectangle> rects)
        {
            Rectangle.RectangleComparer comp = new Rectangle.RectangleComparer();
            comp.Comparison = Rectangle.RectangleComparer.ComparisonType.Area;
            rects.Sort(comp);
            rects.Reverse();
            bool flag = true;
            this.rect = null;
            AlgRectangleContaioner arc = new AlgRectangleContaioner(a, b);
            List<RectangleData> rds = this.StartSequence(rects, a, b, arc);
            RectangleData rd = rds[rds.Count - 1];
            while (flag && !this.stop)
            {
                if ((this.rect = this.CheckEnd(rds, a * b, arc)) != null)
                    return this.rect;
                if (!arc.Move(rd) && (this.rect = this.CheckEnd(rds, a * b, arc)) == null)
                {
                    if (!this.NewSequence(rds, arc, a, b))
                    {
                        this.rect = null;
                        return this.rect;
                    }
                }
                else
                    if(this.rect!=null)
                        return this.rect;
            }
            return this.rect;
        }

        private List<RectangleData> StartSequence(List<Rectangle> rects, int a, int b,
            AlgRectangleContaioner arc)
        {
            List<RectangleData> rds = new List<RectangleData>();
            for (int i = 0; i < rects.Count; ++i)
            {
                rects[i] = rects[i].Move(new Point(0, 0));
                if (rects[i].SideA < rects[i].SideB)
                    rects[i] = rects[i].Rotate();
                rds.Add(new RectangleData(rects[i]));
                arc.InsertInGoodPosition(rds[i]);
            }
            return rds;
        }

        private bool NewSequence(List<RectangleData> rds,
            AlgRectangleContaioner arc, int a, int b)
        {
            for (int i = rds.Count - 2; i >= 0; --i)
            {
                arc.Remove(rds[i + 1]);
                if (!arc.Move(rds[i]))
                    continue;
                
                for (int j = i + 1; j < rds.Count; ++j)
                {
                    rds[j].Point = new Point(0, 0);
                    rds[j].Rot = true;
                    rds[j].Rect = rds[j].Rect.Move(new Point(0, 0));
                    arc.InsertInGoodPosition(rds[j]);
                }
                return true;
            }
            return false;
        }

        private Rectangle CheckEnd(List<RectangleData> rds, int area, AlgRectangleContaioner arc)
        {
            if (arc.EmptyField > 0)
                return null;
            int num00 = 0;
            for (; num00 < rds.Count; ++num00)
                if (rds[num00].Used && rds[num00].Rect.LeftTop.X == 0 && rds[num00].Rect.LeftTop.Y == 0)
                    break;
            if (num00 == rds.Count)
                return null;
            RectangleContainer rc = new RectangleContainer();
            rc.InsertRectangle(rds[num00].Rect);
            for (int i = 0; i < rds.Count; ++i)
                if (rds[i].Used && i != num00)
                    rc.InsertRectangle(rds[i].Rect);
            if (rc.IsCorrectRectangle && rc.MaxCorrectRect.Area == area)
            {
                this.rect = rc.MaxCorrectRect;
                return this.rect;
            }
            return null;
        }

        private RectangleData CheckOutOfBand(RectangleData rd, int a, int b)
        {
            try
            {
                if (a < rd.Rect.RightDown.X)
                    rd.Rect = rd.Rect.Move(new Point(a - rd.Rect.SideA, rd.Rect.LeftTop.Y));
                if (b < rd.Rect.RightDown.Y)
                    rd.Rect = rd.Rect.Move(new Point(rd.Rect.LeftTop.X, b - rd.Rect.SideB));
            }
            catch (ArgumentException)
            {
                rd.Used = false;
                return rd;
            }
            rd.Used = true;
            return rd;
        }

        #region zmniejszenie rozmiarów zadania
        private List<SetCoverEntry> ReductionTask(ref int maxArea, List<Rectangle> rects)
        {
            List<SetCoverEntry> tab = new List<SetCoverEntry>();
            bool flag = true;
            while (flag)
            {
                int maxSide = (int)Math.Sqrt(2 * maxArea);
                for (int i = 0; i < rects.Count; ++i)
                {
                    Rectangle r = rects[i];
                    if (r.LongerSide > maxSide)
                    {
                        rects.RemoveAt(i);
                        --i;
                    }
                }
                List<SetCoverEntry> sides = this.ComputeSides(maxArea);
                int tmpMaxArea = 0;
                tab.Clear();
                foreach (SetCoverEntry s in sides)
                {
                    foreach (Rectangle r in rects)
                        if (r.LongerSide <= s.LongerSide && r.ShorterSide <= s.ShorterSide)
                        {
                            s.Rects.Add(r);
                            s.TmpArea += r.Area;
                        }
                    tmpMaxArea = (tmpMaxArea >= s.TmpArea ? tmpMaxArea : s.TmpArea);
                }
                if (tmpMaxArea < maxArea)
                    --maxArea;
                else
                {
                    for (int i = 0; i < sides.Count; ++i)
                    {
                        if (sides[i].TmpArea < maxArea)
                        {
                            sides.RemoveAt(i);
                            --i;
                        }
                    }
                    flag = false;
                    return sides;
                }
            }
            return tab;
        }

        private List<SetCoverEntry> ComputeSides(int maxArea)
        {
            List<SetCoverEntry> sides = new List<SetCoverEntry>();
            if (maxArea < 1)
                return sides;
            else
                if (maxArea == 1)
                {
                    sides.Add(new SetCoverEntry(1, 1));
                    return sides;
                }
            int max;
            List<IntPair> tab = this.ComputeFactors(maxArea, out max);
            int maxSide = (int)Math.Sqrt(2 * maxArea), minSide = (int)Math.Sqrt(((double)maxArea) / 2.0);
            for (int i = (int)Math.Pow(2, tab.Count - 1) - 1; i >= 0; --i)
            {
                int fact = tab[tab.Count - 1].First;
                for (int j = 0; j < tab.Count - 1; ++j)
                    if ((i >> j & 1) == 1)
                        fact *= tab[j].First;
                if (fact >= minSide && fact <= maxSide && 
                    (((double)fact*fact)/maxArea)<=2.0 &&
                    ((double)maxArea/(fact*fact))<=2.0 )
                    sides.Add(new SetCoverEntry(fact, maxArea / fact));
            }
            for (int i = 0; i < sides.Count; ++i)
                for (int j = i + 1; j < sides.Count; ++j)
                    if (sides[i].A == sides[j].A || sides[i].A == sides[j].B)
                        sides.RemoveAt(j--);
            return sides;
        }

        private List<IntPair> ComputeFactors(int n, out int p)
        {
            p = 2;
            int g = (int)Math.Sqrt(n);
            List<IntPair> tab = new List<IntPair>();
            while (p <= g)
            {
                //if (n % p == 0)
                //    tab.Add(new IntPair(p, 0));
                while (n % p == 0)
                {
                    n /= p;
                    //tab[tab.Count-1].Second++;
                    tab.Add(new IntPair(p, 1));
                }
                if (n > 1)
                    ++p;
                else
                    break;
            }
            if (n > 1)
            {
                p = n;
                tab.Add(new IntPair(n, 1));
            }
            return tab;
        }
        #endregion

        private int ComputeMaximumArea(List<Rectangle> rects)
        {
            int area = 0;
            foreach (Rectangle rect in rects)
                area += rect.Area;
            return area;
        }

        private Rectangle FindGoodRectangleWithMaxArea(List<Rectangle> rects)
        {
            int max = -1;
            int index = -1;
            for (int i = 0; i < rects.Count; ++i)
            {
                if (rects[i].Area > max && rects[i].LongerSide <= 2 * rects[i].ShorterSide)
                {
                    max = rects[i].Area;
                    index = i;
                }
            }
            if (index == -1)
                return null;
            Rectangle rect = rects[index];
            return rect;
        }

        public void StopThread()
        { stop = true; }
        public Rectangle GetRectangle()
        { return rect; }
        public string GetTag()
        { return tag; }




        #region IntPair Class
        private class IntPair
        {
            int first;
            int second;

            public IntPair()
            {
                first = second = 0;
            }

            public IntPair(int first, int second)
            {
                this.first = first;
                this.second = second;
            }

            public int First
            {
                get { return first; }
                set { first = value; }
            }

            public int Second
            {
                get { return second; }
                set { second = value; }
            }

        }
        #endregion

        #region SetCoverEntry Class
        private class SetCoverEntry
        {
            private int a;
            private int b;
            private int tmpArea;
            private List<Rectangle> rects = new List<Rectangle>();

            public SetCoverEntry()
            {
                a = b = 0;
            }

            public SetCoverEntry(int a, int b)
            {
                this.a = a;
                this.b = b;
            }

            public int TmpArea
            {
                get { return tmpArea; }
                set { tmpArea = value; }
            }

            public int A
            {
                get { return a; }
                set { a = value; }
            }
            public int B
            {
                get { return b; }
                set { b = value; }
            }

            public int LongerSide
            {
                get { return a >= b ? a : b; }
            }

            public int ShorterSide
            {
                get { return a <= b ? a : b; }
            }

            public List<Rectangle> Rects
            {
                get { return rects; }
                set { rects = value; }
            }

            public int MaxArea
            {
                get { return a * b; }
            }

        }
        #endregion

        #region RectangleData Class
        private class RectangleData
        {
            private bool rot = true;
            private bool used;
            private Point point = new Point(0, 0);
            private Rectangle rect;

            public RectangleData(Rectangle rect)
            {
                this.used = false;
                this.rot = true;
                this.point = new Point(0, 0);
                this.rect = rect;
            }

            public Rectangle Rect
            {
                get { return rect; }
                set { rect = value; }
            }

            public bool Used
            {
                get { return used; }
                set { used = value; }
            }

            public bool Rot
            {
                get { return rot; }
                set { rot = value; }
            }

            public Point Point
            {
                get { return point; }
                set { point = value; }
            }

        }
        #endregion

        #region RectangleComparator
        private class RectangleCompartator : IComparer<Rectangle>
        {
            public int Compare(Rectangle r1, Rectangle r2)
            {
                if (r1 == null)
                    return -1;
                if (r2 == null)
                    return 1;
                if (r1.LeftTop.X < r2.LeftTop.X)
                    return -1;
                if (r1.LeftTop.X > r2.LeftTop.X)
                    return 1;
                if (r1.LeftTop.Y < r2.LeftTop.Y)
                    return -1;
                if (r1.LeftTop.Y > r2.LeftTop.Y)
                    return 1;
                return 0;
            }

        }
        #endregion

        #region Inna wersja:)
        /*
        #region AlgRectangle Class
        private class AlgRectangle : Rectangle
        {
            private bool rot = true;
            private bool used;
            private Point point = new Point(0, 0);

            public AlgRectangle(Rectangle rect) : base(rect.SideA, rect.SideB, rect.LeftTop)
            {
            }

            public bool Used
            {
                get { return used; }
                set { used = value; }
            }

            public bool Rot
            {
                get { return rot; }
                set { rot = value; }
            }

            public Point Point
            {
                get { return point; }
                set { point = value; }
            }

            public AlgRectangle AlgRotate()
            {
                this.rightDown = new Point(this.leftTop.X + this.SideB, this.leftTop.Y + this.SideA);
                return this;
            }

            public AlgRectangle AlgMove(Point leftTop)
            {
//                if (leftTop.X < 0 || leftTop.Y < 0)
//                    throw new ArgumentException("Incorrect left-top vertex coordinates");

                int xTransposition = leftTop.X - this.leftTop.X;
                int yTransposition = leftTop.Y - this.leftTop.Y;
                this.leftTop = leftTop;
                this.rightDown.X += xTransposition;
                this.rightDown.Y += yTransposition;
                return this;
            }

            public AlgRectangle AlgMove(int x, int y)
            {
                this.leftTop.X += x;
                this.rightDown.X += x;
                this.leftTop.Y += y;
                this.rightDown.Y += y;
                return this;
            }
        }
        #endregion

         * */
        #endregion

        #region AlgRectangleContainer Class
        private class AlgRectangleContaioner
        {
            private int[][] tab;
            private int emptyField;

            public AlgRectangleContaioner(int a, int b)
            {
                this.tab = new int[b][];
                for (int i = 0; i < b; ++i)
                    this.tab[i] = new int[a];
                this.emptyField = a * b;
            }

            public bool InsertRectangle(List<RectangleData> rects)
            {
                bool flag =true;
                for (int i = 0; i < rects.Count; ++i)
                    if (!(rects[i].Used = this.InsertRectangle(rects[i])))
                        flag = false;
                this.emptyField = this.ComputeEmptyField();
                return flag;
            }

            public bool InsertRectangle(RectangleData rect)
            {
                int row = rect.Point.Y + rect.Rect.SideB > tab.Length ?
                    tab.Length - rect.Rect.SideB : rect.Point.Y;
                int col = rect.Point.X + rect.Rect.SideA > tab[0].Length ?
                    tab[0].Length - rect.Rect.SideA : rect.Point.X;
                if (row < 0 || col < 0)
                {
                    if (rect.Rot)
                    {
                        rect.Rot = false;
                        rect.Rect = rect.Rect.Rotate();
                        return this.InsertRectangle(rect);
                    }
                    else
                    {
                        rect.Used = false;
                        return false;
                    }
                }
                rect.Rect = rect.Rect.Move(new Point(col, row));
                for (int i = row; i < row + rect.Rect.SideB; ++i)
                    for (int j = col; j < col + rect.Rect.SideA; ++j)
                    {
                        if (this.tab[i][j] == 0)
                            --this.emptyField;
                        ++this.tab[i][j];
                    }
                rect.Used = true;
                return true;
            }

            public bool Move(RectangleData rect)
            {
                this.Remove(rect);
                if (rect.Rot && rect.Rect.SideA!=rect.Rect.SideB)
                {
                    rect.Rect = rect.Rect.Rotate();
                    rect.Rot = false;
                    this.InsertRectangle(rect);
                    return true;
                }
                else
                {
                    rect.Point = new Point(rect.Point.X + 1, rect.Point.Y);
                    if (rect.Rect.SideA < rect.Rect.SideB)
                        rect.Rect = rect.Rect.Rotate();
                    if(this.InsertInGoodPosition(rect))
                    {
                        rect.Rot = true;
                        return true;
                    }
                    return false;
                }
            }

            public bool InsertInGoodPosition(RectangleData rect)
            {
                rect.Rot = false;
                int x = rect.Point.X, y = rect.Point.Y;
                int maxX = this.tab[0].Length; // -rect.Rect.ShorterSide;
                int maxY = this.tab.Length;    // -rect.Rect.ShorterSide;
                for (; y < maxY; ++y)
                {
                    for (; x < maxX; ++x)
                        if (this.tab[y][x] == 0)
                            break;
                    if (x == maxX)
                        x = 0;
                    else
                        break;
                }
                if (y == maxY)
                    return false;
                else
                {
                    rect.Point = new Point(x, y);
                    rect.Rect = rect.Rect.Move(rect.Point);
                    return this.InsertRectangle(rect);
                }
            }

            public bool Remove(RectangleData rect)
            {
                if (rect.Used)
                {
                    for (int i = rect.Rect.LeftTop.Y; i < rect.Rect.RightDown.Y; ++i)
                        for (int j = rect.Rect.LeftTop.X; j < rect.Rect.RightDown.X; ++j)
                        {
                            --this.tab[i][j];
                            if (this.tab[i][j] == 0)
                                this.emptyField++;
                            if (this.tab[i][j] < 0)
                            {
                                Debug.WriteLine("Jakiœ b³¹d dla: " + i + " " + j);
                                return false;
                            }
                        }
                    rect.Used = false;
                }
                return true;
            }

            public int ComputeEmptyField()
            {
                this.emptyField = 0;
                for (int i = 0; i < this.tab.Length; ++i)
                    for (int j = 0; j < this.tab[i].Length; ++j)
                        if (this.tab[i][j] == 0)
                            ++this.emptyField;
                return this.emptyField;
            }

            public int EmptyField
            {
                get { return emptyField; }
            }
        }
        #endregion
    }
}
