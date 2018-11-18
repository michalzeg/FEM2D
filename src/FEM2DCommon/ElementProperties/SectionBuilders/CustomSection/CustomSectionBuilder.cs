using FEM2DCommon.Sections;
using System.Collections.Generic;

namespace FEM2DCommon.ElementProperties.SectionBuilders.CustomSection
{
    public class CustomSectionBuilder : ISectionBuilder, ICustomSectionBuilderAddFirstPerimeter, ICustomSectionBuilderAddOtherPerimeters, ICustomSectionBuilderFinish
    {
        private IList<Perimeter> perimeters = new List<Perimeter>();

        public static ICustomSectionBuilderAddFirstPerimeter CustomSection
        {
            get
            {
                return new CustomSectionBuilder();
            }
        }

        private CustomSectionBuilder()
        {
        }

        public ICustomSectionBuilderAddOtherPerimeters WithPerimeter(Perimeter perimeter)
        {
            this.perimeters.Add(perimeter);
            return this;
        }

        public ICustomSectionBuilderFinish WithNoMorePerimeters()
        {
            return this;
        }

        public Section Build()
        {
            var result = new Section(this.perimeters);
            return result;
        }
    }
}