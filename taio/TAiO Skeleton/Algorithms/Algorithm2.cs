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

            RectangleContainer container = new RectangleContainer();
            rects = new RectanglesList(rectanglesList);

            Rectangle resultRectangle = rects.GetLargestRect();
            container.InsertRectangle(resultRectangle, Rectangle.Orientation.Vertical);

            //int iter = 0;
            Rectangle w;
            while (!rects.IsEmpty() && running)
            {
                //iter++;

                w = container.MaxCorrectRect;
                //doklejanie
                /*if (w.RectangleOrientation != Rectangle.Orientation.Vertical)
                    w.Rotate();*/

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

                //zalepianie bêdzie k-krokowe (k jak kiedyœ)
                //k-ty krok to SimpleMend czyli proste zalepianie (³atanie)
                //w kazdym kroku powinien byæ chyba tylko jeden emptyField?
                int pathWidth = n.ShorterSide;
                for (int i = 0; i < k - 1; i++)
                {
                    Rectangle t = rects.PeekSmallestNotShorterThan(pathWidth);
                    if (t == null)
                        break;

                    //TODO - to zmienic - nie jest tak prosto, bo moga nie pasowac wymiary prostokata
                    //jeœli jest wiêkszy ni¿ puste, przejdŸmy od razu do k-tego kroku zalepiania
                    if (t.Area >= container.EmptyFieldsArea)
                    {
                        if (SimpleMend(container, t))
                        {
                            simpleMendDone = true;
                            break;
                        }
                    }

                    //tutaj zmienic tak, by uwzglednic czy ostatnio dodany by³ z boku czy z dolu
                    // z boku
                    Point nLT;
                    if (addingToLeft)
                    {
                        if (/*t.ShorterSide >= pathWidth &&*/ !t.RectangleOrientation.Equals(Rectangle.Orientation.Horizontal))
                            t.Rotate();
                        //nLT = new Point(n.RightDown.X - t.SideA, n.RightDown.Y - t.SideB);
                        nLT = new Point(n.RightDown.X - t.SideA, n.RightDown.Y);
                    }
                    else
                    {
                        if (/*t.ShorterSide >= pathWidth && */!t.RectangleOrientation.Equals(Rectangle.Orientation.Vertical))
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

            //sprawdzaæ jeszcze warunek poprawnoœci (mo¿e nowy prostok¹t - ostatni, który spe³nia³ warunki)
            resultRectangle = container.MaxCorrectRect;
            rectangle = resultRectangle;

            return resultRectangle;
        }

        /// <summary>
        /// Tries to mend empty fields using smallest covering rectangles.
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
        /// Tries to mend first empty field on container's EmptyFields list using rectangle r
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



        private bool IsShapeConditionValid(int sideA, int sideB)
        {
            double prop = sideA / (double)sideB;
            if (prop >= 0.5 && prop <= 2)
                return true;
            return false;
        }


    }
}
