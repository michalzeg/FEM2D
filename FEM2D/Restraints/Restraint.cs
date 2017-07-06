using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Restraints
{
    public class Support
    {
        public Node Node { get; set; }
        public Restraint Restraint { get; set; }
    }

    [Flags]
    public enum Restraint
    {
        Free =0,
        FixedX = 2,
        FixedY = 4,
        Fixed = FixedX | FixedY,
    }
}
