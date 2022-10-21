using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentAIController : MonoBehaviour
{
    private GameObject withdrawItems;
    [SerializeField]
    private int minBringItems;
    [SerializeField]
    private int maxBringItems;
    [SerializeField]
    private GameObject stackableBox;
    private NavMeshAgent opponent;
    private GameObject closestItem;
    private StackItems stackItemsController;
    private int bringItems = 0;
    public bool isSpawning = false;
    private Animator opponentAnimator;
    private GameObject[] opponents;
    private int opponentIndex;


    void Awake()
    {
        opponents = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            opponents[i] = transform.GetChild(i).gameObject;
        }

        opponentIndex = 1;
        SetOpponentSkin(opponentIndex);
    }

    void Start()
    {
        opponent = GetComponent<NavMeshAgent>();
        opponentAnimator = opponents[opponentIndex].GetComponent<Animator>();

        stackItemsController = GetComponent<StackItems>();

        bringItems = GetRandomBringItems();
        withdrawItems = GameObject.FindGameObjectWithTag("RedWithdrawItems");
    }

    void Update()
    {
        if (GameManager.Instance.state == GameState.Game)
        {
            HandleOpponentDestination();
            AnimateOpponentMovement();
        }
        else if (GameManager.Instance.state == GameState.Lose || GameManager.Instance.state == GameState.Victory)
        {
            opponent.isStopped = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (withdrawItems != null && other.CompareTag(withdrawItems.tag))
        {
            bringItems = GetRandomBringItems();
            isSpawning = true;
        }
    }

    private void HandleOpponentDestination()
    {
        closestItem = FindClosestItem();

        if (withdrawItems == null)
        {
            withdrawItems = GameObject.FindGameObjectWithTag("RedWithdrawItems");
        }

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
        items = GameObject.FindGameObjectsWithTag(stackableBox.tag);
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
        return Random.Range(minBringItems, maxBringItems);
    }

    public void SetOpponentSkin(int index)
    {
        foreach (GameObject go in opponents)
        {
            go.SetActive(false);
        }

        if (opponents.Length > 0)
        {
            opponents[index].SetActive(true);
            opponentIndex = index;
        }

    }
}
