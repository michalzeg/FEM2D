using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Forces
{
    public class BeamForces
    {
        public double Moment { get; set; }
        public double Shear { get; set; }
        public double Axial { get; set; }

        public static BeamForces FromFEMResult(Vector<double> forces)
        {
            return new BeamForces
            {
                Axial = forces[0],
                Shear = forces[1],
                Moment = forces[2]
            };
        }
    }
}
