using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject crowdContainer;
    [SerializeField]
    private GameObject unitContainer;
    [SerializeField]
    private string spawnerTag;
    private GameObject currentCrowd;

    
    public void InstantiateCrowd()
    {
        Transform t = GameObject.FindGameObjectWithTag(spawnerTag).transform;
        currentCrowd = Instantiate(crowdContainer, t.position, t.rotation);
    }

    public void InstantiateUnitToCrowd()
    {
        Vector3 randomPosition = Random.insideUnitSphere * 0.1f;
        Vector3 unitPosition = new Vector3(currentCrowd.transform.position.x + randomPosition.x, currentCrowd.transform.position.y, currentCrowd.transform.position.z +  + randomPosition.z);
        Instantiate(unitContainer, unitPosition, Quaternion.identity, currentCrowd.transform);
    }
}
