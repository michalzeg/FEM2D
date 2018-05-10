using FEM2D.Solvers;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;
using FEM2DDynamics.Time;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Concurrent;

namespace FEM2DDynamics.Solver
{
    internal class DifferentialEquationMatrixSolver : IEquationOfMotionSolver
    {
        private MatrixData matrixData;
        private readonly LoadPositionProducer loadPositionProducer;
        private readonly BlockingCollection<Payload> payloads;
        private IMatrixReducer matrixReducer;
        private ILoadAggregator loadAggregator;
        private readonly TimeProvider timeProvider;

        public DifferentialEquationMatrixSolver(TimeProvider timeProvider, ILoadAggregator loadAggregator, IMatrixReducer matrixReducer, MatrixData matrixData, LoadPositionProducer loadPositionProducer, BlockingCollection<Payload> payloads)
        {
            this.timeProvider = timeProvider;
            this.loadAggregator = loadAggregator;
            this.matrixReducer = matrixReducer;
            this.matrixData = matrixData;
            this.loadPositionProducer = loadPositionProducer;
            this.payloads = payloads;
        }

        public DynamicDisplacements Result { get; private set; }

        public void Solve()
        {
            Result = new DynamicDisplacements(timeProvider);

            var deltaT = this.timeProvider.DeltaTime;
            var payload = this.payloads.Take();
            var p0 = payload.AggregatedLoad;

            var u0dot = Vector.Build.Sparse(p0.Count, 0d);
            var u0 = Vector.Build.Sparse(p0.Count, 0d);
            var u0dot2 = matrixData.MassMatrix.Inverse() * (p0 - this.matrixData.DampingMatrix * u0dot - this.matrixData.StiffnessMatrix * u0);

            var uiMinus1 = u0 - deltaT * u0dot + 0.5 * deltaT * deltaT * u0dot2;
            var K_ = this.matrixData.MassMatrix / (deltaT * deltaT) + this.matrixData.DampingMatrix / (2 * deltaT);
            var K_Inverted = K_.Inverse();
            var a = this.matrixData.MassMatrix / (deltaT * deltaT) - this.matrixData.DampingMatrix / (2 * deltaT);
            var b = this.matrixData.StiffnessMatrix - 2 * this.matrixData.MassMatrix / (deltaT * deltaT);

            var pi = p0;
            var ui = u0;
            do
            {
                var time = payload.Time;
                var pi_ = pi - a * uiMinus1 - b * ui;
                var uiPlus1 = K_Inverted * pi_;

                var uidot = (uiPlus1 - uiMinus1) / (2 * deltaT);
                var uidot2 = (uiPlus1 - 2 * ui + uiMinus1) / (deltaT * deltaT);

#if DEBUG
                var res = this.matrixData.StiffnessMatrix * ui + this.matrixData.DampingMatrix * uidot + this.matrixData.MassMatrix * uidot2 - pi;
#endif
                Result.AddResult(time, ui, uidot, uidot2);

                uiMinus1 = ui;
                ui = uiPlus1;
                //this.timeProvider.Tick();
                payload = this.payloads.Take();
                pi = payload.AggregatedLoad;
            } while (!this.payloads.IsCompleted);

        }
    }
}