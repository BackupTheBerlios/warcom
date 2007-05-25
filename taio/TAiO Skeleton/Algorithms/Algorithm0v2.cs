using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Taio.Algorithms
{
    /// <summary>
    /// Algorytm dok³adny - idea omówiona w raporcie
    /// </summary>
    class Algorithm0v2 : IAlgorithm
    {
        /// <summary>
        /// tag
        /// </summary>
        private string tag = "AW02";
        /// <summary>
        /// wyliczony prostokat
        /// </summary>
        private Rectangle rect;
        /// <summary>
        /// jest true to przerwij obliczenia
        /// </summary>
        private bool stop;

        /// <summary>
        /// Metoda budujê prostok¹t o maksymalnym polu spoœród prostok¹tów znajduj¹cych siê 
        /// na liœcie.
        /// </summary>
        /// <param name="rectangles">lista prostok¹tów</param>
        /// <returns>maksymalny prostok¹t jaki da siê zbudowaæ za pomoc¹ tego algorytmu</returns>
        public Rectangle ComputeMaximumRectangle(List<Rectangle> rectangles)
        {
            try
            {
                Console.WriteLine("Alg0v2");
                this.rect = this.ComputeRectangles(rectangles);
                if (this.rect != null &&
                    (this.rect.ContainedRectangles == null || this.rect.ContainedRectangles.Count == 0))
                {
                    RectangleContainer rectC = new RectangleContainer();
                    rectC.InsertRectangle(this.rect);
                    if (rectC.IsCorrectRectangle && rectC.MaxCorrectRect.Area == this.rect.Area)
                        this.rect = rectC.MaxCorrectRect;
                }
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Zg³oszono wyj¹tek OutOfMemoryException");
            }
            catch (Exception)
            {

            }
            return this.rect;
        }

        /// <summary>
        /// Metoda budujê prostok¹t o maksymalnym polu spoœród prostok¹tów znajduj¹cych siê 
        /// na liœcie.
        /// </summary>
        /// <param name="rectangles">lista prostok¹tów</param>
        /// <returns>maksymalny prostok¹t jaki da siê zbudowaæ za pomoc¹ tego algorytmu</returns>
        private Rectangle ComputeRectangles(List<Rectangle> rectangles)
        {
            if (rectangles == null)
                throw new ArgumentNullException("Illegal argument");
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
        #region pokrycie prostokata innymi prostokatami
        /// <summary>
        /// tworzy pokrycie dla prostokata axb z prostokatow z rects
        /// </summary>
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
                    if (this.rect != null)
                        return this.rect;
            }
            return this.rect;
        }

        /// <summary>
        /// tworzy pierwsza startowa sekwencje ustawiajac odpowiednio prostokaty
        /// oraz zmienne rotate czy ld
        /// </summary>
        private List<RectangleData> StartSequence(List<Rectangle> rects, int a, int b,
            AlgRectangleContaioner arc)
        {
            List<RectangleData> rds = new List<RectangleData>();
            for (int i = 0; i < rects.Count && !this.stop; ++i)
            {
                rects[i] = rects[i].Move(new Point(0, 0));
                if (rects[i].SideA < rects[i].SideB)
                    rects[i] = rects[i].Rotate();
                rds.Add(new RectangleData(rects[i]));
                arc.InsertInGoodPosition(rds[i]);
            }
            return rds;
        }

        /// <summary>
        /// tworzy nowa startowa sekwencje ustawiajac odpowiednio prostokaty
        /// oraz zmienne rotate czy ld
        /// zwraca true jesli istnieje nowa sekwencja
        /// wpp zwraca false == nie udalo sie pokryc danymi prostokatami axb
        /// </summary>
        private bool NewSequence(List<RectangleData> rds,
            AlgRectangleContaioner arc, int a, int b)
        {
            for (int i = rds.Count - 2; i >= 0; --i)
            {
                if (i == 2 && rds[i].Point.Y == 3)
                    i = 2;
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

        /// <summary>
        ///sprawdza czy udalo sie juz ulozyc z danej sekwencji rozwiazanie, jesli 
        /// zwroci !=null => udalo sie i to jest rozwiazanie wpp zwraca null => szukaj dalej
        /// </summary>
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

        /// <summary>
        /// sprawdza czy dany prostokat znajdujacy sie w rd.Rect nie wystaje poza a i b
        /// jesli tak to go "wciska"
        /// </summary>
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
#endregion

        #region zmniejszenie rozmiarów zadania
        /// <summary>
        /// zmniejsza rozmiary zadania, zmieniajac ewentulanie maxArea
        /// decyzje podejmuje na podstawie rozkladu aktualnego maxArea oraz
        /// prostokatow z listy rects
        /// zwraca liste SetCoverEntry, dla ktorych mozna ulozyc rozwiazanie dla
        /// danych rects oraz danego maxArea
        /// </summary>
        private List<SetCoverEntry> ReductionTask(ref int maxArea, List<Rectangle> rects)
        {
            List<SetCoverEntry> tab = new List<SetCoverEntry>();
            bool flag = true;
            while (flag && !this.stop)
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

        /// <summary>
        /// zwraca liste SetCoverEntry, dla ktorych mozna ulozyc rozwiazanie dla
        /// danego maxArea => rozklada na dwa czynniki maxArea
        /// </summary>
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
            for (int i = (int)Math.Pow(2, tab.Count - 1) - 1; i >= 0 && !this.stop; --i)
            {
                int fact = tab[tab.Count - 1].First;
                for (int j = 0; j < tab.Count - 1; ++j)
                    if ((i >> j & 1) == 1)
                        fact *= tab[j].First;
                if (fact >= minSide && fact <= maxSide &&
                    (((double)fact * fact) / maxArea) <= 2.0 &&
                    ((double)maxArea / (fact * fact)) <= 2.0)
                    sides.Add(new SetCoverEntry(fact, maxArea / fact));
            }
            for (int i = 0; i < sides.Count; ++i)
                for (int j = i + 1; j < sides.Count; ++j)
                    if (sides[i].A == sides[j].A || sides[i].A == sides[j].B)
                        sides.RemoveAt(j--);
            return sides;
        }


        /// <summary>
        /// rozklada na dwa czynniki liczbe n zwracajac pary intow => 
        /// numer inta oraz ile razy wystepuje w rozkladzie
        /// p jest najwiekszym dzielnikiem n
        /// </summary>
        private List<IntPair> ComputeFactors(int n, out int p)
        {
            p = 2;
            int g = (int)Math.Sqrt(n);
            List<IntPair> tab = new List<IntPair>();
            while (p <= g && !this.stop)
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

        /// <summary>
        /// liczy maksymalne pole jakie moze otrzymac algorytm => sumuje pola z listy rects
        /// </summary>
        private int ComputeMaximumArea(List<Rectangle> rects)
        {
            int area = 0;
            foreach (Rectangle rect in rects)
                area += rect.Area;
            return area;
        }

        /// <summary>
        /// znajduje maksymalny prostokat z listy rects spelanijacy warunki 2:1
        /// jesli nie znajdzie zadnego zwraca null
        /// </summary>
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

        /// <summary>
        /// przerywa dzialanie algorytmu
        /// </summary>
        public void StopThread()
        { stop = true; }
        /// <summary>
        /// zwraca stworzony prostokat
        /// </summary>
        public Rectangle GetRectangle()
        { return rect; }
        /// <summary>
        /// zwraca tag
        /// </summary>
        public string GetTag()
        { return tag; }




        #region IntPair Class
        /// <summary>
        /// klasa ktora zawiera dwa inty jako jeden obiekt
        /// </summary>
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
        /// <summary>
        /// klasa SetCoverEntry
        /// zawiera boki z jakich nalezy ulozyc prostokat wynikowy
        /// jego pole
        /// oraz prostokaty ktorymi nalezy wypelniac wynikowy
        /// </summary>
        private class SetCoverEntry
        {
            /// <summary>
            /// bok a prostokata
            /// </summary>
            private int a;
            /// <summary>
            /// bok b prostokata
            /// </summary>
            private int b;
            /// <summary>
            /// pole prostokata
            /// </summary>
            private int tmpArea;
            /// <summary>
            /// lista prostokatow, ktorymi nalezy pokryc prostokat axb
            /// </summary>
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
        /// <summary>
        /// klasa danych o prostokacie wykorzystywanych przy algorytmie
        /// </summary>
        private class RectangleData
        {
            /// <summary>
            /// czy nastapila juz rotacja (obrot) 
            /// jesli true to nie nastapila
            /// </summary>
            private bool rot = true;
            /// <summary>
            /// czy dany prostokat jest uzywany w obecnej sekwencji
            /// </summary>
            private bool used;
            /// <summary>
            /// punkt przebiegu prostokata po polach prostokata wynikowe
            /// na ktorym ostatnim polu rozpatrywalismy prostokat
            /// </summary>
            private Point point = new Point(0, 0);
            /// <summary>
            /// rozpatrywany prostokat
            /// </summary>
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

        #region Inna wersja:) - tzn kod nieuzywany
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
        /// <summary>
        /// klasa pomocnicza - kontener ktory wie jak poruszac prostokaty
        /// oraz wie czy juz zostal rozwiazany problem
        /// </summary>
        private class AlgRectangleContaioner
        {
            /// <summary>
            /// tablica  pol prostokatow
            /// jesli pole = 0 => jest puste
            /// >0 oznacza ile prostokatow lezy na tym polu
            /// </summary>
            private int[][] tab;
            /// <summary>
            /// liczba pustych pol => takich ktore maja tab[i][j]==0
            /// </summary>
            private int emptyField;

            /// <summary>
            /// inicjalizuje klase
            /// </summary>
            public AlgRectangleContaioner(int a, int b)
            {
                this.tab = new int[b][];
                for (int i = 0; i < b; ++i)
                    this.tab[i] = new int[a];
                this.emptyField = a * b;
            }

            /// <summary>
            /// wstawia ciurkiem wszystkie prostokaty w dokladne ich pozycje
            /// zwraca false jesli chociaz jeden jest nieuzywany
            /// </summary>
            public bool InsertRectangle(List<RectangleData> rects)
            {
                bool flag = true;
                for (int i = 0; i < rects.Count; ++i)
                    if (!(rects[i].Used = this.InsertRectangle(rects[i])))
                        flag = false;
                this.emptyField = this.ComputeEmptyField();
                return flag;
            }

            /// <summary>
            /// wstawia prostokat rect.Rect na jego pozycje docelowa
            /// jednoczesnie dodajac wartosci tablicy pol prostokatow
            /// od razu tak ustawia prostokat aby nie wystawal poza dlugosci tablic=>
            /// poza dlugosci bokow prostokata wynikowego a i b
            /// zwraca true jesli prostokat jest uzywany w nowej sekwencji wpp
            /// zwraca false
            /// </summary>
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

            /// <summary>
            /// robi pojedynczy ruch na pierwsze wolne pole jesli rot==false
            /// zas jesli rot==true to obraca go
            /// do wstawiania wykorzystuje funkcjie InsertRectangle
            /// wczesniej usuwa dany prostokat z tablic  korzystajac z  Remove
            /// zwraca true jesli prostokat jest uzywany w nowej sekwencji wpp
            /// zwraca false => trzeba wygenerowac nowa sekwencje dla poprzednikow prostokata
            /// </summary>
            public bool Move(RectangleData rect)
            {
                this.Remove(rect);
                if (rect.Rot && rect.Rect.SideA != rect.Rect.SideB)
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
                    if (this.InsertInGoodPosition(rect))
                    {
                        rect.Rot = true;
                        return true;
                    }
                    return false;
                }
            }

            /// <summary>
            /// wklada prostokat na pierwsze wolne pole
            /// zwraca true jesli prostokat jest uzywany w nowej sekwencji wpp
            /// zwraca false => trzeba wygenerowac nowa sekwencje dla poprzednikow prostokata
            /// </summary>
            public bool InsertInGoodPosition(RectangleData rect)
            {
                rect.Rot = true;
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


            /// <summary>
            /// usuwa prostokat z jego obecnego miejsca jesli byl uzywany,
            /// jednoczesnie zmniejszajac wartosci pol tablicy
            /// oraz podbijajac ewentualnie emptyField
            /// zwraca true jesli sie udalo wpp false
            /// </summary>
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

            /// <summary>
            /// zwraca ilosc wolnych pol, ktore sam wylicza ustawiajac
            /// emptyField
            /// </summary>
            public int ComputeEmptyField()
            {
                this.emptyField = 0;
                for (int i = 0; i < this.tab.Length; ++i)
                    for (int j = 0; j < this.tab[i].Length; ++j)
                        if (this.tab[i][j] == 0)
                            ++this.emptyField;
                return this.emptyField;
            }
            
            /// <summary>
            /// zwraca ilosc wolnych pol
            /// </summary>
            public int EmptyField
            {
                get { return emptyField; }
            }
        }
        #endregion
    }
}
