﻿using FEM2DCommon.Forces;
using FEM2D.Elements.Beam;
using FEM2D.Loads;
using FEM2D.Results.Beams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Results.Beam
{
    public class DynamicBeamElementResult : BeamElementResult
    {
        public double Time { get; }
        private IList<double> acceleration;

        public DynamicBeamElementResult(BeamForces forcesAtStart, IEnumerable<IBeamLoad> loads, IBeamElement element,IList<double> displacements,IList<double> acceleration, double time) 
            :base(forcesAtStart,loads,element,displacements)
        {
            this.Time = time;
            this.acceleration = acceleration;
        }

        public double GetAcceleration(double relativePosition)
        {
            var result = this.GetGeneralizedDisplacement(relativePosition, this.acceleration);
            return result;
        }
    }
}
