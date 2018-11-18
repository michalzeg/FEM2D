using FEM2D.Results;
using FEM2D.Results.Membranes;
using FEM2D.Results.Nodes;
using FEM2DCommon.DTO;

namespace FEM2DOutput.OutputCreator
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

        public static NodeOutputDetailed ConvertToOutputDetailed(this MembraneNodeResult nodeResult)
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