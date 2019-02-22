﻿// Decompiled with JetBrains decompiler
// Type: TriangleNet.Tools.QuadNode
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System.Collections.Generic;
using TriangleNet.Geometry;

namespace TriangleNet.Tools
{
  internal class QuadNode
  {
    private static readonly byte[] BITVECTOR = new byte[4]
    {
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 8
    };
    private const int SW = 0;
    private const int SE = 1;
    private const int NW = 2;
    private const int NE = 3;
    private const double EPS = 1E-06;
    private BoundingBox bounds;
    private Point pivot;
    private QuadTree tree;
    private QuadNode[] regions;
    private List<int> triangles;
    private byte bitRegions;

    public QuadNode(BoundingBox box, QuadTree tree)
      : this(box, tree, false)
    {
    }

    public QuadNode(BoundingBox box, QuadTree tree, bool init)
    {
      this.tree = tree;
      this.bounds = new BoundingBox(box.Xmin, box.Ymin, box.Xmax, box.Ymax);
      this.pivot = new Point((box.Xmin + box.Xmax) / 2.0, (box.Ymin + box.Ymax) / 2.0);
      this.bitRegions = (byte) 0;
      this.regions = new QuadNode[4];
      this.triangles = new List<int>();
      if (!init)
        return;
      this.triangles.Capacity = tree.triangles.Length;
      foreach (ITriangle triangle in tree.triangles)
        this.triangles.Add(triangle.ID);
    }

    public List<int> FindTriangles(Point searchPoint)
    {
      int region = this.FindRegion(searchPoint);
      if (this.regions[region] == null)
        return this.triangles;
      return this.regions[region].FindTriangles(searchPoint);
    }

    public void CreateSubRegion(int currentDepth)
    {
      this.regions[0] = new QuadNode(new BoundingBox(this.bounds.Xmin, this.bounds.Ymin, this.pivot.X, this.pivot.Y), this.tree);
      this.regions[1] = new QuadNode(new BoundingBox(this.pivot.X, this.bounds.Ymin, this.bounds.Xmax, this.pivot.Y), this.tree);
      this.regions[2] = new QuadNode(new BoundingBox(this.bounds.Xmin, this.pivot.Y, this.pivot.X, this.bounds.Ymax), this.tree);
      this.regions[3] = new QuadNode(new BoundingBox(this.pivot.X, this.pivot.Y, this.bounds.Xmax, this.bounds.Ymax), this.tree);
      Point[] triangle1 = new Point[3];
      foreach (int triangle2 in this.triangles)
      {
        ITriangle triangle3 = this.tree.triangles[triangle2];
        triangle1[0] = (Point) triangle3.GetVertex(0);
        triangle1[1] = (Point) triangle3.GetVertex(1);
        triangle1[2] = (Point) triangle3.GetVertex(2);
        this.AddTriangleToRegion(triangle1, triangle3.ID);
      }
      for (int index = 0; index < 4; ++index)
      {
        if (this.regions[index].triangles.Count > this.tree.sizeBound && currentDepth < this.tree.maxDepth)
          this.regions[index].CreateSubRegion(currentDepth + 1);
      }
    }

    private void AddTriangleToRegion(Point[] triangle, int index)
    {
      this.bitRegions = (byte) 0;
      if (QuadTree.IsPointInTriangle(this.pivot, triangle[0], triangle[1], triangle[2]))
      {
        this.AddToRegion(index, 0);
        this.AddToRegion(index, 1);
        this.AddToRegion(index, 2);
        this.AddToRegion(index, 3);
      }
      else
      {
        this.FindTriangleIntersections(triangle, index);
        if (this.bitRegions != (byte) 0)
          return;
        this.regions[this.FindRegion(triangle[0])].triangles.Add(index);
      }
    }

    private void FindTriangleIntersections(Point[] triangle, int index)
    {
      int k = 2;
      for (int index1 = 0; index1 < 3; k = index1++)
      {
        double dx = triangle[index1].X - triangle[k].X;
        double dy = triangle[index1].Y - triangle[k].Y;
        if (dx != 0.0)
          this.FindIntersectionsWithX(dx, dy, triangle, index, k);
        if (dy != 0.0)
          this.FindIntersectionsWithY(dx, dy, triangle, index, k);
      }
    }

    private void FindIntersectionsWithX(double dx, double dy, Point[] triangle, int index, int k)
    {
      double num1 = (this.pivot.X - triangle[k].X) / dx;
      if (num1 < 1.000001 && num1 > -1E-06)
      {
        double num2 = triangle[k].Y + num1 * dy;
        if (num2 < this.pivot.Y)
        {
          if (num2 >= this.bounds.Ymin)
          {
            this.AddToRegion(index, 0);
            this.AddToRegion(index, 1);
          }
        }
        else if (num2 <= this.bounds.Ymax)
        {
          this.AddToRegion(index, 2);
          this.AddToRegion(index, 3);
        }
      }
      double num3 = (this.bounds.Xmin - triangle[k].X) / dx;
      if (num3 < 1.000001 && num3 > -1E-06)
      {
        double num2 = triangle[k].Y + num3 * dy;
        if (num2 <= this.pivot.Y && num2 >= this.bounds.Ymin)
          this.AddToRegion(index, 0);
        else if (num2 >= this.pivot.Y && num2 <= this.bounds.Ymax)
          this.AddToRegion(index, 2);
      }
      double num4 = (this.bounds.Xmax - triangle[k].X) / dx;
      if (num4 >= 1.000001 || num4 <= -1E-06)
        return;
      double num5 = triangle[k].Y + num4 * dy;
      if (num5 <= this.pivot.Y && num5 >= this.bounds.Ymin)
      {
        this.AddToRegion(index, 1);
      }
      else
      {
        if (num5 < this.pivot.Y || num5 > this.bounds.Ymax)
          return;
        this.AddToRegion(index, 3);
      }
    }

    private void FindIntersectionsWithY(double dx, double dy, Point[] triangle, int index, int k)
    {
      double num1 = (this.pivot.Y - triangle[k].Y) / dy;
      if (num1 < 1.000001 && num1 > -1E-06)
      {
        double num2 = triangle[k].X + num1 * dy;
        if (num2 > this.pivot.X)
        {
          if (num2 <= this.bounds.Xmax)
          {
            this.AddToRegion(index, 1);
            this.AddToRegion(index, 3);
          }
        }
        else if (num2 >= this.bounds.Xmin)
        {
          this.AddToRegion(index, 0);
          this.AddToRegion(index, 2);
        }
      }
      double num3 = (this.bounds.Ymin - triangle[k].Y) / dy;
      if (num3 < 1.000001 && num3 > -1E-06)
      {
        double num2 = triangle[k].X + num3 * dx;
        if (num2 <= this.pivot.X && num2 >= this.bounds.Xmin)
          this.AddToRegion(index, 0);
        else if (num2 >= this.pivot.X && num2 <= this.bounds.Xmax)
          this.AddToRegion(index, 1);
      }
      double num4 = (this.bounds.Ymax - triangle[k].Y) / dy;
      if (num4 >= 1.000001 || num4 <= -1E-06)
        return;
      double num5 = triangle[k].X + num4 * dx;
      if (num5 <= this.pivot.X && num5 >= this.bounds.Xmin)
      {
        this.AddToRegion(index, 2);
      }
      else
      {
        if (num5 < this.pivot.X || num5 > this.bounds.Xmax)
          return;
        this.AddToRegion(index, 3);
      }
    }

    private int FindRegion(Point point)
    {
      int num = 2;
      if (point.Y < this.pivot.Y)
        num = 0;
      if (point.X > this.pivot.X)
        ++num;
      return num;
    }

    private void AddToRegion(int index, int region)
    {
      if (((int) this.bitRegions & (int) QuadNode.BITVECTOR[region]) != 0)
        return;
      this.regions[region].triangles.Add(index);
      this.bitRegions |= QuadNode.BITVECTOR[region];
    }
  }
}
