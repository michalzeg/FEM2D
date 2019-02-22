// Decompiled with JetBrains decompiler
// Type: TriangleNet.IO.IGeometryFormat
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using TriangleNet.Geometry;

namespace TriangleNet.IO
{
  public interface IGeometryFormat
  {
    InputGeometry Read(string filename);
  }
}
