﻿using FEM2D.Elements;
using FEM2D.Nodes;
using FEM2D.Nodes.Dofs;
using FEM2DCommon.ElementProperties;
using FEM2DDynamics.Elements.Beam;
using FEM2DDynamics.Solver;
using System.Collections.Generic;
using System.Linq;

namespace FEM2DDynamics.Elements
{
    public class DynamicElementFactory : IDofCountProvider
    {
        private readonly ElementFactory elementFactory;
        private readonly IList<IDynamicElement> elements;

        internal DynamicElementFactory()
        {
            this.elementFactory = new ElementFactory();
            this.elements = new List<IDynamicElement>();
        }

        public IDynamicBeamElement CreateBeam(INode node1, INode node2, DynamicBeamProperties dynamicBeamProperties)
        {
            var element = this.elementFactory.CreateBeam(node1, node2, dynamicBeamProperties.BeamProperties);
            var dynamicElement = new DynamicBeamElement(element, dynamicBeamProperties);

            this.elements.Add(dynamicElement);

            return dynamicElement;
        }

        public IEnumerable<IDynamicElement> GetAll() => this.elements;

        public IEnumerable<IDynamicBeamElement> GetBeamElements() => this.elements.OfType<IDynamicBeamElement>();

        public void UpdateDampingFactor(IDampingFactorCalculator dampingFactors)
        {
            foreach (var element in this.elements)
            {
                element.UpdateDampingFactors(dampingFactors);
            }
        }

        public int GetDOFsCount() => this.elementFactory.GetDOFsCount();
    }
}