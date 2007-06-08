using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace LinZadNajmnKwadr
{
    class Matrix
    {

        protected double[,] m;
        private int rows;
        private int columns;	//ilosc wierszy i kolumn

        private static Regex sizesRegex = new Regex(@"(?<rows>\d), *(?<columns>\d)", RegexOptions.Compiled);
        private static Regex rowRegex = new Regex(@"(?<value>(\d(\.\d*){0,1} +))*", RegexOptions.Compiled);

        public int Rows
        {
            get { return rows; }
        }

        public int Columns
        {
            get { return columns; }
        }

        public Matrix(int a, int b)
        {
            rows = a;
            columns = b;
            m = new double[rows, columns];

        }



        public void SetAllValues(double val)
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    m[i, j] = val;
        }

        public Matrix(Matrix matrix)
        {
            rows = matrix.Rows;
            columns = matrix.Columns;
            m = new double[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    m[i, j] = matrix[i, j];
        }

        public Matrix(Matrix matrix, int numberOfColumns)
        {
            if(numberOfColumns < 1 || numberOfColumns > matrix.columns)
                throw new IndexOutOfRangeException();
            rows = matrix.Rows;
            columns = numberOfColumns;
            m = new double[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    m[i, j] = matrix[i, j];
        }

        public Matrix(double[] array, int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            m = new double[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    m[i, j] = array[i * columns + j];
        }



        public void SaveToFile(string filepath)
        {
            StreamWriter wrt = new StreamWriter(filepath);
            wrt.WriteLine(this.rows + "," + this.columns);
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                    wrt.Write(this[i, j] + " ");
                wrt.WriteLine();
            }
            wrt.Close();
        }

        public double this[int a, int b]
        {
            get
            {
                if (a < 0 || b < 0 || a > rows || b > columns)
                    throw new IndexOutOfRangeException();

                return m[a, b];
            }
            set
            {
                if (a < 0 || b < 0 || a > rows || b > columns)
                    throw new IndexOutOfRangeException();

                m[a, b] = value;
            }
        }


        public Matrix GetColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= columns)
                throw new ArgumentException("Incorrect column index");

            Matrix column = new Matrix(rows, 1);
            for (int i = 0; i < rows; i++)
                column[i, 0] = this[i, columnIndex];
            return column;
        }

        public void SetColumn(int destinationColumnIndex, Matrix source, int sourceColumnIndex)
        {
            if (destinationColumnIndex < 0 || destinationColumnIndex >= columns)
                throw new ArgumentException("Incorrect destinationColumnIndex");
            if (sourceColumnIndex < 0 || sourceColumnIndex >= source.Columns)
                throw new ArgumentException("Incorrect sourceColumnIndex");
            if (source.Rows != this.Rows)
                throw new ArgumentException("Incorrect column source matrix size");

            for (int i = 0; i < rows; i++)
                this[i, destinationColumnIndex] = source[i, sourceColumnIndex];
        }

        public Matrix Transposition()
        {
            Matrix tr = new Matrix(columns, rows);
            for (int i = 0; i < columns; i++)
                for (int j = 0; j < rows; j++)
                    tr[i, j] = m[j, i];
            return tr;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a == null || b==null)
                throw new ArgumentNullException();

            if (a.Columns != b.Rows)
                return null;

            Matrix result = new Matrix(a.Rows, b.Columns);

            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < b.Columns; j++)
                {
                    double sum = 0;
                    for (int l = 0; l < a.Columns; l++)
                    {
                        sum += a[i, l] * b[l, j];
                        result[i, j] = sum;

                    }
                }
            return result;
        }

        public static Matrix operator *(Matrix a, double d)
        {
            if (a == null)
                throw new ArgumentNullException();

            Matrix result = new Matrix(a);
            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; j < result.Columns; j++)
                    result[i, j] *= d;
            return result;
        }

        public static Matrix operator /(Matrix a, double d)
        {
            if (a == null)
                throw new ArgumentNullException();

            Matrix result = new Matrix(a);
            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; j < result.Columns; j++)
                    result[i, j] /= d;
            return result;
        }

        /// <summary>
        /// Norma euklidesowa
        /// </summary>
        /// <returns></returns>
        public double Norm()
        {
            double sum = 0;
            for (int i = 0; i < this.Rows; i++)
                for (int j = 0; j < this.Columns; j++)
                    sum += this[i, j]*this[i, j];

            return Math.Sqrt(sum);
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException();

            if (a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException();

            Matrix result = new Matrix(a.Rows, a.Columns);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Columns; j++)
                    result[i, j] = a[i, j] + b[i, j];
            return result;
        }


        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException();

            if (a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException();

            Matrix result = new Matrix(a.Rows, a.Columns);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Columns; j++)
                    result[i, j] = a[i, j] - b[i, j];
            return result;
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                    sb.Append(this[i, j] + " ");

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public String ToString(int digits)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                    sb.Append(Math.Round(this[i, j],digits) + " ");

                sb.AppendLine();
            }

            return sb.ToString();
        }

        /*Matrix operator/(const Matrix& m, double d)
        {
            Matrix wynik(m.w, m.k);
            for(int i=0; i<m.w; i++)
                for(int j=0; j<m.k; j++)
                    wynik.m[i][j]=m.m[i][j]/d;
            return wynik;
        }

        double ilskal(const Matrix& a, const Matrix& b)
        {
            double ilskal=0;
            if(a.k==1 && b.k==1)
            {
                for(int i=0; i<a.w; i++)
                    ilskal+=a.m[i][0]*b.m[i][0];
                return ilskal;
            }
            else 
            {
                cout<<"Do liczenia iloczynu skalarnego nie wybrane zostaly wektory!!!"<<endl;
                return -123456;//sygnalizacja bledu argumentow
            }
        }

        double norma2(Matrix& wekt)
        {
            double norma=0;
            if(wekt.kol()==1)
            {

                for(int i=0; i<wekt.wier(); i++)
                    norma+=fabs(wekt(i,0))*fabs(wekt(i,0));
                norma=sqrt(norma);
            }
            else norma=-1; //sygnalizacja bledu argumentu
            return norma;
        }*/
        /*
        ostream& operator<<(ostream& out, const Matrix& arg)
        {
            for(int i=0; i<arg.w; i++)
            {
                for(int j=0; j<arg.k; j++)
                {
                    if(fabs(arg.m[i][j])<0.000000001) out<<setw(4)<<0<<" ";
                    else out<<setw(4)<<setprecision(2)<<arg.m[i][j]<<" ";
                }
                out<<endl;
            }
            out<<endl;
            return out; 
        }

        istream& operator>>(istream& wej, Matrix& m)
        {
            for(int i=0; i<m.w; i++)
                for(int j=0; j<m.k; j++)
                {
                    cout<<"Podaj zawartosc pola ("<<i<<","<<j<<"):"<<endl;
                    cin>>m.m[i][j];
                }
                return wej;
        }*/
    }
}
