using System.Collections;
using UnityEngine;

public class CrowdUnit : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyUnit;

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

                Destroy(gameObject);
                Destroy(collision.gameObject);
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
