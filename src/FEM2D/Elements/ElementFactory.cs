using FEM2D.Elements.Beam;
using FEM2D.Elements.Triangle;
using FEM2D.Elements.Truss;
using FEM2D.Nodes;
using FEM2D.Nodes.Dofs;
using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using System.Collections.Generic;
using System.Linq;

namespace FEM2D.Elements
{
    public class ElementFactory : IDofCountProvider
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
            node1.SetRotationDofs();
            node2.SetRotationDofs();
            return element;
        }

        public ITrussElement CreateTruss(Node node1, Node node2, BeamProperties beamProperties)
        {
            var element = new TrussElement(node1, node2, beamProperties, this.freeNumber);
            this.elements.Add(element);
            this.freeNumber++;
            node1.SetTranslationDofs();
            node2.SetTranslationDofs();
            return element;
        }

        public ITriangleElement CreateTriangle(Node node1, Node node2, Node node3, MembraneProperties membraneProperties)
        {
            var element = new TriangleElement(node1, node2, node3, membraneProperties, this.freeNumber);
            this.elements.Add(element);
            freeNumber++;

            node1.SetTranslationDofs();
            node2.SetTranslationDofs();
            node3.SetTranslationDofs();

            return element;
        }

        public IEnumerable<IElement> GetAll() => this.elements;

        public IEnumerable<IBeamElement> GetBeamElements() => this.elements.OfType<IBeamElement>();

        public IEnumerable<ITriangleElement> GetMembraneElements() => this.elements.OfType<ITriangleElement>();

        public IEnumerable<ITrussElement> GetTrussElements() => this.elements.OfType<ITrussElement>();

        public int GetDOFsCount()
        {
            var dofCount = this.elements.Select(e => e.Nodes)
                                 .SelectMany(e => e)
                                 .Select(e => e.GetDOF())
                                 .SelectMany(e => e)
                                 .Distinct()
                                 .Count();
            return dofCount;
        }
    }
}