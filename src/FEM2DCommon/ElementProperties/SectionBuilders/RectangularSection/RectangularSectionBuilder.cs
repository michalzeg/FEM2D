﻿using FEM2DCommon.Sections;

namespace FEM2DCommon.ElementProperties.SectionBuilders.RectangularSection
{
    public class RectangularSectionBuilder : ISectionBuilder, IRectangularSectionBuilderSetHeight, IRectangularSectionBuilderSetWidth, IRectangularSectionBuilderFinish
    {
        private double width;
        private double height;

        public static IRectangularSectionBuilderSetWidth RectangularSection
        {
            get
            {
                return new RectangularSectionBuilder();
            }
        }

        private RectangularSectionBuilder()
        {
        }

        public IRectangularSectionBuilderSetHeight SetWidth(double width)
        {
            this.width = width;
            return this;
        }

        public IRectangularSectionBuilderFinish SetHeight(double height)
        {
            this.height = height;
            return this;
        }

        public Section BuildSection()
        {
            return Section.FromRectangle(this.width, this.height);
        }
    }
}