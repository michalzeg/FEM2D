namespace FEM2D.ShapeFunctions
{
    public static class BeamShapeFunctions
    {
        public static double N1(double position, double length)
        {
            var x = position;
            var L = length;
            return 1 - x / L;
        }

        public static double N2(double position, double length)
        {
            var x = position;
            var L = length;

            return 1 - 3 * x * x / (L * L) + 2 * x * x * x / (L * L * L);
        }

        public static double N3(double position, double length)
        {
            var x = position;
            var L = length;

            return x * (1 - x / L) * (1 - x / L);
        }

        public static double N4(double position, double length)
        {
            var x = position;
            var L = length;

            return x / L;
        }

        public static double N5(double position, double length)
        {
            var x = position;
            var L = length;

            return 3 * x * x / (L * L) - 2 * x * x * x / (L * L * L);
        }

        public static double N6(double position, double length)
        {
            var x = position;
            var L = length;

            return x * x / L * (x / L - 1);
        }
    }
}