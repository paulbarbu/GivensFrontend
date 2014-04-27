using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GivensAlgorithms
{
    public class GivensAlgorithm
    {
        Matrix A, Q, b;
        List<Matrix> G = new List<Matrix>();
        
        public GivensAlgorithm(Matrix A, Matrix b)
        {
            if (A.n != A.m)
            {
                throw new ArgumentException("The first matrix is not square!");
            }

            if (A.n != b.n)
            {
                throw new ArgumentException("The number of lines in the two matrices do not match!");
            }

            if (0 != b.m)
            {
                throw new ArgumentException("The second matrix should have a single dimension!");
            }

            this.A = A;
            this.b = b;
        }

        public Matrix solve(){
            for (int i = 1; i < A.n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (0 != A[i, j])
                    {
                        Debug.WriteLine("G for {0} {1}:", i, j);
                        G.Add(generateG(i, j));
                        Debug.WriteLine(G[G.Count-1]);
                        
                        A = G[G.Count-1] * A;

                        Debug.WriteLine("A =");
                        Debug.WriteLine(A);
                    }
                }
            }

            computeQ();
            
            return solveUpper();
        }

        private Matrix solveUpper()
        {
            //Q^-1 is equal to Q transposed since it's orthogonal
            Q.transpose();
            Debug.WriteLine("Qt =");
            Debug.WriteLine(Q);

            Matrix Qb = Q * b;
            Matrix X = new Matrix(Qb.n);
            
            Debug.WriteLine("Qb =");
            Debug.WriteLine(Qb);

            for (int i = Qb.n - 1; i >= 0; i--)
            {
                double S = 0;

                for(int j = i+1; j<Qb.n; j++){
                    S += X[j] * A[i, j];
                }

                X[i] = (Qb[i] - S) / A[i, i];
            }

            return X;
        }

        private void computeQ()
        {
            G[0].transpose();
            Q = G[0];
            
            for (int i = 1; i < G.Count; i++)
            {
                G[i].transpose();

                Q = Q * G[i];
            }

            Debug.WriteLine("Q =");
            Debug.WriteLine(Q);
        }

        private Matrix generateG(int i, int j){
            double a = A[i-1,j];
            double b = A[i,j];
            //TODO: stable calculation
            double r = Math.Sqrt(a * a + b * b);
            double c = a / r;
            double s = -b / r;

            Matrix G = new Matrix(A.n, A.m);

            for (int k = 0; k < A.n; k++)
            {
                G[k, k] = 1;
            }

            G[i, i] = G[j, j] = c;
            G[i, j] = s;
            G[j, i] = -s;

            return G;
        }
    }
}
