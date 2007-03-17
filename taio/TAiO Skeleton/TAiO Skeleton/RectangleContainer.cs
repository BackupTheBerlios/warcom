using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace TAiO_Rectangles
{
    class RectangleContainer
    {
        private List<Rectangle> rectangles;
        private List<Rectangle> emptyFields;
        private Rectangle maxCorrectRect;
        private Rectangle maxPossibleRect;
        private bool isCorrectRectangle = false;

        #region Constructors
        public RectangleContainer()
        {
            this.rectangles = new List<Rectangle>();
            this.emptyFields = new List<Rectangle>();
        } 
        #endregion

        #region Accessors
        public List<Rectangle> Rectangles
        {
            get { return rectangles; }
        }

        public List<Rectangle> EmptyFields
        {
            get { return emptyFields; }
        }

        public Rectangle MaxCorrectRect
        {
            get { return new Rectangle(maxCorrectRect.LeftTop, maxCorrectRect.RightDown); }
        }

        public Rectangle MaxPossibleRect
        {
            get { return new Rectangle(maxPossibleRect.LeftTop, maxPossibleRect.RightDown); }
        }

        public bool IsCorrectRectangle
        {
            get { return isCorrectRectangle; }
        }
        
        #endregion


        public void AddRectangle(Rectangle r, Point rLeftTop, Rectangle.Orientation o)
        {
            if (r == null)
                throw new ArgumentNullException();

            if (rLeftTop.X < 0 || rLeftTop.Y < 0)
                throw new ArgumentException("Incorrect leftTop coordinates");

            if (rectangles.Count == 0)
            {
                r.LeftTop = new Point(0,0);
                this.maxCorrectRect = r;
                this.isCorrectRectangle = true;
            }
            else
                r.LeftTop = rLeftTop;
            
            if (o.Equals(Rectangle.Orientation.Horizontal))
                r.RightDown = new Point(r.LeftTop.X + r.SideA, r.LeftTop.Y + r.SideB);
            else
                r.RightDown = new Point(r.LeftTop.X + r.SideB, r.LeftTop.Y + r.SideA);
            
            rectangles.Add(r);


            if (rectangles.Count > 1)
            {
                // spr. czy po dodaniu wciaz prawidlowy prostokat
                if (isCorrectRectangle)
                {
                    if (r.LeftTop.X == maxCorrectRect.LeftTop.X &&
                        r.RightDown.X == maxCorrectRect.RightDown.X &&
                        r.LeftTop.Y <= maxCorrectRect.RightDown.Y) // przyp. a)
                    {
                    }
                    else if (r.LeftTop.Y == maxCorrectRect.LeftTop.Y &&
                        r.RightDown.Y == maxCorrectRect.RightDown.Y &&
                        r.LeftTop.X <= maxCorrectRect.RightDown.X) // przyp. b)
                    {
                    }
                    else if (maxCorrectRect.Covers(r)) // przyp. c)
                    {
                    }
                    else
                    {
                        isCorrectRectangle = false;
                        // tu obliczenie czego brakuje (emptyFields) aby prostokat maxPossibleRect byl poprawny
                    }
                }
                else
                {
                    //sprawdzenie czy dodanie prostokata nie pokrylo calkowicie jakichs emptyFields
                    //spr. czy przeciecia dodanego z empty sa niepuste (jesli tak - usuwamy odpow. empty z listy i wstawiamy zamiast niego empty-dodany)
                    //spr. czy nie zmienil sie maxPossibleRect
                }
            }   
        }

        public void AddRectangle(Rectangle r, Rectangle.Orientation o)
        {
            AddRectangle(r, new Point(0, 0), o);
        }
    }
}
