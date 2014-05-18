using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GivensAlgorithms
{
    public class Matrix
    {
        private double[,] a;
        public int n { get; private set; }
        public int m { get; private set; }

        public Matrix(int n, int m=0)
        {
            if (m == 0)
            {
                this.a = new double[n, 1];
            }
            else
            {
                this.a = new double[n, m];
            }

            this.n = n;
            this.m = m;
        }

        public Matrix(double[,] a)
        {
            this.a = a;
            n = a.GetLength(0);
            m = a.GetLength(1);
        }

        public Matrix(double[] a)
        {
            n = a.Length;
            m = 0;

            this.a = new double[n, 1];

            for (int i = 0; i < n; i++)
            {
                this.a[i, 0] = a[i];
            }
        }

        public double this[int i, int j = 0]
        {
            get{
                if (0 == m && i < n && i >= 0)
                {
                    return a[i, 0];
                }
                else if(i < n && j < m && i >= 0 && j >= 0){
                    return a[i, j];
                }

                throw new IndexOutOfRangeException();
            }

            set{
                if (0 == m && i < n && i >= 0)
                {
                    a[i, 0] = value;
                }
                else if (i < n && j < m && i >= 0 && j >= 0)
                {
                    a[i, j] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public static Matrix operator*(Matrix a, Matrix b)
        {
            if(a.m != b.n){
                return null;
            }

            if (0 == b.m)
            {
                double[] r = new double[a.n];

                for (int i = 0; i < a.n; i++)
                {
                        r[i] = 0;
                        for (int k = 0; k < a.m; k++)
                        {
                            r[i] += a[i, k] * b[k];
                        }
                }

                return new Matrix(r);
            }
            else
            {
                double[,] r = new double[a.n, b.m];

                for (int i = 0; i < a.n; i++)
                {
                    for (int j = 0; j < b.m; j++)
                    {
                        r[i, j] = 0;
                        for (int k = 0; k < a.m; k++)
                        {
                            r[i, j] += a[i, k] * b[k, j];
                        }
                    }
                }

                return new Matrix(r);
            }

        }

        public void transpose()
        {
            //TODO: check for m == 0
            if (n == m)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        double aux = a[i, j];
                        a[i, j] = a[j, i];
                        a[j, i] = aux;
                    }

                }
            }
            else
            {
                throw new NotImplementedException("Transposal of non-square matrices is not implemented!");
            }
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();

            for(int i=0; i<n; i++){
                if (0 == m)
                {
                    strBuilder.Append(String.Format("{0,3:f3}", a[i, 0]));
                }
                else
                {
                    for (int j = 0; j < m; j++)
                    {
                        strBuilder.Append(String.Format("{0,3:f3} ", a[i, j]));
                    }
                }
                strBuilder.AppendLine();
            }

            return strBuilder.ToString();
        }
    }
}
