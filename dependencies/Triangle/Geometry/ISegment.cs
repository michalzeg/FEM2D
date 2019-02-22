// Decompiled with JetBrains decompiler
// Type: TriangleNet.Geometry.ISegment
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using TriangleNet.Data;

namespace TriangleNet.Geometry
{
  public interface ISegment
  {
    int P0 { get; }

    int P1 { get; }

    int Boundary { get; }

    Vertex GetVertex(int index);

    ITriangle GetTriangle(int index);
  }
}
