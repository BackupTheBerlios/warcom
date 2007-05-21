using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Taio
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //RotateTest();
            //MoveTest();
            //ContainerTest();
            //IntersectionTest();
            //SubtractTest();
            //SortingTest();
            //CompareWithBest();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        

        #region Tests
        public static void CompareWithBest()
        {
            string path = @"C:\Documents and Settings\Marcin\Pulpit\tests\3x5";
            string[] files = Directory.GetFiles(path);
            List<Rectangle> rects = new List<Rectangle>();
            List<Solution> sols = new List<Solution>();
            Algorithms.Algorithm1Mod alg = new Taio.Algorithms.Algorithm1Mod();
            TextWriter tw = new StreamWriter(path + "\\result.csv", true);
            for (int i = 0; i < files.Length; i++)
            {
                DataLoader dl = new DataLoader();
                dl.LoadSolutions(files[i], ref sols, ref rects);
                int bestArea = sols[0].Rectangle.Area;
                Rectangle rect = alg.ComputeMaximumRectangle(rects);
                int apsArea = 0;
                if (rect != null)
                    apsArea = rect.Area;
                tw.WriteLine(bestArea + ";" + apsArea);

            }
            tw.Close();
        }

        public static void IntersectionTest()
        {
            Rectangle testRect = new Rectangle(new Point(4,3), new Point(7,6));

            List<Rectangle> a = new List<Rectangle>();
            a.Add(new Rectangle(new Point(1, 1), new Point(3, 3)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 6)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 7)));
            a.Add(new Rectangle(new Point(4, 3), new Point(9, 4)));
            a.Add(new Rectangle(new Point(3, 4), new Point(7, 5)));
            a.Add(new Rectangle(new Point(2, 5), new Point(9, 6)));
            a.Add(new Rectangle(new Point(10, 3), new Point(11, 5)));
            a.Add(new Rectangle(new Point(6, 4), new Point(7, 5)));
            a.Add(new Rectangle(new Point(6, 5), new Point(8, 8)));
            a.Add(new Rectangle(new Point(3, 7), new Point(4, 8)));
            a.Add(new Rectangle(new Point(6, 1), new Point(7, 3)));

            List<string> correct = new List<string>();
            correct.Add("0: empty");
            correct.Add("1: {X=5,Y=3}, {X=6,Y=6}; area: 3");
            correct.Add("2: {X=5,Y=3}, {X=6,Y=6}; area: 3");
            correct.Add("3: {X=4,Y=3}, {X=7,Y=4}; area: 3");
            correct.Add("4: {X=4,Y=4}, {X=7,Y=5}; area: 3");
            correct.Add("5: {X=4,Y=5}, {X=7,Y=6}; area: 3");
            correct.Add("6: empty");
            correct.Add("7: {X=6,Y=4}, {X=7,Y=5}; area: 1");
            correct.Add("8: {X=6,Y=5}, {X=7,Y=6}; area: 1");
            correct.Add("9: empty");
            correct.Add("10: empty");

            for (int i = 0; i < a.Count; i++)
            {
                Rectangle intersection = testRect.IntersectionRect(a[i]);
                string answerString;
                if(intersection != null)
                    answerString = i+ ": " + intersection;
                else
                    answerString = i + ": empty";

                bool isCorrect = answerString.Equals(correct[i]);
                Console.WriteLine(answerString + ": " + isCorrect);
            }
        }

        public static void SortingTest()
        {
            List<Rectangle> a = new List<Rectangle>();
            a.Add(new Rectangle(new Point(1, 1), new Point(3, 3)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 6)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 7)));
            a.Add(new Rectangle(new Point(4, 3), new Point(9, 4)));
            a.Add(new Rectangle(new Point(3, 4), new Point(7, 5)));
            a.Add(new Rectangle(new Point(2, 5), new Point(9, 6)));
            a.Add(new Rectangle(new Point(10, 3), new Point(11, 5)));
            a.Add(new Rectangle(new Point(6, 4), new Point(7, 5)));
            a.Add(new Rectangle(new Point(6, 5), new Point(8, 8)));
            a.Add(new Rectangle(new Point(3, 7), new Point(4, 8)));
            a.Add(new Rectangle(new Point(6, 1), new Point(7, 3)));

            Rectangle.RectangleComparer comparer = Rectangle.GetComparer();
            
            a.Sort();
            Console.WriteLine("default");
            foreach(Rectangle r in a)
                Console.WriteLine(r);
            Console.WriteLine();

            comparer.Comparison = Rectangle.RectangleComparer.ComparisonType.Area;
            a.Sort(comparer);
            Console.WriteLine("area");
            foreach (Rectangle r in a)
                Console.WriteLine(r);
            Console.WriteLine();

            comparer.Comparison = Rectangle.RectangleComparer.ComparisonType.LongerSide;
            a.Sort(comparer);
            Console.WriteLine("longer side");
            foreach (Rectangle r in a)
                Console.WriteLine(r);
            Console.WriteLine();

            comparer.Comparison = Rectangle.RectangleComparer.ComparisonType.Number;
            a.Sort(comparer);
            Console.WriteLine("number");
            foreach (Rectangle r in a)
                Console.WriteLine(r);
            Console.WriteLine();

            /*Rectangle re = a[0];
            Console.WriteLine(a.Contains(re));
            a.Remove(re);
            re = new Rectangle(new Point(0, 0), new Point(5, 5), 1);
            Console.WriteLine(a.Contains(re));*/
        }

        public static void SubtractTest()
        {
            Rectangle testRect = new Rectangle(new Point(4, 3), new Point(7, 6));

            List<Rectangle> a = new List<Rectangle>();
            a.Add(new Rectangle(new Point(1, 1), new Point(3, 3)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 6)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 7)));
            a.Add(new Rectangle(new Point(4, 3), new Point(9, 4)));
            a.Add(new Rectangle(new Point(3, 4), new Point(7, 5)));
            a.Add(new Rectangle(new Point(2, 5), new Point(9, 6)));
            a.Add(new Rectangle(new Point(10, 3), new Point(11, 5)));
            a.Add(new Rectangle(new Point(6, 4), new Point(7, 5)));
            a.Add(new Rectangle(new Point(6, 5), new Point(8, 8)));
            a.Add(new Rectangle(new Point(3, 7), new Point(4, 8)));
            a.Add(new Rectangle(new Point(6, 1), new Point(7, 3)));
            a.Add(testRect);
            a.Add(new Rectangle(new Point(3, 2), new Point(8, 7)));

            for (int i = 0; i < a.Count; i++)
            {
                List<Rectangle> subtraction = testRect.Subtract(a[i]);
                Console.WriteLine(i + ": ");
                foreach(Rectangle r in subtraction)
                    Console.WriteLine(r);
                Console.WriteLine();
            }

            /*
             * correct answers:
            0:  {X=4,Y=3}, {X=7,Y=6}; area: 9

            1:  {X=4,Y=3}, {X=5,Y=6}; area: 3
                {X=6,Y=3}, {X=7,Y=6}; area: 3

            2:  {X=4,Y=3}, {X=5,Y=6}; area: 3
                {X=6,Y=3}, {X=7,Y=6}; area: 3

            3:  {X=4,Y=4}, {X=7,Y=6}; area: 6

            4:  {X=4,Y=3}, {X=7,Y=4}; area: 3
                {X=4,Y=5}, {X=7,Y=6}; area: 3

            5:  {X=4,Y=3}, {X=7,Y=5}; area: 6

            6:  {X=4,Y=3}, {X=7,Y=6}; area: 9

            7:  {X=4,Y=3}, {X=6,Y=6}; area: 6
                {X=6,Y=3}, {X=7,Y=4}; area: 1
                {X=6,Y=5}, {X=7,Y=6}; area: 1

            8:  {X=4,Y=3}, {X=6,Y=6}; area: 6
                {X=6,Y=3}, {X=7,Y=5}; area: 2

            9:  {X=4,Y=3}, {X=7,Y=6}; area: 9

            10: {X=4,Y=3}, {X=7,Y=6}; area: 9
                  
            11:           (empty) 
            12:           (empty)
             */
        }

        public static void RotateTest()
        {
            List<Rectangle> a = new List<Rectangle>();
            a.Add(new Rectangle(new Point(1, 1), new Point(3, 3)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 6)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 7)));
            a.Add(new Rectangle(new Point(4, 3), new Point(9, 4)));
            a.Add(new Rectangle(new Point(3, 4), new Point(7, 5)));

            for (int i = 0; i < a.Count; i++)
            {
                a[i].Rotate();
                Console.WriteLine(a[i]);
            }

            /*
             * 
             * correct answers:
             *  {X=1,Y=1}, {X=3,Y=3}; area: 4
                {X=5,Y=2}, {X=9,Y=3}; area: 4
                {X=5,Y=2}, {X=10,Y=3}; area: 5
                {X=4,Y=3}, {X=5,Y=8}; area: 5
                {X=3,Y=4}, {X=4,Y=8}; area: 4
             */
        }

        public static void MoveTest()
        {
            List<Rectangle> a = new List<Rectangle>();
            a.Add(new Rectangle(new Point(1, 1), new Point(3, 3)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 6)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 7)));
            a.Add(new Rectangle(new Point(4, 3), new Point(9, 4)));
            a.Add(new Rectangle(new Point(3, 4), new Point(7, 5)));

            Point zeroPoint = new Point(0, 0);
            for (int i = 0; i < a.Count; i++)
            {
                a[i].Move(zeroPoint);
                Console.WriteLine(a[i]);
            }

            /*
             * 
             * correct answers:
             *  {X=0,Y=0}, {X=2,Y=2}; area: 4
                {X=0,Y=0}, {X=1,Y=4}; area: 4
                {X=0,Y=0}, {X=1,Y=5}; area: 5
                {X=0,Y=0}, {X=5,Y=1}; area: 5
                {X=0,Y=0}, {X=4,Y=1}; area: 4
             */
        }

        private static void ContainerTest()
        {
            RectangleContainer rc = new RectangleContainer();
            /*List<Rectangle> a = new List<Rectangle>();
            a.Add(new Rectangle(new Point(1, 1), new Point(3, 3)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 6)));
            a.Add(new Rectangle(new Point(5, 2), new Point(6, 7)));
            a.Add(new Rectangle(new Point(4, 3), new Point(9, 4)));
            a.Add(new Rectangle(new Point(3, 4), new Point(7, 5)));
            a.Add(new Rectangle(new Point(2, 5), new Point(9, 6)));
            a.Add(new Rectangle(new Point(10, 3), new Point(11, 5)));
            a.Add(new Rectangle(new Point(6, 4), new Point(7, 5)));
            a.Add(new Rectangle(new Point(6, 5), new Point(8, 8)));
            a.Add(new Rectangle(new Point(3, 7), new Point(4, 8)));
            a.Add(new Rectangle(new Point(6, 1), new Point(7, 3)));

            rc.InsertRectangle(a[1], new Point(0, 0), Rectangle.Orientation.Horizontal);
            rc.InsertRectangle(a[2], a[2].LeftTop, Rectangle.Orientation.Vertical);*/

            Rectangle r1, r2, r3;
            r1 = new Rectangle(2, 2);
            r2 = new Rectangle(2, 3);
            r3 = new Rectangle(2, 1);
            rc.InsertRectangle(r1, Rectangle.Orientation.Horizontal);
            rc.InsertRectangle(r2, new Point(2, 0), Rectangle.Orientation.Vertical);
            rc.InsertRectangle(r3, new Point(0, 2), Rectangle.Orientation.Horizontal);
        }
        #endregion
    }
}
