using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Taio
{
    class RectangleContainer
    {
        private List<Rectangle> rectangles;
        private List<Rectangle> emptyFields;
        private Rectangle maxCorrectRect;
        private Rectangle maxPossibleRect;
        private bool isCorrectRectangle = false;

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public RectangleContainer()
        {
            this.rectangles = new List<Rectangle>();
            this.emptyFields = new List<Rectangle>();
        } 
        #endregion

        #region Accessors
        /// <summary>
        /// Contained rectangles list
        /// </summary>
        public List<Rectangle> Rectangles
        {
            get { return rectangles; }
        }

        /// <summary>
        /// List of rectangles which should be filled in so as to build the largest possible rectangle
        /// </summary>
        public List<Rectangle> EmptyFields
        {
            get { return emptyFields; }
        }

        /// <summary>
        /// The largest correct rectangle consisting of rectangles in container
        /// </summary>
        public Rectangle MaxCorrectRect
        {
            get { return maxCorrectRect; }
        }

        /// <summary>
        /// The largest correct rectangle possible to construct when empty fields are filled in
        /// </summary>
        public Rectangle MaxPossibleRect
        {
            get { return new Rectangle(maxPossibleRect.LeftTop, maxPossibleRect.RightDown); }
        }

        /// <summary>
        /// Are all rectangles in container build correct rectangle
        /// </summary>
        public bool IsCorrectRectangle
        {
            get { return isCorrectRectangle; }
        }

        /// <summary>
        /// Area of all rectangles in container which should be filled in to build the largest possible rectangle
        /// </summary>
        public int EmptyFieldsArea
        {
            get
            {
                int area = 0;
                foreach (Rectangle r in emptyFields)
                    if (r != null)
                        area += r.Area;

                return area;
            }
        }
        #endregion

        #region Insert methods
        /// <summary>
        /// Insert rectangles from rects to the container
        /// </summary>
        /// <param name="rects">Rectangles to insert</param>
        public void InsertRectangles(List<Rectangle> rects)
        {
            if (rects == null)
                throw new ArgumentNullException();
            for (int i = 0; i < rects.Count; ++i)
            {
                Rectangle r = rects[i];
                Rectangle.Orientation o = r.LongerSide == r.RightDown.X - r.LeftTop.X ?
                    Rectangle.Orientation.Horizontal : Rectangle.Orientation.Vertical;
                this.InsertRectangle(r, r.LeftTop, o);
            }
        }


        /// <summary>
        /// Insert rectangle using its properties to place it in container
        /// </summary>
        /// <param name="r">Rectangle to insert</param>
        public void InsertRectangle(Rectangle r)
        {
            InsertRectangle(r, r.LeftTop);
        }
        
        /// <summary>
        /// Insert rectangle to the container
        /// </summary>
        /// <param name="r">Rectangle to insert</param>
        /// <param name="o">Inserted rectangle orientation</param>
        public void InsertRectangle(Rectangle r, Rectangle.Orientation o)
        {
            InsertRectangle(r, new Point(0, 0), o);
        }

        /// <summary>
        /// Insert rectangle to the container
        /// </summary>
        /// <param name="r">Rectangle to insert</param>
        /// <param name="rLeftTop">Inserted rectangle left-top point</param>
        public void InsertRectangle(Rectangle r, Point rLeftTop)
        {
            Rectangle.Orientation o;
            if (r.SideA >= r.SideB)
                o = Rectangle.Orientation.Horizontal;
            else
                o = Rectangle.Orientation.Vertical;

            InsertRectangle(r, rLeftTop, o);
        }

        /// <summary>
        /// Insert rectangle to the container
        /// </summary>
        /// <param name="r">Rectangle to insert</param>
        /// <param name="rLeftTop">Inserted rectangle left-top point</param>
        /// <param name="o">Inserted rectangle orientation</param>
        public void InsertRectangle(Rectangle r, Point rLeftTop, Rectangle.Orientation o)
        {
            InsertRectangleCheckParameters(r, rLeftTop);
         
            if (o.Equals(Rectangle.Orientation.Horizontal) && r.SideA < r.SideB)
                r.Rotate();
            else if (o.Equals(Rectangle.Orientation.Vertical) && r.SideA > r.SideB)
                r.Rotate();

            if (rectangles.Count == 0)
                FirstRectanglePreparation(r);
            else
                r.Move(rLeftTop);
            
            rectangles.Add(r);

            //jesli to byl pierwszy prostakat wszystko jest dobrze - nie trzeba tego robic
            if (rectangles.Count > 1)
            {
                // spr. czy po dodaniu wciaz prawidlowy prostokat
                if (isCorrectRectangle)
                {
                        // doklejamy od dolu prostokata
                    if (r.LeftTop.X == maxCorrectRect.LeftTop.X &&
                        r.RightDown.X == maxCorrectRect.RightDown.X &&
                        r.LeftTop.Y <= maxCorrectRect.RightDown.Y)
                        UpdateMaxRectangles(r);

                        // doklejamy z prawej strony prostokata
                    else if (r.LeftTop.Y == maxCorrectRect.LeftTop.Y &&
                        r.RightDown.Y == maxCorrectRect.RightDown.Y &&
                        r.LeftTop.X <= maxCorrectRect.RightDown.X)
                        UpdateMaxRectangles(r);

                        // naklejamy na prostokat
                    else if (maxCorrectRect.Covers(r))
                        r.SetParentRectangle(maxCorrectRect);

                        // zaklejamy ca³y prostok¹t - to równie g³upi przypadek jak poprzedni, ale skoro ktoœ tak chce...
                    else if (r.Covers(maxCorrectRect))
                        UpdateMaxRectangles(r);

                        // calosc przestaje byc poprawnym prostokatem
                    else
                    {
                        isCorrectRectangle = false;
                        AddNewEmptyFields(r);
                        UpdateMaxPossibleRectangle(r);
                    }
                }// gdy calosc nie jest prawidlowym prostokatem
                else
                {
                    //sprawdzenie czy dodanie prostokata nie pokrylo calkowicie jakichs emptyFields
                    //spr. czy przeciecia dodanego z empty sa niepuste (jesli tak - usuwamy odpow. empty z listy i wstawiamy zamiast niego empty-dodany)
                    //spr. czy nie zmienil sie maxPossibleRect

                    UpdateEmptyFields(r);
                    if (emptyFields.Count == 0)
                        UpdateMaxCorrectAfterFillingAllEmpties();
                    UpdateMaxPossibleRectangle(r);
                }
            }
        }

        #region Auxiliary functions for Insert methods
        /// <summary>
        /// Check InsertRectangle function parameters
        /// </summary>
        /// <param name="r">Rectangle to insert</param>
        /// <param name="rLeftTop">LeftTop coordinate for rectangle vertex</param>
        private void InsertRectangleCheckParameters(Rectangle r, Point rLeftTop)
        {
            if (r == null)
                throw new ArgumentNullException();

            if (rLeftTop.X < 0 || rLeftTop.Y < 0)
                throw new ArgumentException("Incorrect leftTop coordinates");
        }

        /// <summary>
        /// Prepare first rectangle to insert into container
        /// </summary>
        /// <param name="r">Rectangle to insert as a first one</param>
        private void FirstRectanglePreparation(Rectangle r)
        {
            r.Move(new Point(0, 0));
            this.maxCorrectRect = new Rectangle(r.LeftTop, r.RightDown);
            this.maxCorrectRect.ContainedRectangles.Add(r);
            this.maxPossibleRect = new Rectangle(r.LeftTop, r.RightDown);
            this.isCorrectRectangle = true;
        }

        /// <summary>
        /// Update MaxCorrectRectangle and MaxPossibleRectangle when there were no EmptyFields
        /// </summary>
        /// <param name="insertedRectangle">Currently inserted rectangle</param>
        private void UpdateMaxRectangles(Rectangle insertedRectangle)
        {
            maxCorrectRect.Resize(insertedRectangle.RightDown);
            insertedRectangle.SetParentRectangle(maxCorrectRect);
                        
            maxPossibleRect.Resize(insertedRectangle.RightDown);
        }

        /// <summary>
        /// Update MaxCorrectRectangle after filling all EmptyFields from MaxPossibleRectangle
        /// </summary>
        private void UpdateMaxCorrectAfterFillingAllEmpties()
        {
            this.maxCorrectRect.Resize(this.maxPossibleRect.RightDown);
            this.isCorrectRectangle = true;

            foreach (Rectangle r in rectangles)
                if (!this.maxCorrectRect.ContainedRectangles.Contains(r))
                    this.maxCorrectRect.ContainedRectangles.Add(r);
        }

        /// <summary>
        /// Update MaxPossibleRectangle after inserting insertedRectangle
        /// </summary>
        /// <param name="insertedRectangle">Currently inserted rectangle</param>
        private void UpdateMaxPossibleRectangle(Rectangle insertedRectangle)
        {
            Point newMaxPossibleRectangleRightDown = ComputeNewMaxPossibleRectangleRightDown(insertedRectangle);
            maxPossibleRect.Resize(newMaxPossibleRectangleRightDown);
        }

        /// <summary>
        /// Compute right-down vertex coordinate of MaxPossibleRectangle after inserting a rectangle
        /// </summary>
        /// <param name="insertedRectangle"></param>
        /// <returns></returns>
        private Point ComputeNewMaxPossibleRectangleRightDown(Rectangle insertedRectangle)
        {
            int rdX, rdY;
            rdX = Math.Max(insertedRectangle.RightDown.X, maxPossibleRect.RightDown.X);
            rdY = Math.Max(insertedRectangle.RightDown.Y, maxPossibleRect.RightDown.Y);
            return new Point(rdX, rdY);
        }

        /// <summary>
        /// Update container's EmptyFields list after inserting insertedRectangle
        /// </summary>
        /// <param name="insertedRectangle">Currently inserted rectangle</param>
        private void UpdateEmptyFields(Rectangle insertedRectangle)
        {
            IEnumerator<Rectangle> enumerator = emptyFields.GetEnumerator();
            //sprawdzamy czy sa jakies puste, ktore zostaly calkowicie pokryte przez ostatnio dodany prostokat
            List<Rectangle> toDelete = new List<Rectangle>();
            while (enumerator.MoveNext())
            {
                Rectangle empty = enumerator.Current;
                if (empty != null)
                    if (insertedRectangle.Covers(empty))
                        //emptyFields.Remove(empty);
                        toDelete.Add(empty);
            }
            foreach (Rectangle r in toDelete)
                emptyFields.Remove(r);
            toDelete.Clear();


            enumerator = emptyFields.GetEnumerator();
            List<Rectangle> toAdd = new List<Rectangle>();
            //sprawdzamy czy sa jakies czesciowo pokryte puste
            while (enumerator.MoveNext())
            {
                Rectangle empty = enumerator.Current;
                if (empty != null)
                {
                    Rectangle intersection = insertedRectangle.IntersectionRect(empty);
                    if (intersection != null)
                    {
                        //emptyFields.Remove(empty);
                        toDelete.Add(empty);
                        List<Rectangle> subtr = empty.Subtract(insertedRectangle);
                        //emptyFields.AddRange(subtr);
                        toAdd.AddRange(subtr);
                    }
                }
            }
            foreach (Rectangle r in toDelete)
                emptyFields.Remove(r);
            toDelete.Clear();
            foreach (Rectangle r in toAdd)
                emptyFields.AddRange(toAdd);
            toAdd.Clear();

            //sprawdzamy czy trzeba dodac jakies nowe EmptyFields
            AddNewEmptyFields(insertedRectangle);
        }

        /// <summary>
        /// Compute and add newly created empty fields after inserting a rectangle
        /// </summary>
        /// <param name="insertedRectangle">Currently inserted rectangle</param>
        private void AddNewEmptyFields(Rectangle insertedRectangle)
        {
            Point newMaxPossibleRD = ComputeNewMaxPossibleRectangleRightDown(insertedRectangle);
            if (!newMaxPossibleRD.Equals(maxPossibleRect.RightDown))
            {
                Rectangle newMaxPossibleRect = new Rectangle(maxPossibleRect.LeftTop, newMaxPossibleRD);
                List<Rectangle> newEmpties = newMaxPossibleRect.Subtract(maxPossibleRect);
                IEnumerator<Rectangle> nEmpEnum = newEmpties.GetEnumerator();
                while (nEmpEnum.MoveNext())
                {
                    Rectangle temp = nEmpEnum.Current;
                    List<Rectangle> tempEmpties = temp.Subtract(insertedRectangle);
                    if (tempEmpties.Count>0)
                        emptyFields.AddRange(tempEmpties);
                }
            }
        }
        #endregion
        #endregion
    }
}

#region Arch
/*public void AddRectangle(Rectangle r, Point rLeftTop, Rectangle.Orientation o)
        {
            if (r == null)
                throw new ArgumentNullException();

            if (rLeftTop.X < 0 || rLeftTop.Y < 0)
                throw new ArgumentException("Incorrect leftTop coordinates");

            if (rectangles.Count == 0)
            {
                //r.LeftTop = new Point(0,0);
                r.Move(new Point(0, 0));
                this.maxCorrectRect = r;
                this.isCorrectRectangle = true;
            }
            else
                r.Move(rLeftTop);
                //r.LeftTop = rLeftTop;
            
            //if (o.Equals(Rectangle.Orientation.Horizontal))
            //    r.RightDown = new Point(r.LeftTop.X + r.SideA, r.LeftTop.Y + r.SideB);
            //else
            //    r.RightDown = new Point(r.LeftTop.X + r.SideB, r.LeftTop.Y + r.SideA);
            
            if (o.Equals(Rectangle.Orientation.Horizontal) && r.SideA < r.SideB)
                r.Rotate();
            else if (o.Equals(Rectangle.Orientation.Vertical && r.SideA > r.SideB))
                r.Rotate();
            
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
        }*/

#endregion