using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Results.Beams;
using FEM2D.Results.Membranes;
using FEM2D.Results.Nodes;
using FEM2D.Results.Trusses;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Results
{
    public class ResultFactory
    {
        public NodeResults NodeResults { get; }
        public MembraneElementResults MembraneResults { get; }
        public BeamElementResults BeamResults { get; }
        public TrussElementResults TrussResults { get; }

        public ResultFactory(Vector<double> displacements, NodeFactory nodeFactory, ElementFactory elementFactory, LoadFactory loadFactory)
        {
            var dofDisplacementMap = new DofDisplacementMap(displacements);

            this.NodeResults = new NodeResults(dofDisplacementMap, nodeFactory.GetAll());
            this.MembraneResults = new MembraneElementResults(dofDisplacementMap, NodeResults, elementFactory.GetMembraneElements());
            this.BeamResults = new BeamElementResults(dofDisplacementMap, elementFactory.GetBeamElements(), loadFactory.GetBeamLoads());
            this.TrussResults = new TrussElementResults(dofDisplacementMap, elementFactory.GetTrussElements());
        }
    }
}