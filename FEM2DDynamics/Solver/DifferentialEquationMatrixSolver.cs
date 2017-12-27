using FEM2D.Solvers;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Solver
{
    internal class DifferentialEquationMatrixSolver : IEquationOfMotionSolver
    {

        private MatrixData matrixData;
        private IMatrixReducer matrixReducer;
        private DynamicLoadFactory loadFactory;
        private int dofNumber;
        private ILoadAggregator loadAggregator;
        private readonly DynamicSolverSettings settings;

        public DifferentialEquationMatrixSolver(DynamicSolverSettings settings)
        {
            this.loadAggregator = new LoadAggregator();
            this.settings = settings;
        }
        public DifferentialEquationMatrixSolver():this(DynamicSolverSettings.Default)
        {
            
        }

        public DynamicDisplacements Solve(MatrixData matrixData, DynamicLoadFactory loadFactory, int dofNumber, IMatrixReducer matrixReducer)
        {
            var result = new DynamicDisplacements();

            this.matrixData = matrixData;
            this.loadFactory = loadFactory;
            this.dofNumber = dofNumber;
            this.matrixReducer = matrixReducer;

            var deltaT = this.settings.DeltaTime;
            var startLoads = this.loadFactory.GetNodalLoads(0);
            var aggregatedStartLoads = this.loadAggregator.Aggregate(startLoads, dofNumber);
            var p0 = this.matrixReducer.ReduceVector(aggregatedStartLoads);
            var u0dot = Vector.Build.Sparse(dofNumber, 0d);
            var u0 = Vector.Build.Sparse(dofNumber, 0d);
            var u0dot2 = matrixData.MassMatrix.Inverse() * (p0 - this.matrixData.DampingMatrix * u0dot - this.matrixData.StiffnessMatrix * u0);


            var uMinus1 = u0 - deltaT * u0dot + 0.5 * deltaT * deltaT * u0dot2;
            var K_ = this.matrixData.MassMatrix / (deltaT * deltaT) + this.matrixData.DampingMatrix / (2 * deltaT);
            var K_Inverted = K_.Inverse();
            var a = this.matrixData.MassMatrix / (deltaT * deltaT) - this.matrixData.DampingMatrix / (2 * deltaT);
            var b = this.matrixData.StiffnessMatrix - 2 * this.matrixData.MassMatrix / (deltaT * deltaT);

            var time = this.settings.StartTime;
            var pi = p0;
            var ui = u0;
            do
            {
                var pi_ = pi - a * uMinus1 - b * ui;
                var uiPlus = K_Inverted * pi_;

                result.AddResult(time, ui.ToList());

                uMinus1 = ui;
                ui = uiPlus;
                time += deltaT;

            } while (time <= this.settings.EndTime);


            return result;
        }

    }
}
