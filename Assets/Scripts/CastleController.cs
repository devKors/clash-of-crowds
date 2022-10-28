using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    public int health;
    private bool isMyCastle;
    [SerializeField]
    private string enemyTag;
    private GameObject[] castles;
    public GameObject healthBar;

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
            }
            else
            {
                Destroy(other.gameObject);
                Destroy(gameObject);

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
    }

    public void SetCastleParams(int health, bool isMyCastle, string enemyTag, int index)
    {
        this.health = health;
        this.isMyCastle = isMyCastle;
        this.enemyTag = enemyTag;

        SetCastleSkin(index);
    }
}
