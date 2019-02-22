// Decompiled with JetBrains decompiler
// Type: TriangleNet.IO.TriangleFormat
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using System.Collections.Generic;
using System.IO;
using TriangleNet.Geometry;

namespace TriangleNet.IO
{
  public class TriangleFormat : IGeometryFormat, IMeshFormat
  {
    public Mesh Import(string filename)
    {
      string extension = Path.GetExtension(filename);
      if (extension == ".node" || extension == ".poly" || extension == ".ele")
      {
        InputGeometry geometry;
        List<ITriangle> triangles;
        FileReader.Read(filename, out geometry, out triangles);
        if (geometry != null && triangles != null)
        {
          Mesh mesh = new Mesh();
          mesh.Load(geometry, triangles);
          return mesh;
        }
      }
      throw new NotSupportedException("Could not load '" + filename + "' file.");
    }

    public void Write(Mesh mesh, string filename)
    {
      FileWriter.WritePoly(mesh, Path.ChangeExtension(filename, ".poly"));
      FileWriter.WriteElements(mesh, Path.ChangeExtension(filename, ".ele"));
    }

    public InputGeometry Read(string filename)
    {
      string extension = Path.GetExtension(filename);
      if (extension == ".node")
        return FileReader.ReadNodeFile(filename);
      if (extension == ".poly")
        return FileReader.ReadPolyFile(filename);
      throw new NotSupportedException("File format '" + extension + "' not supported.");
    }
  }
}
