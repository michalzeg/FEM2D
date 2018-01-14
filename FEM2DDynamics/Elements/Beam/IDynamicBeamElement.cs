using FEM2DCommon.ElementProperties;
using Common.Point;
using FEM2D.Elements.Beam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Elements.Beam
{
    public interface IDynamicBeamElement : IDynamicElement, IBeamElement
    {
        DynamicBeamProperties DynamicBeamProperties { get; }
        bool IsBetweenEnds(PointD point);
    }
}
