﻿using FEM2D.Nodes;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Results;
using FEM2DDynamics.Solver;

namespace FEM2DDynamics.Structure
{
    public class DynamicStructure
    {
        private DynamicSolver solver;
        private readonly DynamicSolverSettings settings;

        public DynamicLoadFactory LoadFactory { get; private set; }
        public NodeFactory NodeFactory { get; private set; }
        public DynamicElementFactory ElementFactory { get; private set; }
        public DynamicResultFactory Results { get; private set; }

        public DynamicStructure(DynamicSolverSettings settings)
        {
            
            this.NodeFactory = new NodeFactory();
            this.ElementFactory = new DynamicElementFactory();
            this.LoadFactory = new DynamicLoadFactory(ElementFactory, NodeFactory);
            this.settings = settings;
        }

        public void Solve()
        {
            this.solver = new DynamicSolver(settings, this.ElementFactory, this.NodeFactory, this.LoadFactory);
            this.Results = this.solver.Solve();
        }
    }
}