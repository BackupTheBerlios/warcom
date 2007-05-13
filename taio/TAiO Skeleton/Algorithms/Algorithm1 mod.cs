using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Taio.Algorithms
{
    class Algorithm1Mod : IAlgorithm
    {
        private int maximumSide = 0;
        private double ratio = 2.0;
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
            Rectangle bigestSingleRect = FindBiggestSingleRect(rects);
            List<Rectangle> correctRects = RemoveTooBigRectangles(rects);
            Rectangle startRect = FindRectangleWithMaxArea(correctRects);
            bool change = true;
            int currentSide = (startRect.LongerSide == startRect.SideA) ? startRect.SideA : startRect.SideB;
            Taio.Rectangle.Orientation orientation =
                (currentSide == startRect.SideA) ? Taio.Rectangle.Orientation.Horizontal :
                Taio.Rectangle.Orientation.Vertical;
            List<Rectangle> tempRectsList = new List<Rectangle>();
            while (change && correctRects.Count > 0 && running)
            {
                change = false;
                Rectangle tempRect = TryFindRectangleToThenNextStep(currentSide, correctRects, orientation);
                if (tempRect != null)
                {
                    int currentSum = tempRect.LongerSide;
                    tempRectsList.Add(tempRect);
                    bool foundLine = TryFillLine(currentSum, currentSide, tempRect, correctRects,
                        tempRectsList, orientation);
                    if (foundLine)
                    {
                        Rectangle maxCorrect = AddFoundRectanglesToStartRect(tempRectsList,
                            startRect, orientation);
                        if (maxCorrect == null || !IsShapeConditionValid(maxCorrect.SideA, maxCorrect.SideB))
                            foreach (Rectangle r in tempRectsList)
                                correctRects.Add(r);
                        else 
                        {
                            change = true;
                            onlySideChange = false;
                            startRect = maxCorrect;
                        }
                    }
                    tempRectsList.Clear();
                }
                if (!change && !onlySideChange)
                {
                    change = true;
                    onlySideChange = true;
                    currentSide = (currentSide == startRect.SideA) ? startRect.SideB :
                        startRect.SideA;
                    orientation = (currentSide == startRect.SideA) ? Taio.Rectangle.Orientation.Horizontal :
                Taio.Rectangle.Orientation.Vertical;
                }
            }
            if (startRect.ContainedRectangles.Count == 0)
            {
                RectangleContainer rc = new RectangleContainer();
                rc.InsertRectangle(startRect);
                startRect = rc.MaxCorrectRect;
            }
            rectangle = startRect;
            if (bigestSingleRect != null)
                if (bigestSingleRect.Area >= rectangle.Area) 
                    rectangle = bigestSingleRect;
            return rectangle;
        }

        private static bool TryFillLine(int currentSum, int currentSide, Rectangle tempRect,
            List<Rectangle> correctRects, List<Rectangle> tempRectsList, Taio.Rectangle.Orientation orientation)
        {
            bool foundLine = true;
            while (currentSum < currentSide)
            {
                int minSide = (orientation == Rectangle.Orientation.Horizontal) ? tempRect.SideB : tempRect.SideA;
                Rectangle tmp = TryFindNextRect(minSide, currentSum, currentSide, correctRects, orientation);
                if (tmp == null)
                {
                    foundLine = false;
                    break;
                }
                currentSum += (orientation == Rectangle.Orientation.Horizontal) ? tmp.SideA : tmp.SideB;
                tempRectsList.Add(tmp);
            }
            return foundLine;
        }

        private static Rectangle AddFoundRectanglesToStartRect(List<Rectangle> foundRects,
            Rectangle startRect, Rectangle.Orientation orientation)
        {
            RectangleContainer rc = new RectangleContainer();
            rc.InsertRectangle(startRect);
            int minHeight = FindMinHeight(foundRects, orientation);
            int offset = 0;
            int startVal = (orientation == Rectangle.Orientation.Horizontal) ? startRect.SideB : startRect.SideA;
            foreach (Rectangle r in foundRects)
            {
                if (orientation == Rectangle.Orientation.Horizontal)
                {
                    if ((offset + r.SideA) > startRect.SideA)
                        offset = startRect.SideA - r.SideA;
                    if (startVal + minHeight - r.SideB < 0)
                        return null;
                    rc.InsertRectangle(r, new Point(offset, startVal + minHeight - r.SideB));
                    offset += r.SideA;
                }
                else
                {
                    if ((offset + r.SideB) > startRect.SideB)
                        offset = startRect.LongerSide - r.SideB;
                    if (startVal + minHeight - r.SideA < 0)
                        return null;
                    rc.InsertRectangle(r, new Point(startVal + minHeight - r.SideA, offset));
                    offset += r.SideB;
                }
            }
            return rc.MaxCorrectRect;
        }

        private static int FindMinHeight(List<Rectangle> rects, Rectangle.Orientation orientation)
        {
            int min = Int32.MaxValue;
            int temp;
            foreach (Rectangle r in rects)
            {
                temp = (orientation == Rectangle.Orientation.Horizontal) ? r.SideB : r.SideA;
                if (temp < min)
                    min = temp;
            }
            return min;
        }

        private static int CountAreaLost(int currentSide, int maxSide, int shorterSide, int sideA, int sideB)
        {
            int lostArea = 0;
            if (sideA > currentSide || sideB > currentSide)
                return Int32.MaxValue;
            if (currentSide + sideA > maxSide)
                lostArea += (currentSide + sideA - maxSide) * sideB;
            if (sideB > shorterSide)
                lostArea += (sideB - shorterSide) * sideA;
            if (sideB < shorterSide)
                lostArea += (shorterSide - sideB) * sideA;
            return lostArea;
        }

        private static Rectangle TryFindNextRect(int shorterSide, int currentSide, int maxSide,
            List<Rectangle> rects, Taio.Rectangle.Orientation orientation)
        {
            int index = -1;
            int val = Int32.MaxValue;
            bool needRotate = true;
            Rectangle rect = null;
            for (int i = 0; i < rects.Count; i++)
            {
                Rectangle tempRect = rects[i];
                int lostAreaHorz = CountAreaLost(currentSide, maxSide, shorterSide, tempRect.SideA,
                    tempRect.SideB);
                int lostAreaVert = CountAreaLost(currentSide, maxSide, shorterSide, tempRect.SideB,
                    tempRect.SideA);
                int lostArea = Math.Min(lostAreaHorz, lostAreaVert);
                if (lostArea < val)
                {
                    index = i;
                    val = lostArea;
                    if (lostArea == lostAreaVert)
                        needRotate = true;
                    else
                        needRotate = false;
                }
            }
            if (index != -1)
            {
                rect = rects[index];
                rects.Remove(rect);
                if (needRotate && orientation == Rectangle.Orientation.Horizontal)
                    rect.Rotate();
                else if (!needRotate && orientation == Rectangle.Orientation.Vertical)
                    rect.Rotate();
            }
            return rect;
        }

        private static Rectangle TryFindRectangleToThenNextStep(int side, List<Rectangle> rects,
            Rectangle.Orientation orientation)
        {
            int index = -1;
            int val = -1;
            Rectangle rect = null;
            for (int i = 0; i < rects.Count; i++)
            {
                if (rects[i].LongerSide <= side && rects[i].LongerSide > val)
                {
                    index = i;
                    val = rects[i].LongerSide;
                }
            }
            if (val != -1)
            {
                rect = rects[index];
                rects.Remove(rect);
                if (orientation == Rectangle.Orientation.Vertical && rect.LongerSide == rect.SideA)
                    rect.Rotate();
                else if (orientation == Rectangle.Orientation.Horizontal && rect.LongerSide == rect.SideB)
                    rect.Rotate();
            }
            return rect;
        }

        private static int ComputeMaximumArea(List<Rectangle> rects)
        {
            int area = 0;
            foreach (Rectangle rect in rects)
                area += rect.Area;
            return area;
        }

        private void SetMaximumSides(int area)
        {
            double shorterSide = (int)Math.Sqrt(area);
            double tempMax = shorterSide;
            double tempValDouble;
            int tempValInt;

            while (tempMax / shorterSide <= ratio)
            {
                maximumSide = (int)tempMax;

                do
                {
                    tempMax++;
                    tempValDouble = (double)area / tempMax;
                    tempValInt = (int)(area / tempMax);

                    if (tempMax / tempValDouble > ratio)
                        return;
                } while (tempValDouble - (double)tempValInt != 0.0);

                shorterSide = area / (int)tempMax;
            }
        }

        private List<Rectangle> RemoveTooBigRectangles(List<Rectangle> rects)
        {
            List<Rectangle> correctRects = new List<Rectangle>();
            foreach (Rectangle rect in rects)
                if (rect.LongerSide <= maximumSide)
                {
                    Rectangle t = new Rectangle(rect.SideA, rect.SideB);
                    t.Number = rect.Number;
                    correctRects.Add(t);
                }
            return correctRects;
        }

        private Rectangle FindBiggestSingleRect(List<Rectangle> rects)
        {
            int index = -1;
            int maxArea = -1;
            Rectangle rectangle = null;
            for (int i = 0; i < rects.Count; ++i)
            {
                Rectangle rect = rects[i];
                if (IsShapeConditionValid(rect.SideA, rect.SideB) && rect.Area > maxArea)
                {
                    index = i;
                    maxArea = rect.Area;
                }
            }
            if (index != -1)
            {
                RectangleContainer rc = new RectangleContainer();
                rectangle = new Rectangle(rects[index].SideA, rects[index].SideB);
                rectangle.Number = rects[index].Number;
                rc.InsertRectangle(rectangle);
                rectangle = rc.MaxCorrectRect;
            }
            return rectangle;
        }

        private static bool IsShapeConditionValid(int sideA, int sideB)
        {
            double prop = sideA / (double)sideB;
            if (prop >= 0.5 && prop <= 2)
                return true;
            return false;
        }

        private static Rectangle FindRectangleWithMaxArea(List<Rectangle> rects)
        {
            int max = -1;
            int index = 0;
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