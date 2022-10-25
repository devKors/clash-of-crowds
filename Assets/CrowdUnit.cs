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
            if (collision != null && collision.gameObject != null && gameObject != null)
            {

                GameObject crowdGO = collision.gameObject.transform.parent.gameObject;
                CrowdController crowdController = crowdGO.GetComponent<CrowdController>();

                if (gameObject.CompareTag("PlayerUnit"))
                {
                    Destroy(collision.gameObject);            
                    Destroy(gameObject);
                }

                IEnumerator formingCoroutine = SetIsForming(crowdController);
                StartCoroutine(formingCoroutine);
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
