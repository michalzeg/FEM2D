using FEM2DCommon.DTO;

namespace TestApplication
{
    public class MockDataProvider
    {
        public static MembraneInputData GetMembraneData()
        {
            var vertex1 = new VertexInput
            {
                Number = 1,
                X = 0,
                Y = 2000,
                SupportX = false,
                SupportY = false,
                LoadX = 500,
                LoadY = 1000,
            };
            var vertex2 = new VertexInput
            {
                Number = 2,
                X = 500,
                Y = 0,
                SupportX = true,
                SupportY = true,
                LoadX = 0,
                LoadY = 0,
            };
            var vertex3 = new VertexInput
            {
                Number = 3,
                X = 1500,
                Y = 0,
                SupportX = true,
                SupportY = true,
                LoadX = 0,
                LoadY = 0,
            };
            var vertex4 = new VertexInput
            {
                Number = 4,
                X = 2000,
                Y = 2000,
                SupportX = false,
                SupportY = false,
                LoadX = -500,
                LoadY = 1000,
            };
            var vertex5 = new VertexInput
            {
                Number = 5,
                X = 1000,
                Y = 2000,
                SupportX = false,
                SupportY = false,
                LoadX = 0,
                LoadY = -2000,
            };

            var edge1 = new Edge
            {
                Number = 1,
                Start = vertex1,
                End = vertex2,
            };
            var edge2 = new Edge
            {
                Number = 2,
                Start = vertex2,
                End = vertex3,
            };
            var edge3 = new Edge
            {
                Number = 3,
                Start = vertex3,
                End = vertex4,
            };
            var edge4 = new Edge
            {
                Number = 3,
                Start = vertex4,
                End = vertex5,
            };
            var edge5 = new Edge
            {
                Number = 5,
                Start = vertex5,
                End = vertex1,
            };

            var membraneData = new MembraneInputData
            {
                Vertices = new[] { vertex1, vertex2, vertex3, vertex4, vertex5 },
                Edges = new[] { edge1, edge2, edge3, edge4, edge5 },
                Properties = new MembraneProperties
                {
                    ModulusOfElasticity = 200000000,
                    PoissonsRation = 0.25,
                    Thickness = 200
                },
            };
            return membraneData;
        }
    }
}