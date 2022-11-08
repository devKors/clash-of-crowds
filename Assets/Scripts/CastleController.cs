using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CastleController : MonoBehaviour
{
    public int health;
    private int baseHealth;
    private bool isMyCastle;
    [SerializeField]
    private string enemyTag;
    private GameObject[] castles;
    public GameObject healthBar;
    private float castleMinHeight = -1.5f;
    public GameObject particle;
    public GameObject destructionParticle;
    private bool canMakeBang = true;
    private Transform arch;

    void Awake()
    {
        castles = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            castles[i] = transform.GetChild(i).gameObject;
        }

        SetCastleSkin(0);
    }

    void OnDestroy()
    {
        if (arch != null)
        {
            Destroy(arch.gameObject);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.state != GameState.Game)
        {
            return;
        }

        if (other.CompareTag(enemyTag))
        {
            if (--health != 0)
            {
                Destroy(other.gameObject);

                float transformFactor = castleMinHeight / baseHealth;
                float height = transformFactor * (baseHealth - health);
                transform.DOMoveY(height, 0.1f).SetEase(Ease.InOutElastic);

                if (canMakeBang)
                {
                    canMakeBang = false;
                    StartCoroutine(AnimateBang());
                }
                
            }
            else
            {
                Destroy(other.gameObject);
                AnimateDestruction();
                Destroy(arch.gameObject);
                transform.DOMoveY(-20, 0.01f);

                if (isMyCastle)
                {
                    GameManager.Instance.SetGameState(GameState.Lose);
                }
                else
                {
                    GameManager.Instance.SetGameState(GameState.Victory);
                }
            }
        }

    }

    public void SetCastleSkin(int index)
    {
        foreach (GameObject go in castles)
        {
            go.SetActive(false);
        }

        if (castles.Length > 0)
        {
            castles[index].SetActive(true);
        }
    }

    public void SetCastleParams(int health, bool isMyCastle, string enemyTag)
    {
        this.health = health;
        this.isMyCastle = isMyCastle;
        this.enemyTag = enemyTag;

        this.baseHealth = health;
    }

    public void SetCastleParams(int health, bool isMyCastle, string enemyTag, int index)
    {
        this.health = health;
        this.isMyCastle = isMyCastle;
        this.enemyTag = enemyTag;

        this.baseHealth = health;

        SetCastleSkin(index);
    }

    public void SetCastleHealth(int health)
    {
        this.health = health;
    }

    public void SetCastleMaterial(Material lightMaterial, Material darkMaterial)
    {
        Material[] m = new Material[] {lightMaterial, darkMaterial};

        foreach (GameObject go in castles)
        {
            go.GetComponent<Renderer>().materials = m;
        }

        Transform castleTransform = transform.GetChild(2);
        Transform archTransform = castleTransform.GetChild(0);
        archTransform.GetComponent<Renderer>().material = lightMaterial;

        arch = Instantiate(archTransform, archTransform.position, Quaternion.identity);

        Destroy(archTransform.gameObject);
    }

    public void SetCustleHealthBar(Material material)
    {
        GameObject hb = Instantiate(healthBar, transform);
        BillBoardCastle controller = hb.GetComponent<BillBoardCastle>();
        controller.SetCoverColor(material.color);
    }

    private IEnumerator AnimateBang()
    {
        Vector3 position = new Vector3(transform.position.x, 0, transform.position.z);
        Instantiate(particle, position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        canMakeBang = true;
    }

    private void AnimateDestruction()
    {
        Vector3 position = new Vector3(transform.position.x, 0, transform.position.z);
        Instantiate(destructionParticle, position, Quaternion.identity);
    }
}
