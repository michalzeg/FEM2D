using FEM2D.Elements;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results.Membranes
{
    public class TriangleElementResults
    {
        private readonly DofDisplacementMap dofDisplacementMap;
        private readonly IEnumerable<ITriangleElement> elements;

        private Dictionary<Node, ITriangleElement[]> nodeElementsMap;
        private Dictionary<ITriangleElement, TriangleResult> triangleResultMap;

        internal TriangleElementResults(DofDisplacementMap dofDisplacementMap, IEnumerable<ITriangleElement> elements)
        {
            this.dofDisplacementMap = dofDisplacementMap;
            this.elements = elements;

            CreateNodeTriangleMap();
            CalculateTriangleElementsResults();
        }

        private void CalculateTriangleElementsResults()
        {
            this.triangleResultMap = this.elements.ToDictionary(e => e,f=> this.CalculateTriangleElementResult(f));
        }
        private TriangleResult CalculateTriangleElementResult(ITriangleElement element)
        {
            var dofs = element.GetDOFs();

            var displacements = this.dofDisplacementMap.GetValue(dofs).ToArray();

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
