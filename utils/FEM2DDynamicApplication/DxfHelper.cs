using FEM2DDynamics.Solver;
using netDxf;
using netDxf.Entities;

namespace FEM2DDynamicApplication
{
    public class DxfHelper
    {
        public static void SaveToDxf(DynamicSolverSettings settings, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam1, FEM2DDynamics.Results.DynamicBeamElementResults results)
        {
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();
            var time = 0d;

            while (time <= settings.EndTime)
            {
                var result = results.GetResult(beam1, time);
                var displ = result.GetDisplacement(1);
                var moment = result.Moment(1);

                document.AddEntity(new Point(time * 100, moment, 0));

                time += settings.DeltaTime;
            }
            document.Save(filePath);
        }
    }
}