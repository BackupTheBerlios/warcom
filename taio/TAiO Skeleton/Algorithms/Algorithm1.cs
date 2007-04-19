using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Taio.Algorithms
{
    class Algorithm1 : IAlgorithm
    {
        private int maximumSideA = 0;
        private int maximumSideB = 0;
        private bool running = true;
        private Rectangle rectangle;
        private string tag = "AW1";
                
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
      
        public Rectangle ComputeMaximumRectangle(List<Rectangle> rects)
        {
            int maxArea = ComputeMaximumArea(rects);
            bool onlySideChange = false;
            SetMaximumSides(maxArea);
            List<Rectangle> correctRects = RemoveTooBigRectangles(rects);
            ConcatenateRectangles(correctRects);
            Rectangle startRect = FindRectangleWithMaxArea(correctRects);
            bool change = true;
            int currentSide = startRect.LongerSide;
            List<Rectangle> tempRectsList = new List<Rectangle>();
            while (change && correctRects.Count > 0 && running)
            {
                change = false;
                Rectangle tempRect = TryFindRectangleToThenNextStep(currentSide, correctRects);
                if (tempRect != null)
                {
                    int currentSum = tempRect.LongerSide;
                    tempRectsList.Add(tempRect);
                    //todo polaczyc funckje TryFillLine i AddFoundRectanglesToStartRect w jedna
                    bool foundLine = TryFillLine(currentSum, currentSide, tempRect, correctRects, tempRectsList);
                    if (foundLine)
                    {
                        Rectangle maxCorrect = AddFoundRectanglesToStartRect(tempRectsList, startRect);
                        if (IsShapeConditionValid(maxCorrect.SideA, maxCorrect.SideB))
                        {
                            change = true;
                            onlySideChange = false;
                            startRect = maxCorrect;                            
                        }
                        else
                        {
                            foreach (Rectangle r in tempRectsList)
                                correctRects.Add(r);
                        }
                    }
                    tempRectsList.Clear();
                }
                if (!change && !onlySideChange)
                {
                    change = true;
                    onlySideChange = true;
                    currentSide = (currentSide == startRect.LongerSide)?startRect.ShorterSide:startRect.LongerSide;
                }        
            }
            rectangle = startRect;
            return startRect;
        }

        private bool TryFillLine(int currentSum, int currentSide, Rectangle tempRect,
            List<Rectangle> correctRects, List<Rectangle> tempRectsList)
        {
            bool foundLine = true;
            while (currentSum < currentSide)
            {
                Rectangle tmp = TryFindNextRect(tempRect.ShorterSide, currentSum, currentSide, correctRects);
                if (tmp == null)
                {
                    foundLine = false;
                    break;
                }
                currentSum += tmp.SideA;
                tempRectsList.Add(tmp);
            }
            return foundLine;
        }

        private Rectangle AddFoundRectanglesToStartRect(List<Rectangle> foundRects, Rectangle startRect)
        {
            RectangleContainer rc = new RectangleContainer();
            rc.InsertRectangle(startRect);
            int minHeight = FindMinHeight(foundRects);
            int offset = 0, startVal = startRect.ShorterSide;
            foreach (Rectangle r in foundRects)
            {
                if ((offset + r.SideA) > startRect.LongerSide)
                    offset = startRect.LongerSide - r.SideA;
                if(startRect.LongerSide == startRect.SideA)
                    rc.InsertRectangle(r, new Point(offset, startVal + minHeight - r.SideB));
                else
                    rc.InsertRectangle(r, new Point(startVal + minHeight - r.SideB, offset));
                offset += r.SideA;
            }
            return rc.MaxCorrectRect;
        }

        private int FindMinHeight(List<Rectangle> rects)
        {
            int min = Int32.MaxValue;
            foreach (Rectangle r in rects)
                if (r.SideB < min)
                    min = r.SideB;
            return min;
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
                {
                    Rectangle t = new Rectangle(rect.SideA, rect.SideB);
                    t.Number = rect.Number;
                    correctRects.Add(t);
                }
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
                        ++j;
                    }
                    ++i;
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
