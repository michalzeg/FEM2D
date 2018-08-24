using FEM2DCommon.Sections;

namespace FEMCommon.ElementProperties.SectionBuilders.CustomSection
{
    public interface ICustomSectionBuilderFinish
    {
        Section Build();
    }
}