using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentAIController : MonoBehaviour
{
    private NavMeshAgent opponent;
    // private int bringItems;
    GameObject closestItem;
    private Animator opponentAnimator;

    void Start()
    {
        opponent = GetComponent<NavMeshAgent>();
        opponentAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        closestItem = FindClosestItem();

        if (closestItem != null)
        {
            opponent.SetDestination(closestItem.transform.position);
        }

        AnimateOpponentMovement();

    }

    public GameObject FindClosestItem()
    {
        GameObject[] items;
        items = GameObject.FindGameObjectsWithTag("BlueStackableBox");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject item in items)
        {
            bool isStacked = item.GetComponent<StackableItem>().isStacked;

            if (isStacked)
            {
                continue;
            }

            Vector3 diff = item.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = item;
                distance = curDistance;
            }
        }

        return closest;
    }

    private void AnimateOpponentMovement()
    {
        float velocity = opponent.velocity.magnitude;

        if (velocity > 0)
        {
            opponentAnimator.SetBool("isMoving", true);
        }
        else 
        {
            opponentAnimator.SetBool("isMoving", false);
        }
    }
}
