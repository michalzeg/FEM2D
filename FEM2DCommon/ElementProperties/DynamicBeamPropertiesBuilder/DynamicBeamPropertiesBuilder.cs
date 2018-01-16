using FEM2DCommon.ElementProperties;
using FEM2DCommon.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEMCommon.ElementProperties.DynamicBeamPropertiesBuilder
{
    public class DynamicBeamPropertiesBuilder : IDynamicBeamPropertiesBuilderFinish, IDynamicBeamPropertiesBuilderSetSection, IDynamicBeamPropertiesBuilderSetModulus, IDynamicBeamPropertiesBuilderSetDensity, IDynamicBeamPropertiesBuilderSetMaterial, IDynamicBeamPropertiesBuilderSetDamping
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
            this.dynamicBeamProperties.Damping = 0.03;
            return this;
        }

       public IDynamicBeamPropertiesBuilderSetDensity SetCustomMaterial()
        {
            return this;
        }

        public IDynamicBeamPropertiesBuilderSetDamping SetDensity(double density)
        {
            this.dynamicBeamProperties.Density = density;
            return this;
        }

        public IDynamicBeamPropertiesBuilderSetModulus SetDamping(double damping)
        {
            this.dynamicBeamProperties.Damping = damping;
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
