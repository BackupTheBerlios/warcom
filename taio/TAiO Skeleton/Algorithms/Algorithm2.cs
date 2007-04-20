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
        private int k = 1;

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
            container.InsertRectangle(resultRectangle,Rectangle.Orientation.Vertical);

            //int iter = 0;
            Rectangle w;
            while (!rects.IsEmpty() && running)
            {
                //iter++;
                
                w = container.MaxCorrectRect;
                //doklejanie
                if (w.RectangleOrientation != Rectangle.Orientation.Vertical)
                    w.Rotate();
                
                Rectangle n = rects.GetLongestRect(w.LongerSide);
                if (n == null)
                    break;
                if (!n.RectangleOrientation.Equals(w.RectangleOrientation))
                    n.Rotate();
                n.Move(new Point(w.RightDown.X, 0));
                container.InsertRectangle(n);

                if (container.IsCorrectRectangle)
                    continue;

                //zalepianie bêdzie k-krokowe (k jak kiedyœ)
                /*for (int i = 0; i < k; i++)
                {

                }*/

                List<Rectangle> cover = new List<Rectangle>();
                foreach(Rectangle empty in container.EmptyFields)
                {
                    Rectangle t = rects.GetSmallestCovering(empty.SideA, empty.SideB);
                    if (t != null)
                    {
                        //TODO: tu sprawdzac jeszcze czy nie wyjdzie poza
                        if(!empty.RectangleOrientation.Equals(t.RectangleOrientation))
                            t.Rotate();
                        t.Move(new Point(empty.RightDown.X - t.SideA, empty.RightDown.Y - t.SideB));
                        cover.Add(t);
                    }
                }
                foreach (Rectangle r in cover)
                    container.InsertRectangle(r);
            }

            //if (container.IsCorrectRectangle)
            
            resultRectangle = container.MaxCorrectRect;
            rectangle = resultRectangle;

            return resultRectangle;
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
