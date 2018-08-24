using MathNet.Numerics.LinearAlgebra;
using System;

namespace FEM2D.Solvers
{
    /// <summary>
    /// Solves equation k*u = p
    /// </summary>
    public class CholeskyDescomposition : IMatrixSolver
    {
        private Matrix<double> K;
        private Vector<double> P;

        private Matrix<double> LL;
        private Vector<double> Y;
        private Vector<double> U;

        private int dofCount;

        public CholeskyDescomposition()
        {
        }

        public Vector<double> Solve(Matrix<double> K, Vector<double> P)
        {
            this.K = K;
            this.P = P;

            Decompose();
            CalculateY();
            CalculateU();
            return this.U;
        }

        private void Decompose()
        {
            this.dofCount = this.P.Count;
            this.LL = Matrix<double>.Build.Sparse(dofCount, dofCount, 0d);
            var tempLL = this.LL; //new HashMatrix<double>(dofCount, dofCount);
            for (int i = 0; i < dofCount; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    var v = 0d;
                    if (i == j)
                    {
                        for (int n = 0; n <= j - 1; n++)
                        {
                            v = v + Math.Pow(tempLL[j, n], 2);
                        }
                        tempLL[j, j] = Math.Sqrt(K[j, j] - v);
                    }
                    else
                    {
                        for (int n = 0; n <= j - 1; n++)
                        {
                            v = v + tempLL[i, n] * tempLL[j, n];
                        }
                        tempLL[i, j] = (K[i, j] - v) / tempLL[j, j];
                    }
                }
            }
            //this.LL = tempLL.GetMatrix();
        }

        private void CalculateY()
        {
            this.Y = Vector<double>.Build.Sparse(this.dofCount, 0d);
            for (int i = 0; i < this.dofCount; i++)
            {
                var v = 0d;
                for (int j = 0; j <= i - 1; j++)
                {
                    v = v + LL[i, j] * Y[j];
                }
                Y[i] = (P[i] - v) / LL[i, i];
            }
        }

        private void CalculateU()
        {
            this.U = Vector<double>.Build.Sparse(this.dofCount, 0d);
            for (int i = this.dofCount - 1; i >= 0; i--)
            {
                var v = 0d;
                for (int j = i + 1; j <= this.dofCount - 1; j++)
                {
                    v = v + LL[j, i] * U[j];
                }
                U[i] = (Y[i] - v) / LL[i, i];
            }
        }
    }
}