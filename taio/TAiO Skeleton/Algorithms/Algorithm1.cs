using System;
using System.Collections.Generic;
using System.Text;

namespace Taio.Algorithms
{
    class Algorithm1 : IAlgorithm
    {
        private int maximumSideA = 0;
        private int maximumSideB = 0;
        private string tag = "WZ1";

        public Algorithm1()
        {
        }

         public string GetTag()
        { return tag;  }
        public void StopThread() { }
        public Rectangle GetRectangle() { return new Rectangle(1, 1); }

        public Rectangle ComputeMaximumRectangle(List<Rectangle> rects)
        {
            int maxArea = ComputeMaximumArea(rects);
            SetMaximumSides(maxArea);
            List<Rectangle> correctRects = RemoveTooBigRectangles(rects);
            ConcatenateRectangles(rects);
            Rectangle startRect = FindRectangleWithMaxArea(rects);
            bool change = true;
            int currentSide = startRect.LongerSide;
            List<Rectangle> tempRectsList = new List<Rectangle>();
            while (change && rects.Count > 0)
            {
                change = false;
                int currentSum = 0;
                Rectangle tempRect = TryFindRectangleToThenNextStep(currentSide, rects);
                currentSum += tempRect.LongerSide;
                if (tempRect != null)
                {
                    tempRectsList.Add(tempRect);
                    while (currentSum < currentSide)
                    {
                        //TODO finish it
                        Rectangle tmp = TryFindNextRect(tempRect.ShorterSide, currentSum, currentSide, rects);
                        currentSum += tmp.SideA;
                        tempRectsList.Add(tmp);
                    }
                    RectangleContainer rc = new RectangleContainer();
                    //rc.InsertRectangle(
                }
            }
            return null;
        }

        private Rectangle TryFindNextRect(int shorterSide, int currentSide, int maxSide, List<Rectangle> rects)
        {
            int index = -1;
            int val = Int32.MaxValue;
            bool longerSide = true;
            Rectangle rect = null;
            for (int i = 0; i < rects.Count; i++)
            {
                Rectangle tempRect = rects[i];
                int lostAreaHorz = 0, lostAreaVert = 0;
                if(currentSide + tempRect.LongerSide > maxSide)
                    lostAreaHorz+= (currentSide + tempRect.LongerSide - maxSide) * tempRect.ShorterSide;
                if(tempRect.ShorterSide < shorterSide)
                    lostAreaHorz+=(shorterSide-tempRect.ShorterSide)*currentSide;
                else if(tempRect.ShorterSide > shorterSide)
                    lostAreaHorz+=(tempRect.ShorterSide - shorterSide)*tempRect.LongerSide;
                if(currentSide + tempRect.ShorterSide > maxSide)
                    lostAreaHorz+= (currentSide + tempRect.ShorterSide - maxSide) * tempRect.LongerSide;
                if(tempRect.LongerSide < shorterSide)
                    lostAreaHorz+=(shorterSide-tempRect.LongerSide)*currentSide;
                else if(tempRect.LongerSide > shorterSide)
                    lostAreaHorz+=(tempRect.LongerSide - shorterSide)*tempRect.ShorterSide;
                int lostArea = Math.Min(lostAreaHorz, lostAreaVert);
                if (lostArea < val)
                {
                    index = i;
                    val = lostArea;
                    if (lostArea == lostAreaHorz)
                        longerSide = true;
                    else
                        longerSide = false;
                }
            }
            if (index != -1)
            {
                rect = rects[index];
                rects.Remove(rect);
                if (longerSide && rect.SideA != rect.LongerSide)
                    rect.Rotate();
                    //TODO jak bede mniej zmeczony, to przemyslec czy ten warunek jest poprawny
                else if (!longerSide && rect.SideB != rect.ShorterSide)
                    rect.Rotate();
            }
            return rect;
        }

        private Rectangle TryFindRectangleToThenNextStep(int side, List<Rectangle> rects)
        {
            int index = -1;
            int val = -1;
            Rectangle rect = null;
            for (int i = 0; i < rects.Count; i++)
            {
                if (rects[i].LongerSide < side && rects[i].LongerSide > val)
                {
                    index = i;
                    val = rects[i].LongerSide;
                }
            }
            if (val != -1)
            {
                rect = rects[index];
                rects.Remove(rect);
            }
            return rect;
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
