using System;
using System.Collections.Generic;
using System.Text;

namespace Taio.Algorithms
{
    class Algorithm1 : IAlgorithm
    {
        private List<Rectangle> rectangles;

        public Algorithm1(List<Rectangle> rectangles)
        {
            this.rectangles = rectangles;
        }

        public Rectangle ComputeMaximumRectangle()
        {
            return null;
        }
    }
}
