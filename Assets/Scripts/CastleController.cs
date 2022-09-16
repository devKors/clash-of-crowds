using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private bool isMyCastle;
    [SerializeField]
    private GameObject enemy;

    void Start()
    {
        health = 10;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemy.tag))
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
}
