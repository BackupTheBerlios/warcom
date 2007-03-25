using System;
using System.Collections.Generic;
using System.Text;

namespace Taio.Algorithms
{
    class Algorithm1 : IAlgorithm
    {
        private int maximumSideA = 0;
        private int maximumSideB = 0;
        public Algorithm1()
        {
        }

        public Rectangle ComputeMaximumRectangle(List<Rectangle> rects)
        {
            int maxArea = ComputeMaximumArea(rects);
            SetMaximumSides(maxArea);
            List<Rectangle> correctRects = RemoveTooBigRectangles(rects);
            ConcatenateRectangles(rects);
            Rectangle startRect = FindRectangleWithMaxArea(rects);
            bool change = true;

            while (change && rects.Count > 0)
            {
                change = false;

            }
            return null;
        }

        private Rectangle FindRectangleToThenNextStep()
        {
            return null;
        }


        private int ComputeMaximumArea(List<Rectangle> rects)
        {
            int area = 0;
            foreach (Rectangle rect in rects)
                area += rect.Area;
            return area;
        }

        private void SetMaximumSides(int area)
        {
            int tempVal = (int)Math.Sqrt(area);
            maximumSideB = tempVal;
            maximumSideA = tempVal;
            while ((maximumSideA + 1) * maximumSideB < area)
                maximumSideA++;
        }

        private List<Rectangle> RemoveTooBigRectangles(List<Rectangle> rects)
        {
            List<Rectangle> correctRects = new List<Rectangle>();
            foreach (Rectangle rect in rects)
                if (rect.LongerSide <= maximumSideA)
                    correctRects.Add(rect);
            return correctRects;
        }

        private bool IsShapeConditionValid(int sideA, int sideB)
        {
            double prop = sideA / (double)sideB;
            if (prop >= 0.5 && prop <= 2)
                return true;
            return false;
        }

        private void ConcatenateRectangles(List<Rectangle> rects)
        {
            bool change = true;
            int i,j;
            while (change)
            {
                change = false;
                i =0; j=0;
                while (i < rects.Count)
                {
                    while(j<rects.Count)
                    {
                        if(i != j)
                            if (rects[i].LongerSide == rects[j].LongerSide &&
                                IsShapeConditionValid(rects[i].LongerSide, rects[i].ShorterSide + rects[j].ShorterSide))
                            {
                                rects[i] = new Rectangle(rects[i].LongerSide, rects[i].ShorterSide + rects[j].ShorterSide);
                                rects.Remove(rects[j]);
                                change = true;
                            }
                    }
                }
            }
        }

        private Rectangle FindRectangleWithMaxArea(List<Rectangle> rects)
        {
            int max = -1;
            int index= 0;
            for (int i = 0; i < rects.Count; ++i)
            {
                if (rects[i].Area > max)
                {
                    max = rects[i].Area;
                    index = i;
                }
            }
            Rectangle rect = rects[index];
            rects.Remove(rect);
            return rect;
        }
    }
}
