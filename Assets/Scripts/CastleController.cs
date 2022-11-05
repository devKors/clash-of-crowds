using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    public int health;
    private int baseHealth;
    private bool isMyCastle;
    [SerializeField]
    private string enemyTag;
    private GameObject[] castles;
    public GameObject healthBar;
    private float transformSize = 1.5f;
    public GameObject particle;
    private bool canMakeBang = true;

    void Awake()
    {
        castles = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            castles[i] = transform.GetChild(i).gameObject;
        }

        SetCastleSkin(0);

        Instantiate(healthBar, transform);
    }

    void Update()
    {
        if (GameManager.Instance.state != GameState.Game)
        {
            return;
        }

        float transformFactor = transformSize / baseHealth;
        float currentHeight = Mathf.Round(transform.position.y + transformSize * 10f) / 10f;
        float nextHeight = Mathf.Round(transformFactor * (baseHealth + baseHealth - health) * 10f) / 10f;

        if (currentHeight != nextHeight)
        {
            Vector3 newPos = new Vector3(transform.position.x,currentHeight - nextHeight, transform.position.z);
            transform.position = newPos;
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
                if (canMakeBang)
                {
                    canMakeBang = false;
                    StartCoroutine(AnimateBang());
                }
                
            }
            else
            {
                Destroy(other.gameObject);
                // Destroy(gameObject);

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
    }

    private IEnumerator AnimateBang()
    {
        Vector3 position = new Vector3(transform.position.x, 0, transform.position.z);
        Instantiate(particle, position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        canMakeBang = true;
    }
}
