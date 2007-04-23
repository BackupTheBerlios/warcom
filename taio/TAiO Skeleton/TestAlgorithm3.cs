using System;
using System.Collections.Generic;
using System.Collections;
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

            rectangles.Add(new Rectangle(5, 2));
            rectangles.Add(new Rectangle(3, 3));
            rectangles.Add(new Rectangle(2, 5));
            rectangles.Add(new Rectangle(1, 2));
            rectangles.Add(new Rectangle(2, 2));
            rectangles.Add(new Rectangle(5, 4));
            rectangles.Add(new Rectangle(3, 5));
            rectangles.Add(new Rectangle(4, 3));
            rectangles.Add(new Rectangle(2, 5));
            rectangles.Add(new Rectangle(1, 3));

            IAlgorithm algorithm = new Algorithm3();
            algorithm.ComputeMaximumRectangle(rectangles);
            Rectangle rect = algorithm.GetRectangle();
        }
    }
}
