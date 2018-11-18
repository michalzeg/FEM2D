using FEM2DCommon.ElementProperties;

namespace FEM2DCommon.ElementProperties.Builder
{
    public interface IDynamicBeamPropertiesBuilderFinish
    {
        DynamicBeamProperties Build();
    }
}