using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Results;
using FEM2D.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Solvers
{
    public sealed class Solver
    {
        private readonly IMatrixAggregator matrixAggregator;
        private readonly ILoadAggregator loadAggregator;
        private readonly IMatrixReducer matrixReducer;
        private readonly IMatrixSolver matrixSolver;

        public ResultFactory Results { get; private set; }

        public Solver()
        {
            this.matrixAggregator = new MatrixAggregator();
            this.loadAggregator = new LoadAggregator();
            this.matrixReducer = new MatrixReducer();
            this.matrixSolver = new CholeskyDescomposition();
        }


        public void Solve(ElementFactory elementFactory, NodeFactory nodeFactory, LoadFactory loadFactory)
        {
            var nodes = nodeFactory.GetAll();
            var elements = elementFactory.GetAll();
            var loads = loadFactory.GetNodalLoads();

            var dofNumber = nodeFactory.GetDOFsCount();
            var stiffnessMatrix = matrixAggregator.Aggregate(elements, dofNumber);
            var loadVector = loadAggregator.Aggregate(loads, dofNumber);

            this.matrixReducer.Initialize(nodes, dofNumber);
            var reducedStiffnessMatrix = this.matrixReducer.ReduceMatrix(stiffnessMatrix);
            var reducedLoadVector = this.matrixReducer.ReduceVector(loadVector);
            

            var displacements = this.matrixSolver.Solve(reducedStiffnessMatrix, reducedLoadVector);
            this.Results = new ResultFactory(displacements, nodeFactory, elementFactory,loadFactory);
        }
    }
}
