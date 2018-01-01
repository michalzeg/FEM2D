using Common.DTO;
using Common.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ElementProperties
{
    public class BeamPropertiesBuilder : IBeamPropertiesBuilderFinish, IBeamPropertiesBuilderSetMaterial, IBeamPropertiesBuilderSetSection
    {
        private BeamProperties beamProperties;

        private BeamPropertiesBuilder()
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
            this.beamProperties.ModulusOfElasticity=200000000;
            return this;
        }
        public IBeamPropertiesBuilderSetSection SetModulusOfElasticity(double modulus)
        {
            this.beamProperties.ModulusOfElasticity = modulus;
            return this;
        }

        public IBeamPropertiesBuilderFinish SetRectangularSection(double width, double height)
        {
            this.beamProperties.Section = Section.FromRectangle(width, height);
            return this;
        }
       

    }
}
