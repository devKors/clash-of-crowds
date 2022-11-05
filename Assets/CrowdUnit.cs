using System.Collections;
using UnityEngine;

public class CrowdUnit : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyUnit;
    public GameObject particle;

    void Start()
    {
        Material m = transform.GetComponentInChildren<Renderer>().material;
        ParticleSystem.MainModule settings = particle.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(m.color);
    }

    void Update()
    {
        Transform child = transform.GetChild(0);
        if (transform.tag == "PlayerUnit")
        {
            child.LookAt(new Vector3(0, 0, 10));
        }
        else
        {
            child.LookAt(new Vector3(0, 0, -10));
        }
    }
    void OnCollisionEnter(Collision collision)
    {
         if (collision.gameObject.CompareTag(enemyUnit.tag))
         {
            if (gameObject != null && collision.gameObject != null && gameObject.activeSelf && collision.gameObject.activeSelf)
            {
                GameObject crowdGO = collision.gameObject.transform.parent.gameObject;
                CrowdController crowdController = crowdGO.GetComponent<CrowdController>();

                IEnumerator formingCoroutine = SetIsForming(crowdController);
                StartCoroutine(formingCoroutine);
  
                gameObject.SetActive(false);
                collision.gameObject.SetActive(false);

                Vector3 position = new Vector3(gameObject.transform.position.x, 1f, gameObject.transform.position.z);

                Destroy(gameObject);
                Destroy(collision.gameObject);

                Instantiate(particle, position, Quaternion.identity);
            }
         }
    }

    private IEnumerator SetIsForming(CrowdController crowdController)
    {
        crowdController.isForming = true;
        yield return new WaitForSeconds(0.5f);
        crowdController.isForming = false;
    }
}
