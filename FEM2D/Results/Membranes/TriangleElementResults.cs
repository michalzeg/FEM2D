using FEM2D.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results.Membranes
{
    public class TriangleElementResults
    {
        internal TriangleElementResults()
        {

        }

        private TriangleResult CalculateTriangleElementResult(ITriangleElement element)
        {
            //var dofs = element.GetDOFs();

            //var displacements = this.globalDisplacements.Keys
            //    .Intersect(dofs)
            //    .Select(e => this.globalDisplacements[e])
            //    .ToArray();
            //displacements = dofs.Select(e => this.globalDisplacements[e]).ToArray();


            //var displacementVector = Vector.Build.DenseOfArray(displacements);

            //var sigma = element.GetD() * element.GetB() * displacementVector;

            //var result = new TriangleResult
            //{
            //    Element = element,
            //    SigmaX = sigma[0],
            //    SigmaY = sigma[1],
            //    TauXY = sigma[2]
            //};
            //return result;
            return null;
        }

        private void CreateNodeElementMap()
        {
            //var elementNodeMap = this.elements.ToDictionary(e => e, f => f.Nodes);
            //var elementNodeList = new List<KeyValuePair<ITriangleElement, Node>>();

            //foreach (var item in elementNodeMap)
            //{
            //    var key = item.Key;
            //    foreach (var node in item.Value)
            //    {
            //        elementNodeList.Add(new KeyValuePair<ITriangleElement, Node>(key, node));
            //    }
            //}
            //this.nodeElementsMap = elementNodeList.ToLookup(e => e.Value, f => f.Key)
            //    .ToDictionary(e => e.Key, f => f.ToArray());
        }
    }
}
