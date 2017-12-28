using FEM2D.Nodes;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Results;
using FEM2DDynamics.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Structure
{
    public class DynamicStructure
    {
        private readonly DynamicSolver solver;


        public DynamicLoadFactory LoadFactory { get; private set; }
        public NodeFactory NodeFactory { get; private set; }
        public DynamicElementFactory ElementFactory { get; private set; }
        public DynamicResultFactory Results { get; private set; }

        public DynamicStructure(DynamicSolverSettings settings)
        {
            this.solver = new DynamicSolver(settings);
            this.NodeFactory = new NodeFactory();
            this.ElementFactory = new DynamicElementFactory();
            this.LoadFactory = new DynamicLoadFactory(ElementFactory, NodeFactory);

        }

        public void Solve()
        {
            this.solver.Solve(this.ElementFactory, this.NodeFactory, this.LoadFactory);
            this.Results = this.solver.Results;
        }
    }
}
