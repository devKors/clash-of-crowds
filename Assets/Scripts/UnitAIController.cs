using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAIController : MonoBehaviour
{
    private NavMeshAgent unit;
    [SerializeField]
    private GameObject enemyCastle;
    [SerializeField]
    private GameObject enemyUnit;

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
        else if (enemyCastle != null)
        {
            unit.SetDestination(enemyCastle.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag(enemyUnit.tag))
         {
            if (other != null && gameObject != null)
            {
                Destroy(other);            
                Destroy(gameObject);
            }
         }
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag(enemyUnit.tag);
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
