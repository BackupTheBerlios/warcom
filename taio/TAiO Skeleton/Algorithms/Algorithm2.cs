using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Taio.Algorithms
{
    class Algorithm2 : IAlgorithm
    {
        private bool running = true;
        private Rectangle rectangle;
        private string tag = "AW2";

        private RectanglesList rects;
        //liczba kroków w zalepianiu
        private int k = 5;

        public void StopThread()
        {
            running = false;
        }

        public Rectangle GetRectangle()
        {
            return rectangle;
        }

        public string GetTag()
        {
            return tag;
        }

        public Rectangle ComputeMaximumRectangle(List<Rectangle> rectanglesList)
        {
            if (rectanglesList == null)
                throw new ArgumentNullException();
            if (rectanglesList.Count == 0)
                throw new ArgumentException("Empty rectangles list");

            k =  (int)(rectanglesList.Count * 0.75);

            RectangleContainer container = new RectangleContainer();
            rects = new RectanglesList(rectanglesList);

            rectangle = rects.GetLargestRect();
            container.InsertRectangle(rectangle, Rectangle.Orientation.Vertical);

            Rectangle bestWithShapeCondition = null;
            if(IsShapeConditionValid(rectangle.SideA, rectangle.SideB))
                bestWithShapeCondition = rectangle;

            Rectangle w;
            while (!rects.IsEmpty() && running)
            {
                w = container.MaxCorrectRect;
                if (IsShapeConditionValid(w.SideA, w.SideB))
                    bestWithShapeCondition = w;

                Rectangle n = rects.GetLongestRect(w.LongerSide);
                if (n == null)
                    break;

                bool simpleMendDone = false;
                bool addingToLeft = true;

                if (w.RectangleOrientation.Equals(Rectangle.Orientation.Vertical))
                {
                    n.Move(new Point(w.RightDown.X, 0));
                    if (!n.RectangleOrientation.Equals(w.RectangleOrientation))
                        n.Rotate();
                }
                else
                {
                    addingToLeft = false;
                    if (!n.RectangleOrientation.Equals(w.RectangleOrientation))
                        n.Rotate();
                    n.Move(new Point(0, w.RightDown.Y));
                }

                container.InsertRectangle(n);

                if (container.IsCorrectRectangle)
                    continue;

                //k-ty krok to SimpleMend czyli proste zalepianie
                int pathWidth = n.ShorterSide;
                for (int i = 0; i < k - 1; i++)
                {
                    Rectangle t = rects.PeekSmallestNotShorterThan(pathWidth);
                    if (t == null)
                        break;

                    //jeœli jest wiêkszy ni¿ puste, przejdŸmy od razu do k-tego kroku zalepiania
                    if (t.Area >= container.EmptyFieldsArea)
                    {
                        if (SimpleMend(container, t))
                        {
                            simpleMendDone = true;
                            break;
                        }
                    }

                    //uwzgledniamy czy ostatnio dodany prostok¹t by³ doklejany z boku czy z dolu
                    // z boku
                    Point nLT;
                    if (addingToLeft)
                    {
                        if (!t.RectangleOrientation.Equals(Rectangle.Orientation.Horizontal))
                            t.Rotate();
                        nLT = new Point(n.RightDown.X - t.SideA, n.RightDown.Y);
                    }
                    else
                    {
                        if (!t.RectangleOrientation.Equals(Rectangle.Orientation.Vertical))
                            t.Rotate();
                        nLT = new Point(n.RightDown.X, n.RightDown.Y - t.SideB);
                    }

                    if (nLT.X >= 0 && nLT.Y >= 0)
                    {
                        t.Move(nLT);
                        container.InsertRectangle(t);
                        rects.RemoveRectangle(t);
                        n = t;
                    }
                }
                if (!simpleMendDone)
                    SimpleMend(container);

                if (!container.IsCorrectRectangle)
                    break;
            }

            rectangle = container.MaxCorrectRect;

            if (!IsShapeConditionValid(rectangle.SideA, rectangle.SideB))
            {
                //Console.WriteLine("Najlepsze znalezione rozwi¹zanie " + rectangle + " nie spe³nia warunku 2:1");
                if (bestWithShapeCondition == null)
                {
                    int bestIndex = -1, largestArea = 0; int i = 0;
                    foreach (Rectangle rec in rectanglesList)
                    {
                        if (IsShapeConditionValid(rec.SideA, rec.SideB) && largestArea < rec.Area)
                        {
                            bestIndex = i;
                            largestArea = rec.Area;
                        }
                        i++;
                    }

                    if (bestIndex > 0)
                        bestWithShapeCondition = rectanglesList[bestIndex];
                }

                rectangle = bestWithShapeCondition;
                //Console.WriteLine("Za rozwi¹zanie przyjmujê " + rectangle);
            }
            //rectangle = resultRectangle;

            return rectangle;
        }

        /// <summary>
        /// Próbuje za³ataæ puste pola u¿ywaj¹c najmniejszych pokrywaj¹cych prostok¹tów
        /// </summary>
        /// <param name="container"></param>
        private void SimpleMend(RectangleContainer container)
        {
            List<Rectangle> cover = new List<Rectangle>();
            foreach (Rectangle empty in container.EmptyFields)
            {
                Rectangle t = rects.PeekSmallestCovering(empty.SideA, empty.SideB);
                if (t != null)
                {
                    if (!empty.RectangleOrientation.Equals(t.RectangleOrientation))
                        t.Rotate();
                    Point nLT = new Point(empty.RightDown.X - t.SideA, empty.RightDown.Y - t.SideB);
                    if (nLT.X >= 0 && nLT.Y >= 0)
                    {
                        t.Move(nLT);
                        rects.RemoveRectangle(t);
                    }
                    cover.Add(t);
                }
            }
            foreach (Rectangle r in cover)
                container.InsertRectangle(r);
        }

        /// <summary>
        /// Próbuje pierwsze puste pole z listy u¿ywaj¹c najmniejszych pokrywaj¹cych prostok¹tów
        /// </summary>
        /// <param name="container"></param>
        /// <param name="r"></param>
        private bool SimpleMend(RectangleContainer container, Rectangle r)
        {
            if (container == null || r == null)
                throw new ArgumentNullException();

            Rectangle empty = container.EmptyFields[0];
            if (empty == null)
                return false;


            if (empty.LongerSide > r.LongerSide || empty.ShorterSide > r.ShorterSide)
                return false;

            if (!empty.RectangleOrientation.Equals(r.RectangleOrientation))
                r.Rotate();
            Point nLT = new Point(empty.RightDown.X - r.SideA, empty.RightDown.Y - r.SideB);
            if (nLT.X >= 0 && nLT.Y >= 0)
            {
                r.Move(nLT);
                rects.RemoveRectangle(r);
            }
            container.InsertRectangle(r);
            return true;
        }


        /// <summary>
        /// Sprawdza warunek poprawnoœci prostok¹ta
        /// </summary>
        /// <param name="sideA"></param>
        /// <param name="sideB"></param>
        /// <returns></returns>
        private bool IsShapeConditionValid(int sideA, int sideB)
        {
            double prop = sideA / (double)sideB;
            if (prop >= 0.5 && prop <= 2)
                return true;
            return false;
        }


    }
}
