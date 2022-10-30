using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Level[] levels;
    public GameObject opponentCastleContainerPrefab;
    public GameObject playerCastleContainerPrefab;
    public GameObject opponentUnitSpawnerPrefab;
    public GameObject playerUnitSpawnerPrefab;
    public GameObject opponentWithdrawZonePrefab;
    public GameObject playerWithdrawZonePrefab;
    public GameObject playerPrefab;
    public GameObject opponentPrefab;
    public Material[] lightMaterials;
    public Material[] darkMaterials;
    private List<GameObject> instances;
    private int materialIndex = 0;


    void Awake()
    {
        Instance = this;
        instances = new List<GameObject>();
    }

    private IEnumerator GenerateLevelRoutine()
    {
        yield return new WaitForEndOfFrame();

        materialIndex = Random.Range(0, lightMaterials.Length);
        InstantiateMap();
        InstantiateDecorations();
        InstantiateCastles();
        InstantiateUnitSpawners();
        InstantiateWithdrawZones();
        InstantiateCharacters();
        InstantiateBlockSpawner();
    }

    public void GenerateLevel()
    {
        StartCoroutine(GenerateLevelRoutine());
    }

    public void ResetLevel()
    {
        if (instances.Count > 0)
        {
            foreach (GameObject go in instances)
            {
                Destroy(go);
            }

            GameObject[] opponentUnits = GameObject.FindGameObjectsWithTag("OpponentCrowd");
            GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("PlayerCrowd");

            GameObject[] combined = opponentUnits.Concat(playerUnits).ToArray();

            for (int i = 0; i < combined.Length; i++)
            {
                Destroy(combined[i]);
            }

            instances = new List<GameObject>();
            GameManager.Instance.SetGameState(GameState.Lobby);
        }
    }

    private void InstantiateMap()
    {
        GameObject mapInstance = Instantiate(levels[0].map, Vector3.zero, Quaternion.Euler(0, -90, 0));
        GameObject opponentPlane = GameObject.FindWithTag("OpponentPlane");
        opponentPlane.GetComponent<Renderer>().material = darkMaterials[materialIndex];

        instances.Add(mapInstance);
    }

    private void InstantiateDecorations()
    {
        GameObject decorationInstance = Instantiate(levels[0].decoration, Vector3.zero, Quaternion.Euler(0, 0, 0));
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Buildings");
        GameObject platform = GameObject.FindWithTag("Platform");
        GameObject platformStand = GameObject.FindWithTag("PlatformStand");
        GameObject floor = GameObject.FindWithTag("Floor");

        platform.GetComponent<Renderer>().material = levels[0].mapPlatformMaterial;
        platformStand.GetComponent<Renderer>().material = levels[0].mapStandMaterial;
        floor.GetComponent<Renderer>().material = levels[0].decorationFloorMaterial;

        foreach (GameObject building in buildings)
        {
            building.GetComponent<Renderer>().material = levels[0].decorationBuildingsMaterial;
        }

        RenderSettings.fogColor = levels[0].fogColor;

        instances.Add(decorationInstance);
    }

    public void InstantiateCastles()
    {
        GameObject opponentCastleCoords = GameObject.Find("OpponentCastleCoords");
        GameObject playerCastleCoords = GameObject.Find("PlayerCastleCoords");

        GameObject opponentCastleContainer = Instantiate(opponentCastleContainerPrefab, opponentCastleCoords.transform.position, Quaternion.identity);
        GameObject playerCastleContainer = Instantiate(playerCastleContainerPrefab, playerCastleCoords.transform.position, Quaternion.identity);

        instances.Add(opponentCastleContainer);
        instances.Add(playerCastleContainer);

        CastleController opponentCastleController = opponentCastleContainer.GetComponent<CastleController>();
        CastleController playerCastleController = playerCastleContainer.GetComponent<CastleController>();

        int towerHealth = PlayerPrefs.GetInt(SerializableFields.TowerHealth, 0);

        int additionalHealth = Random.Range(-2, 2);

        opponentCastleController.SetCastleParams(10 + towerHealth + additionalHealth, false, "PlayerUnit", 2);
        opponentCastleController.SetCastleMaterial(lightMaterials[materialIndex], darkMaterials[materialIndex]);

        playerCastleController.SetCastleParams(10 + towerHealth, true, "OpponentUnit", 2);
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

        WithdrawItems opponentWithdrawZoneController = opponentWithdrawZone.GetComponent<WithdrawItems>();
        opponentWithdrawZoneController.SetMaterial(lightMaterials[materialIndex]);

        instances.Add(opponentWithdrawZone);
        instances.Add(playerWithdrawZone);
    }

    private void InstantiateCharacters()
    {
        GameObject playerCoords = GameObject.Find("PlayerCoords");
        GameObject opponentCoords = GameObject.Find("OpponentCoords");

        GameObject playerContainer = Instantiate(playerPrefab, playerCoords.transform.position, Quaternion.identity);
        GameObject opponentContainer = Instantiate(opponentPrefab, opponentCoords.transform.position, Quaternion.identity);

        OpponentAIController opponentAIController = opponentContainer.GetComponent<OpponentAIController>();
        opponentAIController.SetOpponentMaterial(lightMaterials[materialIndex]);

        instances.Add(playerContainer);
        instances.Add(opponentContainer);
    }

    private void InstantiateBlockSpawner()
    {
        GameObject blockSpawnerInstance = Instantiate(levels[0].stackableBoxSpawners);
        StackableBoxSpawnerController stackableBoxSpawnerController = blockSpawnerInstance.GetComponent<StackableBoxSpawnerController>();

        stackableBoxSpawnerController.SetOpponentMaterial(lightMaterials[materialIndex]);
        instances.Add(blockSpawnerInstance);

    }
}
