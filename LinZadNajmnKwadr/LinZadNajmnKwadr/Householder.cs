using System;
using System.Collections.Generic;
using System.Text;

namespace LinZadNajmnKwadr
{
    class Householder : LeastSquare
    {
        private int l;
        private Matrix q;
        private Matrix r;

        public Householder(Matrix a)
            : base(a)
        {
            this.q = new Matrix(a.Rows, a.Rows);
            this.r = new Matrix(a.Rows, a.Columns);
            l = Math.Min(this.n, this.m - 1);
        }

        new public void Ortogonalization()
        {
            if (a == null)
                throw new ArgumentNullException();
            for (int k = 0; k < l; ++k)
            {
                
            }
        }
    }
}
