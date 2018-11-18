using Common.Geometry;
using FEM2D.Nodes;
using FEM2D.Nodes.Dofs;
using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using FEM2DCommon.Sections;
using FEM2DDynamics.Elements.Beam;
using FEM2DDynamics.Solver;
using MathNet.Numerics.LinearAlgebra.Double;
using NSubstitute;
using NUnit.Framework;

namespace FEM2DDynamicsTests.Elements.Beam
{
    [TestFixture]
    public class DynamicBeamElementTests
    {
        private const double density = 2;
        private const int beamNumber = 10;

        private Node node1;
        private Node node2;
        private DynamicBeamProperties dynamicBeamProperties;

        [SetUp]
        public void SetUp()
        {
            var dofCalculator = Substitute.For<IDofNumberCalculator>();
            dofCalculator.GetFreeDOFNumber().Returns(0);
            this.node1 = new Node(0, 0, 0, dofCalculator);

            this.node2 = new Node(10, 0, 1, dofCalculator);

            this.dynamicBeamProperties = new DynamicBeamProperties()
            {
                BeamProperties = GetBeamProperties(),
                Density = density
            };
        }

        [Test]
        public void DynamicBeamElement_GetMassMatrix_ReturnsProperMatrix()
        {
            var beamLength = 10;
            var expectedMassMatrix = DenseMatrix.OfArray(new double[,]
            {
                {140 , 0      , 0     , 70     , 0      , 0       },
                {0   , 156    , 22*beamLength  , 0      , 54     , -13*beamLength   },
                {0   , 22*beamLength   , 4*beamLength*beamLength , 0      , 13*beamLength   , -3*beamLength*beamLength  },
                {70  , 0      , 0     , 140    , 0      , 0       },
                {0   , 54     , 13*beamLength  , 0      , 156    , -22*beamLength   },
                {0   ,-13*beamLength   , -3*beamLength*beamLength, 0      , -22*beamLength  , 4*beamLength*beamLength   }
            }) * density * beamLength / 420;

            var dynamicBeamElement = this.CreateDynamicBeamElement();

            var actualMassMatrix = dynamicBeamElement.GetMassMatrix();

            Assert.That(actualMassMatrix, Is.EqualTo(expectedMassMatrix));
        }

        [Test]
        public void DynamicBeamElement_GetStiffnessMatrix_ReturnsProperMatrix()
        {
            var length = 10;
            var momentOfIntertia = this.dynamicBeamProperties.BeamProperties.MomentOfInertia;
            var modulusOfElasticity = this.dynamicBeamProperties.BeamProperties.ModulusOfElasticity;
            var area = this.dynamicBeamProperties.BeamProperties.Area;

            var expectedStiffnessMatrix = DenseMatrix.OfArray(new double[,]
            {
                {area*length*length/momentOfIntertia , 0    ,0     , -area*length*length/momentOfIntertia, 0   , 0      },
                {0       , 12   , 6*length  , 0       , -12 , 6*length    },
                {0       , 6*length  , 4*length*length, 0       , -6*length, 2*length*length },
                {-area*length*length/momentOfIntertia, 0    ,0     , area*length*length/momentOfIntertia , 0   , 0      },
                {0       ,-12   , -6*length , 0       , 12  , -6*length   },
                {0       ,6*length   , 2*length*length, 0       , -6*length, 4*length*length  }
            }) * 3 * modulusOfElasticity * momentOfIntertia / (length * length * length);

            var dynamicBeamElement = this.CreateDynamicBeamElement();

            var actualStiffnessMatrix = dynamicBeamElement.GetStiffnessMatrix();

            Assert.That(actualStiffnessMatrix, Is.EqualTo(expectedStiffnessMatrix));
        }

        [Test]
        public void DynamicBeamElement_GetStiffnessMatrix_BeforeUpdateDampingFactors_ReturnsProperMatrix()
        {
            var length = 10;
            var momentOfIntertia = this.dynamicBeamProperties.BeamProperties.MomentOfInertia;
            var modulusOfElasticity = this.dynamicBeamProperties.BeamProperties.ModulusOfElasticity;
            var area = this.dynamicBeamProperties.BeamProperties.Area;

            var stiffnessMatrix = DenseMatrix.OfArray(new double[,]
            {
                {area*length*length/momentOfIntertia , 0    ,0     , -area*length*length/momentOfIntertia, 0   , 0      },
                {0       , 12   , 6*length  , 0       , -12 , 6*length    },
                {0       , 6*length  , 4*length*length, 0       , -6*length, 2*length*length },
                {-area*length*length/momentOfIntertia, 0    ,0     , area*length*length/momentOfIntertia , 0   , 0      },
                {0       ,-12   , -6*length , 0       , 12  , -6*length   },
                {0       ,6*length   , 2*length*length, 0       , -6*length, 4*length*length  }
            }) * 3 * modulusOfElasticity * momentOfIntertia / (length * length * length);

            var massMatrix = DenseMatrix.OfArray(new double[,]
            {
                {140 , 0      , 0     , 70     , 0      , 0       },
                {0   , 156    , 22*length  , 0      , 54     , -13*length   },
                {0   , 22*length   , 4*length*length , 0      , 13*length   , -3*length*length  },
                {70  , 0      , 0     , 140    , 0      , 0       },
                {0   , 54     , 13*length  , 0      , 156    , -22*length   },
                {0   ,-13*length   , -3*length*length, 0      , -22*length  , 4*length*length   }
            }) * density * length / 420;

            var dynamicBeamElement = this.CreateDynamicBeamElement();

            var expectedMassMatrix = stiffnessMatrix + massMatrix;

            var actualMassMatrix = dynamicBeamElement.GetDampingMatrix();

            Assert.That(actualMassMatrix, Is.EqualTo(expectedMassMatrix));
        }

        [Test]
        public void DynamicBeamElement_GetStiffnessMatrix_AfterUpdateDampingFactors_ReturnsProperMatrix()
        {
            var length = 10;
            var momentOfIntertia = this.dynamicBeamProperties.BeamProperties.MomentOfInertia;
            var modulusOfElasticity = this.dynamicBeamProperties.BeamProperties.ModulusOfElasticity;
            var area = this.dynamicBeamProperties.BeamProperties.Area;

            var stiffnessMatrix = DenseMatrix.OfArray(new double[,]
            {
                {area*length*length/momentOfIntertia , 0    ,0     , -area*length*length/momentOfIntertia, 0   , 0      },
                {0       , 12   , 6*length  , 0       , -12 , 6*length    },
                {0       , 6*length  , 4*length*length, 0       , -6*length, 2*length*length },
                {-area*length*length/momentOfIntertia, 0    ,0     , area*length*length/momentOfIntertia , 0   , 0      },
                {0       ,-12   , -6*length , 0       , 12  , -6*length   },
                {0       ,6*length   , 2*length*length, 0       , -6*length, 4*length*length  }
            }) * 3 * modulusOfElasticity * momentOfIntertia / (length * length * length);

            var massMatrix = DenseMatrix.OfArray(new double[,]
            {
                {140 , 0      , 0     , 70     , 0      , 0       },
                {0   , 156    , 22*length  , 0      , 54     , -13*length   },
                {0   , 22*length   , 4*length*length , 0      , 13*length   , -3*length*length  },
                {70  , 0      , 0     , 140    , 0      , 0       },
                {0   , 54     , 13*length  , 0      , 156    , -22*length   },
                {0   ,-13*length   , -3*length*length, 0      , -22*length  , 4*length*length   }
            }) * density * length / 420;

            var massDapingFactor = 0.1;
            var stiffnessDampingFactor = 0.5;

            var dampingFactors = Substitute.For<IDampingFactorCalculator>();
            dampingFactors.MassDampingFactor.Returns(massDapingFactor);
            dampingFactors.StiffnessDampingFactor.Returns(stiffnessDampingFactor);

            var dynamicBeamElement = this.CreateDynamicBeamElement();
            dynamicBeamElement.UpdateDampingFactors(dampingFactors);

            var expectedMassMatrix = stiffnessDampingFactor * stiffnessMatrix + massDapingFactor * massMatrix;
            var actualMassMatrix = dynamicBeamElement.GetDampingMatrix();

            Assert.That(actualMassMatrix, Is.EqualTo(expectedMassMatrix));
        }

        [Test]
        public void DynamicBeamElement_IsBetweenEnds_ReturnsTrue()
        {
            var checkingPoint = new PointD(5, 0);

            var dynamicBeamElement = this.CreateDynamicBeamElement();

            var result = dynamicBeamElement.IsBetweenEnds(checkingPoint);

            Assert.That(result, Is.True);
        }

        [Test]
        public void DynamicBeamElement_IsBetweenEnds__PointAtTheLeftOfStartReturnsFalse()
        {
            var checkingPoint = new PointD(-1, 0);

            var dynamicBeamElement = this.CreateDynamicBeamElement();

            var result = dynamicBeamElement.IsBetweenEnds(checkingPoint);

            Assert.That(result, Is.False);
        }

        [Test]
        public void DynamicBeamElement_IsBetweenEnds__PointAtTheRightOfEndReturnsFalse()
        {
            var checkingPoint = new PointD(11, 0);

            var dynamicBeamElement = this.CreateDynamicBeamElement();

            var result = dynamicBeamElement.IsBetweenEnds(checkingPoint);

            Assert.That(result, Is.False);
        }

        private DynamicBeamElement CreateDynamicBeamElement()
        {
            return new DynamicBeamElement(node1, node2, dynamicBeamProperties, beamNumber);
        }

        private static BeamProperties GetBeamProperties()
        {
            return new BeamProperties()
            {
                ModulusOfElasticity = 200000000,
                Section = Section.FromRectangle(1, 2),
            };
        }
    }
}