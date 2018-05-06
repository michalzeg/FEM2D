using FEM2DCommon.DTO;

namespace FEM2D.Elements.Beam
{
    public interface IBeamElement : IElement
    {
        BeamProperties BeamProperties { get; }
        double Length { get; }
    }
}