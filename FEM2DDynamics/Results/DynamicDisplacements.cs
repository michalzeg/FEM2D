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

        public DynamicDisplacements()
        {

        }

        internal void AddResult(double time, IEnumerable<double> displacements)
        {
            this.timeDisplacementPairs.Add(new TimeDisplacementPair
            {
                Time = time,
                Displacements = displacements,
            });
        }
    }
}
