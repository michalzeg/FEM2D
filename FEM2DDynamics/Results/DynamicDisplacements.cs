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

        internal DynamicDisplacements()
        {

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
            var closestLeft = this.timeDisplacementPairs.TakeWhile(e => e.Time <= time).Last();
            return closestLeft;
        }

        internal TimeDisplacementPair GetClosestLeft(double time)
        {
            var closestRight = this.timeDisplacementPairs.SkipWhile(e => e.Time < time).First();
            return closestRight;
        }
        
    }
}
