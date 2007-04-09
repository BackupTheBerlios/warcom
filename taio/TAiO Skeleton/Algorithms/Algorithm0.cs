using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Taio.Algorithms
{
    class Algorithm0 : IAlgorithm
    {
        private string tag = "AW0";
        private Rectangle rect;
        private bool stop;

        public Rectangle ComputeMaximumRectangle(List<Rectangle> rectangles)
        {
            if (rectangles == null)
                throw new ArgumentNullException();
            if (rectangles.Count == 0)
                throw new ArgumentException("Illegal argument");
            List<Rectangle> rects = new List<Rectangle>();
            foreach (Rectangle r in rectangles)
                rects.Add(r);
            int maxArea = this.ComputeMaximumArea(rects);
            Rectangle maxRect = this.FindGoodRectangleWithMaxArea(rects);
            while (!stop && maxArea>0)
            {
                int a, b;
                List<Rectangle> tmpRects;
                this.ReductionTask(ref maxArea, rects, out tmpRects, out a, out b);
                if (maxRect!=null && maxArea <= maxRect.Area)
                {
                    this.rect = maxRect;
                    return this.rect;
                }
                this.rect = this.SetCover(a, b, tmpRects);
                if (this.rect != null)
                    return this.rect;
                --maxArea;
            }
            this.rect = maxRect;
            return this.rect;
        }

        //TODO
        private Rectangle SetCover(int a, int b, List<Rectangle> rects)
        {
            return null;
        }

        //TODO
        private void ReductionTask(ref int maxArea, List<Rectangle> rects,
            out List<Rectangle> oRects, out int a, out int b)
        {
            int maxSide = (int)(2 * Math.Sqrt(maxArea));
            foreach (Rectangle r in rects)
                if (r.LongerSide > maxSide)
                    rects.Remove(r);
            oRects = new List<Rectangle>();
            a = b = 0;
        }

        private int ComputeMaximumArea(List<Rectangle> rects)
        {
            int area = 0;
            foreach (Rectangle rect in rects)
                area += rect.Area;
            return area;
        }

        private Rectangle FindGoodRectangleWithMaxArea(List<Rectangle> rects)
        {
            int max = -1;
            int index = -1;
            for (int i = 0; i < rects.Count; ++i)
            {
                if (rects[i].Area > max && rects[i].LongerSide <= 2*rects[i].ShorterSide)
                {
                    max = rects[i].Area;
                    index = i;
                }
            }
            if (index == -1)
                return null;
            Rectangle rect = rects[index];
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
