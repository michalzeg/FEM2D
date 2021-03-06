﻿using FEM2D.Nodes;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Results;
using FEM2DDynamics.Solver;
using FEM2DDynamics.Utils;
using System;

namespace FEM2DDynamics.Structure
{
    public class DynamicStructure
    {
        private readonly DynamicSolverSettings settings;

        public DynamicLoadFactory LoadFactory { get; }
        public NodeFactory NodeFactory { get; }
        public DynamicElementFactory ElementFactory { get; }
        public DynamicResultFactory Results { get; private set; }

        public DynamicStructure(DynamicSolverSettings settings)
        {
            this.NodeFactory = new NodeFactory();
            this.ElementFactory = new DynamicElementFactory();
            this.LoadFactory = new DynamicLoadFactory(ElementFactory, NodeFactory);
            this.settings = settings;
        }

        public void Solve(Action<ProgressMessage> progress = null)
        {
            using (var solver = new DynamicSolver(settings, this.ElementFactory, this.NodeFactory, this.LoadFactory, progress))
            {
                solver.Initialize();
                this.Results = solver.Solve();
            }
        }
    }
}