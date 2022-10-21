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
        // float randX = Random.Range(-0.5f, 0.5f);
        // Transform t = GameObject.FindGameObjectWithTag(spawnerTag).transform;
        // Instantiate(unitPrefab, new Vector3(t.position.x + randX, t.position.y, t.position.z), t.rotation);

        Transform t = GameObject.FindGameObjectWithTag(spawnerTag).transform;
        Instantiate(unitPrefab, t.position, t.rotation);
    }
}
