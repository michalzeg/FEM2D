using FEM2DCommon.DTO;

namespace FEM2DCommon.ElementProperties
{
    public class DynamicBeamProperties
    {
        public BarProperties BeamProperties { get; set; } = new BarProperties();
        public double Density { get; set; }

        public static DynamicBeamProperties Default =>
            new DynamicBeamProperties
            {
                BeamProperties = BarProperties.Default,
                Density = 20,
            };
    }
}