using FEM2DCommon.ElementProperties;
using FEM2DCommon.ElementProperties.Builder;

namespace FEM2DApplication
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