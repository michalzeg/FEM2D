using FEM2D.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace FEM2D.Results.Nodes
{
    public class NodeResults
    {
        private IDictionary<INode, NodeResult> nodeResultMap;
        private readonly IEnumerable<INode> nodes;
        private readonly DofDisplacementMap dofDisplacementMap;
        private IEnumerable<NodeResult> nodeResults;

        internal NodeResults(DofDisplacementMap dofDisplacementMap, IEnumerable<INode> nodes)
        {
            this.nodes = nodes;
            this.dofDisplacementMap = dofDisplacementMap;
            CalculateNodesResults();
            CreateNodeResultMap();
        }

        public NodeResult GetNodeResult(INode node)
        {
            var result = this.nodeResultMap[node];
            return result;
        }

        public IEnumerable<NodeResult> GetNodeResult(IEnumerable<INode> nodes)
        {
            var results = this.nodeResultMap.Keys
                .Intersect(nodes)
                .Select(e => this.nodeResultMap[e]);
            return results;
        }

        public IEnumerable<NodeResult> GetNodeResult()
        {
            return this.GetNodeResult(this.nodes);
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

        private NodeResult CalcualteNodeResult(INode node)
        {
            var dofs = node.GetDOF();

            var dofX = dofs[0];
            var dofY = dofs[1];
            var dofR = node.TryGetRotationDOF();

            var uX = this.dofDisplacementMap.GetValue(dofX);
            var uY = this.dofDisplacementMap.GetValue(dofY);
            var rZ = this.dofDisplacementMap.GetValue(dofR);

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