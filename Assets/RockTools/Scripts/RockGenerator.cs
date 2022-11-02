using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace RockTools
{
    public class RockGenerator : MonoBehaviour
    {
        #region Generator Settings

        private const int COUNT_MIN = 1;
        private const int COUNT_MAX = 150;
        private const float ROTATION_MIN = -45f;
        private const float ROTATION_MAX = 45;
        private const float MIN_SCALE_VISIBLE = 0.01f;

        public Material material;

        [Header("Distribution")] [Range(COUNT_MIN, COUNT_MAX)]
        public int density = 120;

        [Range(1f, 5f)] public float radius = 5f;
        [Range(-1f, 1f)] public float asymmetry;
        [Range(0f, 1f)] public float wave;
        [Range(0f, 1f)] public float decentralize = 0.5f;

        [Header("Scale"), Range(0f, 2f)] public float scaleLocal = 2f;

        public AnimationCurve scaleByDistance = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 2, 2));

        [Range(0f, 1f), Tooltip("Increase Height of the Rocks")]
        public float tallness = 0.6f;

        [Range(0f, 1f)] public float flatness;
        [Range(0f, 1f)] public float wideness;

        [Header("Rotation")] [Range(ROTATION_MIN, ROTATION_MAX)]
        public float rotation;

        [Range(0f, 359f)] public float rotationLocal;
        [Range(0f, 1f)] public float rotationRnd = 0.1f;

        #endregion

        [HideInInspector] public List<float> distances;
        [HideInInspector] public List<float> randomDs;
        [HideInInspector] public List<float> randomThetas;
        [HideInInspector] public List<float> randomScales;
        [HideInInspector] public List<Vector3> randomRotations;
        [HideInInspector] public List<bool> activeByDensity;
        [HideInInspector] public List<bool> activeByScale;

        private float largestScale = 2f;
        [HideInInspector] public Transform root;

        private void Reset()
        {
            // create the rocks root
            root = new GameObject("root").transform;
            root.SetParent(transform);
            root.localPosition = Vector3.zero;
            // create the plane
            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.SetParent(transform);
            plane.transform.localPosition = Vector3.zero;
            // get default material
            var diffuse = plane.GetComponent<MeshRenderer>().sharedMaterial;
            material = diffuse;
            Randomize();
        }

        public void Clear()
        {
            root.ClearChildrenEditor();

            distances = new List<float>(COUNT_MAX);
            randomDs = new List<float>(COUNT_MAX);
            randomThetas = new List<float>(COUNT_MAX);
            randomScales = new List<float>(COUNT_MAX);
            randomRotations = new List<Vector3>(COUNT_MAX);
            activeByDensity = new List<bool>(COUNT_MAX);
            activeByScale = new List<bool>(COUNT_MAX);
        }

        public void Randomize()
        {
            Prepare();
            UpdatePositions();
            UpdateRotations();
            UpdateDensities();
            UpdateMaterials();
        }

        private void Prepare()
        {
            Clear();

            for (var i = 0; i < COUNT_MAX; i++)
            {
                var go = new GameObject($"Rock-{i:000}");
                go.AddComponent<MeshRenderer>();
                var mesh = RockBinary.GetRandomMesh();
                go.AddComponent<MeshFilter>().sharedMesh = mesh;
                var meshCollider = go.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = mesh;
                meshCollider.convex = true;
                go.transform.position = root.position;
                go.transform.SetParent(root);
                distances.Add(0f);
                randomDs.Add(Random.value);
                randomThetas.Add(Random.value);
                randomScales.Add(Random.Range(-1f, 1f));
                randomRotations.Add(new Vector3(Random.value, Random.value, Random.value));
                activeByDensity.Add(true);
                activeByScale.Add(true);
            }
        }

        public void UpdatePositions()
        {
            for (var i = 0; i < COUNT_MAX; i++)
            {
                var pow = Mathf.Lerp(0.66f, 0.25f, decentralize);
                var d = Mathf.Pow(randomDs[i], pow);
                var r = d * radius;
                var theta = randomThetas[i] * 2 * Mathf.PI;
                var localPos = new Vector3(r * Mathf.Cos(theta), 0, r * Mathf.Sin(theta));
                root.GetChild(i).localPosition = localPos;
                distances[i] = d;
            }

            UpdateScales();
        }

        public void UpdateRotations()
        {
            for (var i = 0; i < COUNT_MAX; i++)
            {
                var child = root.GetChild(i);
                var rot = new Vector3(0f, 0f, rotation);

                if (rotationRnd > 0f)
                {
                    var localCenter = new Vector3(asymmetry * radius, 0f, 0f);
                    var distance = (child.localPosition - localCenter).magnitude;
                    distance /= radius;

                    if (distance > 0.5f && rotationRnd > 0f)
                    {
                        var rndRot = rotationRnd * 360 * distance;
                        rot.x += randomRotations[i].x * rndRot;
                        rot.y += randomRotations[i].y * rndRot;
                        rot.z += randomRotations[i].z * rndRot;
                    }
                }

                child.rotation = Quaternion.Euler(rot + root.rotation.eulerAngles);
                child.RotateAround(child.position, child.up, rotationLocal);
            }
        }

        public void UpdateScales()
        {
            var tmpLargestScale = 0f;
            for (var i = 0; i < COUNT_MAX; i++)
            {
                var child = root.GetChild(i);

                var localCenter = new Vector3(asymmetry * radius, 0f, 0f);
                var distance = (child.localPosition - localCenter).magnitude;
                distance /= radius;
                var distCurve = scaleByDistance.Evaluate(1 - distance);
                var distCurveReverse = scaleByDistance.Evaluate(distance);
                var localScale = distCurve;
                localScale *= scaleLocal;

                var waveFactor = 0f;
                if (distance > 0.3f)
                {
                    var wavePosition = Mathf.Lerp(radius, -radius, wave);
                    var waveDist = Mathf.Abs(child.localPosition.x - wavePosition);
                    waveFactor = Mathf.InverseLerp(radius / 4f, 0, waveDist) * (distCurveReverse * 4);
                }

                localScale += localScale * waveFactor;

                if (localScale > tmpLargestScale)
                    tmpLargestScale = localScale;

                var _tallness = Mathf.Lerp(1f, 3f, distCurve * tallness);
                var _flatness = Mathf.Lerp(0f, 3f, distCurveReverse * flatness);
                var _wideness = Mathf.Lerp(0f, 3f, distCurveReverse * wideness);
                var height = Mathf.Clamp(_tallness - _flatness, 0.1f, 4f);
                var finalScale = new Vector3(1 + _wideness, height, 1 + _wideness) * localScale;
                child.localScale = finalScale;
                activeByScale[i] = localScale > MIN_SCALE_VISIBLE * largestScale;
                child.gameObject.SetActive(activeByScale[i] && activeByDensity[i]);
            }

            largestScale = tmpLargestScale;
        }

        public void UpdateRadius()
        {
            for (var i = 0; i < COUNT_MAX; i++)
            {
                var child = root.GetChild(i);
                var direction = child.localPosition.normalized;
                var newPos = direction * (distances[i] * radius);
                child.localPosition = newPos;
            }
        }

        public void UpdateDensities()
        {
            for (var i = 0; i < COUNT_MAX; i++)
            {
                activeByDensity[i] = i < density;
                root.GetChild(i).gameObject.SetActive(activeByScale[i] && activeByDensity[i]);
            }
        }

        public void UpdateMaterials()
        {
            var meshRenderers = root.GetComponentsInChildren<MeshRenderer>(true);
            for (var i = 0; i < meshRenderers.Length; i++)
                meshRenderers[i].sharedMaterial = material;
        }

        public void Bake(string path)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayProgressBar("Processing", "Please wait while meshes are being baked", 0f);
            try
            {
#endif
                var combined = new GameObject("Baked-Rock").AddComponent<MeshFilter>();
                var meshRenderer = combined.gameObject.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = root.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial;
                RockBaker.Bake(root, combined, path);
#if UNITY_EDITOR
                Selection.activeObject = combined.gameObject;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
#endif
        }
    }
}