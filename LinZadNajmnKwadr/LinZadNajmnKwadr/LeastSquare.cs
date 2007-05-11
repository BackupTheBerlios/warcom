using System;
using System.Collections.Generic;
using System.Text;

namespace LinZadNajmnKwadr
{
    interface LeastSquare
    {
        void Ortogonalization();
        Matrix GetQ();
        Matrix GetR();

    }
}
