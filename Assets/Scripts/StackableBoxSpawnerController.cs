using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StackableBoxSpawnerController : MonoBehaviour
{
    private List<Transform> spawnSlots;
    [SerializeField]
    private GameObject playerStackableBox;
    [SerializeField]
    private GameObject opponentStackableBox;
    [SerializeField]
    private float waitBetweenSpawn;
    [SerializeField]
    private int percentToSpawnMy;
    [SerializeField]
    private float minSpawnTime;
    [SerializeField]
    private float maxSpawnTime;
    private bool isFirstSpawn;

    void Start()
    {
        isFirstSpawn = true;
        InvokeRepeating("SpanwStackableBoxes", 0.0f, waitBetweenSpawn);
    }

    private void SpanwStackableBoxes()
    {
        foreach (Transform slot in transform)
        {
            if  (slot.childCount == 0)
            {
                InstantiateRandomStackableBox(slot);
            }
        }

        isFirstSpawn = false;
    }

    private void InstantiateRandomStackableBox(Transform slot)
    {
        int percent = UnityEngine.Random.Range(0, 100);

        if (percent <= percentToSpawnMy)
        {
             Instantiate(playerStackableBox, slot);
        }
        else
        {
             Instantiate(opponentStackableBox, slot);
        }

        StartCoroutine(SetActiveStackableBox(slot));
    }

    IEnumerator SetActiveStackableBox(Transform slot)
    {

        if (isFirstSpawn)
        {
            yield return new WaitForSeconds(0.0f);    
        } 
        else
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnTime, maxSpawnTime));
        }

        slot.GetChild(0).gameObject.SetActive(true);
    }
}
