using FEM2DCommon.ElementProperties;
using FEM2DCommon.Sections;

namespace FEMCommon.ElementProperties.DynamicBeamPropertiesBuilder
{
    public class DynamicBeamPropertiesBuilder : IDynamicBeamPropertiesBuilderFinish, IDynamicBeamPropertiesBuilderSetSection, IDynamicBeamPropertiesBuilderSetModulus, IDynamicBeamPropertiesBuilderSetDensity, IDynamicBeamPropertiesBuilderSetMaterial
    {
        private BeamPropertiesBuilder beamPropertiesBuilder;

        private DynamicBeamProperties dynamicBeamProperties;

        internal DynamicBeamPropertiesBuilder()
        {
            this.beamPropertiesBuilder = new BeamPropertiesBuilder();
            this.dynamicBeamProperties = new DynamicBeamProperties();
        }

        public static IDynamicBeamPropertiesBuilderSetMaterial Create()
        {
            return new DynamicBeamPropertiesBuilder();
        }

        public IDynamicBeamPropertiesBuilderSetSection SetSteel()
        {
            this.beamPropertiesBuilder.SetSteel();
            this.dynamicBeamProperties.Density = 7850;
            return this;
        }

        public IDynamicBeamPropertiesBuilderSetDensity SetCustomMaterial()
        {
            return this;
        }

        public IDynamicBeamPropertiesBuilderSetModulus SetDensity(double density)
        {
            this.dynamicBeamProperties.Density = density;
            return this;
        }

        public IDynamicBeamPropertiesBuilderSetSection SetModulusOfElasticity(double modulus)
        {
            this.beamPropertiesBuilder.SetModulusOfElasticity(modulus);
            return this;
        }

        public IDynamicBeamPropertiesBuilderFinish SetSection(Section section)
        {
            this.beamPropertiesBuilder.SetSection(section);
            return this;
        }

        public IDynamicBeamPropertiesBuilderFinish SetRectangularSection(double width, double height)
        {
            this.beamPropertiesBuilder.SetRectangularSection(width, height);
            return this;
        }

        public DynamicBeamProperties Build()
        {
            this.dynamicBeamProperties.BeamProperties = this.beamPropertiesBuilder.Build();
            return this.dynamicBeamProperties;
        }
    }
}