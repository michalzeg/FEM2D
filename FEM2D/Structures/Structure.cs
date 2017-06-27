using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Structures
{
    public class Structure
    {
        IEnumerable<Node> nodes;
        IEnumerable<ITriangleElement> elements;
        IEnumerable<NodalLoad> nodalLoads;


        public Structure()
        {

        }

        public void Solve()
        {

        }
    }
}
