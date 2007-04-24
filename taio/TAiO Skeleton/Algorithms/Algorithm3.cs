using System;
using System.Collections.Generic;
using System.Text;

namespace Taio.Algorithms
{
    /// <summary>
    /// Algorytm 3 (idea oparta na grze Tetris).
    /// </summary>
    class Algorithm3 : IAlgorithm
    {
        private int maximumSide = 0;        // maksymalna d³ugoœæ d³u¿szego boku
        private bool running = true;        
        private Rectangle rectangle;
        private string tag = "AW3";
        private double ratio = 2.0;         // maksymalny stosunek d³u¿szego boku do krótszego

        /// <summary>
        /// Metoda budujê prostok¹t o maksymalnym polu spoœród prostok¹tów znajduj¹cych siê 
        /// na liœcie.
        /// </summary>
        /// <param name="rectangles">lista prostok¹tów</param>
        /// <returns>maksymalny prostok¹t, jaki da siê zbudowaæ za pomoc¹ tego algorytmu</returns>
        public Rectangle ComputeMaximumRectangle(List<Rectangle> rectangles)
        {
            if (rectangles == null)
                throw new ArgumentNullException();
            if (rectangles.Count == 0)
                throw new ArgumentException("Empty rectangles list");

            int maxArea = computeMaximumArea(rectangles);
            setMaximumSides(maxArea);
            List<Rectangle> correctRects = removeTooBigRectangles(rectangles);
            correctRects = sortRectangles(correctRects);

            Rectangle result = null;
            Rectangle rectTemp;
            Rectangle rectToFill;                               // prostok¹t, które bêdzie dodawany do listy rects
            List<Rectangle> rects = new List<Rectangle>();      // prostok¹ty z których, bêdzie budowany prostok¹t wyjœciowy
            List<Rectangle> holes = new List<Rectangle>();      // dziury
            List<int> rectsFill = new List<int>();              // lista prostok¹tów, które czêœciowo wype³niaj¹ dziury
            List<int> rectHoles = new List<int>();              // indeksy dziur, które czêœciowo wype³nia dany prostok¹t    
            //bool isHoleUP = false;                              // jest dziura na górze
            //bool isHoleRight = false;                           // jest dziura po prawej stronie  
            int index;                                          // index danego prostok¹ta

            // dopóki s¹ jakieœ prostok¹ty do wykorzystania oraz algorytm nie zosta³ przerwany
            while (running && correctRects.Count > 0)
            {
                // pobierany pierwszy prostok¹t z listy
                rectTemp = correctRects[0];
                index = 0;
                
                // czyszczone listy
                rectsFill.Clear();
                rectHoles.Clear();

                rectToFill = rectTemp;

                //dodawany prostok¹t
                rects.Add(rectToFill);
                // usuwany prostok¹t
                correctRects.RemoveAt(index);
            }

            result = correctRectangle(rects);
            rectangle = result;
            return result;
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
                maximumSide = (int)tempMax;

                do
                {
                    tempMax++;
                    tempValDouble = (double)area / tempMax;
                    tempValInt = (int)(area / tempMax);

                    if (tempMax / tempValDouble > ratio)
                        return;
                } while (tempValDouble - (double)tempValInt != 0.0);

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
                if (rect.LongerSide <= maximumSide)
                {
                    Rectangle t = new Rectangle(rect.SideA, rect.SideB);
                    t.Number = rect.Number;
                    correctRects.Add(t);
                }
            return correctRects;
        }

        /// <summary>
        /// Metoda sortuje listê prostok¹tów malej¹co pod wzglêdem d³ugoœci d³u¿szego boku.
        /// W przypadku, gdy prostok¹ty maj¹ taki sam d³u¿szy bok, brane jest jako II kryterium
        /// pole powierzchnie (sortowanie w kolejnoœci malej¹cej). 
        /// Zastosowane jest sortowanie przez wstawianie.
        /// </summary>
        /// <param name="rects">lista prostok¹tów do posortowania</param>
        /// <returns>posortowana lista prostok¹tów</returns>
        private List<Rectangle> sortRectangles(List<Rectangle> rects)
        {
            List<Rectangle> sortedRectangles = new List<Rectangle>();
            Rectangle rectTemp;            
            int index;
            bool wasInserted;
            
            while(rects.Count > 0)
            {
                rectTemp = rects[0];
                rects.RemoveAt(0);
                index = 0;
                wasInserted = false;
                
                foreach(Rectangle r in sortedRectangles)
                {
                    if(rectTemp.LongerSide > r.LongerSide)
                    {
                        sortedRectangles.Insert(index, rectTemp);
                        wasInserted = true;
                        break;
                    }
                    else if (rectTemp.LongerSide == r.LongerSide && rectTemp.Area > r.Area)
                    {
                        sortedRectangles.Insert(index, rectTemp);
                        wasInserted = true;
                        break;
                    }
                    index++;
                }
                if (!wasInserted)
                    sortedRectangles.Insert(index, rectTemp);
            }
            
            return sortedRectangles;
        }

        private Rectangle correctRectangle(List<Rectangle> rects)
        {
            Rectangle result;
            RectangleContainer rc = new RectangleContainer();
            rc.InsertRectangles(rects);

            result = rc.MaxCorrectRect;

            return result;
        }
    }
}
