using System;
using System.Collections.Generic;
using System.Text;

namespace LinZadNajmnKwadr
{
    interface LeastSquare
    {
        void Orthogonalization();
        Matrix GetQ();
        Matrix GetR();

    }
}
