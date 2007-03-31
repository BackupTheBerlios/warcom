using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Taio.Algorithms
{
    class Algorithm0 : IAlgorithm
    {
        private string tag = "WZ0";
        private Rectangle rect;
        private bool stop;

        public Rectangle ComputeMaximumRectangle(List<Rectangle> rectangles)
        {
            for (int i = 0; i < 100; ++i)
            {
                Debug.WriteLine(i.ToString());
                Thread.Sleep(100);
            }
            rect = new Rectangle(1, 1);
            return rect;
        }

        public void StopThread() 
        { stop = true; }
        public Rectangle GetRectangle() 
        { return rect; }
        public string GetTag()
        { return tag;  }
    }
}
