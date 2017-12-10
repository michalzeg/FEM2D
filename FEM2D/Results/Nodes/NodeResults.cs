﻿using FEM2D.Nodes;
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

        private IDictionary<Node, NodeResult> nodeResultMap;
        private readonly IEnumerable<Node> nodes;
        private readonly DofDisplacementMap dofDisplacementMap;
        private IEnumerable<NodeResult> nodeResults;

        internal NodeResults(DofDisplacementMap dofDisplacementMap, IEnumerable<Node> nodes)
        {
            this.nodes = nodes;
            this.dofDisplacementMap = dofDisplacementMap;
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
        private NodeResult CalcualteNodeResult(Node node)
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
