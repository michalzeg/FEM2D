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

        internal void AddResult(double time, Vector<double> displacements)
        {
            this.timeDisplacementPairs.Add(new TimeDisplacementPair
            {
                Time = time,
                Displacements = displacements,
            });
        }

        internal Vector<double> GetClosesRight(double time)
        {
            var closestLeft = this.timeDisplacementPairs.TakeWhile(e => e.Time <= time).Last();

            return closestLeft.Displacements;
        }

        internal Vector<double> GetClosestLeft(double time)
        {
            var closestRight = this.timeDisplacementPairs.SkipWhile(e => e.Time < time).First();
            return closestRight.Displacements;
        }
        
    }
}
