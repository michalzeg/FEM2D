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

        internal Vector<double> GetClosesDisplacementRight(double time)
        {
            var closestLeft = this.timeDisplacementPairs.TakeWhile(e => e.Time <= time).Last();
            return closestLeft.Displacements;
        }

        internal Vector<double> GetClosestDisplacementLeft(double time)
        {
            var closestRight = this.timeDisplacementPairs.SkipWhile(e => e.Time < time).First();
            return closestRight.Displacements;
        }

        internal Vector<double> GetClosesVelocitiesRight(double time)
        {
            var closestLeft = this.timeDisplacementPairs.TakeWhile(e => e.Time <= time).Last();
            return closestLeft.Velocities;
        }

        internal Vector<double> GetClosestVelocitiesLeft(double time)
        {
            var closestRight = this.timeDisplacementPairs.SkipWhile(e => e.Time < time).First();
            return closestRight.Velocities;
        }

        internal Vector<double> GetClosesAccelerationsRight(double time)
        {
            var closestLeft = this.timeDisplacementPairs.TakeWhile(e => e.Time <= time).Last();
            return closestLeft.Accelerations;
        }

        internal Vector<double> GetClosestAccelerationsLeft(double time)
        {
            var closestRight = this.timeDisplacementPairs.SkipWhile(e => e.Time < time).First();
            return closestRight.Accelerations;
        }

    }
}
