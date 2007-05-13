using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

namespace Taio.Algorithms
{
    class Algorithm0 : IAlgorithm
    {
        private string tag = "AW0";
        private Rectangle rect;
        private bool stop;
        //private TimeSpan ts;

        public Rectangle ComputeMaximumRectangle(List<Rectangle> rectangles)
        {
            try
            {
                //DateTime dt = DateTime.Now;
                this.rect = this.ComputeRectangles(rectangles);
                //this.ts = DateTime.Now.Subtract(dt);
            }
            catch (Exception) { }
            return this.rect;
        }

        private Rectangle ComputeRectangles(List<Rectangle> rectangles)
        {
            if (rectangles == null)
                throw new ArgumentNullException();
            if (rectangles.Count == 0)
                throw new ArgumentException("Illegal argument");
            List<Rectangle> rects = new List<Rectangle>();
            foreach (Rectangle r in rectangles)
                rects.Add(new Rectangle(r.LeftTop, r.RightDown, r.Number));
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
            List<RectangleData> rds = this.StartSequence(rects, a, b);
            RectangleData rd = rds[rds.Count-1];
            while(flag && !this.stop)
            {
                if ((this.rect = this.CheckEnd(rds, a*b)) != null)
                    return this.rect;
                rd.Used = true;
                rd.Rect = rd.Rect.Move(rd.Point);
                if (rd.Rot)
                {
                    rd.Rect = rd.Rect.Rotate();
                    rd = this.CheckOutOfBand(rd, a, b);
                    rd.Rot = false;
                }
                else
                {
                    //TODO nie o 1, ale po emptyfield
                    rd.Point = new Point(rd.Point.X + 1, rd.Point.Y);
                    if (rd.Point.X > a - rd.Rect.ShorterSide)
                    {
                        rd.Point = new Point(0, rd.Point.Y+1);
                        if (rd.Point.Y > b - rd.Rect.ShorterSide)
                        {
                            if (!(flag = this.NewSequence(rds, a, b)))
                                this.rect = null;
                            rd.Point = new Point(0,0);
                        }
                    }
                    rd.Rot = true;
                    rd.Rect = rd.Rect.Move(rd.Point);
                    rd = this.CheckOutOfBand(rd, a, b);
                }
            }
            return this.rect;
        }

        private List<RectangleData> StartSequence(List<Rectangle> rects, int a, int b)
        {
            //TODO czêœciowo z emptyfieldsami
            for (int i = 0; i < rects.Count; ++i)
                if (rects[i].SideA < rects[i].SideB)
                    rects[i] = rects[i].Rotate();
            List<RectangleData> rds = new List<RectangleData>();
            rects[0] = rects[0].Move(new Point(0, 0));
            rds.Add(new RectangleData(rects[0]));
            if (rects[0].RightDown.X > a || rects[0].RightDown.Y > b)
            {
                rds[0].Rect = rds[0].Rect.Rotate();
                rds[0].Rot = false;
            }
            rds[0].Used = true;
            int minX=rects[0].RightDown.X, minY=0;
            for (int i = 1; i < rects.Count; ++i)
            {
                if (minX == a)
                {
                    minX = 0;
                    minY = rds[i-1].Rect.RightDown.Y;
                    for (int j = i - 1; j >= 0; --j)
                    {
                        if (minY > rds[j].Rect.RightDown.Y)
                            minY = rds[j].Rect.RightDown.Y;
                        if (rds[j].Rect.LeftTop.X == 0)
                            break;
                    }
                }
                rects[i] = rects[i].Move(new Point(minX, minY));
                rds.Add(new RectangleData(rects[i]));
                rds[i].Point = rds[i].Rect.LeftTop;
                rds[i] = this.CheckOutOfBand(rds[i], a, b);
                if (!rds[i].Used)
                {
                    rds[i].Rect = rds[i].Rect.Rotate();
                    rds[i] = this.CheckOutOfBand(rds[i], a, b);
                    rds[i].Rot = false;
                }
                minX = rds[i].Rect.RightDown.X;
            }
            return rds;
        }

        private bool NewSequence(List<RectangleData> rds, int a, int b)
        {
            //TODO
            for (int i = rds.Count - 2; i >= 0; --i)
            {
                RectangleData rd = rds[i];
                rd.Used = true;
                rd.Rect = rd.Rect.Move(rd.Point);
                if (rd.Rot)
                {
                    rd.Rect = rd.Rect.Rotate();
                    rd = this.CheckOutOfBand(rd, a, b);
                    rd.Rot = false;
                }
                else
                {
                    //TODO nie o 1, ale po emptyfield
                    rd.Point = new Point(rd.Point.X + 1, rd.Point.Y);
                    if (rd.Point.X > a - rd.Rect.ShorterSide)
                    {
                        rd.Point = new Point(0, rd.Point.Y + 1);
                        if (rd.Point.Y > b - rd.Rect.ShorterSide)
                            continue;
                    }
                    rd.Rot = true;
                    rd.Rect = rd.Rect.Move(rd.Point);
                    rd = this.CheckOutOfBand(rd, a, b);
                }
                //TODO tutaj mo¿na optymalnie ustawiaæ nowe kombinacje
                for (int j = i + 1; j < rds.Count; ++j)
                {
                    rds[j].Point = new Point(0, 0);
                    rds[j].Rot = true;
                    rds[j].Rect = rds[j].Rect.Move(new Point(0, 0));
                    rds[j] = this.CheckOutOfBand(rds[j], a, b);
                    if (!rds[j].Used)
                    {
                        rds[j].Rect = rds[j].Rect.Rotate();
                        rds[j].Rot = false;
                        rds[j].Used = true;
                    }
                }
                return true;
            }
            return false;
        }

        private Rectangle CheckEnd(List<RectangleData> rds, int area)
        {
            int num00=0;
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
                int maxSide = (int)Math.Sqrt(2*maxArea);
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
                        if (r.LongerSide <= s.LongerSide && r.ShorterSide<= s.ShorterSide)
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
            int maxSide = (int)Math.Sqrt(2 * maxArea), minSide =(int)Math.Sqrt(((double)maxArea)/2.0);
            for (int i = (int)Math.Pow(2,tab.Count-1)-1; i >= 0; --i)
            {
                int fact = tab[tab.Count-1].First;
                for (int j = 0; j < tab.Count-1; ++j)
                    if ((i >> j & 1) == 1)
                        fact *= tab[j].First;
                if (fact >= minSide && fact <= maxSide &&
                    (((double)fact * fact) / maxArea) <= 2.0 &&
                    ((double)maxArea / (fact * fact)) <= 2.0)
                    sides.Add(new SetCoverEntry(fact, maxArea / fact));
            }
            for(int i=0;i<sides.Count;++i)
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
                if (rects[i].Area > max && rects[i].LongerSide <= 2*rects[i].ShorterSide)
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

        #region AlgRectangleContainer Class
        private class AlgRectangleContaioner
        {
            private int[][] tab;
            private int emptyField;

            public AlgRectangleContaioner(int a, int b)
            {
                this.tab = new int[a][];
                for (int i = 0; i < a; ++i)
                    this.tab[i] = new int[b];
                this.emptyField = a * b;
            }

            public bool InsertRectangle(List<AlgRectangle> rects)
            {
                bool flag =true;
                for (int i = 0; i < rects.Count; ++i)
                    if (!(rects[i].Used = this.InsertRectangle(rects[i])))
                        flag = false;
                this.emptyField = this.ComputeEmptyField();
                return flag;
            }

            public bool InsertRectangle(AlgRectangle rect)
            {
                int row = rect.RightDown.Y > tab.Length ? tab.Length - rect.SideB : rect.LeftTop.Y;
                int col = rect.RightDown.X > tab[0].Length ? tab[0].Length - rect.SideA : rect.LeftTop.X;
                if (row < 0 || col < 0)
                {
                    rect.Used = false;
                    return false;
                }
                rect = rect.AlgMove(new Point(row, col));
                for (int i = row; i < row + rect.SideB; ++i)
                    for (int j = col; j < col + rect.SideA; ++j)
                    {
                        if (this.tab[i][j] == 0)
                            --this.emptyField;
                        ++this.tab[i][j];
                    }
                rect.Used = true;
                return true;
            }

            public bool Move(AlgRectangle rect)
            {
                this.Remove(rect);
                if (rect.Rot && rect.SideA!=rect.SideB)
                {
                    rect = rect.AlgRotate();
                    rect.Rot = false;
                }
                else
                {
                    int x = rect.Point.X+1, y= rect.Point.Y;
                    int maxX = this.tab[0].Length - rect.ShorterSide;
                    int maxY = this.tab.Length - rect.ShorterSide;
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
                    if (x == maxX && y == maxY)
                        return false;
                    rect.Rot = true;
                    rect.Point = new Point(x, y);
                    rect = rect.AlgMove(rect.Point);
                }
                this.InsertRectangle(rect);
                return true;
            }

            public bool InsertInGoodPosition(AlgRectangle rect)
            {
                rect.Rot = false;
                int x = rect.Point.X+1, y = rect.Point.Y;
                int maxX = this.tab[0].Length - rect.ShorterSide;
                int maxY = this.tab.Length - rect.ShorterSide;
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
                if (x == maxX && y == maxY)
                    return false;
                else
                {
                    rect.Point = new Point(x, y);
                    rect = rect.AlgMove(rect.Point);
                    return this.InsertRectangle(rect);
                }
            }

            public bool Remove(AlgRectangle rect)
            {
                if (rect.Used)
                {
                    for (int i = rect.LeftTop.Y; i < rect.RightDown.Y; ++i)
                        for (int j = rect.LeftTop.X; j < rect.RightDown.X; ++j)
                        {
                            --this.tab[i][j];
                            if (this.tab[i][j] == 0)
                                this.emptyField++;
                            if (this.tab[i][j] < 0)
                            {
                                Debug.WriteLine("Jakiœ b³¹d dla" + i + " " + j);
                                return false;
                            }
                        }
                    rect.Used = false;
                }
                return true;
            }

            public int ComputeEmptyField()
            {
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
        */
        #endregion
    }

}
