using System;
using System.Collections.Generic;
using System.Text;
using NagCFunctionsAPI;

namespace Garch
{
    public class GarchModel
    {
        private double[] data;
        private int countExitValues = 1;
        private double gamma = 0.1;
        ///wspolczynniki uwzgledniajace dane historyczne - dlugosc = p+q+1
        private double[] theta;
        private int p = 1;
        private int q = 1;


        public GarchModel(int p, int q, double gamma)
        {
            this.P = p;
            this.Q = q;
            this.gamma = gamma;

            //int n = 3;
            //double[] theta = new double[1 + 1 + 1];
            //for (int i = 0; i < theta.Length; ++i)
            //    theta[i] = 0.1;
            //double[] d = new double[n];
            //for (int i = 1; i < n + 1; ++i)
            //    d[i - 1] = 1.0 / i;
            //this.theta = theta;
            //d = this.ComputeGarchModel(d, n);
        }

        public GarchModel(double[] data, int p, int q, double gamma)
            : this(p, q, gamma)
        {
            this.data = data;
        }

        public double[] EntryDataValues
        {
            get { return data; }
            set { data = value; }
        }

        public int CountExitValues
        {
            get { return countExitValues; }
            set
            {
                if (value > 0)
                    countExitValues = value;
            }
        }

        public double Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }
        public int P
        {
            get { return p; }
            set
            {
                if (p > 0 && (this.data == null || p <= this.data.Length))
                    p = value;
            }
        }

        public int Q
        {
            get { return q; }
            set
            {
                if (q >= 0 && (this.data == null || q <= this.data.Length))
                    q = value;
            }
        }

        public double[] Theta
        {
            get { return theta; }
            set
            {
                if (value.Length == p + q + 1)
                    theta = value;
            }
        }


        public double[] ComputeGarchModel()
        {
            if (this.data == null || this.data.Length < 1 || this.countExitValues < 1)
                return null;
            if (this.countExitValues > this.data.Length)
                return null;
            double[] exit = new double[countExitValues];
            NagError fail = new NagError();
            ///rozklad normalny N(0, wynik w chwili t)   - dlugosc = dlugosci wejscia data
            double[] et = new double[this.data.Length];
            Lidgren.Library.Network.NetRandom rand = new Lidgren.Library.Network.NetRandom();
            et[0] = gamma;
            for (int i = 1; i < et.Length; ++i)
                et[i] = (double)rand.NextFloat((float)et[i-1]);
            NagFunctions.g13ffc(this.data.Length, this.countExitValues, this.p, this.q,
                theta, this.gamma, exit, this.data, et, ref fail);
            if (fail.code != 0)
                return null;
            return exit;
        }

        public double[] ComputeGarchModel(int countExitValues)
        {
            this.countExitValues = countExitValues;
            return this.ComputeGarchModel();
        }

        public double[] ComputeGarchModel(double[] data)
        {
            this.data = data;
            return this.ComputeGarchModel();
        }

        public double[] ComputeGarchModel(double[] data, int countExitValues)
        {
            this.countExitValues = countExitValues;
            return this.ComputeGarchModel(data);
        }

        //public static void Main()
        //{
        //    int n = 3;
        //    garch.Garch g = new garch.Garch();
        //    double[] d = new double[n];
        //    for (int i = 1; i < n + 1; ++i)
        //        d[i - 1] = 1.0 / i;
        //    d = g.ComputeGarchModel(d, n);
        //    System.Console.WriteLine("\n------------------------------------------------------");
        //    for (int i = 0; i < d.Length; ++i)
        //    {
        //        System.Console.Write(d[i] + ",");
        //    }
        //    System.Console.ReadLine();

        //}
    }
}
