using Common.DTO;
using FEM2D.Nodes;
using FEM2D.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Output.OutputCreator
{
    public static class Converters
    {
        public static NodeOutput ConvertToOutput(this NodeResult nodeResult)
        {
            var result = new NodeOutput
            {
                Number = nodeResult.Node.Number,
                Ux = nodeResult.UX,
                Uy = nodeResult.UY,
                X = nodeResult.Node.Coordinates.X,
                Y = nodeResult.Node.Coordinates.Y
            };
            return result;
        }

        public static NodeOutputDetailed ConvertToOutputDetailed(this NodeResult nodeResult)
        {
            var result = new NodeOutputDetailed
            {
                Number = nodeResult.Node.Number,
                X = nodeResult.Node.Coordinates.X,
                Y = nodeResult.Node.Coordinates.Y,
                Ux = nodeResult.UX,
                Uy = nodeResult.UY,
                AvgSxx = nodeResult.AverageSigmaXX,
                AvgSyy = nodeResult.AverageSigmaYY,
                AvgTxy = nodeResult.AverageTauXY
            };
            return result;
        }
        
    }
}
