using Common.ElementProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Elements.Beam
{
    public interface IDynamicBeamElement : IDynamicElement
    {
        DynamicBeamProperties DynamicBeamProperties { get; }
    }
}
