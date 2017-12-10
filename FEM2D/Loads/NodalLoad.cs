using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Loads
{
    public class NodalLoad : INodalLoad
    {
        public NodalLoad[] NodalLoads => new[] { this };

        public Node Node { get; }
        public double ValueX { get; }
        public double ValueY { get; }
        public double ValueM { get; }

        public NodalLoad(Node node, double valueX, double valueY, double valueM = 0)
        {
            this.Node = node;
            this.ValueX = valueX;
            this.ValueY = valueY;
            this.ValueM = valueM;
        }

    }
}
