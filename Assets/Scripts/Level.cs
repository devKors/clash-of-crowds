using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public GameObject map;
    public Material mapPlatformMaterial;
    public Material mapStandMaterial;
    public GameObject decoration;
    public Material decorationBuildingsMaterial;
    public Material decorationFloorMaterial;
    public Color fogColor;
    public GameObject stackableBoxSpawners;
}