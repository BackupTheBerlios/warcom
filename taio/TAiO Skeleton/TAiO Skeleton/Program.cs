using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TAiO_Rectangles
{
    class Program
    {
        static void Main(string[] args)
        {
            //IntersectionTest();
            //SubtractTest();
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
    }
}
