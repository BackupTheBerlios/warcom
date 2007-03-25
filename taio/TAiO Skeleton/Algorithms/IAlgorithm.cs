using System;
using System.Collections.Generic;
using System.Text;

namespace Taio.Algorithms
{
    interface IAlgorithm
    {
        Rectangle ComputeMaximumRectangle(List<Rectangle> rectangles);
    }
}
