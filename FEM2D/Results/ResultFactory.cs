using FEM2D.Elements;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results
{
    public class ResultFactory
    {
        private Dictionary<int, double> globalDisplacements;
        private Dictionary<Node, ITriangleElement[]> nodeElementsMap;
        private Dictionary<ITriangleElement, TriangleResult> triangleResultMap;
        private Dictionary<Node, MembraneNodeResult> nodeResultMap;

        private readonly IEnumerable<Node> nodes;
        private readonly IEnumerable<ITriangleElement> elements;

        public IList<MembraneNodeResult> MembraneNodeResults { get; private set; }
        public IList<TriangleResult> TriangleResult { get; private set; }

        public ResultFactory(Vector<double> displacements, IEnumerable<Node> nodes, IEnumerable<IElement> elements)
        {

            this.nodes = nodes;
            this.elements = elements.Cast<ITriangleElement>();

            CreateGlobalDisplacements(displacements);

            CreateNodeElementMap();

            CalculateTriangleElementsResults();
            CalculateNodesResults();
            CreateNodeResultMap();
        }
        private void CreateNodeResultMap()
        {
            this.nodeResultMap = this.MembraneNodeResults.ToDictionary(e => e.Node, f => f);
        }

        private void CreateGlobalDisplacements(Vector<double> displacements)
        {
            this.globalDisplacements = displacements
                .Select((e, i) => new { index = i, value = e })
                .ToDictionary(i => i.index, v => v.value);
        }

        private void CreateNodeElementMap()
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

        private MembraneNodeResult CalcualteNodeResult(Node node)
        {
            var dofs = node.GetDOF();

            var dofX = dofs[0];
            var dofY = dofs[1];

            var uX = this.globalDisplacements[dofX];
            var uY = this.globalDisplacements[dofY];

            var nodeElements = this.nodeElementsMap[node];
            var nodeElementsResults = this.triangleResultMap.Keys.Intersect(nodeElements)
                                          .Select(e=>this.triangleResultMap[e]);

            var averageSxx = nodeElementsResults.Average(e => e.SigmaX);
            var averageSyy = nodeElementsResults.Average(e => e.SigmaY);
            var averageTxy = nodeElementsResults.Average(e => e.TauXY);

            var result = new MembraneNodeResult
            {
                Node = node,
                UX = uX,
                UY = uY,
                AverageSigmaXX = averageSxx,
                AverageSigmaYY = averageSyy,
                AverageTauXY = averageTxy,
            };

            return result;
        }

        private void CalculateNodesResults()
        {
            this.MembraneNodeResults = new List<MembraneNodeResult>();
            foreach (var node in this.nodes)
            {
                var nodeResult = this.CalcualteNodeResult(node);
                this.MembraneNodeResults.Add(nodeResult);
            }
        }

        private TriangleResult CalculateTriangleElementResult(ITriangleElement element)
        {
            var dofs = element.GetDOFs();

            var displacements = this.globalDisplacements.Keys
                .Intersect(dofs)
                .Select(e => this.globalDisplacements[e])
                .ToArray();
            displacements = dofs.Select(e => this.globalDisplacements[e]).ToArray();


            var displacementVector = Vector.Build.DenseOfArray(displacements);

            var sigma = element.GetD() * element.GetB() * displacementVector;

            var result = new TriangleResult
            {
                Element = element,
                SigmaX = sigma[0],
                SigmaY = sigma[1],
                TauXY = sigma[2]
            };
            return result;
        }

        private void CalculateTriangleElementsResults()
        {
            this.TriangleResult = new List<TriangleResult>();
            this.triangleResultMap = new Dictionary<ITriangleElement, TriangleResult>();
            foreach (var element in this.elements)
            {
                var elementResult = this.CalculateTriangleElementResult(element);
                this.TriangleResult.Add(elementResult);
                this.triangleResultMap.Add(element, elementResult);
            }

        }

        public MembraneNodeResult GetNodeResult(Node node)
        {
            var result = this.nodeResultMap[node];
            return result;
        }

        public IEnumerable<MembraneNodeResult> GetNodeResult(IEnumerable<Node> nodes)
        {
            var results = new List<MembraneNodeResult>();
            foreach (var node in nodes)
            {
                var result = this.nodeResultMap[node];
                results.Add(result);
            }
            return results;
        }
        
    }
}
