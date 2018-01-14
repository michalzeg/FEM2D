using FEM2DCommon.DTO;
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
        private readonly IEnumerable<NodeResult> nodeResults;
        private readonly IEnumerable<MembraneElementResult> membraneResults;

        private IList<NodeOutput> nodes;
        private IList<TriangleOutput> triangles;

        public MembraneOutputData Output { get; private set; }
        public bool HasError { get; private set; }

        public OutputCreator(ResultFactory results, MembraneInputData inputData = null)
        {
            this.nodeResults = results.NodeResults.GetNodeResult();
            this.membraneResults = results.MembraneResults.GetElementResult();

            if (nodeResults.Any(e => double.IsNaN(e.UX) || double.IsNaN(e.UY))) 
            {
                this.HasError = true;
                return;
            }

            this.results = results;
            this.inputData = inputData;
        }

        public void CreateOutput()
        {
            this.CreateNodeOutput();
            this.CreateTriangleOutput();

            

            var sxx = this.membraneResults.Select(e => e.SigmaX);
            var syy = this.membraneResults.Select(e => e.SigmaY);
            var txy = this.membraneResults.Select(e => e.TauXY);

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

                MaxUx = nodeResults.Max(e => Math.Abs(e.UX)),
                MaxUy = nodeResults.Max(e => Math.Abs(e.UY)),

                SxxPercentile005 = sxx.Percentile(percentileMin),
                SxxPercentile095 = sxx.Percentile(percentileMax),

                SyyPercentile005 = syy.Percentile(percentileMin),
                SyyPercentile095 = syy.Percentile(percentileMax),

                TxyPercentile005 = txy.Percentile(percentileMin),
                TxyPercentile095 = txy.Percentile(percentileMax),

                InputData = this.inputData,
            };

            this.Output = membraneOutput;
        }

        private void CreateNodeOutput()
        {
            this.nodes = nodeResults
                        .Select(n => n.ConvertToOutput())
                        .ToList();
        }

        private void CreateTriangleOutput()
        {
            this.triangles = new List<TriangleOutput>();

            foreach (var triangleResult in this.membraneResults)
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
