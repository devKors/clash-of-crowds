using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentAIController : MonoBehaviour
{
    private NavMeshAgent opponent;
    private GameObject closestItem;
    [SerializeField]
    private GameObject withdrawItems;
    private Animator opponentAnimator;
    private string itemTag = "BlueStackableBox";
    private string withdrawItemsTag = "BlueWithdrawItems";
    private StackItems stackItemsController;
    private int bringItems = 0;
    private bool isSpawning = false;

    void Start()
    {
        opponent = GetComponent<NavMeshAgent>();
        opponentAnimator = GetComponent<Animator>();

        stackItemsController = GetComponent<StackItems>();

        bringItems = GetRandomBringItems();
    }

    void Update()
    {
        HandleOpponentDestination();
        AnimateOpponentMovement();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(withdrawItemsTag))
        {
            bringItems = GetRandomBringItems();
            isSpawning = true;
        }
    }

    private void HandleOpponentDestination()
    {
        closestItem = FindClosestItem();

        if (stackItemsController.numOfItems == 0) { isSpawning = false; }

        if ((stackItemsController.numOfItems >= bringItems) || (stackItemsController.numOfItems > 0 && closestItem == null)) {
            opponent.SetDestination(withdrawItems.transform.position);
        }
        else if (closestItem != null && !isSpawning)
        {
            opponent.SetDestination(closestItem.transform.position);
        }
    }

    private GameObject FindClosestItem()
    {
        GameObject[] items;
        items = GameObject.FindGameObjectsWithTag(itemTag);
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

        if (velocity > 0.2f)
        {
            opponentAnimator.SetBool("isMoving", true);
        }
        else 
        {
            opponentAnimator.SetBool("isMoving", false);
        }
    }

    private int GetRandomBringItems()
    {
        return Random.Range(11, 13);
    }
}