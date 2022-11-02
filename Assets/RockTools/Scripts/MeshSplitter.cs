using System.Collections.Generic;
using UnityEngine;

namespace RockTools
{
    public static class MeshSplitter
    {
        public static Mesh Split(Mesh mesh, Plane plane)
        {
            var meshTriangles = mesh.triangles;
            var meshVerts = mesh.vertices;
            var meshColors = mesh.colors;

            var newTris = new List<int>();
            var newVerts = new List<Vector3>();
            var newCols = new List<Color>();
            for (var i = 0; i < meshTriangles.Length; i += 3)
            {
                var p1 = meshVerts[i];
                var p1Side = plane.GetSide(p1);

                var p2 = meshVerts[i + 1];
                var p2Side = plane.GetSide(p2);

                var p3 = meshVerts[i + 2];
                var p3Side = plane.GetSide(p3);

                var c1 = meshColors[i];
                var c2 = meshColors[i + 1];
                var c3 = meshColors[i + 2];

                // all points are on the same side
                if (p1Side == p2Side && p1Side == p3Side)
                {
                    if (p1Side)
                        AddVertices(p1, p2, p3, ref newVerts, ref newTris, c1, c2, c3, ref newCols);
                }
                else
                {
                    //we need the two points where the plane intersects the triangle.
                    Vector3 i1;
                    Vector3 i2;

                    Color i1c;
                    Color i2c;

                    //point 1 and 2 are on the same side
                    if (p1Side == p2Side)
                    {
                        //Cast a ray from p2 to p3 and from p3 to p1 to get the intersections                       
                        i1 = GetPointOfIntersection(p2, p3, plane, c2, c3, out i1c);
                        i2 = GetPointOfIntersection(p3, p1, plane, c3, c1, out i2c);

                        if (p1Side)
                        {
                            AddVertices(p1, p2, i1, ref newVerts, ref newTris, c1, c2, i1c, ref newCols);
                            AddVertices(p1, i1, i2, ref newVerts, ref newTris, c1, i1c, i2c, ref newCols);
                        }
                        else
                            AddVertices(i1, p3, i2, ref newVerts, ref newTris, i1c, c3, i2c, ref newCols);
                    }
                    //point 1 and 3 are on the same side
                    else if (p1Side == p3Side)
                    {
                        //Cast a ray from p1 to p2 and from p2 to p3 to get the intersections                       
                        i1 = GetPointOfIntersection(p1, p2, plane, c1, c2, out i1c);
                        i2 = GetPointOfIntersection(p2, p3, plane, c2, c3, out i2c);

                        if (p1Side)
                        {
                            AddVertices(p1, i1, p3, ref newVerts, ref newTris, c1, i1c, c3, ref newCols);
                            AddVertices(i1, i2, p3, ref newVerts, ref newTris, i1c, i2c, c3, ref newCols);
                        }
                        else
                            AddVertices(i1, p2, i2, ref newVerts, ref newTris, i1c, c2, i2c, ref newCols);
                    }
                    //p1 is alone
                    else
                    {
                        //Cast a ray from p1 to p2 and from p1 to p3 to get the intersections                       
                        i1 = GetPointOfIntersection(p1, p2, plane, c1, c2, out i1c);
                        i2 = GetPointOfIntersection(p1, p3, plane, c1, c3, out i2c);

                        if (p1Side)
                            AddVertices(p1, i1, i2, ref newVerts, ref newTris, c1, i1c, i2c, ref newCols);
                        else
                        {
                            AddVertices(i1, p2, p3, ref newVerts, ref newTris, i1c, c2, c3, ref newCols);
                            AddVertices(i1, p3, i2, ref newVerts, ref newTris, i1c, c3, i2c, ref newCols);
                        }
                    }
                }
            }

            return new Mesh
            {
                vertices = newVerts.ToArray(),
                triangles = newTris.ToArray(),
                colors = newCols.ToArray()
            };
        }

        private static Vector3 GetPointOfIntersection(Vector3 p1, Vector3 p2, Plane plane, Color a, Color b,
            out Color c)
        {
            var dist_p1_p2 = p2 - p1;
            var ray = new Ray(p1, dist_p1_p2);
            plane.Raycast(ray, out var distance);
            c = Color.Lerp(a, b, distance / dist_p1_p2.magnitude);
            return ray.GetPoint(distance);
        }

        private static void AddVertices(Vector3 p1, Vector3 p2, Vector3 p3, ref List<Vector3> vertices,
            ref List<int> triangles, Color c1, Color c2, Color c3, ref List<Color> colors)
        {
            var vCount = vertices.Count;

            vertices.Add(p1);
            vertices.Add(p2);
            vertices.Add(p3);

            colors.Add(c1);
            colors.Add(c2);
            colors.Add(c3);

            for (var i = 0; i < 3; i++)
                triangles.Add(vCount + i);
        }
    }
}