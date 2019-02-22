// Decompiled with JetBrains decompiler
// Type: TriangleNet.IO.DataReader
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using System.Collections.Generic;
using TriangleNet.Data;
using TriangleNet.Geometry;
using TriangleNet.Log;

namespace TriangleNet.IO
{
  internal static class DataReader
  {
    public static int Reconstruct(Mesh mesh, InputGeometry input, ITriangle[] triangles)
    {
      Otri newotri = new Otri();
      Otri o2_1 = new Otri();
      Otri otri1 = new Otri();
      Otri o2_2 = new Otri();
      Otri o2_3 = new Otri();
      Osub osub = new Osub();
      int[] numArray1 = new int[3];
      int[] numArray2 = new int[2];
      int num1 = triangles == null ? 0 : triangles.Length;
      int count = input.segments.Count;
      mesh.inelements = num1;
      mesh.regions.AddRange((IEnumerable<RegionPointer>) input.regions);
      for (int index = 0; index < mesh.inelements; ++index)
        mesh.MakeTriangle(ref newotri);
      if (mesh.behavior.Poly)
      {
        mesh.insegments = count;
        for (int index = 0; index < mesh.insegments; ++index)
          mesh.MakeSegment(ref osub);
      }
      List<Otri>[] otriListArray = new List<Otri>[mesh.vertices.Count];
      for (int index = 0; index < mesh.vertices.Count; ++index)
      {
        Otri otri2 = new Otri();
        otri2.triangle = Mesh.dummytri;
        otriListArray[index] = new List<Otri>(3);
        otriListArray[index].Add(otri2);
      }
      int index1 = 0;
      foreach (Triangle triangle in mesh.triangles.Values)
      {
        newotri.triangle = triangle;
        numArray1[0] = triangles[index1].P0;
        numArray1[1] = triangles[index1].P1;
        numArray1[2] = triangles[index1].P2;
        for (int index2 = 0; index2 < 3; ++index2)
        {
          if (numArray1[index2] < 0 || numArray1[index2] >= mesh.invertices)
          {
            SimpleLog.Instance.Error("Triangle has an invalid vertex index.", "MeshReader.Reconstruct()");
            throw new Exception("Triangle has an invalid vertex index.");
          }
        }
        newotri.triangle.region = triangles[index1].Region;
        if (mesh.behavior.VarArea)
          newotri.triangle.area = triangles[index1].Area;
        newotri.orient = 0;
        newotri.SetOrg(mesh.vertices[numArray1[0]]);
        newotri.SetDest(mesh.vertices[numArray1[1]]);
        newotri.SetApex(mesh.vertices[numArray1[2]]);
        for (newotri.orient = 0; newotri.orient < 3; ++newotri.orient)
        {
          int index2 = numArray1[newotri.orient];
          int index3 = otriListArray[index2].Count - 1;
          Otri otri2 = otriListArray[index2][index3];
          otriListArray[index2].Add(newotri);
          Otri o2_4 = otri2;
          if (o2_4.triangle != Mesh.dummytri)
          {
            Vertex vertex1 = newotri.Dest();
            Vertex vertex2 = newotri.Apex();
            do
            {
              Vertex vertex3 = o2_4.Dest();
              Vertex vertex4 = o2_4.Apex();
              if ((Point) vertex2 == (Point) vertex3)
              {
                newotri.Lprev(ref o2_1);
                o2_1.Bond(ref o2_4);
              }
              if ((Point) vertex1 == (Point) vertex4)
              {
                o2_4.Lprev(ref o2_2);
                newotri.Bond(ref o2_2);
              }
              --index3;
              o2_4 = otriListArray[index2][index3];
            }
            while (o2_4.triangle != Mesh.dummytri);
          }
        }
        ++index1;
      }
      int num2 = 0;
      if (mesh.behavior.Poly)
      {
        int index2 = 0;
        foreach (Segment segment in mesh.subsegs.Values)
        {
          osub.seg = segment;
          numArray2[0] = input.segments[index2].P0;
          numArray2[1] = input.segments[index2].P1;
          int boundary = input.segments[index2].Boundary;
          for (int index3 = 0; index3 < 2; ++index3)
          {
            if (numArray2[index3] < 0 || numArray2[index3] >= mesh.invertices)
            {
              SimpleLog.Instance.Error("Segment has an invalid vertex index.", "MeshReader.Reconstruct()");
              throw new Exception("Segment has an invalid vertex index.");
            }
          }
          osub.orient = 0;
          Vertex vertex1 = mesh.vertices[numArray2[0]];
          Vertex vertex2 = mesh.vertices[numArray2[1]];
          osub.SetOrg(vertex1);
          osub.SetDest(vertex2);
          osub.SetSegOrg(vertex1);
          osub.SetSegDest(vertex2);
          osub.seg.boundary = boundary;
          for (osub.orient = 0; osub.orient < 2; ++osub.orient)
          {
            int index3 = numArray2[1 - osub.orient];
            int index4 = otriListArray[index3].Count - 1;
            Otri otri2 = otriListArray[index3][index4];
            Otri tri = otriListArray[index3][index4];
            Vertex vertex3 = osub.Org();
            for (bool flag = true; flag && tri.triangle != Mesh.dummytri; tri = otriListArray[index3][index4])
            {
              Vertex vertex4 = tri.Dest();
              if ((Point) vertex3 == (Point) vertex4)
              {
                otriListArray[index3].Remove(otri2);
                tri.SegBond(ref osub);
                tri.Sym(ref o2_3);
                if (o2_3.triangle == Mesh.dummytri)
                {
                  mesh.InsertSubseg(ref tri, 1);
                  ++num2;
                }
                flag = false;
              }
              --index4;
              otri2 = otriListArray[index3][index4];
            }
          }
          ++index2;
        }
      }
      for (int index2 = 0; index2 < mesh.vertices.Count; ++index2)
      {
        int index3 = otriListArray[index2].Count - 1;
        Otri otri2;
        for (Otri tri = otriListArray[index2][index3]; tri.triangle != Mesh.dummytri; tri = otri2)
        {
          --index3;
          otri2 = otriListArray[index2][index3];
          tri.SegDissolve();
          tri.Sym(ref o2_3);
          if (o2_3.triangle == Mesh.dummytri)
          {
            mesh.InsertSubseg(ref tri, 1);
            ++num2;
          }
        }
      }
      return num2;
    }
  }
}
