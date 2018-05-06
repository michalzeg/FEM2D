using Common.Geometry;
using FEM2D.Elements.Beam;
using FEM2DCommon.ElementProperties;

namespace FEM2DDynamics.Elements.Beam
{
    public interface IDynamicBeamElement : IDynamicElement, IBeamElement
    {
        DynamicBeamProperties DynamicBeamProperties { get; }

        bool IsBetweenEnds(PointD point);
    }
}