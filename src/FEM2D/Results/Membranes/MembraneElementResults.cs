using FEM2D.Elements;
using FEM2D.Elements.Triangle;
using FEM2D.Nodes;
using FEM2D.Results.Nodes;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;
using System.Linq;

namespace FEM2D.Results.Membranes
{
    public class MembraneElementResults
    {
        private readonly DofDisplacementMap dofDisplacementMap;
        private readonly NodeResults nodeResults;
        private readonly IEnumerable<ITriangleElement> elements;

        private Dictionary<Node, ITriangleElement[]> nodeElementsMap;
        private Dictionary<ITriangleElement, MembraneElementResult> triangleResultMap;

        internal MembraneElementResults(DofDisplacementMap dofDisplacementMap, NodeResults nodeResults, IEnumerable<ITriangleElement> elements)
        {
            this.dofDisplacementMap = dofDisplacementMap;
            this.nodeResults = nodeResults;
            this.elements = elements;

            CreateNodeTriangleMap();
            CalculateTriangleElementsResults();
        }

        public MembraneElementResult GetElementResult(ITriangleElement triangle)
        {
            var result = this.triangleResultMap[triangle];
            return result;
        }

        public IEnumerable<MembraneElementResult> GetElementResult(IEnumerable<ITriangleElement> nodes)
        {
            var results = this.triangleResultMap.Keys
                .Intersect(nodes)
                .Select(e => this.triangleResultMap[e]);
            return results;
        }

        public IEnumerable<MembraneElementResult> GetElementResult()
        {
            var results = this.triangleResultMap.Values;
            return results;
        }

        public MembraneNodeResult GetNodeResult(Node node)
        {
            var elements = this.nodeElementsMap[node].Select(e => this.triangleResultMap[e]);

            var averageSxx = elements.Average(e => e.SigmaX);
            var averageSyy = elements.Average(e => e.SigmaY);
            var averageTxy = elements.Average(e => e.TauXY);

            var nodeResult = this.nodeResults.GetNodeResult(node);

            var result = new MembraneNodeResult
            {
                Node = node,
                UX = nodeResult.UX,
                UY = nodeResult.UY,
                AverageSigmaXX = averageSxx,
                AverageSigmaYY = averageSyy,
                AverageTauXY = averageTxy,
            };
            return result;
        }

        public IEnumerable<MembraneNodeResult> GetNodeResult(IEnumerable<Node> nodes)
        {
            var results = nodes.Select(n => this.GetNodeResult(n)).ToList();
            return results;
        }

        public IEnumerable<MembraneNodeResult> GetNodeResult()
        {
            var nodes = this.elements.Select(e => e.Nodes).SelectMany(e => e);

            var result = this.GetNodeResult(nodes);
            return result;
        }

        private void CalculateTriangleElementsResults()
        {
            this.triangleResultMap = this.elements.ToDictionary(e => e, f => this.CalculateTriangleElementResult(f));
        }

        private MembraneElementResult CalculateTriangleElementResult(ITriangleElement element)
        {
            var dofs = element.GetDOFs();

            var displacements = this.dofDisplacementMap.GetValue(dofs).ToArray();

            var displacementVector = Vector.Build.DenseOfArray(displacements);

            var sigma = element.GetD() * element.GetB() * displacementVector;

            var result = new MembraneElementResult
            {
                Element = element,
                SigmaX = sigma[0],
                SigmaY = sigma[1],
                TauXY = sigma[2]
            };
            return result;
        }

        private void CreateNodeTriangleMap()
        {
            var elementNodeMap = this.elements.ToDictionary(e => e, f => f.Nodes);
            var elementNodeList = new List<KeyValuePair<ITriangleElement, Node>>();

            foreach (var item in elementNodeMap)
            {
                var key = item.Key;
                foreach (var node in item.Value)
                {
                    elementNodeList.Add(new KeyValuePair<ITriangleElement, Node>(key, node));
                }
            }
            this.nodeElementsMap = elementNodeList.ToLookup(e => e.Value, f => f.Key)
                .ToDictionary(e => e.Key, f => f.ToArray());
        }
    }
}