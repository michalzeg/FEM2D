using FEM2DCommon.ElementProperties;

namespace TestApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //ResultsToJSon();
            DxfCantilever.ResultsToDxfCantilever();
            //ResultsToDxfMembrane();
            //ResultsToDxfMembrane1x1();
            //AppTest();

            var sectionProperties = BeamPropertiesBuilder.Create()
                .SetSteel()
                .SetRectangularSection(20, 20)
                .Build();
        }
    }
}