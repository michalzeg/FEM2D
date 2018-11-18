using Common.Geometry;
using FEM2DCommon.ElementProperties;
using FEM2DCommon.ElementProperties.Builder;
using FEM2DCommon.Sections;
using System.Collections.Generic;

namespace FEM2DDynamicApplication
{
    public class DataProvider
    {
        public static DynamicBeamProperties GetSection()
        {
            var perimeters = new List<Perimeter>
            {
                new Perimeter(new List<PointD>{
                    new PointD(-0.5,0),
                    new PointD(-0.5,0.1),
                    new PointD(0.5,1),
                    new PointD(0.5,0),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(-0.05,0.1),
                    new PointD(-0.05,1),
                    new PointD(0.05,1),
                    new PointD(0.05,0.1),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(-0.05,1),
                    new PointD(-0.05,2),
                    new PointD(0.05,2),
                    new PointD(0.05,1),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(-0.05,2),
                    new PointD(-0.05,3.9),
                    new PointD(0.05,3.9),
                    new PointD(0.05,2),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(-0.5,3.9),
                    new PointD(-0.5,4),
                    new PointD(0.5,4),
                    new PointD(0.5,3.9),
                }),

                new Perimeter(new List<PointD>{
                    new PointD(2,0),
                    new PointD(2,0.1),
                    new PointD(3,1),
                    new PointD(3,0),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(2.45,0.1),
                    new PointD(2.45,1),
                    new PointD(2.55,1),
                    new PointD(2.55,0.1),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(2.45,1),
                    new PointD(2.45,2),
                    new PointD(2.55,2),
                    new PointD(2.55,1),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(2.45,2),
                    new PointD(2.45,3.9),
                    new PointD(2.55,3.9),
                    new PointD(2.55,2),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(2,3.9),
                    new PointD(2,4),
                    new PointD(3,4),
                    new PointD(3,3.9),
                }),
            };

            var section = new Section(perimeters);

            var dynamicProperties = DynamicBeamPropertiesBuilder.Create()
                .SetSteel()
                .SetSection(section)
                .Build();
            return dynamicProperties;
        }
    }
}