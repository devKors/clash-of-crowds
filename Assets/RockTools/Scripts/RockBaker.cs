using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace RockTools
{
    public static class RockBaker
    {
        public static async void Bake(Transform root, MeshFilter meshFilter, string path = "",
            float optimizePrecision = 0.1f, bool optimize = true)
        {
            // cache current position
            var tmpPos = root.position;

            // move all to Vector3.zero
            meshFilter.transform.position = Vector3.zero;
            root.position = Vector3.zero;

            // combine all parts
            var combined = CombineAllMeshes(root);

            // reset position
            root.position = tmpPos;
            meshFilter.transform.position = tmpPos;

            // split combined mesh
            combined = MeshSplitter.Split(combined, new Plane(Vector3.up, Vector3.zero));

            // clean inside polys
            ResizeChildren(root, 99 / 100f);
            await Task.Delay(100);
            combined = CleanInsidePolygons(root, combined);
            ResizeChildren(root, 100f / 99f);

            if (optimize)
            {
#if LOG_OPTIMIZATONS
                var vBefore = combined.vertices.Length;
                var tBefore = combined.triangles.Length;
#endif

                combined = MeshCleanup(combined, optimizePrecision);
                combined = SeparateTriangles(combined);

#if LOG_OPTIMIZATONS
                var vertsAfter = combined.vertices.Length;
                var trisAfter = combined.triangles.Length;

                var vOpt = vBefore - vertsAfter;
                var tOpt = tBefore - trisAfter;

                var vOptP = (int) (vOpt / (float) vBefore * 100);
                var tOptP = (int) (tOpt / (float) tBefore * 100);

                Debug.Log($"vertices optimized: {vOpt}({vOptP}%), tris optimized: {tOpt}({tOptP}%)");
#endif
            }

            combined.Optimize();
            combined.RecalculateNormals();
#if UNITY_EDITOR
            Unwrapping.GenerateSecondaryUVSet(combined);
#endif
            combined.uv = combined.uv2;
            combined.uv2 = null;
            combined.name = meshFilter.gameObject.name;
            meshFilter.sharedMesh = combined;

            // save mesh as an asset
            SaveMesh(meshFilter, path);
        }

        private static Mesh CombineAllMeshes(Transform root)
        {
            var meshFilters = new List<MeshFilter>(root.childCount);
            for (var i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    var inChildren = child.GetComponentsInChildren<MeshFilter>(false);
                    foreach (var mf in inChildren)
                    {
                        if (mf.sharedMesh != null)
                            meshFilters.Add(mf);
                    }
                }
            }

            var combine = new CombineInstance[meshFilters.Count];

            for (var i = 0; i < meshFilters.Count; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }

            var mesh = new Mesh();
            mesh.CombineMeshes(combine);
            return mesh;
        }

        private static Mesh CleanInsidePolygons(Transform root, Mesh originalMesh)
        {
            var vertices = originalMesh.vertices;
            var triangles = new List<int>(originalMesh.triangles);

            var colliders = root.GetComponentsInChildren<Collider>(false).Where(x => x.gameObject.activeSelf).ToArray();

            var rootPosition = root.position;

            var count = triangles.Count / 3;
            for (var i = count - 1; i >= 0; i--)
            {
                var v1 = vertices[triangles[i * 3]] + rootPosition;
                var v2 = vertices[triangles[i * 3 + 1]] + rootPosition;
                var v3 = vertices[triangles[i * 3 + 2]] + rootPosition;

                if (CollisionCheck(colliders, v1) && CollisionCheck(colliders, v2) && CollisionCheck(colliders, v3))
                    triangles.RemoveRange(i * 3, 3);
            }

            return new Mesh
            {
                vertices = vertices,
                triangles = triangles.ToArray(),
                colors = originalMesh.colors
            };
        }

        private static bool CollisionCheck(Collider[] colliders, Vector3 point)
        {
            foreach (var collider in colliders)
            {
                if (collider.ClosestPoint(point) == point)
                    return true;
            }

            return false;
        }

        private static void ResizeChildren(Transform root, float factor)
        {
            for (var i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                if (child.gameObject.activeSelf)
                    child.localScale *= factor;
            }
        }

        private static void SaveMesh(MeshFilter meshFilter, string path)
        {
#if UNITY_EDITOR
            var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing != null)
            {
                
                existing.Clear();
                existing.SetVertices(meshFilter.sharedMesh.vertices);
                existing.SetTriangles(meshFilter.sharedMesh.triangles, 0);
                existing.SetColors(meshFilter.sharedMesh.colors);
                existing.SetNormals(meshFilter.sharedMesh.normals);
            }
            else
                AssetDatabase.CreateAsset(meshFilter.sharedMesh, path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        private static Mesh MeshCleanup(Mesh mesh, float precision = 0.001f)
        {
            var sqrPrecision = precision * precision;

            var mesh_triangles = new List<int>(mesh.triangles);
            var mesh_vertices = new List<Vector3>(mesh.vertices);
            var mesh_colors = new List<Color>(mesh.colors);

            var triangles = new List<int>(mesh_triangles.Count);
            var vertices = new List<Vector3>(mesh_vertices.Count);
            var colors = new List<Color>(mesh_colors.Count);

            // dictionary to store triangles hashes and find triangle duplicates
            var hashList = new List<string>(triangles.Count);

            int vIndex;
            for (var i = 0; i < mesh_triangles.Count; i += 3)
            {
                var tmp_tris = new int [3];
                for (var j = 0; j < 3; j++)
                {
                    vIndex = mesh_triangles[j + i];
                    var foundIndex =
                        vertices.FindIndex(x => Mathf.Abs((x - mesh_vertices[vIndex]).sqrMagnitude) < sqrPrecision);
                    if (foundIndex == -1)
                    {
                        vertices.Add(mesh_vertices[vIndex]);
                        colors.Add(mesh_colors[vIndex]);
                        tmp_tris[j] = vertices.Count - 1;
                    }
                    else
                        tmp_tris[j] = foundIndex;
                }

                var i1 = tmp_tris[0];
                var i2 = tmp_tris[1];
                var i3 = tmp_tris[2];

                var v1 = vertices[i1];
                var v2 = vertices[i2];
                var v3 = vertices[i3];

                // add if triangle doesn't have two duplicate points
                if (v1 != v2 && v1 != v3 && v2 != v3)
                {
                    var hash = $"{i1}-{i2}-{i3}/{i2}-{i3}-{i1}/{i3}-{i1}-{i2}";
                    if (!hashList.Contains(hash))
                    {
                        triangles.AddRange(tmp_tris);
                        hashList.Add(hash);
                    }
                }
            }

            return new Mesh
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray(),
                colors = colors.ToArray(),
            };
        }

        private static Mesh SeparateTriangles(Mesh mesh)
        {
            var mesh_triangles = new List<int>(mesh.triangles);
            var mesh_vertices = new List<Vector3>(mesh.vertices);
            var mesh_colors = new List<Color>(mesh.colors);

            var triangles = new List<int>(mesh_triangles.Count);
            var vertices = new List<Vector3>(mesh_vertices.Count);
            var colors = new List<Color>(mesh_colors.Count);

            for (var i = 0; i < mesh_triangles.Count; i += 3)
            {
                for (var j = 0; j < 3; j++)
                {
                    var vIndex = mesh_triangles[j + i];

                    vertices.Add(mesh_vertices[vIndex]);
                    colors.Add(mesh_colors[vIndex]);
                    triangles.Add(vertices.Count - 1);
                }
            }

            return new Mesh
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray(),
                colors = colors.ToArray(),
            };
        }
    }
}