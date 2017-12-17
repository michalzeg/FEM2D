using Common.DTO;
using FEM2D.Elements;
using FEM2D.Nodes;
using FEM2DDynamics.Elements.Beam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Elements
{
    public class DynamicElementFactory
    {
        private readonly ElementFactory elementFactory;
        private readonly IList<IDynamicElement> elements;

        internal DynamicElementFactory()
        {
            this.elementFactory = new ElementFactory();
            this.elements = new List<IDynamicElement>();
        }

        public IDynamicBeamElement CreateBeam(Node node1, Node node2, BeamProperties beamProperties)
        {
            var element = this.elementFactory.CreateBeam(node1, node2, beamProperties);
            var dynamicElement = new DynamicBeamElement(element);

            this.elements.Add(dynamicElement);
            
            return dynamicElement;
        }


        public IEnumerable<IDynamicElement> GetAll()
        {
            return this.elements;
        }

        public IEnumerable<IDynamicBeamElement> GetBeamElements()
        {
            var result = this.elements.OfType<IDynamicBeamElement>();
            return result;
        }

    }
}
