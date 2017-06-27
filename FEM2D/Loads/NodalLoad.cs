using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Loads
{
    public class NodalLoad
    {
        public Node Node { get; set; }
        public double Value { get; set; }


    }
}
