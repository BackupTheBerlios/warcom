using System;
using System.Collections.Generic;
using System.Text;


namespace LinZadNajmnKwadr
{
    class MGS
    {
        private Matrix q;
        private Matrix r;
        private Matrix a;

        private int m, n;

        public MGS(Matrix a)
        {
            if (a == null)
                throw new ArgumentNullException();

            this.a = a;
            this.q = new Matrix(a.Rows, a.Columns);
            this.r = new Matrix(a.Columns, a.Columns);

            this.m = a.Rows;
            this.n = a.Columns;
        }

        public void Ortogonalization()
        {
            if (a == null)
                throw new ArgumentNullException();

            for (int k = 0; k < n; k++)
            {
                q.SetColumn(k, a, k);
                for (int i = 0; i < k - 1; i++)
                {
                    Matrix temp = q.GetColumn(i).Transposition()*q.GetColumn(k);
                    r[i, k] = temp[0, 0];

                    temp = q.GetColumn(k) - q.GetColumn(i)*r[i,k];
                    q.SetColumn(k, temp, 0);
                }

                r[k, k] = q.GetColumn(k).Norm();
                Matrix qk = q.GetColumn(k);
                qk /= r[k, k];
                q.SetColumn(k, qk,0);
            }
        }

        public Matrix R
        {
            get { return r; }
        }

        public Matrix Q
        {
            get { return q; }
        }

        public Matrix A
        {
            get { return a; }
        }
    }
}


/*for (int i = 0; i < a.Columns; i++)
{
    q.SetColumn(i, a, i);
    wa[i] = a.pkolumne(i);
    double norm = q.GetColumn(i).Norm();
    double di = norm * norm;
                
    for (int j = i + 1; j < a.Columns; j++)
    {
        Matrix temp = a.GetColumn(j).Transposition() * q.GetColumn(i);
        r(j, i) = temp[0,0] / di;
        wa[j] = wa[j] - wq[i] * r(i, j);
    }
}

for (int i = 0; i < a.Columns; i++)
{
    r[i, i] = 1;
    q.wkolumne(i, wq[i]);
    a.wkolumne(i, wa[i]);
}*/