using System;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace RockTools
{
    [CustomEditor(typeof(RockGenerator))]
    public class RockGeneratorEditor : Editor
    {
        private const float CHANGE_SENSITIVITY = 0.0001f;
        private RockGenerator rockGen;
        private SerializedProperty pMaterial;
        private SerializedProperty pDensity;
        private SerializedProperty pRadius;
        private SerializedProperty pSymmetry;
        private SerializedProperty pWave;
        private SerializedProperty pDecentralize;
        private SerializedProperty pScaleLocal;
        private SerializedProperty pScaleRandom;
        private SerializedProperty pScaleByDistance;
        private SerializedProperty pTallness;
        private SerializedProperty pFlatness;
        private SerializedProperty pWideness;
        private SerializedProperty pRotation;
        private SerializedProperty pRotationLocal;
        private SerializedProperty pRotationRnd;
        private SerializedProperty prRotationRndRange;

        private Object tmpMaterial;
        private int tmpDensity;
        private float tmpRadius;
        private float tmpSymmetry;
        private float tmpWave;
        private float tmpDecentralize;
        private float tmpScaleLocal;
        private AnimationCurve tmpScaleByDistance;
        private float tmpTallness;
        private float tmpFlatness;
        private float tmpWideness;
        private float tmpRotation;
        private float tmpRotationLocal;
        private float tmpRotationZRnd;

        private void OnEnable()
        {
            rockGen = target as RockGenerator;

            pMaterial = serializedObject.FindProperty("material");
            pDensity = serializedObject.FindProperty("density");
            pRadius = serializedObject.FindProperty("radius");
            pSymmetry = serializedObject.FindProperty("asymmetry");
            pWave = serializedObject.FindProperty("wave");
            pDecentralize = serializedObject.FindProperty("decentralize");
            pScaleLocal = serializedObject.FindProperty("scaleLocal");
            pScaleByDistance = serializedObject.FindProperty("scaleByDistance");
            pTallness = serializedObject.FindProperty("tallness");
            pFlatness = serializedObject.FindProperty("flatness");
            pWideness = serializedObject.FindProperty("wideness");
            pRotation = serializedObject.FindProperty("rotation");
            pRotationLocal = serializedObject.FindProperty("rotationLocal");
            pRotationRnd = serializedObject.FindProperty("rotationRnd");

            SceneView.duringSceneGui += DuringSceneGui;

            tmpMaterial = pMaterial.objectReferenceValue;
            tmpDensity = pDensity.intValue;
            tmpRadius = pRadius.floatValue;
            tmpSymmetry = pSymmetry.floatValue;
            tmpWave = pWave.floatValue;
            tmpDecentralize = pDecentralize.floatValue;
            tmpScaleLocal = pScaleLocal.floatValue;
            tmpScaleByDistance = pScaleByDistance.animationCurveValue;
            tmpTallness = pTallness.floatValue;
            tmpFlatness = pFlatness.floatValue;
            tmpWideness = pWideness.floatValue;
            tmpRotation = pRotation.floatValue;
            tmpRotationLocal = pRotationLocal.floatValue;
            tmpRotationZRnd = pRotationRnd.floatValue;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= DuringSceneGui;
        }

        public override void OnInspectorGUI()
        {
            if (rockGen == null)
                return;

            if (GUILayout.Button("Randomize"))
                rockGen.Randomize();

            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Bake"))
            {
                PreBake(path => { rockGen.Bake(path); });
                GUIUtility.ExitGUI();
                return;
            }

            if (tmpMaterial != pMaterial.objectReferenceValue)
                rockGen.UpdateMaterials();

            else if (tmpDensity != pDensity.intValue)
                rockGen.UpdateDensities();
            else if (Math.Abs(tmpRadius - pRadius.floatValue) > CHANGE_SENSITIVITY)
                rockGen.UpdateRadius();
            else if (Math.Abs(tmpSymmetry - pSymmetry.floatValue) > CHANGE_SENSITIVITY)
            {
                rockGen.UpdateScales();
                rockGen.UpdateRotations();
            }
            else if (Math.Abs(tmpWave - pWave.floatValue) > CHANGE_SENSITIVITY)
                rockGen.UpdateScales();
            else if (Math.Abs(tmpDecentralize - pDecentralize.floatValue) > CHANGE_SENSITIVITY)
            {
                rockGen.UpdatePositions();
                rockGen.UpdateRotations();
            }

            else if (Math.Abs(tmpScaleLocal - pScaleLocal.floatValue) > CHANGE_SENSITIVITY)
                rockGen.UpdateScales();
            else if (!tmpScaleByDistance.Equals(pScaleByDistance.animationCurveValue))
                rockGen.UpdateScales();
            else if (Math.Abs(tmpTallness - pTallness.floatValue) > CHANGE_SENSITIVITY)
                rockGen.UpdateScales();
            else if (Math.Abs(tmpFlatness - pFlatness.floatValue) > CHANGE_SENSITIVITY)
                rockGen.UpdateScales();
            else if (Math.Abs(tmpWideness - pWideness.floatValue) > CHANGE_SENSITIVITY)
                rockGen.UpdateScales();

            else if (Math.Abs(tmpRotation - pRotation.floatValue) > CHANGE_SENSITIVITY)
                rockGen.UpdateRotations();
            else if (Math.Abs(tmpRotationLocal - pRotationLocal.floatValue) > CHANGE_SENSITIVITY)
                rockGen.UpdateRotations();
            else if (Math.Abs(tmpRotationZRnd - pRotationRnd.floatValue) > CHANGE_SENSITIVITY)
                rockGen.UpdateRotations();

            tmpMaterial = pMaterial.objectReferenceValue;
            tmpDensity = pDensity.intValue;
            tmpRadius = pRadius.floatValue;
            tmpSymmetry = pSymmetry.floatValue;
            tmpWave = pWave.floatValue;
            tmpDecentralize = pDecentralize.floatValue;
            tmpScaleLocal = pScaleLocal.floatValue;
            tmpScaleByDistance = pScaleByDistance.animationCurveValue;
            tmpTallness = pTallness.floatValue;
            tmpFlatness = pFlatness.floatValue;
            tmpWideness = pWideness.floatValue;
            tmpRotation = pRotation.floatValue;
            tmpRotationLocal = pRotationLocal.floatValue;
            tmpRotationZRnd = pRotationRnd.floatValue;
        }

        private void DuringSceneGui(SceneView obj)
        {
            if (rockGen != null)
                Handles.DrawWireDisc(rockGen.transform.position, Vector3.up, rockGen.radius);
        }

        private async void PreBake(Action<string> PreBakeDone)
        {
            var scenePath = SceneManager.GetActiveScene().path;

            // check if scene has a valid path
            if (string.IsNullOrEmpty(scenePath))
            {
                if (EditorUtility.DisplayDialog("The untitled scene needs saving",
                    "You need to save the scene before baking rock.", "Save Scene", "Cancel"))
                    scenePath = EditorUtility.SaveFilePanel("Save Scene", "Assets/", "", "unity");

                scenePath = FileUtil.GetProjectRelativePath(scenePath);

                if (string.IsNullOrEmpty(scenePath))
                {
                    Debug.LogWarning("Scene was not saved, bake canceled.");
                    return;
                }

                var saveOK = EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);

                if (!saveOK)
                {
                    Debug.LogWarning("Scene was not saved, bake canceled.");
                    return;
                }

                AssetDatabase.Refresh();
                await Task.Delay(100);
            }

            scenePath = SceneManager.GetActiveScene().path;
            if (string.IsNullOrEmpty(scenePath))
                return;

            var assetPath = $"{Path.ChangeExtension(scenePath, null)}-generated-mesh/baked-rock.asset";
            var assetDir = Path.GetDirectoryName(assetPath);

            if (string.IsNullOrEmpty(assetDir))
                return;

            if (!Directory.Exists(assetDir))
            {
                Directory.CreateDirectory(assetDir);
                AssetDatabase.Refresh();
                await Task.Delay(100);
            }

            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
            PreBakeDone.Invoke(assetPath);
        }
    }
}