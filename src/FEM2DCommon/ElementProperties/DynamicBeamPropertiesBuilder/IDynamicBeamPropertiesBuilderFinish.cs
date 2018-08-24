using FEM2DCommon.ElementProperties;

namespace FEMCommon.ElementProperties.DynamicBeamPropertiesBuilder
{
    public interface IDynamicBeamPropertiesBuilderFinish
    {
        DynamicBeamProperties Build();
    }
}