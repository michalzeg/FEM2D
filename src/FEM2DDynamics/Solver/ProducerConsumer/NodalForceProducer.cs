using FEM2DDynamics.Loads;
using FEM2DDynamics.Time;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace FEM2DDynamics.Solver.ProducerConsumer
{
    internal class NodalForceProducer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
            while (this.timeProvider.IsWorking())
            {
                try
                {
                    var loads = this.loadFactory.GetNodalLoads(this.timeProvider.CurrentTime).ToList();

                    var result = new NodalForcePayload
                    {
                        NodalLoads = loads,
                        Time = this.timeProvider.CurrentTime
                    };
                    this.nodalLoadsPayloads.Add(result);
                    this.timeProvider.Tick();
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Nodal Force Producer");
                }
            }
            this.nodalLoadsPayloads.CompleteAdding();
        }
    }
}