// Decompiled with JetBrains decompiler
// Type: TriangleNet.IO.IMeshFormat
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

namespace TriangleNet.IO
{
  public interface IMeshFormat
  {
    Mesh Import(string filename);

    void Write(Mesh mesh, string filename);
  }
}
