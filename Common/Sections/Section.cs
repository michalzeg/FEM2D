using Common.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Sections
{
    public class Section
    {
        public IEnumerable<Perimeter> Perimeters { get;}
        public SectionProperties SectionProperties { get; }

        public Section(IEnumerable<Perimeter> perimeters)
        {
            this.Perimeters = perimeters;
            var calculator = new SectionPropertiesCalculator();
            this.SectionProperties = calculator.CalculateProperties(perimeters);
        }

        public static Section FromRectangle(double width, double height)
        {
            var coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(width, 0),
                new PointD(width, height),
                new PointD(0, height)
            };

            var perimeter = new Perimeter(coordinates);

            var result = new Section(new[] { perimeter });
            return result;
        }
    }
}
