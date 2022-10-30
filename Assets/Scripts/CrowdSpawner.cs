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
        Transform t = GameObject.FindWithTag(spawnerTag).transform;
        currentCrowd = Instantiate(crowdContainer, t.position, Quaternion.identity);
    }

    public void InstantiateUnitToCrowd(Material material)
    {
        Vector3 randomPosition = Random.insideUnitSphere * 0.1f;
        Vector3 unitPosition = new Vector3(currentCrowd.transform.position.x + randomPosition.x, currentCrowd.transform.position.y, currentCrowd.transform.position.z +  + randomPosition.z);
        GameObject unit = Instantiate(unitContainer, unitPosition, Quaternion.identity, currentCrowd.transform);
        
        if (material != null)
        {
            unit.GetComponentInChildren<Renderer>().material = material;
        }
    }
}
