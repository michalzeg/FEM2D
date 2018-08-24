namespace FEM2DCommon.DTO
{
    public class NodeOutput
    {
        public double X { get; set; }
        public double Y { get; set; }

        public int Number { get; set; }
        public double Ux { get; set; }
        public double Uy { get; set; }
    }

    public class NodeOutputDetailed : NodeOutput
    {
        public double AvgSxx { get; set; }
        public double AvgSyy { get; set; }
        public double AvgTxy { get; set; }
    }
}