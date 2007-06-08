using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LinZadNajmnKwadr
{
    class Program
    {
        static void Main(string[] args)
        {
            //MGSOrtogonalizationTest();
            //Console.WriteLine("--------------------------");
            //HouseholderOrtogonalizationTest();
            LLSTest();         
        }

        private static void LLSTest()
        {
            //int an = 4, am = 3, bm = 4;
            int an = 4, am = 3, bm = 4;
            double[] tab = new double[] { 1, 1, 1, 1, 2, 3, 1, 3, 6, 1,4, 10, 1, 5, 15, 1, 6, 21 };
                                        
            double[] tabb = new double[] { -3, 4, 1, 0,2,5 };
            // rozw [ -1.8000, 0.9429, -0.0000]
            //Matrix A = new Matrix(tab, 6, 3);
            //Matrix b = new Matrix(tabb, 6, 1);                 //rozw [-0.25 0 0.25]
            
            
            double[] tab3 = new double[]{1, 2, 3, 
                                        4, 5, 6, 
                                        7, 8, 9,
                                        10,11, 13};
            /*double[] tab = new double[]{1, 1, 1, 1, 1,
                                        0.00000001, 0, 0, 0, 0,
                                        0, 0.00000001, 0, 0, 0,
                                        0, 0, 0, 0.00000001, 0,
                                        0, 0, 0, 0, 0.00000001};*/
            double[] tabb3 = new double[] { 0, 1, 1, 0 };        // rozw [4 -2 0 1]
            //double[] tabb = new double[] { 0, 1, 1, 0, 1 };

            double[] tab2 = new double[]{1,2,-1,0,
                                        4,8,-7,1,
                                        1,2,-1,1,
                                        -1,1,4,6};
            double[] tabb2 = new double[] { 0, 1, 1, 0 };
            
            
            Matrix A = new Matrix(tab3, an, am);
            Matrix b = new Matrix(tabb3, bm, 1);

            Console.WriteLine("A:");
            Console.WriteLine(A);
            Console.WriteLine("b:");
            Console.WriteLine(b);

            Console.WriteLine("MGS");
            LinearLeastSquares lls = new LinearLeastSquares(A,b, LinearLeastSquares.Method.MGS);
            lls.Solve();
            Console.WriteLine("x:");
            Console.WriteLine(lls.X);
            Console.WriteLine("r:");
            Console.WriteLine(lls.ResidualVector);

            Console.WriteLine("Hosueholder");
            lls = new LinearLeastSquares(A, b, LinearLeastSquares.Method.Householder);
            lls.Solve();
            Console.WriteLine("x:");
            Console.WriteLine(lls.X);
            Console.WriteLine("r:");
            Console.WriteLine(lls.ResidualVector);
        }

        private static void MGSOrtogonalizationTest()
        {
            //przyklad 1
            double[] tab = new double[]{1,-2,3,
                                       3,1,4,
                                        2,5,1};

            //przyklad 2
            double[] tab1 = new double[]{1,2,3,
                                        2,5,8,
                                        3,8,14};

            //przyklad 3
            double[] tab2 = new double[]{1,2,-1,0,
                                        4,8,-7,1,
                                        1,2,-1,1,
                                        -1,1,4,6};

            //przyklad 4
            double[] tab3 = new double[]{2,1,-1,1,
                                        3,-2,2,-3,
                                        5,1,-1,2,
                                        2,-1,1,-3};

            double[] tab4 = new double[]{1, 2, 3, 
                                        4, 5, 6, 
                                        7, 8, 9,
                                        10,11, 13};
            MGS[] mgs = new MGS[] {new MGS(new Matrix(tab, 3, 3)),
                                    new MGS(new Matrix(tab1,3,3)),
                                    new MGS(new Matrix(tab2, 4,4)),
                                    new MGS(new Matrix(tab3, 4,4)),
                                    new MGS(new Matrix(tab4, 4,3))};

            for (int i = 0; i < mgs.Length; i++)
            {
                mgs[i].Orthogonalization();
                Console.WriteLine("A:");
                Console.WriteLine(mgs[i].A.ToString(4));
                Console.WriteLine("Q:");
                Console.WriteLine(mgs[i].Q.ToString(4));
                Console.WriteLine("R:");
                Console.WriteLine(mgs[i].R.ToString(4));
                Console.WriteLine("spr:");
                Console.WriteLine("Q*R");
                Console.WriteLine((mgs[i].Q * mgs[i].R).ToString(4));
                Console.WriteLine("Q.Transposition()*Q");
                Console.WriteLine((mgs[i].Q.Transposition() * mgs[i].Q).ToString(4));
            }
        }

        private static void HouseholderOrtogonalizationTest()
        {
            double[] tab = new double[]{1,-2,3,
                                       3,1,4,
                                        2,5,1};
            //double[] tabb= new double[]{-7,5,18};//przyklad 1


            double[] tab1 = new double[]{1,2,3,
                                        2,5,8,
                                        3,8,14};
            //double[] tabb1= new double[]{4,9,11};//przyklad 2

            double[] tab2 = new double[]{1,2,-1,0,
                                        4,8,-7,1,
                                        1,2,-1,1,
                                        -1,1,4,6};
            //double[] tabb2= new double[]{0,1,1,0};//przyklad 3

            double[] tab3 = new double[]{2,1,-1,1,
                                        3,-2,2,-3,
                                        5,1,-1,2,
                                        2,-1,1,-3};
            //double[] tabb3 = new double[] { 1, 2, -1, 4 };//przyklad 4

            Householder[] hous = new Householder[] {new Householder(new Matrix(tab, 3, 3)),
                                    new Householder(new Matrix(tab1,3,3)),
                                    new Householder(new Matrix(tab2, 4,4)),
                                    new Householder(new Matrix(tab3, 4,4))};

            for (int i = 0; i < hous.Length; i++)
            {
                hous[i].Orthogonalization();
                Console.WriteLine("Q:");
                Console.WriteLine(hous[i].Q);
                Console.WriteLine("R:");
                Console.WriteLine(hous[i].R);
                Console.WriteLine("spr");
                Console.WriteLine(hous[i].Q * hous[i].R);
            }
        }
    }
}
