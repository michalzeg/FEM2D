namespace FEM2DCommon.ElementProperties
{
    public class MembraneProperties
    {
        public double ModulusOfElasticity { get; set; }
        public double PoissonsRation { get; set; }
        public double Thickness { get; set; }

        public static MembraneProperties Default => new MembraneProperties
        {
            ModulusOfElasticity = 200,
            PoissonsRation = 0.3,
            Thickness = 0.5
        };
    }
}