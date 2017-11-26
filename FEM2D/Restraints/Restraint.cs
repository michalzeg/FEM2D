using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Restraints
{
    
    [Flags]
    public enum Restraint
    {
        Free =0,
        FixedX = 2,
        FixedY = 4,
        Fixed = FixedX | FixedY,
    }
}
