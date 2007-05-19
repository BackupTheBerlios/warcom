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
        private int sideMax = 0;        // maksymalna d³ugoœæ d³u¿szego boku
        private bool running = true;
        private Rectangle rectangle;
        private string tag = "AW3";
        private double ratio = 2.0;         // maksymalny stosunek d³u¿szego boku do krótszego

        private int Min_X;
        private int Min_Y;
        private int Max_X;
        private int Max_Y;
        //int counter;
        #endregion

        #region Metody podstawowe
        /// <summary>
        /// Metoda budujê prostok¹t o maksymalnym polu spoœród prostok¹tów znajduj¹cych siê 
        /// na liœcie.
        /// </summary>
        /// <param name="rectangles">lista prostok¹tów</param>
        /// <returns>maksymalny prostok¹t jaki da siê zbudowaæ za pomoc¹ tego algorytmu</returns>
        public Rectangle ComputeMaximumRectangle(List<Rectangle> rectangles)
        {
            Rectangle maxRect = null;

            try
            {
                if (rectangles == null)
                    throw new ArgumentNullException();
                if (rectangles.Count == 0)
                    throw new ArgumentException("Empty rectangles list");

                // algorytm w³aœciwy
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
        /// Metoda potrzebna do zatrzymywania obliczeñ algorytmu.
        /// </summary>
        public void StopThread()
        {
            running = false;
        }

        /// <summary>
        /// Akcesor do wyliczonego maksymalnego prostok¹ta
        /// </summary>
        /// <returns>maksymalny prostok¹t</returns>
        public Rectangle GetRectangle()
        {
            return rectangle;
        }

        /// <summary>
        /// Akcesor do zmiennej opisuj¹cej algortym.
        /// </summary>
        /// <returns>informacja opisuj¹ca dany algorytm</returns>
        public string GetTag()
        {
            return tag;
        }
        #endregion

         #region Metody pomocnicze
        /// <summary>
        /// Metoda budujê prostok¹t o maksymalnym polu spoœród prostok¹tów znajduj¹cych siê 
        /// na liœcie.
        /// </summary>
        /// <param name="rectangles">lista prostok¹tów</param>
        /// <returns>maksymalny prostok¹t, jaki da siê zbudowaæ za pomoc¹ tego algorytmu</returns>
        private Rectangle computeRectangles(List<Rectangle> rectangles)
        {
            //counter = 0;

            int maxArea = computeMaximumArea(rectangles);
            setMaximumSides(maxArea);
            List<Rectangle> correctRects = removeTooBigRectangles(rectangles);

            // brak prawid³owych prostok¹tów
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

            // dopóki s¹ jakieœ prostok¹ty do wykorzystania oraz algorytm nie zosta³ przerwany
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

        private bool correctRectangleFound(Rectangle rect)
        {
            if (rect == null)
                return false;

            if (rect.LongerSide / rect.ShorterSide > ratio)
                return false;

            return true;
        }

        private void stickRectangle(List<OutRect> outsRight, List<OutRect> outsDown, 
                            List<Hole> holesRight, List<Hole> holesDown, Rectangle rect,
                            bool rightSide)
        {
            if (rightSide)
                stickRectangleRight(outsRight, outsDown, holesRight, holesDown, rect);
            else
                stickRectangleDown(outsRight, outsDown, holesRight, holesDown, rect);
        }

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

            if (hole.OrientDown && hole.OrientRight && result == 0)
            {
                if(rect.RightDown.X > Min_X && !rightSide)
                    outsRight.Add(new OutRect(rect.RightDown.X - Min_X, rect.SideB,
                                    new Point(Min_X, rect.LeftTop.Y)));

                if (rect.RightDown.Y > Min_Y && rightSide)
                    outsDown.Add(new OutRect(rect.SideA, rect.RightDown.Y - Min_Y,
                                    new Point(rect.LeftTop.X, Min_Y)));

            }
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
        /// Wyliczana suma wszystkich prostok¹tów wejœciowych.
        /// </summary>
        /// <param name="rects">prostok¹ty wejœciowe</param>
        /// <returns>suma pól prostok¹tów</returns>
        private int computeMaximumArea(List<Rectangle> rects)
        {
            int area = 0;
            foreach (Rectangle rect in rects)
                area += rect.Area;
            return area;
        }

        /// <summary>
        /// Wyliczane maksymalne boki prostok¹ta na podstawie warunku kszta³tu
        /// </summary>
        /// <param name="area">pole maksymalnego prostok¹ta</param>
        private void setMaximumSides(int area)
        {
            // shorterSide i tempMax musz¹ mieæ zawsze wartoœæi ca³kowite
            double shorterSide = (int)Math.Sqrt(area);
            double tempMax = shorterSide;
            double tempValDouble;
            int tempValInt;

            // stosunek d³u¿szego boku do krótszego nie mo¿e byæ wiêkszy ni¿ 2.0
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
        /// Odrzucana prostok¹ty o zbyt du¿ym wiêkszym boku (one nigdy nie wejd¹ w sk³ad
        /// wyjœciowego prostok¹ta).
        /// </summary>
        /// <param name="rects">prostok¹ty wejœciowe</param>
        /// <returns>lista poprawnych prostok¹tów</returns>
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

        private Rectangle correctMaxRectangle(List<Rectangle> rects)
        {
            Rectangle result = null, rectX, rectY;
            List<Rectangle> rectsX, rectsY;
            bool ok = false;

            if(rects == null || rects.Count == 0)
                return result;

            while(!ok)
            {
                RectangleContainer rc = new RectangleContainer();
                rc.InsertRectangles(rects);

                if(rc.IsCorrectRectangle)
                    return rc.MaxCorrectRect;
                
                rectsX = copyRectangles(rects);
                if(Min_X > 0)
                    Min_X--;
                else
                    return result;
                correctRectangles(rectsX);
                RectangleContainer rc1 = new RectangleContainer();
                rc1.InsertRectangles(rectsX);
                rectX = rc1.MaxCorrectRect;

                rectsY = copyRectangles(rects);
                if(Min_Y > 0)
                    Min_Y--;
                else
                    return result;
                Min_X++;
                correctRectangles(rectsY);
                RectangleContainer rc2 = new RectangleContainer();
                rc2.InsertRectangles(rectsY);
                rectY = rc2.MaxCorrectRect;

                if (rc1.IsCorrectRectangle && rc2.IsCorrectRectangle)
                {
                    if (rectX.Area > rectY.Area)
                        return rectX;
                    else
                        return rectY;
                }
                else if (rc1.IsCorrectRectangle)
                    return rectX;
                else if (rc2.IsCorrectRectangle)
                    return rectY;


                Min_X--;
            } 

            return result;
        }

        private List<Rectangle> copyRectangles(List<Rectangle> rects)
        {
            List<Rectangle> newRects;

            if (rects == null)
                return null;

            newRects = new List<Rectangle>();
            foreach (Rectangle rc in rects)
            {
                Rectangle t = new Rectangle(rc.SideA, rc.SideB, rc.LeftTop);
                t.Number = rc.Number;
                newRects.Add(t);
            }

            return newRects;
        }

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

        private void updateMaxValues(Rectangle rect)
        {
            if (rect.RightDown.X > Max_X)
                Max_X = rect.RightDown.X;
            if (rect.RightDown.Y > Max_Y)
                Max_Y = rect.RightDown.Y;
        }

        private void updateHoles(List<OutRect> outsRight, List<OutRect> outsDown, List<Hole> holesRight,
                                    List<Hole> holesDown)
        {
            ///TODO - poprawiæ tutaj
            int min_x = minHoles_X(holesRight, outsRight);
            int min_y = minHoles_Y(holesDown, outsDown);
            OutRect tempRight = null;
            OutRect tempDown = null;

            if (outsRight.Count > 0)
                tempRight = outsRight[outsRight.Count - 1].copy();
            if (outsDown.Count > 0)
                tempDown = outsDown[outsDown.Count - 1].copy();

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

        private void updateOutsRight(List<OutRect> outsRight)
        {
            List<OutRect> outsToRemove = new List<OutRect>();

            foreach (OutRect or in outsRight)
            {
                if (!or.updateX(Min_X))
                    outsToRemove.Add(or);
            }

            foreach (OutRect or in outsToRemove)
                outsRight.Remove(or);
        }

        private void updateOutsDown(List<OutRect> outsDown)
        {
            List<OutRect> outsToRemove = new List<OutRect>();

            foreach (OutRect or in outsDown)
            {
                if (!or.updateY(Min_Y))
                    outsToRemove.Add(or);
            }

            foreach (OutRect or in outsToRemove)
                outsDown.Remove(or);
        }

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

        private bool isCornerHole(List<Hole> holes)
        {
            ///TODO - sprawdziæ czy dziury s¹ zawsze w dobrej kolejnoœci
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
        private class OutRect : Rectangle
        {
            private bool _removed;

            public OutRect(int sideA, int sideB) : base(sideA, sideB) 
            {
                _removed = false;
            }
            public OutRect(int sideA, int sideB, Point pt) : base(sideA, sideB, pt) 
            {
                _removed = false;
            }

            public bool saveResize(int x, int y)
            {
                if (x <= 0 || y <= 0)
                    return false;

                Point pt = new Point(leftTop.X + x, leftTop.Y + y);
                Resize(pt);

                return true;
            }

            public bool updateX(int max_X)
            {
                int temp = rightDown.X;

                if(max_X < 0 || max_X >= temp)
                {
                    _removed = true;
                    return false;
                }

                Move(new Point(max_X, leftTop.Y));
                return saveResize(temp - max_X, SideB);
            }

            public bool updateY(int max_Y)
            {
                int temp = rightDown.Y;

                if (max_Y < 0 || max_Y >= temp)
                {
                    _removed = true;
                    return false;
                }

                Move(new Point(leftTop.X, max_Y));
                return saveResize(SideA, temp - max_Y);
            }

            public bool Removed
            {
                get { return _removed; }
                set { _removed = value; }
            }

            public OutRect copy()
            {
                return new OutRect(SideA, SideB, new Point(leftTop.X, leftTop.Y));
            }
        }

        private class Hole 
        {
            private bool _orientRight;
            private bool _orientDown;
            private bool _corner;
            private Rectangle _rect;
            private OutRect _neighbourOne;
            private OutRect _neighbourSecond;            

            public Hole(int sideA, int sideB, Point pt)
            {
                _rect = new Rectangle(sideA, sideB, pt);
                _orientDown = _orientRight = _corner = false;
                _neighbourOne = _neighbourSecond = null;
            }

            public bool saveResize(int x, int y)
            {
                if (x <= 0 || y <= 0)
                    return false;

                Point pt = new Point(_rect.LeftTop.X + x, _rect.LeftTop.Y + y);
                _rect.Resize(pt);

                return true;
            }

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

            public int neighboursCount()
            {
                int result = 0;

                if (_neighbourOne != null)
                    result++;
                if (_neighbourSecond != null)
                    result++;

                return result;
            }

            public bool OrientRight
            {
                get { return _orientRight; }
                set { _orientRight = value; }
            }

            public bool OrientDown
            {
                get { return _orientDown; }
                set { _orientDown = value; }
            }

            public bool Corner
            {
                get { return _corner; }
                set 
                {
                    if(_orientRight && _orientDown)
                        _corner = value; 
                }
            }

            public Rectangle Rect
            {
                get { return _rect; }
                set { _rect = value; }
            }

            public OutRect NeighbourOne
            {
                get { return _neighbourOne; }
                set { _neighbourOne = value; }
            }

            public OutRect NeighbourSecond
            {
                get { return _neighbourSecond; }
                set { _neighbourSecond = value; }
            }
        }
        #endregion
    }
}
