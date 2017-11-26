using Common.Point;
using FEM2D.Restraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Nodes
{
    public class NodeCollection
    {
        private readonly IDictionary<PointD, Node> coordinatesNodeMap;

        private int freeNumber = 1;

        public NodeCollection()
        {
            this.coordinatesNodeMap = new Dictionary<PointD, Node>();
        }

        public Node Create(PointD coordinates, Restraint restraint = Restraint.Free)
        {

            if (this.coordinatesNodeMap.ContainsKey(coordinates))
            {
                return this.coordinatesNodeMap[coordinates];
            }
            var node = new Node(coordinates, this.freeNumber, restraint);
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
    }
}
