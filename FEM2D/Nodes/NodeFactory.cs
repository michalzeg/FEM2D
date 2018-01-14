using Common.Geometry;
using FEM2D.Restraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Nodes
{
    public class NodeFactory
    {
        private readonly IDictionary<PointD, Node> coordinatesNodeMap;
        private readonly IDofNumberCalculator dofCalculator;

        private int freeNumber = 1;

        public NodeFactory()
        {
            this.dofCalculator = new DofNumberCalculator();
            this.coordinatesNodeMap = new Dictionary<PointD, Node>();
        }

        public Node Create(PointD coordinates, Restraint restraint = Restraint.Free)
        {

            if (this.coordinatesNodeMap.ContainsKey(coordinates))
            {
                return this.coordinatesNodeMap[coordinates];
            }
            var node = new Node(coordinates, this.freeNumber,this.dofCalculator, restraint);
            this.coordinatesNodeMap.Add(coordinates, node);
            this.freeNumber++;
            return node;

        }
        public Node Create(double x, double y, Restraint restraint = Restraint.Free)
        {
            return this.Create(new PointD(x, y), restraint);
        }

        public IEnumerable<Node> GetAll()
        {
            return this.coordinatesNodeMap.Select(n => n.Value).ToList();
        }

        public int GetNodeCount()
        {
            return this.coordinatesNodeMap.Count;
        }

        public int GetDOFsCount()
        {
            return this.coordinatesNodeMap.Select(e => e.Value)
                .Select(e => e.GetDOF())
                .SelectMany(e => e)
                .Distinct()
                .Count();
        }
    }
}
