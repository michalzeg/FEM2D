using FEM2DCommon.DTO;
using FEM2D.Elements.Beam;
using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Elements
{
    public class ElementFactory
    {
        private readonly IList<IElement> elements;
        private int freeNumber = 1;

        public ElementFactory()
        {
            this.elements = new List<IElement>();
        }

        public IBeamElement CreateBeam(Node node1, Node node2, BeamProperties beamProperties)
        {
            var element = new BeamElement(node1, node2, beamProperties, this.freeNumber);
            this.elements.Add(element);
            this.freeNumber++;
            node1.SetBeamDofs();
            node2.SetBeamDofs();
            return element;
        }

        public ITriangleElement CreateTriangle(Node node1, Node node2, Node node3, MembraneProperties membraneProperties)
        {
            var element = new TriangleElement(node1, node2, node3, membraneProperties, this.freeNumber);
            this.elements.Add(element);
            freeNumber++;

            node1.SetMembraneDofs();
            node2.SetMembraneDofs();
            node3.SetMembraneDofs();

            return element;
        }

        public IEnumerable<IElement> GetAll()
        {
            return this.elements;
        }

        public IEnumerable<IBeamElement> GetBeamElements()
        {
            var result = this.elements.OfType<IBeamElement>();
            return result;
        }

        public IEnumerable<ITriangleElement> GetMembraneElements()
        {
            var result = this.elements.OfType<ITriangleElement>();
            return result;
        }
    }
}
