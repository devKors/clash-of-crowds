using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace RockTools
{
    public static class RockBinary
    {
        private static List<Mesh> PredefinedMeshes;

        static RockBinary()
        {
            Initialize();
        }

        public static Mesh GetRandomMesh()
        {
            if (PredefinedMeshes == null)
            {
                Initialize();

                if (PredefinedMeshes == null)
                    return null;
            }

            var selected = SelectRandomMesh();

            if (selected == null)
            {
                Initialize();
                selected = SelectRandomMesh();

                if (selected == null)
                    return null;
            }

            return Object.Instantiate(selected);
        }

        private static Mesh SelectRandomMesh()
        {
            var rnd = Random.Range(0, PredefinedMeshes.Count);
            var rndItem = PredefinedMeshes[rnd];
            return rndItem;
        }

        private static void Initialize()
        {
            try
            {
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
                var asset = Resources.Load<TextAsset>("RockBinary");
                var bf = new BinaryFormatter();
                Stream stream = new MemoryStream(asset.bytes);
                PredefinedMeshes = ((List<sMesh>) bf.Deserialize(stream)).Select(x => (Mesh) x).ToList();
                stream.Close();
            }
            catch
            {
                Debug.LogError(
                    "RockBinary file not found! [RockBinary.bytes] should exists inside [Resources] folder");
            }
        }

        [Serializable]
        public class sMesh
        {
            private sVector3[] vertices;
            private int[] triangles;
            private sColor[] colors;
            private sVector3[] normals;

            public static implicit operator Mesh(sMesh data)
            {
                return new Mesh()
                {
                    vertices = data.vertices.Select(x => (Vector3) x).ToArray(),
                    normals = data.normals.Select(x => (Vector3) x).ToArray(),
                    colors = data.colors.Select(x => (Color) x).ToArray(),
                    triangles = data.triangles,
                };
            }

            public static implicit operator sMesh(Mesh mesh)
            {
                return new sMesh()
                {
                    vertices = mesh.vertices.Select(x => (sVector3) x).ToArray(),
                    normals = mesh.normals.Select(x => (sVector3) x).ToArray(),
                    colors = mesh.colors.Select(x => (sColor) x).ToArray(),
                    triangles = mesh.triangles,
                };
            }
        }

        [Serializable]
        public struct sVector3
        {
            public float x;
            public float y;
            public float z;

            public sVector3(float rX, float rY, float rZ)
            {
                x = rX;
                y = rY;
                z = rZ;
            }

            public static implicit operator Vector3(sVector3 value)
            {
                return new Vector3(value.x, value.y, value.z);
            }

            public static implicit operator sVector3(Vector3 value)
            {
                return new sVector3(value.x, value.y, value.z);
            }
        }

        [Serializable]
        public struct sColor
        {
            public float r;
            public float g;
            public float b;
            public float a;

            public sColor(float rR, float rG, float rB, float rA)
            {
                r = rR;
                g = rG;
                b = rB;
                a = rA;
            }

            public static implicit operator Color(sColor value)
            {
                return new Color(value.r, value.g, value.b, value.a);
            }

            public static implicit operator sColor(Color value)
            {
                return new sColor(value.r, value.g, value.b, value.a);
            }
        }
    }
}