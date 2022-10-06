using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
        Instance = this;
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

    private void InstantiateMap()
    {
        Instantiate(mapPrefabs[0], Vector3.zero, Quaternion.Euler(0, -90, 0));
    }

    private void InstantiateCastles()
    {
        GameObject opponentCastleCoords = GameObject.Find("OpponentCastleCoords");
        GameObject playerCastleCoords = GameObject.Find("PlayerCastleCoords");

        GameObject opponentCastleContainer = Instantiate(opponentCastleContainerPrefab, opponentCastleCoords.transform.position, Quaternion.identity);
        GameObject playerCastleContainer = Instantiate(playerCastleContainerPrefab, playerCastleCoords.transform.position, Quaternion.identity);

        CastleController opponentCastleController = opponentCastleContainer.GetComponent<CastleController>();
        CastleController playerCastleController = playerCastleContainer.GetComponent<CastleController>();

        opponentCastleController.SetCastleParams(10, false, "PlayerUnit");
        playerCastleController.SetCastleParams(10, true, "OpponentUnit", 1);
    }

    private void InstantiateUnitSpawners()
    {
        GameObject opponentUnitSpawnerCoords = GameObject.Find("OpponentUnitSpawnerCoords");
        GameObject playerUnitSpawnerCoords = GameObject.Find("PlayerUnitSpawnerCoords");

        GameObject opponentUnitSpawner = Instantiate(opponentUnitSpawnerPrefab, opponentUnitSpawnerCoords.transform.position, Quaternion.identity);
        GameObject playerUnitSpawner = Instantiate(playerUnitSpawnerPrefab, playerUnitSpawnerCoords.transform.position, Quaternion.identity);
    }

    private void InstantiateWithdrawZones()
    {
        GameObject opponentCastleCoords = GameObject.Find("OpponentCastleCoords");
        GameObject playerCastleCoords = GameObject.Find("PlayerCastleCoords");

        GameObject opponentWithdrawZone = Instantiate(opponentWithdrawZonePrefab, opponentCastleCoords.transform.position, Quaternion.identity);
        GameObject playerWithdrawZone = Instantiate(playerWithdrawZonePrefab, playerCastleCoords.transform.position, Quaternion.identity);
    }

    private void InstantiateCharacters()
    {
        GameObject playerCoords = GameObject.Find("PlayerCoords");
        GameObject opponentCoords = GameObject.Find("OpponentCoords");

        GameObject playerContainer = Instantiate(playerPrefab, playerCoords.transform.position, Quaternion.identity);
        GameObject opponentContainer = Instantiate(opponentPrefab, opponentCoords.transform.position, Quaternion.identity);
    }

    private void InstantiateBlockSpawner()
    {
        Instantiate(stackableBoxSpawners[0]);
    }
}
