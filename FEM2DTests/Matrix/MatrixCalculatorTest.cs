using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEM2D.Matrix;
using FEM2D.Materials;
using Common.Extensions;
using FEM2D.Nodes;

namespace FEM2DTests.MatrixTest
{
    [TestFixture]
    public class MatrixCalculatorTest
    {
        [Test]
        public void MatrixCalculator_GetD()
        {
            var calculator = new MatrixCalculator();

            var E = 200;
            var v = 0.5;
            var material = new Material
            {
                ModulusOfElasticity = E,
                PoissonsRation = v
            };

            var expectedD = DenseMatrix.OfArray(new double[,]
            {
                {1,v,0 },
                {v,1,0 },
                {0,0,(1-v)/2 }
            }) * E / (1 - v * v);

            var actualD = calculator.GetD(material);
            Assert.That(expectedD.IsApproximatelyEqualTo(actualD));
        }

        [Test]
        public void MatrixCalculator_GetB()
        {
            var node1 = new Node(3, 0);
            var node2 = new Node(3, 2);
            var node3 = new Node(0, 0);

            var nodes = new[] { node1, node2, node3 };

            //taken from book
            var expectedB = DenseMatrix.OfArray(new double[,]
            {
                {2,0,0,0,-2,0 },
                {0,-3,0,3,0,0 },
                {-3,2,3,0,0,-2 }
            }) * 1 / 6;

            var calculator = new MatrixCalculator();
            var actualB = calculator.GetB(nodes);

            Assert.That(actualB.IsApproximatelyEqualTo(expectedB));
        }

        [Test]
        public void MatrixCalculator_GetK()
        {



            var t = 2;
            var A = 3;

            var B = DenseMatrix.OfArray(new double[,]
            {
                {2,0,0,0,-2,0 },
                {0,-3,0,3,0,0 },
                {-3,2,3,0,0,-2 }
            });
            var D = DenseMatrix.OfArray(new double[,]
            {
                {3,8,0 },
                {8,3,0 },
                {0,0,1 },
            });

            var expectedK = DenseMatrix.OfArray(new double[,]
            {
                {126,-324,-54,288,-72,36 },
                {-324,186,36,-162,288,-24 },
                {-54,36,54,0,0,-36 },
                {288,-162,0,162,-288,0 },
                {-72,288,0,-288,72,0 },
                {36,-24,-36,0,0,24 }
            });

            var calculator = new MatrixCalculator();
            var actualK = calculator.GetK(t, A, B, D);

            Assert.That(actualK.IsApproximatelyEqualTo(expectedK));
        }
    }
}
