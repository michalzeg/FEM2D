using FEM2DDynamics.Loads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Results
{
    public class DynamicResultFactory
    {
        private readonly IDynamicDofDisplacementMap dofDisplacementMap;
        private readonly DynamicLoadFactory loadFactory;

        public DynamicBeamElementResults BeamResults { get; }

        internal DynamicResultFactory(DynamicDisplacements dynamicDisplacements, DynamicLoadFactory loadFactory)
        {
            this.dofDisplacementMap = new SimpleDynamicDofDisplacementMap(dynamicDisplacements);
            this.loadFactory = loadFactory;

            this.BeamResults = new DynamicBeamElementResults(dofDisplacementMap,loadFactory);
            
        }
    }
}
