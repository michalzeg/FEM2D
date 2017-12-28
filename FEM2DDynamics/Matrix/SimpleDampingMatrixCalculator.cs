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

        private const double alfa = 0.002;
        private const double beta = 0.003;

        public Matrix<double> GetDampingMatrix(Matrix<double> stiffness, Matrix<double> mass)
        {
            var result = CalculateDampingMatrix(stiffness, mass);
            return result;
        }

        public static Matrix<double> CalculateDampingMatrix(Matrix<double> stiffness, Matrix<double> mass)
        {
            var result = alfa * stiffness + beta + mass;
            return result;
        }

        
    }
}
