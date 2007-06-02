using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Taio
{
    /// <summary>
    /// Klasa zapewniaj¹ca narzêdzia do tworzenia prostok¹ta z³o¿onego z innych prostok¹tów
    /// </summary>
    class RectangleContainer
    {
        private List<Rectangle> rectangles;
        private List<Rectangle> emptyFields;
        private Rectangle maxCorrectRect;
        private Rectangle maxPossibleRect;
        private bool isCorrectRectangle = false;

        #region Konstruktory
        /// <summary>
        /// 
        /// </summary>
        public RectangleContainer()
        {
            this.rectangles = new List<Rectangle>();
            this.emptyFields = new List<Rectangle>();
        }
        #endregion

        #region Akcesory
        /// <summary>
        /// Lista prostok¹tów w kontenerze
        /// </summary>
        public List<Rectangle> Rectangles
        {
            get { return rectangles; }
        }

        /// <summary>
        /// Lista prostok¹tnych pustych miejsc, które musz¹ byæ wype³nione, by utworzyæ MaxPossibleRect
        /// </summary>
        public List<Rectangle> EmptyFields
        {
            get { return emptyFields; }
        }

        /// <summary>
        /// Najwiêkszy poprawny prostok¹t z³o¿ony z prostok¹tów w kontenerze
        /// </summary>
        public Rectangle MaxCorrectRect
        {
            get { return maxCorrectRect; }
        }

        /// <summary>
        /// Najwiêkszy poprawny prostok¹t mo¿liwy do osi¹gniêcia w przypadku wype³nienia pustych miejsc
        /// </summary>
        public Rectangle MaxPossibleRect
        {
            get { return new Rectangle(maxPossibleRect.LeftTop, maxPossibleRect.RightDown); }
        }

        /// <summary>
        /// Czy wszystkie prostok¹ty w kontenerze tworz¹ prawid³owy prostok¹t
        /// </summary>
        public bool IsCorrectRectangle
        {
            get { return isCorrectRectangle; }
        }

        public Rectangle SmallestEmptyField
        {
            get
            {
                Rectangle res = null;
                int area = Int32.MaxValue;
                foreach (Rectangle r in emptyFields)
                {
                    if (r != null)
                        if (area > r.Area)
                        {
                            res = r;
                            area = r.Area;
                        }
                }
                return res;
            }
        }

        public Rectangle LargestEmptyField
        {
            get
            {
                Rectangle res = null;
                int area = 0;
                foreach (Rectangle r in emptyFields)
                {
                    if (r != null)
                        if (area < r.Area)
                        {
                            res = r;
                            area = r.Area;
                        }
                }
                return res;
            }
        }

        /// <summary>
        /// Suma pól wszystkich pustych obszarów
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

        #region Metody wstawiaj¹ce
        /// <summary>
        /// Wstawia prostok¹ty z listy do kontenera
        /// </summary>
        /// <param name="rects">Prostok¹ty do wstawienia</param>
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
        /// Wstawia prostok¹t do kontenera
        /// </summary>
        /// <param name="r">Prostok¹t do wstawienia</param>
        public void InsertRectangle(Rectangle r)
        {
            InsertRectangle(r, r.LeftTop);
        }

        /// <summary>
        /// Wstawia prostok¹t do kontenera
        /// </summary>
        /// <param name="r">Prostok¹t do wstawienia</param>
        /// <param name="o">Orientacja prostok¹ta</param>
        public void InsertRectangle(Rectangle r, Rectangle.Orientation o)
        {
            InsertRectangle(r, new Point(0, 0), o);
        }

        /// <summary>
        /// Wstawia prostok¹t do kontenera
        /// </summary>
        /// <param name="r">Prostok¹t do wstawienia</param>
        /// <param name="rLeftTop">Lewy górny wierzcho³ek prostok¹ta</param>
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
        /// Wstawia prostok¹t do kontenera
        /// </summary>
        /// <param name="r">Prostok¹t do wstawienia</param>
        /// <param name="rLeftTop">Lewy górny wierzcho³ek prostok¹ta</param>
        /// <param name="o">Orientacja prostok¹ta</param>
        public void InsertRectangle(Rectangle r, Point rLeftTop, Rectangle.Orientation o)
        {
            Console.WriteLine("insert rect[(" + r.LeftTop.X + ", " + r.LeftTop.Y + "), (" +
                r.RightDown.X + ", " + r.RightDown.Y + ")]" + " -> (" + rLeftTop.X + ", " +
                rLeftTop.Y + ")");

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
        ///Sprawdza parametry dla funkcji insert
        /// </summary>
        /// <param name="r">Prostok¹t do wstawienia</param>
        /// <param name="rLeftTop">Lewy górny wierzcho³ek prostok¹ta</param>
        private void InsertRectangleCheckParameters(Rectangle r, Point rLeftTop)
        {
            if (r == null)
                throw new ArgumentNullException();

            if (rLeftTop.X < 0 || rLeftTop.Y < 0)
                throw new ArgumentException("Incorrect leftTop coordinates");
        }

        /// <summary>
        /// Przygotowuje pierwszy prostokat do wstawienia do kontenera
        /// </summary>
        /// <param name="r">Prostok¹t do wstawienia jako pierwszy</param>
        private void FirstRectanglePreparation(Rectangle r)
        {
            r.Move(new Point(0, 0));
            this.maxCorrectRect = new Rectangle(r.LeftTop, r.RightDown);
            this.maxCorrectRect.ContainedRectangles.Add(r);
            this.maxPossibleRect = new Rectangle(r.LeftTop, r.RightDown);
            this.isCorrectRectangle = true;
        }

        /// <summary>
        /// Uaktualnia MaxCorrectRectangle i MaxPossibleRectangle gdy nie ma EmptyFields
        /// </summary>
        /// <param name="insertedRectangle">Wstawiany prostok¹t</param>
        private void UpdateMaxRectangles(Rectangle insertedRectangle)
        {
            maxCorrectRect.Resize(insertedRectangle.RightDown);
            insertedRectangle.SetParentRectangle(maxCorrectRect);

            maxPossibleRect.Resize(insertedRectangle.RightDown);
        }

        /// <summary>
        /// Uaktualnia MaxCorrectRectangle
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
        /// Uaktualnia MaxCorrectRectangle
        /// </summary>
        /// <param name="insertedRectangle">Wstawiany prostok¹t</param>
        private void UpdateMaxPossibleRectangle(Rectangle insertedRectangle)
        {
            Point newMaxPossibleRectangleRightDown = ComputeNewMaxPossibleRectangleRightDown(insertedRectangle);
            maxPossibleRect.Resize(newMaxPossibleRectangleRightDown);
        }

        /// <summary>
        /// Oblicz prawy dolny wierzcholek MaxPossibleRectangle po wstawieniu prostok¹ta
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
        /// Uaktualnij EmptyFields
        /// </summary>
        /// <param name="insertedRectangle">Wstawiany prostok¹t</param>
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

                        // poprawione - Pawe³
                        //toAdd.AddRange(subtr);
                        addToList(toAdd, subtr);
                    }
                }
            }
            foreach (Rectangle r in toDelete)
                emptyFields.Remove(r);
            toDelete.Clear();
            foreach (Rectangle r in toAdd)
                // poprawione - Pawe³
                //emptyFields.AddRange(toAdd);
                addToList(emptyFields, toAdd);
            toAdd.Clear();

            //sprawdzamy czy trzeba dodac jakies nowe EmptyFields
            AddNewEmptyFields(insertedRectangle);
        }

        /// <summary>
        /// Oblicz i dodaj nowe EmptyFields
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
                    if (tempEmpties.Count > 0)
                        // poprawione - Pawe³
                        //emptyFields.AddRange(tempEmpties);
                        addToList(emptyFields, tempEmpties);
                }
            }
        }
        #endregion
        #endregion

        #region Korekta
        /// <summary>
        /// Metoda sprwadza, czy prostok¹t o danych wspó³rzêdnych i wymiarach znajduje siê
        /// ju¿ na liœcie.
        /// </summary>
        /// <param name="rectangles">lista prostok¹tów</param>
        /// <param name="rect">sprawdzany prostok¹t</param>
        private bool isOnList(List<Rectangle> rectangles, Rectangle rect)
        {
            if (rectangles == null || rect == null)
                return false;

            foreach (Rectangle r in rectangles)
                if (r.LeftTop.X == rect.LeftTop.X && r.LeftTop.Y == rect.LeftTop.Y &&
                    r.RightDown.X == rect.RightDown.X && r.RightDown.Y == rect.RightDown.Y)
                    return true;

            return false;
        }

        /// <summary>
        /// Metoda dodaje prostok¹ty z jednej listy do drugiej sprawdzaj¹c jednoczeœnie,
        /// czy dany prostok¹ty siê nie powtarzaj¹.
        /// </summary>
        /// <param name="rectsTarget">lista prostok¹tów, na któr¹ bêd¹ dodawane nowe prostok¹ty</param>
        /// <param name="rectsSource">lista prostok¹tów do dodania</param>
        private void addToList(List<Rectangle> rectsTarget, List<Rectangle> rectsSource)
        {
            if (rectsTarget == null || rectsSource == null)
                return;

            foreach (Rectangle rect in rectsSource)
            {
                if (!isOnList(rectsTarget, rect))
                    rectsTarget.Add(rect);
            }
        }
        #endregion
    }

}
