// Decompiled with JetBrains decompiler
// Type: TriangleNet.Geometry.ITriangle
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using TriangleNet.Data;

namespace TriangleNet.Geometry
{
  public interface ITriangle
  {
    int ID { get; }

    int P0 { get; }

    int P1 { get; }

    int P2 { get; }

    Vertex GetVertex(int index);

    bool SupportsNeighbors { get; }

    int N0 { get; }

    int N1 { get; }

    int N2 { get; }

    ITriangle GetNeighbor(int index);

    ISegment GetSegment(int index);

    double Area { get; set; }

    int Region { get; }
  }
}
