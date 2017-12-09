using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Results.Beams;
using FEM2D.Results.Membranes;
using FEM2D.Results.Nodes;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results
{
    public class ResultFactory
    {
        public NodeResults NodeResults { get; private set; }
        public MembraneElementResults MembraneResults { get; private set; }
        public BeamElementResults BeamResults { get; private set; }

        public ResultFactory(Vector<double> displacements, NodeFactory nodeFactory, ElementFactory elementFactory, LoadFactory loadFactory)
        {
            var dofDisplacementMap = new DofDisplacementMap(displacements);

            this.NodeResults = new NodeResults(dofDisplacementMap, nodeFactory.GetAll());
            this.MembraneResults = new MembraneElementResults(dofDisplacementMap, NodeResults, elementFactory.GetMembraneElements());
            this.BeamResults = new BeamElementResults(dofDisplacementMap, elementFactory.GetBeamElements(), loadFactory.GetBeamLoads());    
        }
        
        
    }
}
