using UnityEngine;
public class CrowdController : MonoBehaviour
{
    public bool isForming = false;
    [SerializeField]
    private GameObject enemyCrowd;
    [SerializeField]
    private GameObject enemyCastle;
    [SerializeField]
    private GameObject withdrawItemsGO;
    private WithdrawItems withdrawItems;
    public bool canRun = false;

    
    void Awake()
    {
        enemyCastle = GameObject.FindGameObjectWithTag(enemyCastle.tag);
        withdrawItemsGO = GameObject.FindGameObjectWithTag(withdrawItemsGO.tag);
        withdrawItems = withdrawItemsGO.GetComponent<WithdrawItems>();
    }

    void FixedUpdate()
    {
        if (!withdrawItems.isSpawning)
        {
            canRun = true;
        }

        if (canRun)
        {
            GameObject closestCrowd = FindClosestCrowd();

            if (closestCrowd != null)
            {
                MoveToPoint(closestCrowd);
            }
            else
            {
                MoveToPoint(enemyCastle);
            }

            if (isForming)
            {
                FormatCrowd();
            }
        }
        else
        {
            FormatCrowd();
        }
    }

    private void MoveToPoint(GameObject point)
    {
        transform.position = Vector3.MoveTowards
            (
                transform.position,
                new Vector3(point.transform.position.x, transform.position.y, point.transform.position.z),
                2f * Time.fixedDeltaTime
            );
    }

    public GameObject FindClosestCrowd()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag(enemyCrowd.tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {   
            if (enemy.transform.childCount == 0)
            {
                continue;
            }

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

    private void FormatCrowd()
    {
        foreach (Transform child in transform)
        {
            child.position = Vector3.MoveTowards(child.position, transform.position, Time.fixedDeltaTime * 2f);
        }
    }
}
