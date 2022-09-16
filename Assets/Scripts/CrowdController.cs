using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    private NavMeshAgent unit;
    [SerializeField]
    private GameObject enemyCastle;
    [SerializeField]
    private string enemyUnitTag;
    [SerializeField]
    private string enemyCastleTag;

    void Start()
    {
        unit = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            unit.SetDestination(closestEnemy.transform.position);
        }
        else
        {
            unit.SetDestination(enemyCastle.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag(enemyUnitTag))
         {
            if (other != null && gameObject != null)
            {
                Destroy(other);            
                Destroy(gameObject);
            }
         }
         else if (other.CompareTag(enemyCastleTag))
         {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
         }
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(enemyUnitTag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        return closest;
    }


}
