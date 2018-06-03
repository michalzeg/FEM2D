using FEM2DDynamics.Loads;
using FEM2DDynamics.Time;
using System.Collections.Concurrent;
using System.Linq;

namespace FEM2DDynamics.Solver
{
    internal class NodalForceProducer
    {
        private readonly DynamicLoadFactory loadFactory;
        private readonly ITimeProvider timeProvider;
        private BlockingCollection<NodalForcePayload> nodalLoadsPayloads;

        public NodalForceProducer(DynamicLoadFactory loadFactory, ITimeProvider timeProvider, BlockingCollection<NodalForcePayload> nodalLoadsPayloads)
        {
            this.loadFactory = loadFactory;
            this.timeProvider = timeProvider;
            this.nodalLoadsPayloads = nodalLoadsPayloads;
        }

        public void Execute()
        {
            do
            {
                var loads = this.loadFactory.GetNodalLoads(this.timeProvider.CurrentTime).ToList();

                var result = new NodalForcePayload
                {
                    NodalLoads = loads,
                    Time = this.timeProvider.CurrentTime
                };
                this.nodalLoadsPayloads.Add(result);
                this.timeProvider.Tick();
            } while (this.timeProvider.IsWorking());
            this.nodalLoadsPayloads.CompleteAdding();
        }
    }
}