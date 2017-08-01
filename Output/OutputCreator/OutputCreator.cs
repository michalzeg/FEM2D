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

        private readonly ResultProvider results;

        private IList<NodeOutput> nodes;
        private IList<TriangleOutput> triangles;

        public OutputCreator(ResultProvider results)
        {
            this.results = results;
            
        }

        public MembraneOutputData CreateOutput()
        {
            this.CreateNodeOutput();
            this.CreateTriangleOutput();

            var sxx = this.results.TriangleResult.Select(e => e.SigmaX);
            var syy = this.results.TriangleResult.Select(e => e.SigmaY);
            var txy = this.results.TriangleResult.Select(e => e.TauXY);

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

                MaxUx = this.results.NodeResults.Max(e => Math.Abs(e.UX)),
                MaxUy = this.results.NodeResults.Max(e => Math.Abs(e.UY)),

                SxxPercentile005 = sxx.Percentile(percentileMin),
                SxxPercentile095 = sxx.Percentile(percentileMax),

                SyyPercentile005 = syy.Percentile(percentileMin),
                SyyPercentile095 = syy.Percentile(percentileMax),

                TxyPercentile005 = txy.Percentile(percentileMin),
                TxyPercentile095 = txy.Percentile(percentileMax),

            };

            return membraneOutput;
        }

        private void CreateNodeOutput()
        {
            this.nodes = this.results.NodeResults
                        .Select(n => n.ConvertToOutput())
                        .ToList();
        }

        private void CreateTriangleOutput()
        {
            this.triangles = new List<TriangleOutput>();

            foreach (var triangleResult in this.results.TriangleResult)
            {
                var node0Result = this.results.GetNodeResult(triangleResult.Element.Nodes[0]);
                var node1Result = this.results.GetNodeResult(triangleResult.Element.Nodes[1]);
                var node2Result = this.results.GetNodeResult(triangleResult.Element.Nodes[2]);

                var triangleOutput = new TriangleOutput
                {
                    Number = triangleResult.Element.Number,
                    Node0Number = triangleResult.Element.Nodes[0].Number,
                    Node1Number = triangleResult.Element.Nodes[1].Number,
                    Node2Number = triangleResult.Element.Nodes[2].Number,

                    Node0X = triangleResult.Element.Nodes[0].Coordinates.X,
                    Node0Y = triangleResult.Element.Nodes[0].Coordinates.Y,
                    Node1X = triangleResult.Element.Nodes[1].Coordinates.X,
                    Node1Y = triangleResult.Element.Nodes[1].Coordinates.Y,
                    Node2X = triangleResult.Element.Nodes[2].Coordinates.X,
                    Node2Y = triangleResult.Element.Nodes[2].Coordinates.Y,

                    Sxx = triangleResult.SigmaX,
                    Syy = triangleResult.SigmaY,
                    Txy = triangleResult.TauXY,

                    AvgSxx0 = node0Result.AverageSigmaXX,
                    AvgSyy0 = node0Result.AverageSigmaYY,
                    AvgTxy0 = node0Result.AverageTauXY,

                    AvgSxx1 = node1Result.AverageSigmaXX,
                    AvgSyy1 = node1Result.AverageSigmaYY,
                    AvgTxy1 = node1Result.AverageTauXY,

                    AvgSxx2 = node2Result.AverageSigmaXX,
                    AvgSyy2 = node2Result.AverageSigmaYY,
                    AvgTxy2 = node2Result.AverageTauXY
                };

                this.triangles.Add(triangleOutput);
            }
        }
    }
}
