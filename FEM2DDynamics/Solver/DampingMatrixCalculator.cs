using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Solver
{
    public class SimpleDampingMatrixCalculator : IDampingMatrixCalculator
    {

        private double alfa = 0.02;
        private double beta = 0.03;

        public Matrix<double> GetDampingMatrix(Matrix<double> stiffness, Matrix<double> mass)
        {
            var result = this.alfa * stiffness + this.beta + mass;
            return result;
        }

    }
}
