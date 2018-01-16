using FEM2DCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DCommon.ElementProperties
{
    public class DynamicBeamProperties
    {
        public BeamProperties BeamProperties { get; set; }
        public double Density { get; set; }

        public static DynamicBeamProperties Default =>
            new DynamicBeamProperties
            {
                BeamProperties = BeamProperties.Default,
                Density = 20,
            };
        
    }
}
