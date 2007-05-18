using System;
using System.Collections.Generic;
using System.Text;


namespace LinZadNajmnKwadr
{
    class MGS : LeastSquare
    {
        private Matrix a;
        private Matrix q;
        private Matrix r;

        private int m, n;

        public MGS(Matrix a)
        {
            if (a == null)
                throw new ArgumentNullException();

            this.a = new Matrix(a);
            this.m = a.Rows;
            this.n = a.Columns;
            this.q = new Matrix(a.Rows, a.Columns);
            this.r = new Matrix(a.Columns, a.Columns);
        }

        /*public void Orthogonalization()
        {
            if (a == null)
                throw new ArgumentNullException();

            for (int k = 0; k < n; k++)
            {
                q.SetColumn(k, a, k);
                for (int i = 0; i < k - 1; i++)
                {
                    Matrix temp = q.GetColumn(i).Transposition() * q.GetColumn(k);
                    r[i, k] = temp[0, 0];

                    temp = q.GetColumn(k) - (q.GetColumn(i) * r[i, k]);
                    q.SetColumn(k, temp, 0);
                }
                r[k, k] = q.GetColumn(k).Norm();
                Matrix qk = q.GetColumn(k);
                qk /= r[k, k];
                q.SetColumn(k, qk, 0);
            }
        }*

        /*
         * for k=1:n
                R(k,k) = norm(A(:,k));
                Q(:,k) = A(:,k)/R(k,k);
                
         *      R(k,k+1:n) = Q(:,k)'*A(:,k+1:n);
                A(:,k+1:n) = A(:,k+1:n) - Q(:,k)*R(k,k+1:n);
            end
         */

        public void Orthogonalization()
        {
            if (a == null)
                throw new ArgumentNullException();

            Matrix acopy = new Matrix(a);

            for (int k = 0; k < n; k++)
            {
                Matrix ak = acopy.GetColumn(k);
                r[k, k] = ak.Norm();
                
                Matrix qk = ak / r[k, k];
                q.SetColumn(k, qk, 0);

                for (int i = k+1; i < n; i++)
                {
                    Matrix temp = q.GetColumn(k).Transposition() * acopy.GetColumn(i);
                    r[k, i] = temp[0, 0];

                    temp = acopy.GetColumn(i) - q.GetColumn(k) * r[k, i];
                    acopy.SetColumn(i, temp, 0);
                }
            }
        }


        public Matrix A
        {
            get { return a; }
        }

        public Matrix R
        {
            get { return r; }
        }

        public Matrix Q
        {
            get { return q; }
        }

        public Matrix GetQ()
        {
            return Q;
        }

        public Matrix GetR()
        {
            return R;
        }
    }
}