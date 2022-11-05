using System.Collections;
using UnityEngine;

public class CrowdUnit : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyUnit;
    public GameObject particle;
    private Animator unitAnimator;


    void Start()
    {
        Material m = transform.GetComponentInChildren<Renderer>().material;
        ParticleSystem.MainModule settings = particle.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(m.color);

        unitAnimator = transform.GetComponentInChildren<Animator>();
        unitAnimator.SetBool("isMoving", true);

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
        HandleDance();
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

    private void HandleDance()
    {
        if (GameManager.Instance.state != GameState.Game)
        {
            bool isPlayer = transform.tag == "PlayerUnit";

            if (GameManager.Instance.state == GameState.Victory)
            {
                unitAnimator.SetBool("isMoving", false);
                unitAnimator.SetBool(isPlayer ? "isDancing" : "isDefeated", true);
            }
            else if (GameManager.Instance.state == GameState.Lose)
            {
                unitAnimator.SetBool("isMoving", false);
                unitAnimator.SetBool(isPlayer ? "isDefeated" : "isDancing", true);
            }
        }
    }
}
