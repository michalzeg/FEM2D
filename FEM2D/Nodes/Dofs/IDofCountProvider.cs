using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Nodes.Dofs
{
    public interface IDofCountProvider
    {
        int GetDOFsCount();
    }
}
