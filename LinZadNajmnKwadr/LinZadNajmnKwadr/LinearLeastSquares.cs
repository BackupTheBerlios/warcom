using System;
using System.Collections.Generic;
using System.Text;

namespace LinZadNajmnKwadr
{
    class LinearLeastSquares
    {
        private LeastSquare leastSquare;
        private Matrix r;
        private Matrix x;

        private Matrix b;
        private int n;

        public LinearLeastSquares(Matrix a, Matrix b, Method method)
        {
            if (a == null || b == null)
                throw new ArgumentNullException();

            if (a.Rows != b.Rows)
                throw new ArgumentException();

            if (b.Columns != 1)
                throw new ArgumentException("b must be a vector!");

            this.n = a.Columns;
            if (method == Method.MGS)
                leastSquare = new MGS(a);
            else if (method == Method.Householder)
                leastSquare = new Householder(a);
            
            leastSquare.Orthogonalization();
            Console.WriteLine("Q");
            Console.WriteLine(leastSquare.GetQ().ToString(4));
            Console.WriteLine("R");
            Console.WriteLine(leastSquare.GetR().ToString(4));
            this.b = b;
            this.x = new Matrix(a.Columns, 1);
        }

        public void Solve()
        {
            r = new Matrix(b);
            for (int i = 0; i < n; i++)
            {
                Matrix temp = leastSquare.GetQ().GetColumn(i).Transposition()*r;
                x[i, 0] = temp[0, 0];
                r -= leastSquare.GetQ().GetColumn(i) * x[i, 0];
            }

            for (int k = n - 1; k > 0; k--)
            {
                x[k, 0] /= leastSquare.GetR()[k, k];
                for (int i = 0; i < k - 1 ; i++)
                    x[i, 0] -= leastSquare.GetR()[i, k] * x[k, 0];
            }
        }

        public Matrix X
        {
            get { return x; }
        }

        public enum Method
        {
            MGS, Householder
        }
    }
}
