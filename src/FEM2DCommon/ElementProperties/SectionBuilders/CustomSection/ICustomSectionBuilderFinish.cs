using FEM2DCommon.Sections;

namespace FEM2DCommon.ElementProperties.SectionBuilders.CustomSection
{
    public interface ICustomSectionBuilderFinish
    {
        Section Build();
    }
}