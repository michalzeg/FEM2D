// Decompiled with JetBrains decompiler
// Type: TriangleNet.Algorithm.Dwyer
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using TriangleNet.Data;
using TriangleNet.Geometry;
using TriangleNet.Log;

namespace TriangleNet.Algorithm
{
  internal class Dwyer
  {
    private static Random rand = new Random(DateTime.Now.Millisecond);
    private bool useDwyer = true;
    private Vertex[] sortarray;
    private Mesh mesh;

    private void VertexSort(int left, int right)
    {
      int left1 = left;
      int right1 = right;
      if (right - left + 1 < 32)
      {
        for (int index1 = left + 1; index1 <= right; ++index1)
        {
          Vertex vertex = this.sortarray[index1];
          int index2;
          for (index2 = index1 - 1; index2 >= left && (this.sortarray[index2].x > vertex.x || this.sortarray[index2].x == vertex.x && this.sortarray[index2].y > vertex.y); --index2)
            this.sortarray[index2 + 1] = this.sortarray[index2];
          this.sortarray[index2 + 1] = vertex;
        }
      }
      else
      {
        int index = Dwyer.rand.Next(left, right);
        double x = this.sortarray[index].x;
        double y = this.sortarray[index].y;
        --left;
        ++right;
        while (left < right)
        {
          do
          {
            ++left;
          }
          while (left <= right && (this.sortarray[left].x < x || this.sortarray[left].x == x && this.sortarray[left].y < y));
          do
          {
            --right;
          }
          while (left <= right && (this.sortarray[right].x > x || this.sortarray[right].x == x && this.sortarray[right].y > y));
          if (left < right)
          {
            Vertex vertex = this.sortarray[left];
            this.sortarray[left] = this.sortarray[right];
            this.sortarray[right] = vertex;
          }
        }
        if (left > left1)
          this.VertexSort(left1, left);
        if (right1 <= right + 1)
          return;
        this.VertexSort(right + 1, right1);
      }
    }

    private void VertexMedian(int left, int right, int median, int axis)
    {
      int num1 = right - left + 1;
      int left1 = left;
      int right1 = right;
      if (num1 == 2)
      {
        if (this.sortarray[left][axis] <= this.sortarray[right][axis] && (this.sortarray[left][axis] != this.sortarray[right][axis] || this.sortarray[left][1 - axis] <= this.sortarray[right][1 - axis]))
          return;
        Vertex vertex = this.sortarray[right];
        this.sortarray[right] = this.sortarray[left];
        this.sortarray[left] = vertex;
      }
      else
      {
        int index = Dwyer.rand.Next(left, right);
        double num2 = this.sortarray[index][axis];
        double num3 = this.sortarray[index][1 - axis];
        --left;
        ++right;
        while (left < right)
        {
          do
          {
            ++left;
          }
          while (left <= right && (this.sortarray[left][axis] < num2 || this.sortarray[left][axis] == num2 && this.sortarray[left][1 - axis] < num3));
          do
          {
            --right;
          }
          while (left <= right && (this.sortarray[right][axis] > num2 || this.sortarray[right][axis] == num2 && this.sortarray[right][1 - axis] > num3));
          if (left < right)
          {
            Vertex vertex = this.sortarray[left];
            this.sortarray[left] = this.sortarray[right];
            this.sortarray[right] = vertex;
          }
        }
        if (left > median)
          this.VertexMedian(left1, left - 1, median, axis);
        if (right >= median - 1)
          return;
        this.VertexMedian(right + 1, right1, median, axis);
      }
    }

    private void AlternateAxes(int left, int right, int axis)
    {
      int num1 = right - left + 1;
      int num2 = num1 >> 1;
      if (num1 <= 3)
        axis = 0;
      this.VertexMedian(left, right, left + num2, axis);
      if (num1 - num2 < 2)
        return;
      if (num2 >= 2)
        this.AlternateAxes(left, left + num2 - 1, 1 - axis);
      this.AlternateAxes(left + num2, right, 1 - axis);
    }

    private void MergeHulls(
      ref Otri farleft,
      ref Otri innerleft,
      ref Otri innerright,
      ref Otri farright,
      int axis)
    {
      Otri o2_1 = new Otri();
      Otri o2_2 = new Otri();
      Otri otri1 = new Otri();
      Otri o2_3 = new Otri();
      Otri o2_4 = new Otri();
      Otri o2_5 = new Otri();
      Otri o2_6 = new Otri();
      Otri otri2 = new Otri();
      Vertex ptr1 = innerleft.Dest();
      Vertex vertex1 = innerleft.Apex();
      Vertex ptr2 = innerright.Org();
      Vertex vertex2 = innerright.Apex();
      if (this.useDwyer && axis == 1)
      {
        Vertex vertex3 = farleft.Org();
        Vertex vertex4 = farleft.Apex();
        Vertex vertex5 = farright.Dest();
        Vertex vertex6 = farright.Apex();
        for (; vertex4.y < vertex3.y; vertex4 = farleft.Apex())
        {
          farleft.LnextSelf();
          farleft.SymSelf();
          vertex3 = vertex4;
        }
        innerleft.Sym(ref o2_6);
        for (Vertex vertex7 = o2_6.Apex(); vertex7.y > ptr1.y; vertex7 = o2_6.Apex())
        {
          o2_6.Lnext(ref innerleft);
          vertex1 = ptr1;
          ptr1 = vertex7;
          innerleft.Sym(ref o2_6);
        }
        for (; vertex2.y < ptr2.y; vertex2 = innerright.Apex())
        {
          innerright.LnextSelf();
          innerright.SymSelf();
          ptr2 = vertex2;
        }
        farright.Sym(ref o2_6);
        for (Vertex vertex7 = o2_6.Apex(); vertex7.y > vertex5.y; vertex7 = o2_6.Apex())
        {
          o2_6.Lnext(ref farright);
          vertex6 = vertex5;
          vertex5 = vertex7;
          farright.Sym(ref o2_6);
        }
      }
      bool flag1;
      do
      {
        flag1 = false;
        if (Primitives.CounterClockwise((Point) ptr1, (Point) vertex1, (Point) ptr2) > 0.0)
        {
          innerleft.LprevSelf();
          innerleft.SymSelf();
          ptr1 = vertex1;
          vertex1 = innerleft.Apex();
          flag1 = true;
        }
        if (Primitives.CounterClockwise((Point) vertex2, (Point) ptr2, (Point) ptr1) > 0.0)
        {
          innerright.LnextSelf();
          innerright.SymSelf();
          ptr2 = vertex2;
          vertex2 = innerright.Apex();
          flag1 = true;
        }
      }
      while (flag1);
      innerleft.Sym(ref o2_1);
      innerright.Sym(ref o2_2);
      this.mesh.MakeTriangle(ref otri2);
      otri2.Bond(ref innerleft);
      otri2.LnextSelf();
      otri2.Bond(ref innerright);
      otri2.LnextSelf();
      otri2.SetOrg(ptr2);
      otri2.SetDest(ptr1);
      Vertex vertex8 = farleft.Org();
      if ((Point) ptr1 == (Point) vertex8)
        otri2.Lnext(ref farleft);
      Vertex vertex9 = farright.Dest();
      if ((Point) ptr2 == (Point) vertex9)
        otri2.Lprev(ref farright);
      Vertex ptr3 = ptr1;
      Vertex ptr4 = ptr2;
      Vertex ptr5 = o2_1.Apex();
      Vertex ptr6 = o2_2.Apex();
      while (true)
      {
        bool flag2 = Primitives.CounterClockwise((Point) ptr5, (Point) ptr3, (Point) ptr4) <= 0.0;
        bool flag3 = Primitives.CounterClockwise((Point) ptr6, (Point) ptr3, (Point) ptr4) <= 0.0;
        if (!flag2 || !flag3)
        {
          if (!flag2)
          {
            o2_1.Lprev(ref otri1);
            otri1.SymSelf();
            Vertex ptr7 = otri1.Apex();
            if ((Point) ptr7 != (Point) null)
            {
              for (bool flag4 = Primitives.InCircle((Point) ptr3, (Point) ptr4, (Point) ptr5, (Point) ptr7) > 0.0; flag4; flag4 = (Point) ptr7 != (Point) null && Primitives.InCircle((Point) ptr3, (Point) ptr4, (Point) ptr5, (Point) ptr7) > 0.0)
              {
                otri1.LnextSelf();
                otri1.Sym(ref o2_4);
                otri1.LnextSelf();
                otri1.Sym(ref o2_3);
                otri1.Bond(ref o2_4);
                o2_1.Bond(ref o2_3);
                o2_1.LnextSelf();
                o2_1.Sym(ref o2_5);
                otri1.LprevSelf();
                otri1.Bond(ref o2_5);
                o2_1.SetOrg(ptr3);
                o2_1.SetDest((Vertex) null);
                o2_1.SetApex(ptr7);
                otri1.SetOrg((Vertex) null);
                otri1.SetDest(ptr5);
                otri1.SetApex(ptr7);
                ptr5 = ptr7;
                o2_3.Copy(ref otri1);
                ptr7 = otri1.Apex();
              }
            }
          }
          if (!flag3)
          {
            o2_2.Lnext(ref otri1);
            otri1.SymSelf();
            Vertex ptr7 = otri1.Apex();
            if ((Point) ptr7 != (Point) null)
            {
              for (bool flag4 = Primitives.InCircle((Point) ptr3, (Point) ptr4, (Point) ptr6, (Point) ptr7) > 0.0; flag4; flag4 = (Point) ptr7 != (Point) null && Primitives.InCircle((Point) ptr3, (Point) ptr4, (Point) ptr6, (Point) ptr7) > 0.0)
              {
                otri1.LprevSelf();
                otri1.Sym(ref o2_4);
                otri1.LprevSelf();
                otri1.Sym(ref o2_3);
                otri1.Bond(ref o2_4);
                o2_2.Bond(ref o2_3);
                o2_2.LprevSelf();
                o2_2.Sym(ref o2_5);
                otri1.LnextSelf();
                otri1.Bond(ref o2_5);
                o2_2.SetOrg((Vertex) null);
                o2_2.SetDest(ptr4);
                o2_2.SetApex(ptr7);
                otri1.SetOrg(ptr6);
                otri1.SetDest((Vertex) null);
                otri1.SetApex(ptr7);
                ptr6 = ptr7;
                o2_3.Copy(ref otri1);
                ptr7 = otri1.Apex();
              }
            }
          }
          if (flag2 || !flag3 && Primitives.InCircle((Point) ptr5, (Point) ptr3, (Point) ptr4, (Point) ptr6) > 0.0)
          {
            otri2.Bond(ref o2_2);
            o2_2.Lprev(ref otri2);
            otri2.SetDest(ptr3);
            ptr4 = ptr6;
            otri2.Sym(ref o2_2);
            ptr6 = o2_2.Apex();
          }
          else
          {
            otri2.Bond(ref o2_1);
            o2_1.Lnext(ref otri2);
            otri2.SetOrg(ptr4);
            ptr3 = ptr5;
            otri2.Sym(ref o2_1);
            ptr5 = o2_1.Apex();
          }
        }
        else
          break;
      }
      this.mesh.MakeTriangle(ref otri1);
      otri1.SetOrg(ptr3);
      otri1.SetDest(ptr4);
      otri1.Bond(ref otri2);
      otri1.LnextSelf();
      otri1.Bond(ref o2_2);
      otri1.LnextSelf();
      otri1.Bond(ref o2_1);
      if (!this.useDwyer || axis != 1)
        return;
      Vertex vertex10 = farleft.Org();
      Vertex vertex11 = farleft.Apex();
      Vertex vertex12 = farright.Dest();
      Vertex vertex13 = farright.Apex();
      farleft.Sym(ref o2_6);
      for (Vertex vertex3 = o2_6.Apex(); vertex3.x < vertex10.x; vertex3 = o2_6.Apex())
      {
        o2_6.Lprev(ref farleft);
        vertex11 = vertex10;
        vertex10 = vertex3;
        farleft.Sym(ref o2_6);
      }
      for (; vertex13.x > vertex12.x; vertex13 = farright.Apex())
      {
        farright.LprevSelf();
        farright.SymSelf();
        vertex12 = vertex13;
      }
    }

    private void DivconqRecurse(
      int left,
      int right,
      int axis,
      ref Otri farleft,
      ref Otri farright)
    {
      Otri newotri = new Otri();
      Otri otri1 = new Otri();
      Otri otri2 = new Otri();
      Otri otri3 = new Otri();
      Otri otri4 = new Otri();
      Otri otri5 = new Otri();
      int num1 = right - left + 1;
      switch (num1)
      {
        case 2:
          this.mesh.MakeTriangle(ref farleft);
          farleft.SetOrg(this.sortarray[left]);
          farleft.SetDest(this.sortarray[left + 1]);
          this.mesh.MakeTriangle(ref farright);
          farright.SetOrg(this.sortarray[left + 1]);
          farright.SetDest(this.sortarray[left]);
          farleft.Bond(ref farright);
          farleft.LprevSelf();
          farright.LnextSelf();
          farleft.Bond(ref farright);
          farleft.LprevSelf();
          farright.LnextSelf();
          farleft.Bond(ref farright);
          farright.Lprev(ref farleft);
          break;
        case 3:
          this.mesh.MakeTriangle(ref newotri);
          this.mesh.MakeTriangle(ref otri1);
          this.mesh.MakeTriangle(ref otri2);
          this.mesh.MakeTriangle(ref otri3);
          double num2 = Primitives.CounterClockwise((Point) this.sortarray[left], (Point) this.sortarray[left + 1], (Point) this.sortarray[left + 2]);
          if (num2 == 0.0)
          {
            newotri.SetOrg(this.sortarray[left]);
            newotri.SetDest(this.sortarray[left + 1]);
            otri1.SetOrg(this.sortarray[left + 1]);
            otri1.SetDest(this.sortarray[left]);
            otri2.SetOrg(this.sortarray[left + 2]);
            otri2.SetDest(this.sortarray[left + 1]);
            otri3.SetOrg(this.sortarray[left + 1]);
            otri3.SetDest(this.sortarray[left + 2]);
            newotri.Bond(ref otri1);
            otri2.Bond(ref otri3);
            newotri.LnextSelf();
            otri1.LprevSelf();
            otri2.LnextSelf();
            otri3.LprevSelf();
            newotri.Bond(ref otri3);
            otri1.Bond(ref otri2);
            newotri.LnextSelf();
            otri1.LprevSelf();
            otri2.LnextSelf();
            otri3.LprevSelf();
            newotri.Bond(ref otri1);
            otri2.Bond(ref otri3);
            otri1.Copy(ref farleft);
            otri2.Copy(ref farright);
            break;
          }
          newotri.SetOrg(this.sortarray[left]);
          otri1.SetDest(this.sortarray[left]);
          otri3.SetOrg(this.sortarray[left]);
          if (num2 > 0.0)
          {
            newotri.SetDest(this.sortarray[left + 1]);
            otri1.SetOrg(this.sortarray[left + 1]);
            otri2.SetDest(this.sortarray[left + 1]);
            newotri.SetApex(this.sortarray[left + 2]);
            otri2.SetOrg(this.sortarray[left + 2]);
            otri3.SetDest(this.sortarray[left + 2]);
          }
          else
          {
            newotri.SetDest(this.sortarray[left + 2]);
            otri1.SetOrg(this.sortarray[left + 2]);
            otri2.SetDest(this.sortarray[left + 2]);
            newotri.SetApex(this.sortarray[left + 1]);
            otri2.SetOrg(this.sortarray[left + 1]);
            otri3.SetDest(this.sortarray[left + 1]);
          }
          newotri.Bond(ref otri1);
          newotri.LnextSelf();
          newotri.Bond(ref otri2);
          newotri.LnextSelf();
          newotri.Bond(ref otri3);
          otri1.LprevSelf();
          otri2.LnextSelf();
          otri1.Bond(ref otri2);
          otri1.LprevSelf();
          otri3.LprevSelf();
          otri1.Bond(ref otri3);
          otri2.LnextSelf();
          otri3.LprevSelf();
          otri2.Bond(ref otri3);
          otri1.Copy(ref farleft);
          if (num2 > 0.0)
          {
            otri2.Copy(ref farright);
            break;
          }
          farleft.Lnext(ref farright);
          break;
        default:
          int num3 = num1 >> 1;
          this.DivconqRecurse(left, left + num3 - 1, 1 - axis, ref farleft, ref otri4);
          this.DivconqRecurse(left + num3, right, 1 - axis, ref otri5, ref farright);
          this.MergeHulls(ref farleft, ref otri4, ref otri5, ref farright, axis);
          break;
      }
    }

    private int RemoveGhosts(ref Otri startghost)
    {
      Otri o2_1 = new Otri();
      Otri o2_2 = new Otri();
      Otri o2_3 = new Otri();
      bool flag = !this.mesh.behavior.Poly;
      startghost.Lprev(ref o2_1);
      o2_1.SymSelf();
      Mesh.dummytri.neighbors[0] = o2_1;
      startghost.Copy(ref o2_2);
      int num = 0;
      do
      {
        ++num;
        o2_2.Lnext(ref o2_3);
        o2_2.LprevSelf();
        o2_2.SymSelf();
        if (flag && o2_2.triangle != Mesh.dummytri)
        {
          Vertex vertex = o2_2.Org();
          if (vertex.mark == 0)
            vertex.mark = 1;
        }
        o2_2.Dissolve();
        o2_3.Sym(ref o2_2);
        this.mesh.TriangleDealloc(o2_3.triangle);
      }
      while (!o2_2.Equal(startghost));
      return num;
    }

    public int Triangulate(Mesh m)
    {
      Otri otri = new Otri();
      Otri farright = new Otri();
      this.mesh = m;
      this.sortarray = new Vertex[m.invertices];
      int num1 = 0;
      foreach (Vertex vertex in m.vertices.Values)
        this.sortarray[num1++] = vertex;
      this.VertexSort(0, m.invertices - 1);
      int index1 = 0;
      for (int index2 = 1; index2 < m.invertices; ++index2)
      {
        if (this.sortarray[index1].x == this.sortarray[index2].x && this.sortarray[index1].y == this.sortarray[index2].y)
        {
          if (Behavior.Verbose)
            SimpleLog.Instance.Warning(string.Format("A duplicate vertex appeared and was ignored (ID {0}).", (object) this.sortarray[index2].hash), "DivConquer.DivconqDelaunay()");
          this.sortarray[index2].type = VertexType.UndeadVertex;
          ++m.undeads;
        }
        else
        {
          ++index1;
          this.sortarray[index1] = this.sortarray[index2];
        }
      }
      int num2 = index1 + 1;
      if (this.useDwyer)
      {
        int left = num2 >> 1;
        if (num2 - left >= 2)
        {
          if (left >= 2)
            this.AlternateAxes(0, left - 1, 1);
          this.AlternateAxes(left, num2 - 1, 1);
        }
      }
      this.DivconqRecurse(0, num2 - 1, 0, ref otri, ref farright);
      return this.RemoveGhosts(ref otri);
    }
  }
}
