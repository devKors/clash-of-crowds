using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject[] mapPrefabs;
    public GameObject[] stackableBoxSpawners;
    public GameObject opponentCastleContainerPrefab;
    public GameObject playerCastleContainerPrefab;
    public GameObject opponentUnitSpawnerPrefab;
    public GameObject playerUnitSpawnerPrefab;
    public GameObject opponentWithdrawZonePrefab;
    public GameObject playerWithdrawZonePrefab;
    public GameObject playerPrefab;
    public GameObject opponentPrefab;
    private List<GameObject> instances;

    void Awake()
    {
        Instance = this;
        instances = new List<GameObject>();
    }

    public void GenerateLevel()
    {
        InstantiateMap();
        InstantiateCastles();
        InstantiateUnitSpawners();
        InstantiateWithdrawZones();
        InstantiateCharacters();
        InstantiateBlockSpawner();
    }

    public void ResetLevel()
    {
        if (instances.Count > 0)
        {
            foreach (GameObject go in instances)
            {
                Destroy(go);
            }

            GameObject[] opponentUnits = GameObject.FindGameObjectsWithTag("OpponentUnit");
            GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");

            GameObject[] combined = opponentUnits.Concat(playerUnits).ToArray();

            for (int i = 0; i < combined.Length; i++)
            {
                Destroy(combined[i]);
            }

            GameManager.Instance.SetGameState(GameState.Lobby);
        }   
    }

    private void InstantiateMap()
    {
        GameObject mapInstance = Instantiate(mapPrefabs[0], Vector3.zero, Quaternion.Euler(0, -90, 0));
        instances.Add(mapInstance);
    }

    private void InstantiateCastles()
    {
        GameObject opponentCastleCoords = GameObject.Find("OpponentCastleCoords");
        GameObject playerCastleCoords = GameObject.Find("PlayerCastleCoords");

        GameObject opponentCastleContainer = Instantiate(opponentCastleContainerPrefab, opponentCastleCoords.transform.position, Quaternion.identity);
        GameObject playerCastleContainer = Instantiate(playerCastleContainerPrefab, playerCastleCoords.transform.position, Quaternion.identity);

        instances.Add(opponentCastleContainer);
        instances.Add(playerCastleContainer);

        CastleController opponentCastleController = opponentCastleContainer.GetComponent<CastleController>();
        CastleController playerCastleController = playerCastleContainer.GetComponent<CastleController>();

        opponentCastleController.SetCastleParams(10, false, "PlayerUnit", 1);
        playerCastleController.SetCastleParams(10, true, "OpponentUnit", 1);
    }

    private void InstantiateUnitSpawners()
    {
        GameObject opponentUnitSpawnerCoords = GameObject.Find("OpponentUnitSpawnerCoords");
        GameObject playerUnitSpawnerCoords = GameObject.Find("PlayerUnitSpawnerCoords");

        GameObject opponentUnitSpawner = Instantiate(opponentUnitSpawnerPrefab, opponentUnitSpawnerCoords.transform.position, Quaternion.Euler(0, 180, 0));
        GameObject playerUnitSpawner = Instantiate(playerUnitSpawnerPrefab, playerUnitSpawnerCoords.transform.position, Quaternion.identity);

        instances.Add(opponentUnitSpawner);
        instances.Add(playerUnitSpawner);
    }

    private void InstantiateWithdrawZones()
    {
        GameObject opponentCastleCoords = GameObject.Find("OpponentCastleCoords");
        GameObject playerCastleCoords = GameObject.Find("PlayerCastleCoords");

        GameObject opponentWithdrawZone = Instantiate(opponentWithdrawZonePrefab, opponentCastleCoords.transform.position, Quaternion.identity);
        GameObject playerWithdrawZone = Instantiate(playerWithdrawZonePrefab, playerCastleCoords.transform.position, Quaternion.identity);

        instances.Add(opponentWithdrawZone);
        instances.Add(playerWithdrawZone);
    }

    private void InstantiateCharacters()
    {
        GameObject playerCoords = GameObject.Find("PlayerCoords");
        GameObject opponentCoords = GameObject.Find("OpponentCoords");

        GameObject playerContainer = Instantiate(playerPrefab, playerCoords.transform.position, Quaternion.identity);
        GameObject opponentContainer = Instantiate(opponentPrefab, opponentCoords.transform.position, Quaternion.identity);

        instances.Add(playerContainer);
        instances.Add(opponentContainer);
    }

    private void InstantiateBlockSpawner()
    {
        GameObject blockSpawnerInstance = Instantiate(stackableBoxSpawners[0]);
        instances.Add(blockSpawnerInstance);

    }
}
