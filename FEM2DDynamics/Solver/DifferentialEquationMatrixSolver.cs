using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;
using FEM2DDynamics.Time;
using FEM2DDynamics.Utils;
using MathNet.Numerics.LinearAlgebra.Double;
using NLog;
using System;
using System.Collections.Concurrent;

namespace FEM2DDynamics.Solver
{
    internal class DifferentialEquationMatrixSolver : IEquationOfMotionSolver
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly MatrixData matrixData;
        private readonly BlockingCollection<AggregatedLoadPayload> aggregatedLoadPayloads;
        private readonly IProgress<ProgressMessage> progress;
        private readonly ITimeData timeData;

        public DifferentialEquationMatrixSolver(ITimeData timeData, MatrixData matrixData, BlockingCollection<AggregatedLoadPayload> aggregatedLoadPayloads)
        {
            this.timeData = timeData;
            this.matrixData = matrixData;
            this.aggregatedLoadPayloads = aggregatedLoadPayloads;
        }

        public DynamicDisplacements Result { get; private set; }

        public void Solve()
        {
            Result = new DynamicDisplacements(timeData);

            var deltaT = this.timeData.DeltaTime;
            var payload = this.aggregatedLoadPayloads.Take();
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
                try
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
                    payload = this.aggregatedLoadPayloads.Take();
                    pi = payload.AggregatedLoad;
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Solver");
                }
            } while (!this.aggregatedLoadPayloads.IsCompleted);
        }
    }
}