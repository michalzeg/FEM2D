using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Elements
{
    public interface IBarElement : IElement
    {
        IBarElementProperties BeamProperties { get; }
        double Length { get; }
    }
}