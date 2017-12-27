﻿using Common.Point;
using FEM2D.Elements.Beam;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2DDynamics.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Loads
{
    public class DynamicLoadFactory
    {

        private readonly IList<IMovingPointLoad> movingLoads = new List<IMovingPointLoad>();
        private readonly DynamicElementFactory elementFactory;
        private readonly NodeFactory nodeFactory;
        

        public DynamicLoadFactory(DynamicElementFactory elementFactory, NodeFactory nodeFactory)
        {
            this.elementFactory = elementFactory;
            this.nodeFactory = nodeFactory;
        }

        public void AddPointMovingLoad( double value,double basePosition)
        {
            var load = new PointMovingLoad(value, basePosition);
            this.movingLoads.Add(load);
        }

        public IEnumerable<NodalLoad> GetNodalLoads(double time)
        {

            var result = new List<NodalLoad>();
            var nodes = this.nodeFactory.GetAll();
            var beams = this.elementFactory.GetBeamElements();

            foreach (var movingLoad in this.movingLoads)
            {
                var position = movingLoad.GetPosition(time);
                var value = movingLoad.GetValueY(time);
                var nodalLoads = GenerateNodeLoads(nodes, position, value);
                var beamNodalLoads = GenerateBeamNodalLoads(beams, position, value);

                result.AddRange(nodalLoads);
                result.AddRange(beamNodalLoads);
            }

            return result;
        }

        private static IEnumerable<NodalLoad> GenerateBeamNodalLoads(IEnumerable<Elements.Beam.IDynamicBeamElement> beams, PointD position, double value)
        {
            var loadedBeams = beams.Where(e => e.IsBetweenEnds(position));
            var beamNodalLoads = loadedBeams.Select(b => BeamPointLoad.FromGlobalPosition(b, value, position.X))
                                            .Select(e => e.NodalLoads)
                                            .SelectMany(e => e);
            return beamNodalLoads;
        }

        private static IEnumerable<NodalLoad> GenerateNodeLoads(IEnumerable<Node> nodes, PointD position, double value)
        {
            var loadedNodes = nodes.Where(e => e.Coordinates == position);
            var nodalLoads = loadedNodes.Select(n => new NodalLoad(n, 0, value));
            return nodalLoads;
        }

    }
}
