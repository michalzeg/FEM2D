// Decompiled with JetBrains decompiler
// Type: TriangleNet.Tools.Statistic
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using TriangleNet.Data;
using TriangleNet.Geometry;

namespace TriangleNet.Tools
{
  public class Statistic
  {
    public static long InCircleCount = 0;
    public static long InCircleCountDecimal = 0;
    public static long CounterClockwiseCount = 0;
    public static long CounterClockwiseCountDecimal = 0;
    public static long Orient3dCount = 0;
    public static long HyperbolaCount = 0;
    public static long CircumcenterCount = 0;
    public static long CircleTopCount = 0;
    public static long RelocationCount = 0;
    private static readonly int[] plus1Mod3 = new int[3]
    {
      1,
      2,
      0
    };
    private static readonly int[] minus1Mod3 = new int[3]
    {
      2,
      0,
      1
    };
    private double minEdge;
    private double maxEdge;
    private double minAspect;
    private double maxAspect;
    private double minArea;
    private double maxArea;
    private double minAngle;
    private double maxAngle;
    private int inVetrices;
    private int inTriangles;
    private int inSegments;
    private int inHoles;
    private int outVertices;
    private int outTriangles;
    private int outEdges;
    private int boundaryEdges;
    private int intBoundaryEdges;
    private int constrainedEdges;
    private int[] angleTable;
    private int[] minAngles;
    private int[] maxAngles;

    public double ShortestEdge
    {
      get
      {
        return this.minEdge;
      }
    }

    public double LongestEdge
    {
      get
      {
        return this.maxEdge;
      }
    }

    public double ShortestAltitude
    {
      get
      {
        return this.minAspect;
      }
    }

    public double LargestAspectRatio
    {
      get
      {
        return this.maxAspect;
      }
    }

    public double SmallestArea
    {
      get
      {
        return this.minArea;
      }
    }

    public double LargestArea
    {
      get
      {
        return this.maxArea;
      }
    }

    public double SmallestAngle
    {
      get
      {
        return this.minAngle;
      }
    }

    public double LargestAngle
    {
      get
      {
        return this.maxAngle;
      }
    }

    public int InputVertices
    {
      get
      {
        return this.inVetrices;
      }
    }

    public int InputTriangles
    {
      get
      {
        return this.inTriangles;
      }
    }

    public int InputSegments
    {
      get
      {
        return this.inSegments;
      }
    }

    public int InputHoles
    {
      get
      {
        return this.inHoles;
      }
    }

    public int Vertices
    {
      get
      {
        return this.outVertices;
      }
    }

    public int Triangles
    {
      get
      {
        return this.outTriangles;
      }
    }

    public int Edges
    {
      get
      {
        return this.outEdges;
      }
    }

    public int BoundaryEdges
    {
      get
      {
        return this.boundaryEdges;
      }
    }

    public int InteriorBoundaryEdges
    {
      get
      {
        return this.intBoundaryEdges;
      }
    }

    public int ConstrainedEdges
    {
      get
      {
        return this.constrainedEdges;
      }
    }

    public int[] AngleHistogram
    {
      get
      {
        return this.angleTable;
      }
    }

    public int[] MinAngleHistogram
    {
      get
      {
        return this.minAngles;
      }
    }

    public int[] MaxAngleHistogram
    {
      get
      {
        return this.maxAngles;
      }
    }

    private void GetAspectHistogram(Mesh mesh)
    {
      int[] numArray1 = new int[16];
      double[] numArray2 = new double[16]
      {
        1.5,
        2.0,
        2.5,
        3.0,
        4.0,
        6.0,
        10.0,
        15.0,
        25.0,
        50.0,
        100.0,
        300.0,
        1000.0,
        10000.0,
        100000.0,
        0.0
      };
      Otri otri = new Otri();
      Vertex[] vertexArray = new Vertex[3];
      double[] numArray3 = new double[3];
      double[] numArray4 = new double[3];
      double[] numArray5 = new double[3];
      otri.orient = 0;
      foreach (Triangle triangle in mesh.triangles.Values)
      {
        otri.triangle = triangle;
        vertexArray[0] = otri.Org();
        vertexArray[1] = otri.Dest();
        vertexArray[2] = otri.Apex();
        double num1 = 0.0;
        for (int index1 = 0; index1 < 3; ++index1)
        {
          int index2 = Statistic.plus1Mod3[index1];
          int index3 = Statistic.minus1Mod3[index1];
          numArray3[index1] = vertexArray[index2].x - vertexArray[index3].x;
          numArray4[index1] = vertexArray[index2].y - vertexArray[index3].y;
          numArray5[index1] = numArray3[index1] * numArray3[index1] + numArray4[index1] * numArray4[index1];
          if (numArray5[index1] > num1)
            num1 = numArray5[index1];
        }
        double num2 = Math.Abs((vertexArray[2].x - vertexArray[0].x) * (vertexArray[1].y - vertexArray[0].y) - (vertexArray[1].x - vertexArray[0].x) * (vertexArray[2].y - vertexArray[0].y)) / 2.0;
        double num3 = num2 * num2 / num1;
        double num4 = num1 / num3;
        int index = 0;
        while (num4 > numArray2[index] * numArray2[index] && index < 15)
          ++index;
        ++numArray1[index];
      }
    }

    public void Update(Mesh mesh, int sampleDegrees)
    {
      this.inVetrices = mesh.invertices;
      this.inTriangles = mesh.inelements;
      this.inSegments = mesh.insegments;
      this.inHoles = mesh.holes.Count;
      this.outVertices = mesh.vertices.Count - mesh.undeads;
      this.outTriangles = mesh.triangles.Count;
      this.outEdges = mesh.edges;
      this.boundaryEdges = mesh.hullsize;
      this.intBoundaryEdges = mesh.subsegs.Count - mesh.hullsize;
      this.constrainedEdges = mesh.subsegs.Count;
      Point[] pointArray = new Point[3];
      sampleDegrees = 60;
      double[] numArray1 = new double[sampleDegrees / 2 - 1];
      double[] numArray2 = new double[3];
      double[] numArray3 = new double[3];
      double[] numArray4 = new double[3];
      double num1 = Math.PI / (double) sampleDegrees;
      double num2 = 180.0 / Math.PI;
      this.angleTable = new int[sampleDegrees];
      this.minAngles = new int[sampleDegrees];
      this.maxAngles = new int[sampleDegrees];
      for (int index = 0; index < sampleDegrees / 2 - 1; ++index)
      {
        numArray1[index] = Math.Cos(num1 * (double) (index + 1));
        numArray1[index] = numArray1[index] * numArray1[index];
      }
      for (int index = 0; index < sampleDegrees; ++index)
        this.angleTable[index] = 0;
      this.minAspect = mesh.bounds.Width + mesh.bounds.Height;
      this.minAspect *= this.minAspect;
      this.maxAspect = 0.0;
      this.minEdge = this.minAspect;
      this.maxEdge = 0.0;
      this.minArea = this.minAspect;
      this.maxArea = 0.0;
      this.minAngle = 0.0;
      this.maxAngle = 2.0;
      bool flag1 = true;
      bool flag2 = true;
      foreach (Triangle triangle in mesh.triangles.Values)
      {
        double num3 = 0.0;
        double num4 = 1.0;
        pointArray[0] = (Point) triangle.vertices[0];
        pointArray[1] = (Point) triangle.vertices[1];
        pointArray[2] = (Point) triangle.vertices[2];
        double num5 = 0.0;
        for (int index1 = 0; index1 < 3; ++index1)
        {
          int index2 = Statistic.plus1Mod3[index1];
          int index3 = Statistic.minus1Mod3[index1];
          numArray2[index1] = pointArray[index2].X - pointArray[index3].X;
          numArray3[index1] = pointArray[index2].Y - pointArray[index3].Y;
          numArray4[index1] = numArray2[index1] * numArray2[index1] + numArray3[index1] * numArray3[index1];
          if (numArray4[index1] > num5)
            num5 = numArray4[index1];
          if (numArray4[index1] > this.maxEdge)
            this.maxEdge = numArray4[index1];
          if (numArray4[index1] < this.minEdge)
            this.minEdge = numArray4[index1];
        }
        double num6 = Math.Abs((pointArray[2].X - pointArray[0].X) * (pointArray[1].Y - pointArray[0].Y) - (pointArray[1].X - pointArray[0].X) * (pointArray[2].Y - pointArray[0].Y));
        if (num6 < this.minArea)
          this.minArea = num6;
        if (num6 > this.maxArea)
          this.maxArea = num6;
        double num7 = num6 * num6 / num5;
        if (num7 < this.minAspect)
          this.minAspect = num7;
        double num8 = num5 / num7;
        if (num8 > this.maxAspect)
          this.maxAspect = num8;
        for (int index1 = 0; index1 < 3; ++index1)
        {
          int index2 = Statistic.plus1Mod3[index1];
          int index3 = Statistic.minus1Mod3[index1];
          double num9 = numArray2[index2] * numArray2[index3] + numArray3[index2] * numArray3[index3];
          double num10 = num9 * num9 / (numArray4[index2] * numArray4[index3]);
          int index4 = sampleDegrees / 2 - 1;
          for (int index5 = index4 - 1; index5 >= 0; --index5)
          {
            if (num10 > numArray1[index5])
              index4 = index5;
          }
          if (num9 <= 0.0)
          {
            ++this.angleTable[index4];
            if (num10 > this.minAngle)
              this.minAngle = num10;
            if (flag1 && num10 < this.maxAngle)
              this.maxAngle = num10;
            if (num10 > num3)
              num3 = num10;
            if (flag2 && num10 < num4)
              num4 = num10;
          }
          else
          {
            ++this.angleTable[sampleDegrees - index4 - 1];
            if (flag1 || num10 > this.maxAngle)
            {
              this.maxAngle = num10;
              flag1 = false;
            }
            if (flag2 || num10 > num4)
            {
              num4 = num10;
              flag2 = false;
            }
          }
        }
        int index6 = sampleDegrees / 2 - 1;
        for (int index1 = index6 - 1; index1 >= 0; --index1)
        {
          if (num3 > numArray1[index1])
            index6 = index1;
        }
        ++this.minAngles[index6];
        int index7 = sampleDegrees / 2 - 1;
        for (int index1 = index7 - 1; index1 >= 0; --index1)
        {
          if (num4 > numArray1[index1])
            index7 = index1;
        }
        if (flag2)
          ++this.maxAngles[index7];
        else
          ++this.maxAngles[sampleDegrees - index7 - 1];
        flag2 = true;
      }
      this.minEdge = Math.Sqrt(this.minEdge);
      this.maxEdge = Math.Sqrt(this.maxEdge);
      this.minAspect = Math.Sqrt(this.minAspect);
      this.maxAspect = Math.Sqrt(this.maxAspect);
      this.minArea *= 0.5;
      this.maxArea *= 0.5;
      this.minAngle = this.minAngle < 1.0 ? num2 * Math.Acos(Math.Sqrt(this.minAngle)) : 0.0;
      if (this.maxAngle >= 1.0)
        this.maxAngle = 180.0;
      else if (flag1)
        this.maxAngle = num2 * Math.Acos(Math.Sqrt(this.maxAngle));
      else
        this.maxAngle = 180.0 - num2 * Math.Acos(Math.Sqrt(this.maxAngle));
    }
  }
}
