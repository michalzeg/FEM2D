using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Results
{
    internal class TimeDisplacementPair
    {
        public double Time { get; set; }
        public IEnumerable<double> Displacements { get; set; }
    }
}
