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
    private GameObject[] unitSkins;
    private int unitSkinIndex;
    private Animator unitAnimator;


    void Awake()
    {
        unitSkins = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            unitSkins[i] = transform.GetChild(i).gameObject;
        }

        unitSkinIndex = 0;
        SetUnitSkin(unitSkinIndex);
    }
    void Start()
    {
        unit = GetComponent<NavMeshAgent>();
        enemyCastle = GameObject.FindGameObjectWithTag(enemyCastle.tag);
        unitAnimator = unitSkins[unitSkinIndex].GetComponent<Animator>();
        unit.speed = 3.0f;
        unitAnimator.SetBool("isMoving", true);
    }

    void Update()
    {
        if (GameManager.Instance.state == GameState.Game)
        {
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                unit.SetDestination(closestEnemy.transform.position);
            }
            else if (enemyCastle != null)
            {
                unit.SetDestination(enemyCastle.transform.position);
                // unit.SetDestination(new Vector3(enemyCastle.transform.position.x + Random.Range(-0.5f, 0.5f), enemyCastle.transform.position.y, enemyCastle.transform.position.z));
            }

            // CheckIsSpawning();
        }
        else
        {
            unit.isStopped = true;
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

    public void SetUnitSkin(int index)
    {
        foreach (GameObject go in unitSkins)
        {
            go.SetActive(false);
        }

        if (unitSkins.Length > 0)
        {
            unitSkins[index].SetActive(true);
            unitSkinIndex = index;
        }
    }

    private void CheckIsSpawning()
    {
        GameObject goAIController = GameObject.FindGameObjectWithTag("Opponent");

        if (goAIController != null)
        {
            OpponentAIController aiController = goAIController.GetComponent<OpponentAIController>();

            if (aiController != null && !aiController.isSpawning)
            {
                unit.speed = 3.0f;
                unitAnimator.SetBool("isMoving", true);
            }
        }
        
    }
}
