// Decompiled with JetBrains decompiler
// Type: TriangleNet.Quality
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using System.Collections.Generic;
using TriangleNet.Data;
using TriangleNet.Geometry;
using TriangleNet.Log;

namespace TriangleNet
{
  internal class Quality
  {
    private Queue<BadSubseg> badsubsegs;
    private BadTriQueue queue;
    private Mesh mesh;
    private Behavior behavior;
    private NewLocation newLocation;
    private Func<Point, Point, Point, double, bool> userTest;
    private ILog<SimpleLogItem> logger;

    public Quality(Mesh mesh)
    {
      this.logger = SimpleLog.Instance;
      this.badsubsegs = new Queue<BadSubseg>();
      this.queue = new BadTriQueue();
      this.mesh = mesh;
      this.behavior = mesh.behavior;
      this.newLocation = new NewLocation(mesh);
    }

    public void AddBadSubseg(BadSubseg badseg)
    {
      this.badsubsegs.Enqueue(badseg);
    }

    public bool CheckMesh()
    {
      Otri otri = new Otri();
      Otri o2_1 = new Otri();
      Otri o2_2 = new Otri();
      bool noExact = Behavior.NoExact;
      Behavior.NoExact = false;
      int num = 0;
      foreach (Triangle triangle in this.mesh.triangles.Values)
      {
        otri.triangle = triangle;
        for (otri.orient = 0; otri.orient < 3; ++otri.orient)
        {
          Vertex vertex1 = otri.Org();
          Vertex vertex2 = otri.Dest();
          if (otri.orient == 0)
          {
            Vertex vertex3 = otri.Apex();
            if (Primitives.CounterClockwise((Point) vertex1, (Point) vertex2, (Point) vertex3) <= 0.0)
            {
              this.logger.Warning("Triangle is flat or inverted.", "Quality.CheckMesh()");
              ++num;
            }
          }
          otri.Sym(ref o2_1);
          if (o2_1.triangle != Mesh.dummytri)
          {
            o2_1.Sym(ref o2_2);
            if (otri.triangle != o2_2.triangle || otri.orient != o2_2.orient)
            {
              if (otri.triangle == o2_2.triangle)
                this.logger.Warning("Asymmetric triangle-triangle bond: (Right triangle, wrong orientation)", "Quality.CheckMesh()");
              ++num;
            }
            Vertex vertex3 = o2_1.Org();
            Vertex vertex4 = o2_1.Dest();
            if ((Point) vertex1 != (Point) vertex4 || (Point) vertex2 != (Point) vertex3)
            {
              this.logger.Warning("Mismatched edge coordinates between two triangles.", "Quality.CheckMesh()");
              ++num;
            }
          }
        }
      }
      this.mesh.MakeVertexMap();
      foreach (Vertex vertex in this.mesh.vertices.Values)
      {
        if (vertex.tri.triangle == null)
          this.logger.Warning("Vertex (ID " + (object) vertex.id + ") not connected to mesh (duplicate input vertex?)", "Quality.CheckMesh()");
      }
      if (num == 0)
        this.logger.Info("Mesh topology appears to be consistent.");
      Behavior.NoExact = noExact;
      return num == 0;
    }

    public bool CheckDelaunay()
    {
      Otri otri = new Otri();
      Otri o2 = new Otri();
      Osub os = new Osub();
      bool noExact = Behavior.NoExact;
      Behavior.NoExact = false;
      int num = 0;
      foreach (Triangle triangle in this.mesh.triangles.Values)
      {
        otri.triangle = triangle;
        for (otri.orient = 0; otri.orient < 3; ++otri.orient)
        {
          Vertex vertex1 = otri.Org();
          Vertex vertex2 = otri.Dest();
          Vertex vertex3 = otri.Apex();
          otri.Sym(ref o2);
          Vertex vertex4 = o2.Apex();
          bool flag = o2.triangle != Mesh.dummytri && !Otri.IsDead(o2.triangle) && (otri.triangle.id < o2.triangle.id && (Point) vertex1 != (Point) this.mesh.infvertex1) && ((Point) vertex1 != (Point) this.mesh.infvertex2 && (Point) vertex1 != (Point) this.mesh.infvertex3 && ((Point) vertex2 != (Point) this.mesh.infvertex1 && (Point) vertex2 != (Point) this.mesh.infvertex2)) && ((Point) vertex2 != (Point) this.mesh.infvertex3 && (Point) vertex3 != (Point) this.mesh.infvertex1 && ((Point) vertex3 != (Point) this.mesh.infvertex2 && (Point) vertex3 != (Point) this.mesh.infvertex3) && ((Point) vertex4 != (Point) this.mesh.infvertex1 && (Point) vertex4 != (Point) this.mesh.infvertex2)) && (Point) vertex4 != (Point) this.mesh.infvertex3;
          if (this.mesh.checksegments && flag)
          {
            otri.SegPivot(ref os);
            if (os.seg != Mesh.dummysub)
              flag = false;
          }
          if (flag && Primitives.NonRegular((Point) vertex1, (Point) vertex2, (Point) vertex3, (Point) vertex4) > 0.0)
          {
            this.logger.Warning(string.Format("Non-regular pair of triangles found (IDs {0}/{1}).", (object) otri.triangle.id, (object) o2.triangle.id), "Quality.CheckDelaunay()");
            ++num;
          }
        }
      }
      if (num == 0)
        this.logger.Info("Mesh is Delaunay.");
      Behavior.NoExact = noExact;
      return num == 0;
    }

    public int CheckSeg4Encroach(ref Osub testsubseg)
    {
      Otri ot = new Otri();
      Osub o2 = new Osub();
      int num1 = 0;
      int num2 = 0;
      Vertex vertex1 = testsubseg.Org();
      Vertex vertex2 = testsubseg.Dest();
      testsubseg.TriPivot(ref ot);
      if (ot.triangle != Mesh.dummytri)
      {
        ++num2;
        Vertex vertex3 = ot.Apex();
        double num3 = (vertex1.x - vertex3.x) * (vertex2.x - vertex3.x) + (vertex1.y - vertex3.y) * (vertex2.y - vertex3.y);
        if (num3 < 0.0 && (this.behavior.ConformingDelaunay || num3 * num3 >= (2.0 * this.behavior.goodAngle - 1.0) * (2.0 * this.behavior.goodAngle - 1.0) * ((vertex1.x - vertex3.x) * (vertex1.x - vertex3.x) + (vertex1.y - vertex3.y) * (vertex1.y - vertex3.y)) * ((vertex2.x - vertex3.x) * (vertex2.x - vertex3.x) + (vertex2.y - vertex3.y) * (vertex2.y - vertex3.y))))
          num1 = 1;
      }
      testsubseg.Sym(ref o2);
      o2.TriPivot(ref ot);
      if (ot.triangle != Mesh.dummytri)
      {
        ++num2;
        Vertex vertex3 = ot.Apex();
        double num3 = (vertex1.x - vertex3.x) * (vertex2.x - vertex3.x) + (vertex1.y - vertex3.y) * (vertex2.y - vertex3.y);
        if (num3 < 0.0 && (this.behavior.ConformingDelaunay || num3 * num3 >= (2.0 * this.behavior.goodAngle - 1.0) * (2.0 * this.behavior.goodAngle - 1.0) * ((vertex1.x - vertex3.x) * (vertex1.x - vertex3.x) + (vertex1.y - vertex3.y) * (vertex1.y - vertex3.y)) * ((vertex2.x - vertex3.x) * (vertex2.x - vertex3.x) + (vertex2.y - vertex3.y) * (vertex2.y - vertex3.y))))
          num1 += 2;
      }
      if (num1 > 0 && (this.behavior.NoBisect == 0 || this.behavior.NoBisect == 1 && num2 == 2))
      {
        BadSubseg badSubseg = new BadSubseg();
        if (num1 == 1)
        {
          badSubseg.encsubseg = testsubseg;
          badSubseg.subsegorg = vertex1;
          badSubseg.subsegdest = vertex2;
        }
        else
        {
          badSubseg.encsubseg = o2;
          badSubseg.subsegorg = vertex2;
          badSubseg.subsegdest = vertex1;
        }
        this.badsubsegs.Enqueue(badSubseg);
      }
      return num1;
    }

    public void TestTriangle(ref Otri testtri)
    {
      Otri o2_1 = new Otri();
      Otri o2_2 = new Otri();
      Osub os = new Osub();
      Vertex enqorg = testtri.Org();
      Vertex enqdest = testtri.Dest();
      Vertex enqapex = testtri.Apex();
      double num1 = enqorg.x - enqdest.x;
      double num2 = enqorg.y - enqdest.y;
      double num3 = enqdest.x - enqapex.x;
      double num4 = enqdest.y - enqapex.y;
      double num5 = enqapex.x - enqorg.x;
      double num6 = enqapex.y - enqorg.y;
      double num7 = num1 * num1;
      double num8 = num2 * num2;
      double num9 = num3 * num3;
      double num10 = num4 * num4;
      double num11 = num5 * num5;
      double num12 = num6 * num6;
      double num13 = num7 + num8;
      double num14 = num9 + num10;
      double num15 = num11 + num12;
      double minedge;
      double num16;
      Vertex vertex1;
      Vertex vertex2;
      if (num13 < num14 && num13 < num15)
      {
        minedge = num13;
        double num17 = num3 * num5 + num4 * num6;
        num16 = num17 * num17 / (num14 * num15);
        vertex1 = enqorg;
        vertex2 = enqdest;
        testtri.Copy(ref o2_1);
      }
      else if (num14 < num15)
      {
        minedge = num14;
        double num17 = num1 * num5 + num2 * num6;
        num16 = num17 * num17 / (num13 * num15);
        vertex1 = enqdest;
        vertex2 = enqapex;
        testtri.Lnext(ref o2_1);
      }
      else
      {
        minedge = num15;
        double num17 = num1 * num3 + num2 * num4;
        num16 = num17 * num17 / (num13 * num14);
        vertex1 = enqapex;
        vertex2 = enqorg;
        testtri.Lprev(ref o2_1);
      }
      if (this.behavior.VarArea || this.behavior.fixedArea || this.behavior.Usertest)
      {
        double num17 = 0.5 * (num1 * num4 - num2 * num3);
        if (this.behavior.fixedArea && num17 > this.behavior.MaxArea)
        {
          this.queue.Enqueue(ref testtri, minedge, enqapex, enqorg, enqdest);
          return;
        }
        if (this.behavior.VarArea && num17 > testtri.triangle.area && testtri.triangle.area > 0.0)
        {
          this.queue.Enqueue(ref testtri, minedge, enqapex, enqorg, enqdest);
          return;
        }
        if (this.behavior.Usertest && this.userTest != null && this.userTest((Point) enqorg, (Point) enqdest, (Point) enqapex, num17))
        {
          this.queue.Enqueue(ref testtri, minedge, enqapex, enqorg, enqdest);
          return;
        }
      }
      double num18 = num13 <= num14 || num13 <= num15 ? (num14 <= num15 ? (num13 + num14 - num15) / (2.0 * Math.Sqrt(num13 * num14)) : (num13 + num15 - num14) / (2.0 * Math.Sqrt(num13 * num15))) : (num14 + num15 - num13) / (2.0 * Math.Sqrt(num14 * num15));
      if (num16 <= this.behavior.goodAngle && (num18 >= this.behavior.maxGoodAngle || this.behavior.MaxAngle == 0.0))
        return;
      if (vertex1.type == VertexType.SegmentVertex && vertex2.type == VertexType.SegmentVertex)
      {
        o2_1.SegPivot(ref os);
        if (os.seg == Mesh.dummysub)
        {
          o2_1.Copy(ref o2_2);
          do
          {
            o2_1.OprevSelf();
            o2_1.SegPivot(ref os);
          }
          while (os.seg == Mesh.dummysub);
          Vertex vertex3 = os.SegOrg();
          Vertex vertex4 = os.SegDest();
          do
          {
            o2_2.DnextSelf();
            o2_2.SegPivot(ref os);
          }
          while (os.seg == Mesh.dummysub);
          Vertex vertex5 = os.SegOrg();
          Vertex vertex6 = os.SegDest();
          Vertex vertex7 = (Vertex) null;
          if (vertex4.x == vertex5.x && vertex4.y == vertex5.y)
            vertex7 = vertex4;
          else if (vertex3.x == vertex6.x && vertex3.y == vertex6.y)
            vertex7 = vertex3;
          if ((Point) vertex7 != (Point) null)
          {
            double num17 = (vertex1.x - vertex7.x) * (vertex1.x - vertex7.x) + (vertex1.y - vertex7.y) * (vertex1.y - vertex7.y);
            double num19 = (vertex2.x - vertex7.x) * (vertex2.x - vertex7.x) + (vertex2.y - vertex7.y) * (vertex2.y - vertex7.y);
            if (num17 < 1001.0 / 1000.0 * num19 && num17 > 0.999 * num19)
              return;
          }
        }
      }
      this.queue.Enqueue(ref testtri, minedge, enqapex, enqorg, enqdest);
    }

    private void TallyEncs()
    {
      Osub testsubseg = new Osub();
      testsubseg.orient = 0;
      foreach (Segment segment in this.mesh.subsegs.Values)
      {
        testsubseg.seg = segment;
        this.CheckSeg4Encroach(ref testsubseg);
      }
    }

    private void SplitEncSegs(bool triflaws)
    {
      Otri otri1 = new Otri();
      Otri otri2 = new Otri();
      Osub os = new Osub();
      Osub osub = new Osub();
      while (this.badsubsegs.Count > 0 && this.mesh.steinerleft != 0)
      {
        BadSubseg badSubseg = this.badsubsegs.Dequeue();
        Osub encsubseg = badSubseg.encsubseg;
        Vertex vertex1 = encsubseg.Org();
        Vertex vertex2 = encsubseg.Dest();
        if (!Osub.IsDead(encsubseg.seg) && (Point) vertex1 == (Point) badSubseg.subsegorg && (Point) vertex2 == (Point) badSubseg.subsegdest)
        {
          encsubseg.TriPivot(ref otri1);
          otri1.Lnext(ref otri2);
          otri2.SegPivot(ref os);
          bool flag1 = os.seg != Mesh.dummysub;
          otri2.LnextSelf();
          otri2.SegPivot(ref os);
          bool flag2 = os.seg != Mesh.dummysub;
          if (!this.behavior.ConformingDelaunay && !flag1 && !flag2)
          {
            Vertex vertex3 = otri1.Apex();
            while (vertex3.type == VertexType.FreeVertex && (vertex1.x - vertex3.x) * (vertex2.x - vertex3.x) + (vertex1.y - vertex3.y) * (vertex2.y - vertex3.y) < 0.0)
            {
              this.mesh.DeleteVertex(ref otri2);
              encsubseg.TriPivot(ref otri1);
              vertex3 = otri1.Apex();
              otri1.Lprev(ref otri2);
            }
          }
          otri1.Sym(ref otri2);
          if (otri2.triangle != Mesh.dummytri)
          {
            otri2.LnextSelf();
            otri2.SegPivot(ref os);
            bool flag3 = os.seg != Mesh.dummysub;
            flag2 = flag2 || flag3;
            otri2.LnextSelf();
            otri2.SegPivot(ref os);
            bool flag4 = os.seg != Mesh.dummysub;
            flag1 = flag1 || flag4;
            if (!this.behavior.ConformingDelaunay && !flag4 && !flag3)
            {
              Vertex vertex3 = otri2.Org();
              while (vertex3.type == VertexType.FreeVertex && (vertex1.x - vertex3.x) * (vertex2.x - vertex3.x) + (vertex1.y - vertex3.y) * (vertex2.y - vertex3.y) < 0.0)
              {
                this.mesh.DeleteVertex(ref otri2);
                otri1.Sym(ref otri2);
                vertex3 = otri2.Apex();
                otri2.LprevSelf();
              }
            }
          }
          double num1;
          if (flag1 || flag2)
          {
            double num2 = Math.Sqrt((vertex2.x - vertex1.x) * (vertex2.x - vertex1.x) + (vertex2.y - vertex1.y) * (vertex2.y - vertex1.y));
            double num3 = 1.0;
            while (num2 > 3.0 * num3)
              num3 *= 2.0;
            while (num2 < 1.5 * num3)
              num3 *= 0.5;
            num1 = num3 / num2;
            if (flag2)
              num1 = 1.0 - num1;
          }
          else
            num1 = 0.5;
          Vertex newvertex = new Vertex(vertex1.x + num1 * (vertex2.x - vertex1.x), vertex1.y + num1 * (vertex2.y - vertex1.y), encsubseg.Mark(), this.mesh.nextras);
          newvertex.type = VertexType.SegmentVertex;
          newvertex.hash = this.mesh.hash_vtx++;
          newvertex.id = newvertex.hash;
          this.mesh.vertices.Add(newvertex.hash, newvertex);
          for (int index = 0; index < this.mesh.nextras; ++index)
            newvertex.attributes[index] = vertex1.attributes[index] + num1 * (vertex2.attributes[index] - vertex1.attributes[index]);
          if (!Behavior.NoExact)
          {
            double num2 = Primitives.CounterClockwise((Point) vertex1, (Point) vertex2, (Point) newvertex);
            double num3 = (vertex1.x - vertex2.x) * (vertex1.x - vertex2.x) + (vertex1.y - vertex2.y) * (vertex1.y - vertex2.y);
            if (num2 != 0.0 && num3 != 0.0)
            {
              double d = num2 / num3;
              if (!double.IsNaN(d))
              {
                newvertex.x += d * (vertex2.y - vertex1.y);
                newvertex.y += d * (vertex1.x - vertex2.x);
              }
            }
          }
          if (newvertex.x == vertex1.x && newvertex.y == vertex1.y || newvertex.x == vertex2.x && newvertex.y == vertex2.y)
          {
            this.logger.Error("Ran out of precision: I attempted to split a segment to a smaller size than can be accommodated by the finite precision of floating point arithmetic.", "Quality.SplitEncSegs()");
            throw new Exception("Ran out of precision");
          }
          switch (this.mesh.InsertVertex(newvertex, ref otri1, ref encsubseg, true, triflaws))
          {
            case InsertVertexResult.Successful:
            case InsertVertexResult.Encroaching:
              if (this.mesh.steinerleft > 0)
                --this.mesh.steinerleft;
              this.CheckSeg4Encroach(ref encsubseg);
              encsubseg.NextSelf();
              this.CheckSeg4Encroach(ref encsubseg);
              break;
            default:
              this.logger.Error("Failure to split a segment.", "Quality.SplitEncSegs()");
              throw new Exception("Failure to split a segment.");
          }
        }
        badSubseg.subsegorg = (Vertex) null;
      }
    }

    private void TallyFaces()
    {
      Otri testtri = new Otri();
      testtri.orient = 0;
      foreach (Triangle triangle in this.mesh.triangles.Values)
      {
        testtri.triangle = triangle;
        this.TestTriangle(ref testtri);
      }
    }

    private void SplitTriangle(BadTriangle badtri)
    {
      Otri otri = new Otri();
      double xi = 0.0;
      double eta = 0.0;
      Otri poortri = badtri.poortri;
      Vertex torg = poortri.Org();
      Vertex tdest = poortri.Dest();
      Vertex tapex = poortri.Apex();
      if (Otri.IsDead(poortri.triangle) || !((Point) torg == (Point) badtri.triangorg) || (!((Point) tdest == (Point) badtri.triangdest) || !((Point) tapex == (Point) badtri.triangapex)))
        return;
      bool flag = false;
      Point point = this.behavior.fixedArea || this.behavior.VarArea ? Primitives.FindCircumcenter((Point) torg, (Point) tdest, (Point) tapex, ref xi, ref eta, this.behavior.offconstant) : this.newLocation.FindLocation(torg, tdest, tapex, ref xi, ref eta, true, poortri);
      if (point.x == torg.x && point.y == torg.y || point.x == tdest.x && point.y == tdest.y || point.x == tapex.x && point.y == tapex.y)
      {
        if (Behavior.Verbose)
        {
          this.logger.Warning("New vertex falls on existing vertex.", "Quality.SplitTriangle()");
          flag = true;
        }
      }
      else
      {
        Vertex newvertex = new Vertex(point.x, point.y, 0, this.mesh.nextras);
        newvertex.type = VertexType.FreeVertex;
        for (int index = 0; index < this.mesh.nextras; ++index)
          newvertex.attributes[index] = torg.attributes[index] + xi * (tdest.attributes[index] - torg.attributes[index]) + eta * (tapex.attributes[index] - torg.attributes[index]);
        if (eta < xi)
          poortri.LprevSelf();
        Osub splitseg = new Osub();
        switch (this.mesh.InsertVertex(newvertex, ref poortri, ref splitseg, true, true))
        {
          case InsertVertexResult.Successful:
            newvertex.hash = this.mesh.hash_vtx++;
            newvertex.id = newvertex.hash;
            this.mesh.vertices.Add(newvertex.hash, newvertex);
            if (this.mesh.steinerleft > 0)
            {
              --this.mesh.steinerleft;
              break;
            }
            break;
          case InsertVertexResult.Encroaching:
            this.mesh.UndoVertex();
            break;
          case InsertVertexResult.Violating:
            break;
          default:
            if (Behavior.Verbose)
            {
              this.logger.Warning("New vertex falls on existing vertex.", "Quality.SplitTriangle()");
              flag = true;
              break;
            }
            break;
        }
      }
      if (flag)
      {
        this.logger.Error("The new vertex is at the circumcenter of triangle: This probably means that I am trying to refine triangles to a smaller size than can be accommodated by the finite precision of floating point arithmetic.", "Quality.SplitTriangle()");
        throw new Exception("The new vertex is at the circumcenter of triangle.");
      }
    }

    public void EnforceQuality()
    {
      this.TallyEncs();
      this.SplitEncSegs(false);
      if (this.behavior.MinAngle > 0.0 || this.behavior.VarArea || (this.behavior.fixedArea || this.behavior.Usertest))
      {
        this.TallyFaces();
        this.mesh.checkquality = true;
        while (this.queue.Count > 0 && this.mesh.steinerleft != 0)
        {
          BadTriangle badtri = this.queue.Dequeue();
          this.SplitTriangle(badtri);
          if (this.badsubsegs.Count > 0)
          {
            this.queue.Enqueue(badtri);
            this.SplitEncSegs(true);
          }
        }
      }
      if (!Behavior.Verbose || !this.behavior.ConformingDelaunay || (this.badsubsegs.Count <= 0 || this.mesh.steinerleft != 0))
        return;
      this.logger.Warning("I ran out of Steiner points, but the mesh has encroached subsegments, and therefore might not be truly Delaunay. If the Delaunay property is important to you, try increasing the number of Steiner points.", "Quality.EnforceQuality()");
    }
  }
}
