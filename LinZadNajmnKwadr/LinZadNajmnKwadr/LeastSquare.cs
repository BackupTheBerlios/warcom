using System;
using System.Collections.Generic;
using System.Text;

namespace LinZadNajmnKwadr
{
    class LeastSquare
    {
        protected Matrix a;
        protected Matrix q;
        protected Matrix r;

        protected int m, n;

        public LeastSquare(Matrix a)
        {
            if (a == null)
                throw new ArgumentNullException();

            this.a = new Matrix(a);
            this.m = a.Rows;
            this.n = a.Columns;
        }

        //method intented to be overdriven
        public void Ortogonalization()
        {
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
    }
}
