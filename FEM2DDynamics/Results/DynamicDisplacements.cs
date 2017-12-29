using FEM2DDynamics.Solver;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Results
{


    internal class DynamicDisplacements
    {
        private readonly IList<TimeDisplacementPair> timeDisplacementPairs = new List<TimeDisplacementPair>();
        private readonly DynamicSolverSettings dynamicSolverSettings;

        internal DynamicDisplacements(DynamicSolverSettings dynamicSolverSettings)
        {
            this.dynamicSolverSettings = dynamicSolverSettings;
        }

        internal void AddResult(double time, Vector<double> displacements, Vector<double> velocities, Vector<double> accelerations)
        {
            this.timeDisplacementPairs.Add(new TimeDisplacementPair
            {
                Time = time,
                Displacements = displacements,
                Velocities = velocities,
                Accelerations = accelerations
            });
        }

        internal TimeDisplacementPair GetClosesRight(double time)
        {
            var index = this.CalculateClosestRightIndex(time);
            var closestLeft = this.timeDisplacementPairs[index];
            return closestLeft;
        }

        internal TimeDisplacementPair GetClosestLeft(double time)
        {
            var index = this.CalculateClosestLeftIndex(time);
            var closestRight = this.timeDisplacementPairs[index];
            return closestRight;
        }
        
        private int CalculateClosestRightIndex(double time)
        {
            var index = this.CalculateClosestLeftIndex(time) + 1;

            var result = index >= this.timeDisplacementPairs.Count-1 ? this.timeDisplacementPairs.Count-1 : index;

            return result;
        }

        private int CalculateClosestLeftIndex(double time)
        {
            var result = (int)Math.Floor(time / this.dynamicSolverSettings.DeltaTime);
            return result;
        }
    }
}
