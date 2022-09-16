using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnerController : MonoBehaviour
{
    [SerializeField]
    private GameObject unitPrefab;

    public void SpawnUnit()
    {
        Instantiate(unitPrefab, transform.position, Quaternion.identity);
    }
}
