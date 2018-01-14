using Common.Extensions;
using FEM2D.Solvers;
using FEM2DCommon.Extensions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DTests.Solvers
{
    [TestFixture]
    public class CholeskyDescompositionTests
    {
        [Test]
        public void TestCholesky2x2_Passed()
        {
            var K = DenseMatrix.OfArray(new double[,]
            {
                {2,2},
                {2,4}
            });
            var P = DenseVector.OfArray(new double[]
            {
                8,
                15
            });

            var cholesky = new CholeskyDescomposition();
            var expectdResult = DenseVector.OfArray(new double[]
            {
                0.5,
                3.5
            });
            var actualResult = cholesky.Solve(K, P);

            Assert.That(actualResult.IsApproximatelyEqualTo(expectdResult));
        }

        [Test]
        public void TestCholesky3x3_Passed()
        {
            var K = DenseMatrix.OfArray(new double[,]
            {
                {2,0,-1},
                {0,3,2},
                {-1,2,8 }
            });
            var P = DenseVector.OfArray(new double[]
            {
                1,
                3,
                5,
            });

            var cholesky = new CholeskyDescomposition();
            var expectdResult = DenseVector.OfArray(new double[]
            {
                0.78378378378,
                0.62162162162,
                0.56756756757
            });
            var actualResult = cholesky.Solve(K, P);

            Assert.That(actualResult.IsApproximatelyEqualTo(expectdResult));
        }

        [Test]
        public void TestCholesky15x15_Passed()
        {

            var K = DenseMatrix.OfArray(new double[,]
            {
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,600000,0,0,-300000,0,0,0,0,0,0,0,0},
                {0,0,0,0,1500,0,0,0,3750,0,0,0,0,0,0},
                {0,0,0,0,0,50000,0,0,12500,0,0,0,0,0,0},
                {0,0,0,-300000,0,0,600000,0,0,-300000,0,0,0,0,0},
                {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
                {0,0,0,0,3750,12500,0,0,50000,0,-3750,12500,0,0,0},
                {0,0,0,0,0,0,-300000,0,0,600000,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,-3750,0,1500,0,0,0,0},
                {0,0,0,0,0,0,0,0,12500,0,0,50000,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},

            });
            var c = K.IsHermitian();
            var d = K.Cholesky();
            var P = DenseVector.OfArray(new double[]
            {
                1,
                3,
                5,
                -8,
                -8,
                10,
                0,
                0,
                10,
                50,
                10,
                12,
                -10,
                -5,
                0
            });

            var cholesky = new CholeskyDescomposition();
            var expectdResult = K.Inverse() * P;
            var actualResult = cholesky.Solve(K, P);

            Assert.That(actualResult.IsApproximatelyEqualTo(expectdResult));
        }
    }
}
