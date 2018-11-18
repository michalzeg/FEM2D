using FEM2DCommon.DTO;
using FEM2DCommon.Sections;

namespace FEM2DCommon.ElementProperties.Builder
{
    public class BeamPropertiesBuilder : IBeamPropertiesBuilderFinish, IBeamPropertiesBuilderSetMaterial, IBeamPropertiesBuilderSetSection
    {
        private BeamProperties beamProperties;

        internal BeamPropertiesBuilder()
        {
            this.beamProperties = new BeamProperties();
        }

        public static IBeamPropertiesBuilderSetMaterial Create()
        {
            var builder = new BeamPropertiesBuilder();
            return builder;
        }

        public BeamProperties Build()
        {
            return this.beamProperties;
        }

        public IBeamPropertiesBuilderSetSection SetSteel()
        {
            this.beamProperties.ModulusOfElasticity = 200000000;
            return this;
        }

        public IBeamPropertiesBuilderSetSection SetModulusOfElasticity(double modulus)
        {
            this.beamProperties.ModulusOfElasticity = modulus;
            return this;
        }

        public IBeamPropertiesBuilderFinish SetSection(Section section)
        {
            this.beamProperties.Section = section;
            return this;
        }

        public IBeamPropertiesBuilderFinish SetRectangularSection(double width, double height)
        {
            this.beamProperties.Section = Section.FromRectangle(width, height);
            return this;
        }
    }
}