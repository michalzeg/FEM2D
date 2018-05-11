using FEM2D.Solvers;
using System.Collections.Concurrent;

namespace FEM2DDynamics.Solver
{
    internal class AggregatedLoadProducer
    {
        private readonly ILoadAggregator loadAggregator;
        private readonly IMatrixReducer matrixReducer;
        private BlockingCollection<AggregatedLoadPayload> aggregatedLoadPayloads;
        private BlockingCollection<NodalForcePayload> nodalLoadPayloads;

        public AggregatedLoadProducer(BlockingCollection<AggregatedLoadPayload> aggregatedLoadPayloads, BlockingCollection<NodalForcePayload> nodalForcePayloads, ILoadAggregator matrixAggregator, IMatrixReducer matrixReducer)
        {
            this.aggregatedLoadPayloads = aggregatedLoadPayloads;
            this.nodalLoadPayloads = nodalForcePayloads;
            this.loadAggregator = matrixAggregator;
            this.matrixReducer = matrixReducer;
        }

        public void Execute()
        {
            do
            {
                var payload = this.nodalLoadPayloads.Take();
                var loads = payload.NodalLoads;

                var aggregatedLoad = this.loadAggregator.Aggregate(loads);
                var reducedLoad = this.matrixReducer.ReduceVector(aggregatedLoad);

                var result = new AggregatedLoadPayload
                {
                    AggregatedLoad = reducedLoad,
                    Time = payload.Time
                };
                this.aggregatedLoadPayloads.Add(result);
            }
            while (!this.nodalLoadPayloads.IsCompleted);
            this.aggregatedLoadPayloads.CompleteAdding();
        }
    }
}