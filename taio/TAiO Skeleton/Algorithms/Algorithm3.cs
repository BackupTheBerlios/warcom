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
        private int maximumSide = 0;        // maksymalna d�ugo�� d�u�szego boku
        private bool running = true;        
        private Rectangle rectangle;
        private string tag = "AW3";
        private double ratio = 2.0;         // maksymalny stosunek d�u�szego boku do kr�tszego

        /// <summary>
        /// Metoda buduj� prostok�t o maksymalnym polu spo�r�d prostok�t�w znajduj�cych si� 
        /// na li�cie.
        /// </summary>
        /// <param name="rectangles">lista prostok�t�w</param>
        /// <returns>maksymalny prostok�t, jaki da si� zbudowa� za pomoc� tego algorytmu</returns>
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
            Rectangle rectToFill;                               // prostok�t, kt�re b�dzie dodawany do listy rects
            List<Rectangle> rects = new List<Rectangle>();      // prostok�ty z kt�rych, b�dzie budowany prostok�t wyj�ciowy
            List<Rectangle> holes = new List<Rectangle>();      // dziury
            List<int> rectsFill = new List<int>();              // lista prostok�t�w, kt�re cz�ciowo wype�niaj� dziury
            List<int> rectHoles = new List<int>();              // indeksy dziur, kt�re cz�ciowo wype�nia dany prostok�t    
            //bool isHoleUP = false;                              // jest dziura na g�rze
            //bool isHoleRight = false;                           // jest dziura po prawej stronie  
            int index;                                          // index danego prostok�ta

            // dop�ki s� jakie� prostok�ty do wykorzystania oraz algorytm nie zosta� przerwany
            while (running && correctRects.Count > 0)
            {
                // pobierany pierwszy prostok�t z listy
                rectTemp = correctRects[0];
                index = 0;
                
                // czyszczone listy
                rectsFill.Clear();
                rectHoles.Clear();

                rectToFill = rectTemp;

                //dodawany prostok�t
                rects.Add(rectToFill);
                // usuwany prostok�t
                correctRects.RemoveAt(index);
            }

            result = correctRectangle(rects);
            rectangle = result;
            return result;
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
        /// Odrzucana prostok�ty o zbyt du�ym wi�kszym boku (one nigdy nie wejd� w sk�ad
        /// wyj�ciowego prostok�ta).
        /// </summary>
        /// <param name="rects">prostok�ty wej�ciowe</param>
        /// <returns>lista poprawnych prostok�t�w</returns>
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
        /// Metoda sortuje list� prostok�t�w malej�co pod wzgl�dem d�ugo�ci d�u�szego boku.
        /// W przypadku, gdy prostok�ty maj� taki sam d�u�szy bok, brane jest jako II kryterium
        /// pole powierzchnie (sortowanie w kolejno�ci malej�cej). 
        /// Zastosowane jest sortowanie przez wstawianie.
        /// </summary>
        /// <param name="rects">lista prostok�t�w do posortowania</param>
        /// <returns>posortowana lista prostok�t�w</returns>
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
