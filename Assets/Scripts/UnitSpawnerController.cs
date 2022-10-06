using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnerController : MonoBehaviour
{
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private string spawnerTag;

    public void SpawnUnit()
    {
        Transform t = GameObject.FindGameObjectWithTag(spawnerTag).transform;
        Debug.Log(t.position + " t.position");
        Instantiate(unitPrefab, t.position, Quaternion.identity);
    }
}
