using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ElementProperties
{
    public class BeamPropertiesBuilder
    {
        private BeamProperties beamProperties;

        public BeamPropertiesBuilder()
        {
            this.beamProperties = new BeamProperties();
        }

        public BeamProperties Build()
        {
            return this.beamProperties;
        }

        public BeamPropertiesBuilder SetSteel()
        {
            this.beamProperties.ModulusOfElasticity=200000000;
            return this;
        }
        public BeamPropertiesBuilder SetModulusOfElasticity(double modulus)
        {
            this.beamProperties.ModulusOfElasticity = modulus;
            return this;
        }


       

    }
}
