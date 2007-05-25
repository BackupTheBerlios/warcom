using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Text;
using Taio.Algorithms;

namespace Taio
{
    class TestAlgorithm3
    {
        [STAThread]
        static void Main(string[] args)
        {
            // lista prostok¹tów
            List<Rectangle> rectangles = new List<Rectangle>();
            /*rectangles.Add(new Rectangle(10, 10));
            rectangles.Add(new Rectangle(10, 15));
            rectangles.Add(new Rectangle(5, 15));
            rectangles.Add(new Rectangle(20, 15));
            rectangles.Add(new Rectangle(10, 100));
            rectangles.Add(new Rectangle(79, 25));*/

            /*rectangles.Add(new Rectangle(20, 20));
            rectangles.Add(new Rectangle(10, 20));
            rectangles.Add(new Rectangle(5, 15));
            rectangles.Add(new Rectangle(10, 10));*/

            /*rectangles.Add(new Rectangle(10, 3));
            rectangles.Add(new Rectangle(2, 10));*/

            /*rectangles.Add(new Rectangle(5, 2));
            rectangles.Add(new Rectangle(3, 3));
            rectangles.Add(new Rectangle(2, 5));
            rectangles.Add(new Rectangle(1, 2));
            rectangles.Add(new Rectangle(2, 2));
            rectangles.Add(new Rectangle(5, 4));
            rectangles.Add(new Rectangle(3, 5));
            rectangles.Add(new Rectangle(4, 3));
            rectangles.Add(new Rectangle(2, 5));
            rectangles.Add(new Rectangle(1, 3));*/

            rectangles.Add(new Rectangle(1, 1));
            rectangles.Add(new Rectangle(1, 1)); 
            rectangles.Add(new Rectangle(1, 1));
            rectangles.Add(new Rectangle(1, 1));
            rectangles.Add(new Rectangle(1, 1));
            rectangles.Add(new Rectangle(1, 1));
            /*rectangles.Add(new Rectangle(1, 1));
            rectangles.Add(new Rectangle(1, 1));
            rectangles.Add(new Rectangle(1, 1));
            rectangles.Add(new Rectangle(1, 1));*/

            /*rectangles.Add(new Rectangle(5, 10));
            rectangles.Add(new Rectangle(5, 2));
            rectangles.Add(new Rectangle(3, 3));*/

            /*rectangles.Add(new Rectangle(18, 10));
            rectangles.Add(new Rectangle(2, 2));*/

            /*rectangles.Add(new Rectangle(4, 4, new Point(0, 0)));
            rectangles.Add(new Rectangle(2, 5, new Point(4, 0)));
            rectangles.Add(new Rectangle(3, 3, new Point(0, 4)));
            rectangles.Add(new Rectangle(1, 2, new Point(3, 4)));
            rectangles.Add(new Rectangle(3, 1, new Point(3, 6)));
            rectangles.Add(new Rectangle(2, 1, new Point(4, 5)));*/

            /*rectangles.Add(new Rectangle(3, 2, new Point(0, 0)));
            rectangles.Add(new Rectangle(2, 3, new Point(3, 0)));
            rectangles.Add(new Rectangle(3, 1, new Point(0, 2)));
            rectangles.Add(new Rectangle(2, 2, new Point(5, 0)));*/

            IAlgorithm algorithm = new Algorithm3();
            algorithm.ComputeMaximumRectangle(rectangles);
            Rectangle rect = algorithm.GetRectangle();
        }
    }
}
