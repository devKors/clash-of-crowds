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
    private Material opponentMaterial;
    private Material playerMaterial;

    void Start()
    {
        isFirstSpawn = true;
        InvokeRepeating("SpanwStackableBoxes", 0.0f, waitBetweenSpawn);
    }

    private void SpanwStackableBoxes()
    {
        if (GameManager.Instance.state == GameState.Game) {
            foreach (Transform slot in transform)
            {
                if  (slot.childCount == 0)
                {
                    InstantiateRandomStackableBox(slot);
                }
            }

            isFirstSpawn = false;
        }
    }

    private void InstantiateRandomStackableBox(Transform slot)
    {

        if (isFirstSpawn)
        {
            int percent = UnityEngine.Random.Range(0, 100);

            if (percent <= percentToSpawnMy)
            {
             GameObject go = Instantiate(playerStackableBox, slot);
                if (playerMaterial != null)
                {
                    go.GetComponentInChildren<Renderer>().material = playerMaterial;
                }
            }
            else
            {
                GameObject go = Instantiate(opponentStackableBox, slot);
                if (opponentMaterial != null)
                {
                    go.GetComponentInChildren<Renderer>().material = opponentMaterial;
                }
            }
        }
        else
        {
            int playerBoxesCount = GameObject.FindGameObjectsWithTag(playerStackableBox.tag).Length;
            int opponentBoxesCount = GameObject.FindGameObjectsWithTag(opponentStackableBox.tag).Length;

            if ((playerBoxesCount - 8) <= opponentBoxesCount)
            {
                GameObject go = Instantiate(playerStackableBox, slot);
                if (playerMaterial != null)
                {
                    go.GetComponentInChildren<Renderer>().material = playerMaterial;
                }
            }
            else
            {
                GameObject go = Instantiate(opponentStackableBox, slot);
                if (opponentMaterial != null)
                {
                    go.GetComponentInChildren<Renderer>().material = opponentMaterial;
                }
            }
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

    public void SetPlayerMaterial(Material m)
    {
        this.playerMaterial = m;
    }

    public void SetOpponentMaterial(Material m)
    {
        this.opponentMaterial = m;
    }
}
