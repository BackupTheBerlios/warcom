using System;
using System.Collections.Generic;
using System.Text;

namespace LinZadNajmnKwadr
{
    class LinearLeastSquares
    {
        private LeastSquare mgs;
        private Matrix r;
        private Matrix x;

        private Matrix b;
        private int n;

        public LinearLeastSquares(Matrix a, Matrix b,bool isSwitchOn)
        {
            if (a == null || b == null)
                throw new ArgumentNullException();

            if (a.Rows != b.Rows)
                throw new ArgumentException();

            if (b.Columns != 1)
                throw new ArgumentException("b must be a vector!");

            this.n = a.Columns;
            if (isSwitchOn)
                mgs = new MGS(a);
            else
                mgs = null;
            mgs.Ortogonalization();
            this.b = b;
            this.x = new Matrix(a.Columns, 1);
        }

        public void Solve()
        {
            r = new Matrix(b);
            for (int i = 0; i < n; i++)
            {
                Matrix temp = mgs.Q.GetColumn(i).Transposition()*r;
                x[i, 0] = temp[0, 0];
                r -= mgs.Q.GetColumn(i) * x[i, 0];
            }

            for (int k = n - 1; k > 0; k--)
            {
                /*x[k, 0] /= r[k, k];
                for (int i = 0; i < k; i++)
                    x[i, 0] -= r[i, k] * x[k, 0];*/
                x[k, 0] /= mgs.R[k, k];
                for (int i = 0; i < k; i++)
                    x[i, 0] -= mgs.R[i, k] * x[k, 0];
            }
        }

        public Matrix X
        {
            get { return x; }
        }
    }
}
