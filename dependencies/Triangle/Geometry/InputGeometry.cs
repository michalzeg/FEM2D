﻿// Decompiled with JetBrains decompiler
// Type: TriangleNet.Geometry.InputGeometry
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using System.Collections.Generic;
using TriangleNet.Data;

namespace TriangleNet.Geometry
{
  public class InputGeometry
  {
    private int pointAttributes = -1;
    internal List<Vertex> points;
    internal List<Edge> segments;
    internal List<Point> holes;
    internal List<RegionPointer> regions;
    private BoundingBox bounds;

    public InputGeometry()
      : this(3)
    {
    }

    public InputGeometry(int capacity)
    {
      this.points = new List<Vertex>(capacity);
      this.segments = new List<Edge>();
      this.holes = new List<Point>();
      this.regions = new List<RegionPointer>();
      this.bounds = new BoundingBox();
      this.pointAttributes = -1;
    }

    public BoundingBox Bounds
    {
      get
      {
        return this.bounds;
      }
    }

    public bool HasSegments
    {
      get
      {
        return this.segments.Count > 0;
      }
    }

    public int Count
    {
      get
      {
        return this.points.Count;
      }
    }

    public IEnumerable<Point> Points
    {
      get
      {
        return (IEnumerable<Point>) this.points;
      }
    }

    public ICollection<Edge> Segments
    {
      get
      {
        return (ICollection<Edge>) this.segments;
      }
    }

    public ICollection<Point> Holes
    {
      get
      {
        return (ICollection<Point>) this.holes;
      }
    }

    public ICollection<RegionPointer> Regions
    {
      get
      {
        return (ICollection<RegionPointer>) this.regions;
      }
    }

    public void Clear()
    {
      this.points.Clear();
      this.segments.Clear();
      this.holes.Clear();
      this.regions.Clear();
      this.pointAttributes = -1;
    }

    public void AddPoint(double x, double y)
    {
      this.AddPoint(x, y, 0);
    }

    public void AddPoint(double x, double y, int boundary)
    {
      this.points.Add(new Vertex(x, y, boundary));
      this.bounds.Update(x, y);
    }

    public void AddPoint(double x, double y, int boundary, double attribute)
    {
      this.AddPoint(x, y, 0, new double[1]{ attribute });
    }

    public void AddPoint(double x, double y, int boundary, double[] attribs)
    {
      if (this.pointAttributes < 0)
      {
        this.pointAttributes = attribs == null ? 0 : attribs.Length;
      }
      else
      {
        if (attribs == null && this.pointAttributes > 0)
          throw new ArgumentException("Inconsitent use of point attributes.");
        if (attribs != null && this.pointAttributes != attribs.Length)
          throw new ArgumentException("Inconsitent use of point attributes.");
      }
      List<Vertex> points = this.points;
      Vertex vertex1 = new Vertex(x, y, boundary);
      vertex1.attributes = attribs;
      Vertex vertex2 = vertex1;
      points.Add(vertex2);
      this.bounds.Update(x, y);
    }

    public void AddHole(double x, double y)
    {
      this.holes.Add(new Point(x, y));
    }

    public void AddRegion(double x, double y, int id)
    {
      this.regions.Add(new RegionPointer(x, y, id));
    }

    public void AddSegment(int p0, int p1)
    {
      this.AddSegment(p0, p1, 0);
    }

    public void AddSegment(int p0, int p1, int boundary)
    {
      if (p0 == p1 || p0 < 0 || p1 < 0)
        throw new NotSupportedException("Invalid endpoints.");
      this.segments.Add(new Edge(p0, p1, boundary));
    }
  }
}
