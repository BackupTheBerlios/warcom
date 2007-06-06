using System;
using System.Collections.Generic;
using System.Text;

namespace LinZadNajmnKwadr
{
    class Householder : LeastSquare
    {
        private int l;
        private Matrix qk;
        private Matrix a;
        private Matrix q;
        private Matrix r;

        private int m, n;
        
        public Householder(Matrix a)
        {
            if (a == null)
                throw new ArgumentNullException();

            this.a = new Matrix(a);
            this.m = a.Rows;
            this.n = a.Columns;
            qk = new Matrix(1, a.Columns);
            l = Math.Min(this.n , this.m - 1);
        }

        public void Orthogonalization()
        {
            if (a == null)
                throw new ArgumentNullException();
            for (int k = 0; k < l; ++k)
            {
                int sign = (a[k, k] > 0) ? -1 : 1;
                Matrix temp = new Matrix(1, this.m - k);
                for (int tt = k; tt < this.m; ++tt)
                    temp[0, tt - k] = a[tt, k];
                qk[0, k] = sign * temp.Norm();
                a[k, k] = 1 - a[k, k] / qk[0, k];
                for (int i = k + 1; i < this.m; ++i)
                    a[i, k] = ( - a[i, k] ) / qk[0, k];
                for (int j = k + 1; j < this.n; ++j)
                {
                    double beta = 0;
                    for (int i = k; i < this.m; ++i)
                        beta += a[i, k] * a[i, j];
                    beta /= a[k, k];
                    for (int i = k; i < this.m; ++i)
                        a[i, j] -= a[i, k] * beta;
                }
            }
            if (this.l < this.n)
                qk[0, n - 1] = a[n - 1, n - 1];
        }

        public Matrix R
        {
            get
            {
                if (r == null)
                {
                    this.r = new Matrix(a.Columns, a.Columns);
                    for (int i = 0; i < a.Columns; ++i)
                        r[i, i] = qk[0, i];
                    for (int i = 0; i < a.Rows; i++)
                        for (int j = i + 1; j < a.Columns; j++)
                            r[i, j] = a[i, j];
                }
                return r;
            }
        }

        public Matrix Q
        {
            get {
                if (q == null)
                {

                    Matrix I = new Matrix(a.Rows, a.Rows);
                    for (int i = 0; i < I.Columns; i++)
                        I[i, i] = 1;
                    this.q = new Matrix(a.Rows, a.Columns);

                    Matrix vk = new Matrix(a.Rows, 1);
                    for (int j = 0; j < this.m; j++)
                        vk[j, 0] = a[j, 0];
                    Matrix vkt = vk.Transposition();
                    q = I - ((vk * vkt) / a[0, 0]);

                    for (int k = 1; k < this.l; k++)
                    {
                        vk.SetAllValues(0);
                        for (int j = k; j < this.m; j++)
                            vk[j, 0] = a[j, k];
                        vkt = vk.Transposition();
                        q *= I - ((vk * vkt) / a[k, k]);
                    }
                }


                return q;
            }
        }

        public Matrix GetQ()
        {
            return Q;
        }

        public Matrix GetR()
        {
            return R;
        }
        
        public Matrix A
        {
            get { return a; }
        }
    }
}
