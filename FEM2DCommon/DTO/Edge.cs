namespace FEM2DCommon.DTO
{
    public class Edge
    {
        public int Number { get; set; }

        public VertexInput Start { get; set; }
        public VertexInput End { get; set; }

        public int LoadX { get; set; }
        public int LoadY { get; set; }
    }
}