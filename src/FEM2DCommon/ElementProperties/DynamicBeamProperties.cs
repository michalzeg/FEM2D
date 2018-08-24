using FEM2DCommon.DTO;

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