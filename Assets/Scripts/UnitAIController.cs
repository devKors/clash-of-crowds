using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAIController : MonoBehaviour
{
    private NavMeshAgent unit;
    private GameObject enemyCastle;

    [SerializeField]
    private string enemyUnitTag;
    [SerializeField]
    private string enemyCastleTag;

    void Start()
    {
        unit = GetComponent<NavMeshAgent>();
        enemyCastle = GameObject.FindWithTag(enemyCastleTag);
    }

    void Update()
    {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            unit.SetDestination(closestEnemy.transform.position);
        }
        else if (enemyCastle != null)
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
        //  else if (other.CompareTag(enemyCastleTag))
        //  {
        //     if (gameObject != null)
        //     {
        //         Destroy(gameObject);
        //     }
        //  }
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag(enemyUnitTag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = enemy;
                distance = curDistance;
            }
        }

        return closest;
    }
}
