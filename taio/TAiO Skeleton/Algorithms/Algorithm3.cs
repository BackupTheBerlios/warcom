using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Taio.Algorithms
{
    /// <summary>
    /// Algorytm 3 (idea oparta na grze Tetris).
    /// </summary>
    class Algorithm3 : IAlgorithm
    {
        #region Zmienne klasy
        /// <summary>
        /// maksymalna d�ugo�c d�u�szego boku
        /// </summary>
        private int sideMax = 0;            
        /// <summary>
        /// zmienna m�wi, czy algorytm ma by� dalej wykonywany
        /// </summary>
        private bool running = true;
        /// <summary>
        /// wyliczany prostok�t
        /// </summary>
        private Rectangle rectangle;
        /// <summary>
        /// identyfikator algorytmu
        /// </summary>
        private string tag = "AW3";
        /// <summary>
        /// maksymalny stosunek d�u�szego boku do kr�tszego
        /// </summary>
        private double ratio = 2.0;         

        /// <summary>
        /// szeroko�� prostok�ta, kt�ry mo�na zbudowa�
        /// </summary>
        private int Min_X;
        /// <summary>
        /// wysoko�� prostok�ta, kt�ry mo�na zbudowa�
        /// </summary>
        private int Min_Y;
        /// <summary>
        /// szeroko�� docelowego prostok�ta
        /// </summary>
        private int Max_X;
        /// <summary>
        /// wysoko�� docelowego prostok�ta
        /// </summary>
        private int Max_Y;        
        #endregion

        #region Metody podstawowe
        /// <summary>
        /// Metoda buduj� prostok�t o maksymalnym polu spo�r�d prostok�t�w znajduj�cych si� 
        /// na li�cie.
        /// </summary>
        /// <param name="rectangles">lista prostok�t�w</param>
        /// <returns>maksymalny prostok�t jaki da si� zbudowa� za pomoc� tego algorytmu</returns>
        public Rectangle ComputeMaximumRectangle(List<Rectangle> rectangles)
        {
            Rectangle maxRect = null;

            try
            {
                if (rectangles == null)
                    throw new ArgumentNullException();
                if (rectangles.Count == 0)
                    throw new ArgumentException("Empty rectangles list");

                // algorytm w�a�ciwy
                maxRect = computeRectangles(rectangles);
                rectangle = maxRect;
            }
            catch (Exception e)
            {
                Console.WriteLine("wyjatek: " + e.Message);
            }

            return maxRect;
        }

        /// <summary>
        /// Metoda potrzebna do zatrzymywania oblicze� algorytmu.
        /// </summary>
        public void StopThread()
        {
            running = false;
        }

        /// <summary>
        /// Akcesor do wyliczonego maksymalnego prostok�ta
        /// </summary>
        /// <returns>maksymalny prostok�t</returns>
        public Rectangle GetRectangle()
        {
            return rectangle;
        }

        /// <summary>
        /// Akcesor do zmiennej opisuj�cej algortym.
        /// </summary>
        /// <returns>informacja opisuj�ca dany algorytm</returns>
        public string GetTag()
        {
            return tag;
        }
        #endregion

         #region Metody pomocnicze
        /// <summary>
        /// Metoda buduj� prostok�t o maksymalnym polu spo�r�d prostok�t�w znajduj�cych si� 
        /// na li�cie.
        /// </summary>
        /// <param name="rectangles">lista prostok�t�w</param>
        /// <returns>maksymalny prostok�t, jaki da si� zbudowa� za pomoc� tego algorytmu</returns>
        private Rectangle computeRectangles(List<Rectangle> rectangles)
        {
            int maxArea = computeMaximumArea(rectangles);
            setMaximumSides(maxArea);
            List<Rectangle> correctRects = removeTooBigRectangles(rectangles);

            // brak prawid�owych prostok�t�w
            if (correctRects.Count == 0)
                return null;

            // inicjalizacja danych
            Rectangle result = null;
            Rectangle maxRectangle = findMaxAreaRectangle(correctRects);
            Rectangle rectToFill = findAnyMaxRectangle(correctRects);
            Hole hole = null;
            List<Rectangle> rects = new List<Rectangle>();
            List<OutRect> outsRight = new List<OutRect>();
            List<OutRect> outsDown = new List<OutRect>();
            List<Hole> holesRight = new List<Hole>();
            List<Hole> holesDown = new List<Hole>();

            correctRects.Remove(rectToFill);                       
            rectToFill.Move(new Point(0, 0));
            rects.Add(rectToFill);
            Min_X = Max_X = rectToFill.RightDown.X;
            Min_Y = Max_Y = rectToFill.RightDown.Y;

            // dop�ki s� jakie� prostok�ty do wykorzystania oraz algorytm nie zosta� przerwany
            while (running && correctRects.Count > 0)
            {
                bool rightSide;

                hole = findHole(holesRight, holesDown, out rightSide);
                rectToFill = findRectangle(correctRects, hole, rightSide);
                                              
                if (hole != null)
                    fillHole(outsRight, outsDown, holesRight, holesDown, rectToFill, hole, rightSide);
                else
                    stickRectangle(outsRight, outsDown, holesRight, holesDown, rectToFill,
                                        rightSide);

                rects.Add(rectToFill);
                correctRects.Remove(rectToFill);

                if (holesRight.Count == 0 && outsRight.Count > 0)
                {
                    updateOutsRight(outsRight);
                    updateHolesRight(outsRight, outsDown, holesRight, holesDown);
                }
                if (holesDown.Count == 0 && outsDown.Count > 0)
                {
                    updateOutsDown(outsDown);
                    updateHolesDown(outsRight, outsDown, holesRight, holesDown);
                }
            }

            // wybierany maksymalny prostok�t spe�niaj�cy warunki zadania
            result = maxCorrectRectangle(rects);
            if (result != null)
            {
                if (!correctRectangleFound(result))
                    result = maxRectangle;
                else if (maxRectangle != null && maxRectangle.Area > result.Area)
                    result = maxRectangle;
            }
            else
                result = maxRectangle;            

            return result;
        }

        /// <summary>
        /// Metoda sprawdza czy dany prostok�t spe�nia warunki zadania
        /// </summary>
        /// <param name="rect">sprawdzany prostok�t</param>
        /// <returns>czy dany prostok�t spe�nia warunki zadania</returns>
        private bool correctRectangleFound(Rectangle rect)
        {
            if (rect == null)
                return false;

            if (rect.LongerSide / rect.ShorterSide > ratio)
                return false;

            return true;
        }

        /// <summary>
        /// Prostok�t do��czany do boku budowanego prostak�ta.
        /// </summary>
        /// <param name="outsRight">lista prostok�t�w na brzegu z prawej strony</param>
        /// <param name="outsDown">lista prostok�t�w na brzegu z do�u</param>
        /// <param name="holesRight">lista dziur z prawej strony</param>
        /// <param name="holesDown">lista dziur z do�u</param>
        /// <param name="rect">do��czany prostok�t</param>
        /// <param name="rightSide">czy prostok�t ma by� do��czany z prawej czy z do�u</param>
        private void stickRectangle(List<OutRect> outsRight, List<OutRect> outsDown, 
                            List<Hole> holesRight, List<Hole> holesDown, Rectangle rect,
                            bool rightSide)
        {
            if (rightSide)
                stickRectangleRight(outsRight, outsDown, holesRight, holesDown, rect);
            else
                stickRectangleDown(outsRight, outsDown, holesRight, holesDown, rect);
        }

        /// <summary>
        /// Prostok�t do��czany do boku budowanego prostak�ta z prawej strony.
        /// </summary>
        /// <param name="outsRight">lista prostok�t�w na brzegu z prawej strony</param>
        /// <param name="outsDown">lista prostok�t�w na brzegu z do�u</param>
        /// <param name="holesRight">lista dziur z prawej strony</param>
        /// <param name="holesDown">lista dziur z do�u</param>
        /// <param name="rect">do��czany prostok�t</param>        
        private void stickRectangleRight(List<OutRect> outsRight, List<OutRect> outsDown, 
                            List<Hole> holesRight, List<Hole> holesDown, Rectangle rect)
        {
            OutRect outRect;
            Hole hole;
     
            rect.Move(new Point(Min_X, 0));
            updateMaxValues(rect);

            if (rect.SideB < Min_Y)
            {
                outRect = new OutRect(rect.SideA, rect.SideB, new Point(rect.LeftTop.X,
                                                rect.LeftTop.Y));
                outsRight.Add(outRect);

                hole = new Hole(rect.SideA, Max_Y - rect.SideB, new Point(Min_X,
                                    rect.RightDown.Y));
                hole.NeighbourOne = outRect;
                hole.OrientDown = true;
                hole.OrientRight = true;

                if (isCornerHole(holesDown))
                    hole.saveResize(rect.SideA, Min_Y - rect.SideB);
                else
                {
                    hole.Corner = true;
                    if (outsDown.Count > 0)
                        hole.NeighbourSecond = outsDown[outsDown.Count - 1];
                }

                holesRight.Add(hole);
            }
            else
            {
                if (Max_Y > Min_Y && !isCornerHole(holesDown) && outsDown.Count > 0)
                {
                    hole = new Hole(Max_X - Min_X, Max_Y - Min_Y, new Point(Min_X, Min_Y));
                    hole.NeighbourOne = outsDown[outsDown.Count - 1];
                    hole.OrientDown = true;
                    hole.OrientRight = true;
                    hole.Corner = true;
                    holesDown.Add(hole);
                }

                Min_X = rect.RightDown.X;
            }
        }

        /// <summary>
        /// Prostok�t do��czany do boku budowanego prostak�ta z do�u.
        /// </summary>
        /// <param name="outsRight">lista prostok�t�w na brzegu z prawej strony</param>
        /// <param name="outsDown">lista prostok�t�w na brzegu z do�u</param>
        /// <param name="holesRight">lista dziur z prawej strony</param>
        /// <param name="holesDown">lista dziur z do�u</param>
        /// <param name="rect">do��czany prostok�t</param>        
        private void stickRectangleDown(List<OutRect> outsRight, List<OutRect> outsDown, 
                            List<Hole> holesRight, List<Hole> holesDown, Rectangle rect)
        {
            OutRect outRect;
            Hole hole;

            rect.Move(new Point(0, Min_Y));
            updateMaxValues(rect);

            if (rect.SideA < Min_X)
            {
                outRect = new OutRect(rect.SideA, rect.SideB, new Point(rect.LeftTop.X,
                                                rect.LeftTop.Y));
                outsDown.Add(outRect);

                hole = new Hole(Max_X - rect.SideA, rect.SideB, new Point(rect.RightDown.X,
                                    Min_Y));
                hole.NeighbourOne = outRect;
                hole.OrientDown = true;
                hole.OrientRight = true;

                if (isCornerHole(holesDown))
                    hole.saveResize(Min_X - rect.SideA, rect.SideB);
                else
                {
                    hole.Corner = true;
                    if (outsRight.Count > 0)
                        hole.NeighbourSecond = outsRight[outsRight.Count - 1];
                }

                holesDown.Add(hole);
            }
            else
            {
                if (Max_X > Min_X && !isCornerHole(holesRight) && outsRight.Count > 0)
                {
                    hole = new Hole(Max_X - Min_X, Max_Y - Min_Y, new Point(Min_X, Min_Y));
                    hole.NeighbourOne = outsRight[outsRight.Count - 1];
                    hole.OrientDown = true;
                    hole.OrientRight = true;
                    hole.Corner = true;
                    holesRight.Add(hole);
                }

                Min_Y = rect.RightDown.Y;
            }
        }

        /// <summary>
        /// Prostok�t ma wype�ni� dan� dziur�.
        /// </summary>
        /// <param name="outsRight">lista prostok�t�w na brzegu z prawej strony</param>
        /// <param name="outsDown">lista prostok�t�w na brzegu z do�u</param>
        /// <param name="holesRight">dziury z prawej strony</param>
        /// <param name="holesDown">dziury z do�u</param>
        /// <param name="rect">do��czany prostok�t</param>
        /// <param name="hole">dziura do wype�nienia</param>
        /// <param name="rightSide">czy dziura jest z prawej strony czy z do�u</param>
        private void fillHole(List<OutRect> outsRight, List<OutRect> outsDown,
                            List<Hole> holesRight, List<Hole> holesDown, Rectangle rect,
                            Hole hole, bool rightSide)
        {
            Hole newHole;
            OutRect outRect, or;
            List<OutRect> outRects;
            List<Hole> holes;
            int result, index;
            
            result = hole.filled(rect, out newHole);
            updateMaxValues(rect);

            if (rightSide)
            {
                outRects = outsRight;
                holes = holesRight;
            }
            else
            {
                outRects = outsDown;
                holes = holesDown;
            }

            or = hole.NeighbourOne;
            index = -1;
            if (or != null)
                index = outRects.IndexOf(or);
            outRect = new OutRect(rect.SideA, rect.SideB, new Point(rect.LeftTop.X,
                                                rect.LeftTop.Y));
            outRects.Insert(index + 1, outRect);
            hole.NeighbourOne = outRect;

            // dziura jest na rogu i prostok�t w ca�o�ci j� wepe�ni
            if (hole.OrientDown && hole.OrientRight && result == 0)
            {
                if(rect.RightDown.X > Min_X && !rightSide)
                    outsRight.Add(new OutRect(rect.RightDown.X - Min_X, rect.SideB,
                                    new Point(Min_X, rect.LeftTop.Y)));

                if (rect.RightDown.Y > Min_Y && rightSide)
                    outsDown.Add(new OutRect(rect.SideA, rect.RightDown.Y - Min_Y,
                                    new Point(rect.LeftTop.X, Min_Y)));

            }
            // dziura na rogu wype�niona cz�ciowo - powstaje nowa dziura
            if (hole.OrientDown && hole.OrientRight && result > 0 && newHole != null)
            {
                
                OutRect or2 = null;
                if (outsDown.Count > 0)
                   or2 = outsDown[outsDown.Count - 1];
                newHole.NeighbourOne = or2;

                if (newHole.Rect.LeftTop.Y >= Min_Y)
                    holesDown.Add(newHole);                     
            }

            if (result == 0)
                holes.Remove(hole);
      
            updateHoles(outsRight, outsDown, holesRight, holesDown);
        }

        /// <summary>
        /// Wyliczana suma wszystkich prostok�t�w wej�ciowych.
        /// </summary>
        /// <param name="rects">prostok�ty wej�ciowe</param>
        /// <returns>suma p�l prostok�t�w</returns>
        private int computeMaximumArea(List<Rectangle> rects)
        {
            int area = 0;
            foreach (Rectangle rect in rects)
                area += rect.Area;
            return area;
        }

        /// <summary>
        /// Wyliczane maksymalne boki prostok�ta na podstawie warunku kszta�tu
        /// </summary>
        /// <param name="area">pole maksymalnego prostok�ta</param>
        private void setMaximumSides(int area)
        {
            // shorterSide i tempMax musz� mie� zawsze warto��i ca�kowite
            double shorterSide = (int)Math.Sqrt(area);
            double tempMax = shorterSide;
            double tempValDouble;
            int tempValInt;

            // stosunek d�u�szego boku do kr�tszego nie mo�e by� wi�kszy ni� 2.0
            while (tempMax / shorterSide <= ratio)
            {
                sideMax = (int)tempMax;
                tempMax++;
                tempValDouble = (double)area / tempMax;
                tempValInt = (int)(area / tempMax);

                if (tempMax / tempValDouble > ratio)
                    return;

                shorterSide = area / (int)tempMax;
            }
        }

        /// <summary>
        /// Odrzucane prostok�ty o zbyt du�ym wi�kszym boku (one nigdy nie wejd� w sk�ad
        /// wyj�ciowego prostok�ta).
        /// </summary>
        /// <param name="rects">prostok�ty wej�ciowe</param>
        /// <returns>lista poprawnych prostok�t�w</returns>
        private List<Rectangle> removeTooBigRectangles(List<Rectangle> rects)
        {
            List<Rectangle> correctRects = new List<Rectangle>();
            foreach (Rectangle rect in rects)
                if (rect.LongerSide <= sideMax)
                {
                    Rectangle t = new Rectangle(rect.SideA, rect.SideB, rect.LeftTop);
                    t.Number = rect.Number;
                    correctRects.Add(t);
                }
            return correctRects;
        }

        /// <summary>
        /// Znajdowana dziura do wype�nienia
        /// </summary>
        /// <param name="holesRight">dziury po prawej stronie</param>
        /// <param name="holesDown">dziury z do�u</param>
        /// <param name="rightSide">czy dziura zostanie znaleziona z prawej strony czy z do�u</param>
        /// <returns></returns>
        private Hole findHole(List<Hole> holesRight, List<Hole> holesDown, out bool rightSide)
        {
            int rightCount;
            int downCount;
            Hole holeRight = findHoleRight(holesRight, out rightCount);
            Hole holeDown = findHoleDown(holesDown, out downCount);
            Hole result = null;
            int min_x, min_y;

            rightSide = true;

            if (holeRight != null)
                min_x = holeRight.Rect.LeftTop.X;
            else
                min_x = Min_X;

            if (holeDown != null)
                min_y = holeDown.Rect.LeftTop.Y;
            else
                min_y = Min_Y;

            if (min_x == min_y)
            {
                if (rightCount < downCount)
                {
                    result = holeDown;
                    rightSide = false;
                }
                else
                {
                    result = holeRight;
                    rightSide = true;
                }
            }
            else if (min_y < min_x)
            {
                result = holeDown;
                rightSide = false;
            }
            else
            {
                result = holeRight;
                rightSide = true;
            }


            return result;
        }

        /// <summary>
        /// Dziura znajdowana z prawej strony.
        /// </summary>
        /// <param name="holesRight">dziury z prawej strony</param>
        /// <param name="holesCount">ilo�� dziur po prawej stronie</param>
        /// <returns>znaleziona dziura</returns>
        private Hole findHoleRight(List<Hole> holesRight, out int holesCount)
        {
            Hole result = null;
            int min, temp;
            holesCount = 0;

            if(holesRight == null || holesRight.Count == 0)
                return result;

            result = holesRight[0];
            min = result.Rect.LeftTop.X;
            foreach (Hole hl in holesRight)
            {
                holesCount++;
                temp = hl.Rect.LeftTop.X;

                if (temp < min)
                {
                    min = temp;
                    result = hl;
                }
                else if (temp == min && hl.Rect.Area > result.Rect.Area)
                    result = hl;

            }

            return result;
        }

        /// <summary>
        /// Dziura znajdowana z do�u.
        /// </summary>
        /// <param name="holesRight">dziury z do�u</param>
        /// <param name="holesCount">ilo�� dziur z do�u</param>
        /// <returns>znaleziona dziura</returns>
        private Hole findHoleDown(List<Hole> holesDown, out int holesCount)
        {
            Hole result = null;
            int min, temp;
            holesCount = 0;

            if (holesDown == null || holesDown.Count == 0) 
                return result;

            result = holesDown[0];
            min = result.Rect.LeftTop.Y;
            foreach (Hole hl in holesDown)
            {
                holesCount++;
                temp = hl.Rect.LeftTop.Y;

                if (temp < min)
                {
                    min = temp;
                    result = hl;
                }
                else if (temp == min && hl.Rect.Area > result.Rect.Area)
                    result = hl;

            }

            return result;
        }

        /// <summary>
        /// Znajdowany prostok�t o maksymalnym polu.
        /// </summary>
        /// <param name="rectangles">Lista prostok�t�w</param>
        /// <returns>maksymalny prostok�t</returns>
        private Rectangle findMaxAreaRectangle(List<Rectangle> rectangles)
        {
            Rectangle rect = null;

            if (rectangles == null || rectangles.Count == 0)
                return rect;

            rect = rectangles[0];
            foreach (Rectangle rc in rectangles)
            {
                if(rc.Area > rect.Area)
                    rect = rc;
            }

            return rect;
        }

        /// <summary>
        /// Znajdowany prostok�t o najd�u�szym boku i mo�liwie najwi�kszym polu
        /// spe�niaj�cy warunki zadania.
        /// </summary>
        /// <param name="rectangles">lista prostok�t�w</param>
        /// <returns>znaleziony prostok�t</returns>
        private Rectangle findMaxRectangle(List<Rectangle> rectangles)
        {
            Rectangle rect = null;

            if (rectangles == null || rectangles.Count == 0)
                return rect;

            foreach (Rectangle rc in rectangles)
            {
                if (rc.LongerSide / rc.ShorterSide <= ratio)
                {
                    if (rect == null)
                        rect = rc;
                    else if (rc.LongerSide > rect.LongerSide)
                        rect = rc;
                    else if (rc.LongerSide == rect.LongerSide && rc.Area > rect.Area)
                        rect = rc;
                }
            }

            return rect;
        }

        /// <summary>
        /// Znajdowany prostok�t o najd�u�szym boku i mo�liwie najwi�kszym polu.
        /// </summary>
        /// <param name="rectangles">lista prostok�t�w</param>
        /// <returns>znaleziony prostok�t</returns>
        private Rectangle findAnyMaxRectangle(List<Rectangle> rectangles)
        {
            Rectangle rect = null;

            if (rectangles == null || rectangles.Count == 0)
                return rect;

            foreach (Rectangle rc in rectangles)
            {
                if (rect == null)
                    rect = rc;
                else if (rc.LongerSide > rect.LongerSide)
                    rect = rc;
                else if (rc.LongerSide == rect.LongerSide && rc.Area > rect.Area)
                        rect = rc;                
            }

            return rect;
        }

        /// <summary>
        /// Znajdowany prostok�t o najmniejszym boku i mo�liwie najwi�kszym polu.
        /// </summary>
        /// <param name="rectangles">lista prostok�t�w</param>
        /// <returns>znaleziony prostok�t</returns>
        private Rectangle findMinRectangle(List<Rectangle> rectangles)
        {
            Rectangle rect = null;

            if (rectangles == null || rectangles.Count == 0)
                return rect;

            rect = rectangles[0];
            foreach (Rectangle rc in rectangles)
            {
                if (rc.ShorterSide < rect.ShorterSide)
                    rect = rc;
                else if (rc.ShorterSide == rect.ShorterSide && rc.Area > rect.Area)
                    rect = rc;
            }

            return rect;
        }

        /// <summary>
        /// Metoda zwraca prostok�t o maksymalnym polu, spe�niaj�cy warunki zadania, 
        /// zbudowany z listy prostok�t�w.
        /// </summary>
        /// <param name="rectangles">lista prostokat�w do budowy</param>
        /// <returns>maksymalny prostok�t</returns>
        private Rectangle maxCorrectRectangle(List<Rectangle> rectangles)
        {
            Rectangle result = null;
            RectangleContainer rc = new RectangleContainer();
           

            correctRectangles(rectangles);

            foreach (Rectangle rect in rectangles)
            {
                rc.InsertRectangle(rect, rect.LeftTop);
            }

            result = rc.MaxCorrectRect;           

            return result;
        }        
        
        /// <summary>
        /// Usuwane prostok�ty, z kt�rych nie da si� zbudowa� wyj�ciowego prostok�ta.
        /// </summary>
        /// <param name="rectangles">lista prostok�t�w</param>
        private void correctRectangles(List<Rectangle> rectangles)
        {
            foreach(Rectangle rect in rectangles)
            {
                if (rect.RightDown.X > Min_X)
                    rect.Move(new Point(Math.Max(0, rect.LeftTop.X + Min_X - rect.RightDown.X), 
                                    rect.LeftTop.Y));
                if (rect.RightDown.Y > Min_Y)
                    rect.Move(new Point(rect.LeftTop.X, Math.Max(0, rect.LeftTop.Y + 
                                    Min_Y - rect.RightDown.Y)));
            }
        }

        /// <summary>
        /// Znajdowany prostok�t, kt�ry wype�ni dziur�. 
        /// </summary>
        /// <param name="rectangles">lista prostok�t�w</param>
        /// <param name="hole">dana dziura</param>
        /// <param name="rightSide">czy prostok�t ma by� dodany z prawej strony  czy z do�u</param>
        /// <returns>znaleziony prostok�t</returns>
        private Rectangle findRectangle(List<Rectangle> rectangles, Hole hole, bool rightSide)
        {
            Rectangle rect = null;
            int sideA, sideB;
            int searchSide;
            
            if (hole != null)
            {
                if(rightSide)
                    searchSide = hole.Rect.SideB;
                else
                    searchSide = hole.Rect.SideA;
                
                rect = findRectangle(rectangles, searchSide);
                sideA = searchSide - rect.SideA;
                sideB = searchSide - rect.SideB;

                if(rightSide)
                {
                    if(sideA < 0 && sideB < 0 && rect.SideA < rect.SideB)
                        rect.Rotate();
                    else if(sideA == 0)
                        rect.Rotate();
                    else if(sideA > 0 && sideB > 0 && rect.SideA > rect.SideB)
                        rect.Rotate();
                }
                else
                {
                    if(sideA < 0 && sideB < 0 && rect.SideA > rect.SideB)
                        rect.Rotate();
                    else if(sideB == 0)
                        rect.Rotate();
                    else if(sideA > 0 && sideB > 0 && rect.SideA < rect.SideB)
                        rect.Rotate();
                }
            }
            else
            {
                rect = findAnyMaxRectangle(rectangles);

                if (rightSide && rect.SideB != rect.LongerSide)
                    rect.Rotate();
                else if (!rightSide && rect.SideA != rect.LongerSide)
                    rect.Rotate();
            }
                        
            return rect;
        }

        /// <summary>
        /// Znajdowany prostok�t, kt�ry najlepiej by pasowa� do danego boku.
        /// </summary>
        /// <param name="rectangles">lista prostok�t�w</param>
        /// <param name="side">dany bok</param>
        /// <returns>znaleziony prostok�t</returns>
        private Rectangle findRectangle(List<Rectangle> rectangles, int side)
        {
            Rectangle rect = null;
            int sideA, sideB, min, minTemp;
            
            if (rectangles == null || rectangles.Count == 0)
                    return rect;

            rect = findMinRectangle(rectangles);
            if(rect.SideA > side && rect.SideB > side)
                return rect;
            
            min = side - rect.ShorterSide;
            foreach (Rectangle rc in rectangles)
            {
                sideA = side - rc.SideA;
                sideB = side - rc.SideB;

                if (sideA < 0 && sideB < 0)
                    continue;
                else if (sideA == 0)
                {
                    if (min > 0)
                    {
                        min = 0;
                        rect = rc;
                    }
                    else if (min == 0 && rc.Area > rect.Area)
                        rect = rc;
                }
                else if (sideB == 0)
                {
                    if (min > 0)
                    {
                        min = 0;
                        rect = rc;
                    }
                    else if (min == 0 && rc.Area > rect.Area)
                        rect = rc;
                }
                else 
                {
                    minTemp = Math.Min(sideA, sideB);

                    if(min > minTemp && minTemp >= 0)
                    {
                        min = minTemp;
                        rect = rc;
                    }
                    else if(min == minTemp && rc.Area > rect.Area)
                        rect = rc;
                }               
            }
           
            return rect;
        }

        /// <summary>
        /// Aktualizowane boki prostok�ta docelowego.
        /// </summary>
        /// <param name="rect">dodawany prostok�t</param>
        private void updateMaxValues(Rectangle rect)
        {
            if (rect.RightDown.X > Max_X)
                Max_X = rect.RightDown.X;
            if (rect.RightDown.Y > Max_Y)
                Max_Y = rect.RightDown.Y;
        }

        /// <summary>
        /// Aktualizowane dziury
        /// </summary>
        /// <param name="outsRight">prostok�ty na brzegu z prawej strony</param>
        /// <param name="outsDown">prostok�ty na brzegu z do�u</param>
        /// <param name="holesRight">dziury z prawej strony</param>
        /// <param name="holesDown">dziury z do�u</param>
        private void updateHoles(List<OutRect> outsRight, List<OutRect> outsDown, List<Hole> holesRight,
                                    List<Hole> holesDown)
        {
            int min_x = minHoles_X(holesRight, outsRight);
            int min_y = minHoles_Y(holesDown, outsDown);
            OutRect tempRight = null;
            OutRect tempDown = null;

            if (outsRight.Count > 0)
                tempRight = outsRight[outsRight.Count - 1].copy();
            if (outsDown.Count > 0)
                tempDown = outsDown[outsDown.Count - 1].copy();

            // nale�y zaktualizowa� dziury i prostok�ty na brzegu z prawej strony
            if (min_x > Min_X)
            {
                Min_X = min_x;
                updateOutsRight(outsRight);
                updateHolesRight(outsRight, outsDown, holesRight, holesDown);

                if (holesRight.Count > 0 && tempRight != null && holesRight[holesRight.Count - 1].OrientDown
                    && tempRight.RightDown.Y > Min_Y && holesRight[holesRight.Count - 1].Rect.LeftTop.Y >= Min_Y)
                {
                    holesRight.RemoveAt(holesRight.Count-1);
                    outsDown.Add(tempRight);
                    updateOutsDown(outsDown);
                    updateHolesDown(outsRight, outsDown, holesRight, holesDown);
                }
            }

            // nale�y zaktualizowa� dziury i prostok�ty na brzegu z do�u
            if (min_y > Min_Y)
            {
                Min_Y = min_y;
                updateOutsDown(outsDown);
                updateHolesDown(outsRight, outsDown, holesRight, holesDown);

                if (holesDown.Count > 0 && tempDown != null && holesDown[holesDown.Count - 1].OrientRight
                    && tempDown.RightDown.X > Min_X && holesDown[holesDown.Count - 1].Rect.LeftTop.X >= Min_X)
                {
                    holesDown.RemoveAt(holesDown.Count - 1);
                    outsRight.Add(tempDown);
                    updateOutsRight(outsRight);
                    updateHolesRight(outsRight, outsDown, holesRight, holesDown);
                }
            }
        }

        /// <summary>
        /// Aktualizacja prostok�t�w na brzegu z prawej strony.
        /// </summary>
        /// <param name="outsRight">prostok�ty na brzegu z prawej strony</param>
        private void updateOutsRight(List<OutRect> outsRight)
        {
            List<OutRect> outsToRemove = new List<OutRect>();

            foreach (OutRect or in outsRight)
            {
                if (!or.updateX(Min_X))
                    outsToRemove.Add(or);
            }

            // usuwane zb�dne prostok�ty
            foreach (OutRect or in outsToRemove)
                outsRight.Remove(or);
        }

        /// <summary>
        /// Aktualizacja prostok�t�w na brzegu z do�u.
        /// </summary>
        /// <param name="outsRight">prostok�ty na brzegu z do�u</param>
        private void updateOutsDown(List<OutRect> outsDown)
        {
            List<OutRect> outsToRemove = new List<OutRect>();

            foreach (OutRect or in outsDown)
            {
                if (!or.updateY(Min_Y))
                    outsToRemove.Add(or);
            }

            // usuwane zb�dne prostok�ty
            foreach (OutRect or in outsToRemove)
                outsDown.Remove(or);
        }

        /// <summary>
        /// Znajdowana minimalna wsp�rz�dna x dziury z prawej strony lub prost�k�ta na brzegu
        /// (je�li nie ma �adnej dziury).
        /// </summary>
        /// <param name="holesRight">dziury z prawej strony</param>
        /// <param name="outsRight">prostok�ty na brzegu z prawej strony</param>
        /// <returns>znaleziona wsp�rz�dna x</returns>
        private int minHoles_X(List<Hole> holesRight, List<OutRect> outsRight)
        {
            if(holesRight == null || holesRight.Count==0)
                return minOutsRight(outsRight);

            int result = -1;

            foreach (Hole hl in holesRight)
            {
                if (result == -1 && hl.Rect.LeftTop.Y < Min_Y)
                    result = hl.Rect.LeftTop.X;
                if (result > hl.Rect.LeftTop.X && hl.Rect.LeftTop.Y < Min_Y)
                    result = hl.Rect.LeftTop.X;                
            }

            if (result == -1)
                result = Min_X;

            result = Math.Min(result, minOutsRight(outsRight));

            return result;
        }

        /// <summary>
        /// Znajdowana minimalna wsp�rz�dna y dziury z do�u lub prost�k�ta na brzegu
        /// (je�li nie ma �adnej dziury).
        /// </summary>
        /// <param name="holesDown">dziury z do�u</param>
        /// <param name="outsDown">prostok�ty na brzegu z do�u</param>
        /// <returns>znaleziona wsp�rz�dna y</returns>
        private int minHoles_Y(List<Hole> holesDown, List<OutRect> outsDown)
        {
            if(holesDown == null || holesDown.Count==0)
                return minOutsDown(outsDown);
            
            int result = -1;

            foreach (Hole hl in holesDown)
            {
                if (result == -1 && hl.Rect.LeftTop.X < Min_X)
                    result = hl.Rect.LeftTop.Y;
                if (result > hl.Rect.LeftTop.Y && hl.Rect.LeftTop.X < Min_X)
                    result = hl.Rect.LeftTop.Y;
            }

            if (result == -1)
                result = Min_Y;

            result = Math.Min(result, minOutsDown(outsDown));

            return result;
        }

        /// <summary>
        /// Znajdowana minimalna wsp�rz�dna x prostok�ta na brzegu z prawej strony. 
        /// Je�li �adnego nie ma, to zwracana jest Min_X.
        /// </summary>
        /// <param name="outsRight">prostok�ty na brzegu z prawej strony</param>
        /// <returns>znaleziona wsp�rz�dna x</returns>
        private int minOutsRight(List<OutRect> outsRight)
        {
            if (outsRight == null || outsRight.Count == 0)
                return Min_X;
                        
            int result = outsRight[0].RightDown.X;
            int yPos = 0;
            OutRect last = outsRight[0];

            foreach (OutRect or in outsRight)
            {
                if (or.LeftTop.Y > yPos)
                    return Min_X;
                yPos = or.RightDown.Y;
                last = or;

                if (result > or.RightDown.X)
                    result = or.RightDown.X;
            }

            if (last.RightDown.Y < Min_Y)
                return Min_X;

            return result;
        }

        /// <summary>
        /// Znajdowana minimalna wsp�rz�dna y prostok�ta na brzegu z do�u. 
        /// Je�li �adnego nie ma, to zwracana jest Min_Y.
        /// </summary>
        /// <param name="outsDown">prostok�ty na brzegu z do�u</param>
        /// <returns>znaleziona wsp�rz�dna y</returns>
        private int minOutsDown(List<OutRect> outsDown)
        {
            if (outsDown == null || outsDown.Count == 0)
                return Min_Y;

            int result = outsDown[0].RightDown.Y;
            int xPos = 0;
            OutRect last = outsDown[0];

            foreach (OutRect or in outsDown)
            {
                if (or.LeftTop.X > xPos)
                    return Min_Y;
                xPos = or.RightDown.X;
                last = or;

                if (result > or.RightDown.Y)
                    result = or.RightDown.Y;
            }

            if (last.RightDown.X < Min_X)
                return Min_Y;

            return result;
        }

        /// <summary>
        /// Metoda sprawdza czy na li�cie jest dziura naro�na.
        /// </summary>
        /// <param name="holes">lista dziur</param>
        /// <returns>czy jest dziura naro�na</returns>
        private bool isCornerHole(List<Hole> holes)
        {
            foreach (Hole hl in holes)
            {
                if (hl.Corner)
                {
                    hl.Rect.Resize(new Point(Max_X, Max_Y));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Aktualizacja dziur z prawej strony.
        /// </summary>
        /// <param name="outsRight">prostok�ty na brzegu z prawej strony</param>
        /// <param name="outsDown">prostok�ty na brzegu z do�u</param>
        /// <param name="holesRight">dziury z prawej strony</param>
        /// <param name="holesDown">dziury z do�u</param>
        private void updateHolesRight(List<OutRect> outsRight, List<OutRect> outsDown, List<Hole> holesRight,
                                    List<Hole> holesDown)
        {
            OutRect orOld = null;
            Hole hole;

            // tworzona nowa lista dziur
            holesRight.Clear();

            foreach (OutRect or in outsRight)
            {
                if (orOld == null)
                {
                    if (or.LeftTop.Y > 0)
                    {
                        hole = new Hole(or.SideA, or.LeftTop.Y, new Point(Min_X, 0));
                        hole.OrientRight = true;
                        hole.NeighbourSecond = or;
                        holesRight.Add(hole);
                    }
                }
                else if (or.LeftTop.Y > orOld.RightDown.Y)
                {
                    hole = new Hole(Math.Min(or.SideA, orOld.SideA), or.LeftTop.Y - orOld.RightDown.Y,
                                new Point(Min_X, orOld.RightDown.Y));
                    hole.OrientRight = true;
                    hole.NeighbourOne = orOld;
                    hole.NeighbourSecond = or;
                    holesRight.Add(hole);
                }
                                
                orOld = or;
            }

            if (orOld != null && orOld.RightDown.Y < Max_Y)
            {
                hole = new Hole(orOld.SideA, Max_Y - orOld.RightDown.Y,
                            new Point(Min_X, orOld.RightDown.Y));
                hole.OrientDown = true;
                hole.OrientRight = true;
                hole.NeighbourOne = orOld;

                if (!isCornerHole(holesDown))
                {
                    hole.Corner = true;
                    if (outsDown.Count > 0)
                        hole.NeighbourSecond = outsDown[outsDown.Count - 1];
                }
                else
                {
                    if (holesDown.Count > 0)
                        hole.saveResize(hole.Rect.SideA, 
                            holesDown[holesDown.Count -1 ].Rect.LeftTop.Y - orOld.RightDown.Y);                    
                }

                holesRight.Add(hole);
            }

        }

        /// <summary>
        /// Aktualizacja dziur z do�u.
        /// </summary>
        /// <param name="outsRight">prostok�ty na brzegu z prawej strony</param>
        /// <param name="outsDown">prostok�ty na brzegu z do�u</param>
        /// <param name="holesRight">dziury z prawej strony</param>
        /// <param name="holesDown">dziury z do�u</param>
        private void updateHolesDown(List<OutRect> outsRight, List<OutRect> outsDown, List<Hole> holesRight,
                                    List<Hole> holesDown)
        {
            OutRect orOld = null;
            Hole hole;

            // tworzona nowa lista dziur
            holesDown.Clear();

            foreach (OutRect or in outsDown)
            {
                if (orOld == null)
                {
                    if (or.LeftTop.X > 0)
                    {
                        hole = new Hole(or.LeftTop.X, or.SideB, new Point(0, Min_Y));
                        hole.OrientDown = true;
                        hole.NeighbourSecond = or;
                        holesDown.Add(hole);
                    }
                }
                else if (or.LeftTop.X > orOld.RightDown.X)
                {
                    hole = new Hole(or.LeftTop.X - orOld.RightDown.X, Math.Min(or.SideB, orOld.SideB),
                                new Point(orOld.RightDown.X, Min_Y));
                    hole.OrientDown = true;
                    hole.NeighbourOne = orOld;
                    hole.NeighbourSecond = or;
                    holesDown.Add(hole);
                }
                                
                orOld = or;
            }

            if (orOld != null && orOld.RightDown.X < Max_X)
            {
                hole = new Hole(Max_X - orOld.RightDown.X, orOld.SideB,
                            new Point(orOld.RightDown.X, Min_Y));
                hole.OrientDown = true;
                hole.OrientRight = true;
                hole.NeighbourOne = orOld;

                if (!isCornerHole(holesRight))
                {
                    hole.Corner = true;
                    if (outsRight.Count > 0)
                        hole.NeighbourSecond = outsRight[outsRight.Count - 1];
                }
                else
                {
                    if(holesRight.Count > 0)
                        hole.saveResize(holesRight[holesRight.Count - 1].Rect.LeftTop.X - orOld.RightDown.X,
                                            hole.Rect.SideB);
                }

                holesDown.Add(hole);
            }

        }
        #endregion

        #region Klasy pomocnicze
        /// <summary>
        /// Klasa implementuj�ca funkcjonalno�� prostok�ta na brzegu
        /// </summary>
        private class OutRect : Rectangle
        {
            /// <summary>
            /// Czy dany prostok�t na brzegu jest do usuni�cia
            /// </summary>
            private bool _removed;

            /// <summary>
            /// Konstruktor 2-parametrowy
            /// </summary>
            /// <param name="sideA">szeroko��</param>
            /// <param name="sideB">wysoko��</param>
            public OutRect(int sideA, int sideB) : base(sideA, sideB) 
            {
                _removed = false;
            }

            /// <summary>
            /// Konstruktor 3-parametrowy
            /// </summary>
            /// <param name="sideA">szeroko��</param>
            /// <param name="sideB">wysoko��</param>
            /// <param name="pt">punkt lewy-g�rny</param>
            public OutRect(int sideA, int sideB, Point pt) : base(sideA, sideB, pt) 
            {
                _removed = false;
            }

            /// <summary>
            /// Bezpieczna zmiana rozmiaru.
            /// </summary>
            /// <param name="x">nowa szeroko��</param>
            /// <param name="y">nowa wysoko��</param>
            /// <returns>czy nast�pi�a zmiana</returns>
            public bool saveResize(int x, int y)
            {
                if (x <= 0 || y <= 0)
                    return false;

                Point pt = new Point(leftTop.X + x, leftTop.Y + y);
                Resize(pt);

                return true;
            }

            /// <summary>
            /// Aktualizacja szeroko�ci prostok�ta na brzegu.
            /// </summary>
            /// <param name="min_X">jaka mo�e by� minimalna wsp�rz�dna x prostok�ta</param>
            /// <returns>czy prostok�t zmini� rozmiar - je�li nie, to trzeba go usun��</returns>
            public bool updateX(int min_X)
            {
                int temp = rightDown.X;

                if (min_X < 0 || min_X >= temp)
                {
                    _removed = true;
                    return false;
                }

                Move(new Point(min_X, leftTop.Y));
                return saveResize(temp - min_X, SideB);
            }

            /// <summary>
            /// Aktualizacja wysoko�ci prostok�ta na brzegu.
            /// </summary>
            /// <param name="min_Y">jaka mo�e by� minimalna wsp�rz�dna y prostok�ta</param>
            /// <returns>czy prostok�t zmini� rozmiar - je�li nie, to trzeba go usun��</returns>
            public bool updateY(int min_Y)
            {
                int temp = rightDown.Y;

                if (min_Y < 0 || min_Y >= temp)
                {
                    _removed = true;
                    return false;
                }

                Move(new Point(leftTop.X, min_Y));
                return saveResize(SideA, temp - min_Y);
            }

            /// <summary>
            /// Czy prostok�t ma by� usuni�ty.
            /// </summary>
            public bool Removed
            {
                get { return _removed; }
                set { _removed = value; }
            }

            /// <summary>
            /// Tworzony prostok�t na brzegu b�d�cy kopi� danego.
            /// </summary>
            /// <returns></returns>
            public OutRect copy()
            {
                return new OutRect(SideA, SideB, new Point(leftTop.X, leftTop.Y));
            }
        }

        /// <summary>
        /// Klasa implementuje fukcjonalno�� dziury.
        /// </summary>
        private class Hole 
        {
            /// <summary>
            /// dziura z prawej strony
            /// </summary>
            private bool _orientRight;
            /// <summary>
            /// dziura z do�u
            /// </summary>
            private bool _orientDown;
            /// <summary>
            /// dziura noro�na
            /// </summary>
            private bool _corner;
            /// <summary>
            /// wielko�� dziury
            /// </summary>
            private Rectangle _rect;
            /// <summary>
            /// pierwszy s�siad dziury - prostok�t na brzegu
            /// </summary>
            private OutRect _neighbourOne;
            /// <summary>
            /// drugi s�siad dziury - prostok�t na brzegu
            /// </summary>
            private OutRect _neighbourSecond;            

            /// <summary>
            /// Konstruktor 3-parametrowy
            /// </summary>
            /// <param name="sideA">szeroko��</param>
            /// <param name="sideB">wysoko��</param>
            /// <param name="pt">puntk lewy-g�rny</param>
            public Hole(int sideA, int sideB, Point pt)
            {
                _rect = new Rectangle(sideA, sideB, pt);
                _orientDown = _orientRight = _corner = false;
                _neighbourOne = _neighbourSecond = null;
            }

            /// <summary>
            /// Bezpieczna zmiana rozmiaru.
            /// </summary>
            /// <param name="x">nowa szeroko��</param>
            /// <param name="y">nowa wysoko��</param>
            /// <returns>czy nast�pi�a zmina</returns>
            public bool saveResize(int x, int y)
            {
                if (x <= 0 || y <= 0)
                    return false;

                Point pt = new Point(_rect.LeftTop.X + x, _rect.LeftTop.Y + y);
                _rect.Resize(pt);

                return true;
            }

            /// <summary>
            /// Prostok�t wype�nia na dan� dziur�. Ewentualnie mo�e powsta� nowa dziura.
            /// </summary>
            /// <param name="rect">dany prostok�t</param>
            /// <param name="newHole">powsta�a nowa dziura</param>
            /// <returns>
            /// 0 - prostok�t w ca�o�ci wype�ni� dan� dziur� - dziura do usuni�cia
            /// 1 - prostok�t wype�ni� cze�ciowo dan� dziur� - mo�liwa aktualizacja Min_X lub Min_Y
            /// -1 - prostok�t wype�ni� cz�ciowo dan� dziur�
            /// </returns>
            public int filled(Rectangle rect, out Hole newHole)
            {
                Point leftTop = new Point(_rect.LeftTop.X, _rect.LeftTop.Y);
                //Point rightTop = new Point(_rect.RightDown.X, _rect.RightDown.Y);
                int sideA = rect.SideA - _rect.SideA;
                int sideB = rect.SideB - _rect.SideB;
                int result = 0;

                rect.Move(leftTop);
                newHole = null;

                if (sideA >=0 && sideB >= 0)
                    return 0;

                if (_orientRight && _orientDown)
                {
                    result = 1;

                    if (sideA < 0 && sideB < 0)
                    {
                        int holeSideA = _rect.SideA;

                        _rect.Move(new Point(rect.RightDown.X, rect.LeftTop.Y));
                        saveResize(-sideA, rect.SideB);
                        newHole = new Hole(holeSideA, -sideB, new Point(rect.LeftTop.X, rect.RightDown.Y));

                        if (_corner)
                        {
                            _corner = false;
                            newHole.OrientRight = true;
                            newHole.OrientDown = true;
                            newHole.Corner = true;
                        }
                    }
                    else if (sideA >= 0)
                    {
                        _rect.Move(new Point(rect.LeftTop.X, rect.RightDown.Y));
                        saveResize(rect.SideA, -sideB);
                    }
                    else if (sideB >= 0)
                    {
                        _rect.Move(new Point(rect.RightDown.X, rect.LeftTop.Y));
                        saveResize(-sideA, rect.SideB);
                    }
                }
                else if (_orientRight)
                {
                    if (sideB >= 0)
                    {
                        _rect.Move(new Point(rect.RightDown.X, rect.LeftTop.Y));
                        saveResize(-sideA, _rect.SideB);
                        result = 1;
                    }
                    else
                    {
                        _rect.Move(new Point(rect.LeftTop.X, rect.RightDown.Y));
                        if(_neighbourSecond != null)
                            saveResize(Math.Min(rect.SideA, _neighbourSecond.SideA), -sideB);
                        result = -1;
                    }
                }
                else if (_orientDown)
                {
                    if (sideA >= 0)
                    {
                        _rect.Move(new Point(rect.LeftTop.X, rect.RightDown.Y));
                        saveResize(_rect.SideA, -sideB);
                        result = 1;
                    }
                    else
                    {
                        _rect.Move(new Point(rect.RightDown.X, rect.LeftTop.Y));
                        if (_neighbourSecond != null)
                            saveResize(-sideA, Math.Min(rect.SideB, _neighbourSecond.SideB));
                        result = -1;
                    }
                }

                return result;
            }

            /// <summary>
            /// Metoda zwraca ilu jest s�siad�w danej dziury.
            /// </summary>
            /// <returns>ilo�� s�siad�w</returns>
            public int neighboursCount()
            {
                int result = 0;

                if (_neighbourOne != null)
                    result++;
                if (_neighbourSecond != null)
                    result++;

                return result;
            }

            /// <summary>
            /// Dziura z prawej strony.
            /// </summary>
            public bool OrientRight
            {
                get { return _orientRight; }
                set { _orientRight = value; }
            }

            /// <summary>
            /// Dziura z do�u.
            /// </summary>
            public bool OrientDown
            {
                get { return _orientDown; }
                set { _orientDown = value; }
            }

            /// <summary>
            /// Dziura noro�na.
            /// </summary>
            public bool Corner
            {
                get { return _corner; }
                set 
                {
                    if(_orientRight && _orientDown)
                        _corner = value; 
                }
            }

            /// <summary>
            /// Rozmiar dziury.
            /// </summary>
            public Rectangle Rect
            {
                get { return _rect; }
                set { _rect = value; }
            }

            /// <summary>
            /// Pierwszy s�siad.
            /// </summary>
            public OutRect NeighbourOne
            {
                get { return _neighbourOne; }
                set { _neighbourOne = value; }
            }

            /// <summary>
            /// Drugi s�siad.
            /// </summary>
            public OutRect NeighbourSecond
            {
                get { return _neighbourSecond; }
                set { _neighbourSecond = value; }
            }
        }
        #endregion
    }
}
