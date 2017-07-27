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
    public class ResultProvider
    {
        private Dictionary<int, double> globalDisplacements;
        private Dictionary<Node, ITriangleElement[]> nodeElementsMap;
        private Dictionary<ITriangleElement, TriangleElementResult> triangleResultMap;

        private readonly IEnumerable<Node> nodes;
        private readonly IEnumerable<ITriangleElement> elements;

        public IList<NodeResult> NodeResults { get; private set; }
        public IList<TriangleElementResult> TriangleResult { get; private set; }

        public ResultProvider(Vector<double> displacements, IEnumerable<Node> nodes, IEnumerable<ITriangleElement> elements)
        {

            this.nodes = nodes;
            this.elements = elements;

            CreateGlobalDisplacements(displacements);

            CreateNodeElementMap();

            GetTriangleElementsResults();
            GetNodesResults();
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

        private NodeResult GetNodeResult(Node node)
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

            var result = new NodeResult
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

        private void GetNodesResults()
        {
            this.NodeResults = new List<NodeResult>();
            foreach (var node in this.nodes)
            {
                var nodeResult = this.GetNodeResult(node);
                this.NodeResults.Add(nodeResult);
            }
        }


        private TriangleElementResult GetTriangleElementResult(ITriangleElement element)
        {
            var dofs = element.GetDOFs();

            var displacements = this.globalDisplacements.Keys
                .Intersect(dofs)
                .Select(e => this.globalDisplacements[e])
                .ToArray();
            displacements = dofs.Select(e => this.globalDisplacements[e]).ToArray();


            var displacementVector = Vector.Build.DenseOfArray(displacements);

            var sigma = element.GetD() * element.GetB() * displacementVector;

            var result = new TriangleElementResult
            {
                Element = element,
                SigmaX = sigma[0],
                SigmaY = sigma[1],
                TauXY = sigma[2]
            };
            return result;
        }

        private void GetTriangleElementsResults()
        {
            this.TriangleResult = new List<TriangleElementResult>();
            this.triangleResultMap = new Dictionary<ITriangleElement, TriangleElementResult>();
            foreach (var element in this.elements)
            {
                var elementResult = this.GetTriangleElementResult(element);
                this.TriangleResult.Add(elementResult);
                this.triangleResultMap.Add(element, elementResult);
            }

        }
    }
}
