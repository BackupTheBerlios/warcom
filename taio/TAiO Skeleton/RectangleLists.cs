using System;
using System.Collections.Generic;
using System.Text;

namespace Taio
{
    class RectanglesList
    {
        //prostok�ty posortowane malej�co wg pola
        private List<Rectangle> aRects = new List<Rectangle>();
        //prostok�ty posortowane malej�co wg d�u�szego boku
        private List<Rectangle> sRects = new List<Rectangle>();
        //kopia danych wej�ciowych
        private List<Rectangle> myRects = new List<Rectangle>();

        private Rectangle.RectangleComparer rectComparer = Rectangle.GetComparer();


        private enum GettingMethodType { Largest, Smallest, Longest, LimitedLongest, SmallestCovering, SmallestNotShorterThan };

        public RectanglesList(List<Rectangle> rectangles)
        {
            if (rectangles == null)
                throw new ArgumentNullException();
            if (rectangles.Count == 0)
                throw new ArgumentException("Rectangles list cannot be empty.");

            InitializeRectanglesLists(rectangles);
        }

        private void InitializeRectanglesLists(List<Rectangle> rects)
        {
            foreach (Rectangle r in rects)
                myRects.Add(new Rectangle(r.LeftTop, r.RightDown, r.Number));

            rectComparer.Comparison = Rectangle.RectangleComparer.ComparisonType.Area;

            aRects.AddRange(myRects);
            aRects.Sort(rectComparer);
            aRects.Reverse();

            rectComparer.Comparison = Rectangle.RectangleComparer.ComparisonType.LongerSide;
            sRects.AddRange(aRects);
            sRects.Sort(rectComparer);
            sRects.Reverse();
        }

        //jak z perspektywy czasu patrz� na to, my�l�, �e to chyba by� g�upi pomys� (przez to, �e potem okaza�o si�, ze potrzebne s� metody, kt�re przyjmuja argumenty)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gmt"></param>
        /// <param name="gettingMethodArgument">Additional method parameter.
        /// If gmt is LimitedLongest it should be maxSideLength, if SmallestCovering - rectangle to cover, if SmallestNotShorterThan - minSideLength.
        /// Otherwise it is unused.</param>
        /// <returns></returns>
        private Rectangle GetRectangleFromList(GettingMethodType gmt, object gettingMethodArgument)
        {
            Rectangle result = null;
            if (aRects != null && sRects != null)
            {
                switch (gmt)
                {
                    case GettingMethodType.Largest:
                        result = PeekLargestRect();
                        break;
                    case GettingMethodType.Smallest:
                        result = PeekSmallestRect();
                        break;
                    case GettingMethodType.Longest:
                        result = PeekLongestRect();
                        break;
                    case GettingMethodType.LimitedLongest:
                        int maxSideLength = 0;
                        if(gettingMethodArgument == null || gettingMethodArgument.GetType() != maxSideLength.GetType())
                            throw new ArgumentException();
                        maxSideLength = (int) gettingMethodArgument;
                        result = PeekLongestRect(maxSideLength);
                        break;
                    case GettingMethodType.SmallestCovering:
                        Rectangle r = new Rectangle(1,1);
                        if(gettingMethodArgument == null || gettingMethodArgument.GetType() != r.GetType())
                            throw new ArgumentException();
                        r = (Rectangle)gettingMethodArgument;
                        result = PeekSmallestCovering(r.SideA, r.SideB);
                        break;
                    case GettingMethodType.SmallestNotShorterThan:
                        int minSideLength = 0;
                        if (gettingMethodArgument == null || gettingMethodArgument.GetType() != minSideLength.GetType())
                            throw new ArgumentException();
                        minSideLength = (int)gettingMethodArgument;
                        result = PeekSmallestNotShorterThan(minSideLength);
                        break;
                }

                if (result != null)
                {
                    aRects.Remove(result);
                    sRects.Remove(result);
                }
            }
            return result;
        }

        /// <summary>
        /// Zwraca prostok�t o najwi�kszym polu bez usuwania go z list
        /// </summary>
        /// <returns></returns>
        public Rectangle PeekLargestRect()
        {
            Rectangle result = null;
            if (aRects != null && sRects != null)
                result = aRects[0];
            return result;
        }

        /// <summary>
        /// Zwraca prostok�t o najwi�kszym polu i usuwa go z list
        /// </summary>
        /// <returns></returns>
        public Rectangle GetLargestRect()
        {
            return GetRectangleFromList(GettingMethodType.Largest, 0);
        }

        /// <summary>
        /// Zwraca prostok�t o najmniejszym polu bez usuwania go z list
        /// </summary>
        /// <returns></returns>
        public Rectangle PeekSmallestRect()
        {
            Rectangle result = null;
            if (aRects != null && sRects != null)
                result = aRects[aRects.Count - 1];
            return result;
        }

        /// <summary>
        /// Zwraca prostok�t o najmniejszym polu i usuwa go z list
        /// </summary>
        /// <returns></returns>
        public Rectangle GetSmallestRect()
        {
            return GetRectangleFromList(GettingMethodType.Smallest, 0);
        }

        /// <summary>
        /// Zwraca prostok�t o najd�u�szym boku bez usuwania go z list. Je�li dwa lub wi�cej prostok�t�w ma taki sam d�u�szy bok, zwracany jest ten o najwi�kszym polu.
        /// </summary>
        /// <returns></returns>
        public Rectangle PeekLongestRect()
        {
            Rectangle result = null;
            if (aRects != null && sRects != null && sRects.Count != 0)
            {
                List<Rectangle> temp = new List<Rectangle>();
                int longestSide = sRects[0].LongerSide;
                for (int i = 0; i < sRects.Count; i++)
                {
                    Rectangle r = sRects[i];
                    if (r.LongerSide == longestSide)
                        temp.Add(r);
                }
                rectComparer.Comparison = Rectangle.RectangleComparer.ComparisonType.Area;
                temp.Sort(rectComparer);
                temp.Reverse();
                if (temp.Count > 0)
                    result = temp[0];
            }
            return result;
        }

        /// <summary>
        /// Zwraca prosotk�t o d�u�szym boku kr�tszym lub r�wnym  maxSideLength bez usuwania go.
        /// </summary>
        /// <param name="maxSideLength">Maksymalna akceptowana d�ugo�� boku</param>
        /// <returns></returns>
        public Rectangle PeekLongestRect(int maxSideLength)
        {
            if (maxSideLength <= 0)
                throw new ArgumentException("maxSideLength must be greater than zero");

            Rectangle result = null;
            if (aRects != null && sRects != null && sRects.Count != 0)
            {
                List<Rectangle> limited = new List<Rectangle>();
                for (int i = 0; i < sRects.Count; i++)
                {
                    if (sRects[i].LongerSide <= maxSideLength)
                        limited.Add(sRects[i]);
                }
                if (limited.Count == 0)
                    return result;

                int longestSide = limited[0].LongerSide;
                List<Rectangle> temp = new List<Rectangle>();
                for (int i = 0; i < limited.Count; i++)
                {
                    Rectangle r = limited[i];
                    if (r.LongerSide == longestSide)
                        temp.Add(r);
                }

                rectComparer.Comparison = Rectangle.RectangleComparer.ComparisonType.Area;
                temp.Sort(rectComparer);
                temp.Reverse();
                if(temp.Count>0)
                    result = temp[0];
            }
            return result;
        }

        /// <summary>
        /// Zwraca prostok�t o najd�u�szym boku i usuwa go z list.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetLongestRect()
        {
            return GetRectangleFromList(GettingMethodType.Longest, 0);
        }

        /// <summary>
        /// Zwraca prostok�t o najd�u�szym boku kr�tszym lub r�wnym maxSideLength i usuwa go z list
        /// </summary>
        /// <param name="maxSideLength">Maximal accepted side length</param>
        /// <returns></returns>
        public Rectangle GetLongestRect(int maxSideLength)
        {
            if (maxSideLength <= 0)
                throw new ArgumentException("maxSideLength must be greater than zero");

            return GetRectangleFromList(GettingMethodType.LimitedLongest, maxSideLength);
        }

        /// <summary>
        /// Zwraca prostok�t o najmniejszym polu kt�ry ma bok d�u�szy lub r�wny minSide bez usuwania go z list
        /// </summary>
        /// <param name="minSide"></param>
        /// <returns></returns>
        public Rectangle PeekSmallestNotShorterThan(int minSide)
        {
            if (minSide <= 0)
                throw new ArgumentException();

            Rectangle result = null;

            List<Rectangle> temp = new List<Rectangle>();
            foreach (Rectangle r in sRects)
            {
                if (r.LongerSide >= minSide)
                        temp.Add(r);
                else
                    break;
            }

            if (temp.Count == 0)
                return result;

            if (temp.Count > 1)
            {
                rectComparer.Comparison = Rectangle.RectangleComparer.ComparisonType.Area;
                temp.Sort(rectComparer);
            }
            result = temp[0];

            return result;
        }

        /// <summary>
        /// /// Zwraca prostok�t o najmniejszym polu kt�ry ma bok kr�tszy lub r�wny maxSideLength bez usuwania go z list
        /// </summary>
        /// <param name="minSide"></param>
        /// <returns></returns>
        public Rectangle GetSmallestNotShorterThan(int minSide)
        {
            return GetRectangleFromList(GettingMethodType.SmallestNotShorterThan, minSide);
        }

        /// <summary>
        /// Zwraca najmniejszy prosotk�t pokrywaj�cy prostok�t o zadanych bokach bez usuwania go z list.
        /// </summary>
        /// <param name="sideA"></param>
        /// <param name="sideB"></param>
        /// <returns></returns>
        public Rectangle PeekSmallestCovering(int sideA, int sideB)
        {
            if (sideA <= 0 || sideB <= 0)
                throw new ArgumentException();

            Rectangle result = null;

            int longer, shorter;
            if (sideA >= sideB)
            {
                longer = sideA;
                shorter = sideB;
            }
            else
            {
                longer = sideB;
                shorter = sideA;
            }

            List<Rectangle> temp = new List<Rectangle>();
            foreach (Rectangle r in sRects)
            {
                if (r.LongerSide >= longer)
                {
                    if (r.ShorterSide >= shorter)
                        temp.Add(r);
                }
                else
                    break;
            }

            if (temp.Count == 0)
                return result;

            if (temp.Count > 1)
            {
                rectComparer.Comparison = Rectangle.RectangleComparer.ComparisonType.Area;
                temp.Sort(rectComparer);
            }
            result = temp[0];

            return result;
        }

        /// <summary>
        /// Zwraca najmniejszy prosotk�t pokrywaj�cy prostok�t o zadanych bokach i usuwa go z list.
        /// </summary>
        /// <param name="sideA"></param>
        /// <param name="sideB"></param>
        /// <returns></returns>
        public Rectangle GetSmallestCovering(int sideA, int sideB)
        {
            return GetRectangleFromList(GettingMethodType.SmallestCovering, new Rectangle(sideA, sideB));
        }

        /// <summary>
        /// Usuwa prostok�t r z list.
        /// </summary>
        /// <param name="r">Rectangle to remove.</param>
        public void RemoveRectangle(Rectangle r)
        {
            if (r == null)
                throw new ArgumentNullException();

            if (aRects != null && sRects != null)
            {
                aRects.Remove(r);
                sRects.Remove(r);
            }
        }

        /// <summary>
        /// Czy listy s� puste.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return aRects.Count == 0 ? true : false;
        }

        /// <summary>
        /// Liczba aktualnie przechowywanych prostok�t�w w RectanglesList.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return aRects.Count;
        }
    }
}
