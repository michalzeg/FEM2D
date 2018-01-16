using Common.Extensions;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Solver
{
    internal class NaturalFrequencyCalculator : INaturalFrequencyCalculator
    {
        private readonly Matrix<double> mass;
        private readonly Matrix<double> stiffness;

        private IList<double> naturalFrequency;

        public NaturalFrequencyCalculator(Matrix<double> mass, Matrix<double> stiffness)
        {
            this.mass = mass;
            this.stiffness = stiffness;

            this.Calculate();
        }

        private void Calculate()
        {
            var eigen = (this.mass.Inverse() * this.stiffness).Evd();

            this.naturalFrequency = eigen.EigenValues
                                         .OrderBy(e => e.Real)
                                         .Select(e=>e.Real)
                                         .Where(e=>!e.IsApproximatelyEqualTo(1d))
                                         .ToList();

        }

        public double GetFirstMode()
        {
            return this.naturalFrequency[0];
        }

        public double GetSecondMode()
        {
            return this.naturalFrequency[1];
        }

    }
}
