using Common.Geometry;
using FEM2D.Restraints;

namespace FEM2D.Nodes
{
    public interface INode
    {
        PointD Coordinates { get; }
        int Number { get; }
        Restraint Restraint { get; }

        double DistanceTo(INode node);

        int[] GetDOF();

        void SetFixedSupport();

        void SetFixedWithRotationSupport();

        void SetPinnedSupport();

        void SetRestraint(Restraint restraint);

        void AddRestraint(Restraint restraint);

        void SetRotationDofs();

        void SetTranslationDofs();

        int TryGetRotationDOF();
    }
}