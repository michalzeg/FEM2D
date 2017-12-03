using Common.DTO;
using FEM2D.Nodes;
using FEM2D.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace Output.OutputCreator
{
    public class OutputCreator
    {
        private const int percentileMin = 5;
        private const int percentileMax = 95;

        private readonly ResultFactory results;
        private readonly MembraneInputData inputData;

        private IList<NodeOutput> nodes;
        private IList<TriangleOutput> triangles;

        public OutputCreator(ResultFactory results, MembraneInputData inputData = null)
        {
            this.results = results;
            this.inputData = inputData;
        }

        public MembraneOutputData CreateOutput()
        {
            this.CreateNodeOutput();
            this.CreateTriangleOutput();

            var triangleResults = this.results.MembraneResults.GetElementResult();
            var triangleNodeResults = this.results.MembraneResults.GetNodeResult();
            var sxx = triangleResults.Select(e => e.SigmaX);
            var syy = triangleResults.Select(e => e.SigmaY);
            var txy = triangleResults.Select(e => e.TauXY);

            var membraneOutput = new MembraneOutputData
            {
                Nodes = this.nodes,
                Triangles = this.triangles,
                MaxSxx = sxx.Max(),
                MaxSyy = syy.Max(),
                MaxTxy = txy.Max(),

                MinSxx = sxx.Min(),
                MinSyy = syy.Min(),
                MinTxy = txy.Min(),

                MaxUx = triangleNodeResults.Max(e => Math.Abs(e.UX)),
                MaxUy = triangleNodeResults.Max(e => Math.Abs(e.UY)),

                SxxPercentile005 = sxx.Percentile(percentileMin),
                SxxPercentile095 = sxx.Percentile(percentileMax),

                SyyPercentile005 = syy.Percentile(percentileMin),
                SyyPercentile095 = syy.Percentile(percentileMax),

                TxyPercentile005 = txy.Percentile(percentileMin),
                TxyPercentile095 = txy.Percentile(percentileMax),

                InputData = this.inputData,
            };

            return membraneOutput;
        }

        private void CreateNodeOutput()
        {
            this.nodes = this.results.NodeResults.GetNodeResult()
                        .Select(n => n.ConvertToOutput())
                        .ToList();
        }

        private void CreateTriangleOutput()
        {
            this.triangles = new List<TriangleOutput>();

            foreach (var triangleResult in this.results.MembraneResults.GetElementResult())
            {

                var nodeResults = this.results.MembraneResults.GetNodeResult(triangleResult.Element.Nodes);

                var nodeDetails = nodeResults.Select(n => n.ConvertToOutputDetailed());

                var triangleOutput = new TriangleOutput
                {
                    Number = triangleResult.Element.Number,
                    
                    Nodes = nodeDetails,
                    Sxx = triangleResult.SigmaX,
                    Syy = triangleResult.SigmaY,
                    Txy = triangleResult.TauXY,
                };

                this.triangles.Add(triangleOutput);
            }
        }
    }
}
