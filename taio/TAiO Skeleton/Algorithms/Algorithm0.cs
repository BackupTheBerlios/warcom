using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Taio.Algorithms
{
    class Algorithm0 : IAlgorithm
    {
        public Rectangle ComputeMaximumRectangle(List<Rectangle> rectangles)
        {
            for (int i = 0; i < 100; ++i)
            {
                Debug.WriteLine(i.ToString());
                Thread.Sleep(100);
            }
            return new Rectangle(1, 1);
        }
        
        public void StopThread() { }
        public Rectangle GetRectangle() { return new Rectangle(1, 1); }
    }
}
