using FEM2D.Elements;
using FEM2D.Geometries;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Results;
using FEM2D.Solvers;
using FEM2DCommon.DTO;

namespace FEM2D.Structures
{
    public class Structure
    {
        public LoadFactory LoadFactory { get; private set; }
        public NodeFactory NodeFactory { get; private set; }
        public ElementFactory ElementFactory { get; private set; }
        public ResultFactory Results { get; private set; }

        public Structure()
        {
            this.NodeFactory = new NodeFactory();
            this.ElementFactory = new ElementFactory();
            this.LoadFactory = new LoadFactory();
        }

        public void Solve()
        {
            var solver = new Solver(this.ElementFactory, this.NodeFactory, this.LoadFactory);
            solver.Solve();
            this.Results = solver.Results;
        }

        public void AddMembraneGeometry(MembraneInputData membraneData)
        {
            var membraneCreator = new MembraneCreator(this.NodeFactory, this.ElementFactory, this.LoadFactory);
            membraneCreator.CreateGeometry(membraneData);
        }
    }
}