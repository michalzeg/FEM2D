using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results.Nodes
{
    public class NodeResults
    {
        private IDictionary<int, double> dofDisplacementMap;
        private IDictionary<Node, NodeResult> nodeResultMap;
        private readonly IEnumerable<Node> nodes;
        private IEnumerable<NodeResult> nodeResults;

        public NodeResults(Vector<double> displacements, IEnumerable<Node> nodes)
        {
            this.nodes = nodes;

            CreateDofDisplacementMap(displacements);
            CalculateNodesResults();
            CreateNodeResultMap();
        }

        public NodeResult GetNodeResult(Node node)
        {
            var result = this.nodeResultMap[node];
            return result;
        }

        public IEnumerable<NodeResult> GetNodeResult(IEnumerable<Node> nodes)
        {
            var results = this.nodeResultMap.Keys
                .Intersect(nodes)
                .Select(e => this.nodeResultMap[e]);
            return results;
        }

        private void CreateDofDisplacementMap(Vector<double> displacements)
        {
            this.dofDisplacementMap = displacements
                .Select((e, i) => new { index = i, value = e })
                .ToDictionary(i => i.index, v => v.value);
        }

        private void CreateNodeResultMap()
        {
            this.nodeResultMap = this.nodeResults.ToDictionary(e => e.Node, f => f);
        }

        private void CalculateNodesResults()
        {
            this.nodeResults = this.nodes
                               .Select(n => this.CalcualteNodeResult(n));
        }
        private NodeResult CalcualteNodeResult(Node node)
        {
            var dofs = node.GetDOF();

            var dofX = dofs[0];
            var dofY = dofs[1];
            var dofR = dofs.Length == 3 ? dofs[2] : -1;

            var uX = this.dofDisplacementMap[dofX];
            var uY = this.dofDisplacementMap[dofY];
            var rZ = dofR == -1 ? 0 : this.dofDisplacementMap[dofR];

            var result = new NodeResult
            {
                Node = node,
                UX = uX,
                UY = uY,
                Rz = rZ
            };

            return result;
        }

    }
}
