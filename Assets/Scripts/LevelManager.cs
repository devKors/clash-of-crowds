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
    public Material[] outlinedMaterials;
    private List<GameObject> instances;
    private int materialIndex = 0;
    private int playerMaterialIndex = 0;
    private int level = 0;


    void Awake()
    {
        Instance = this;
        instances = new List<GameObject>();
    }

    private IEnumerator GenerateLevelRoutine()
    {
        yield return new WaitForEndOfFrame();

        int currentLevel = PlayerPrefs.GetInt(SerializableFields.Level, 0);
        level =  currentLevel % levels.Length;
        materialIndex = Random.Range(1, outlinedMaterials.Length);
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
        GameObject mapInstance = Instantiate(levels[level].map, Vector3.zero, Quaternion.Euler(0, -90, 0));
        GameObject opponentPlane = GameObject.FindWithTag("OpponentPlane");
        opponentPlane.GetComponent<Renderer>().material = outlinedMaterials[materialIndex];

        GameObject playerPlane = GameObject.FindWithTag("PlayerPlane");
        playerPlane.GetComponent<Renderer>().material = outlinedMaterials[playerMaterialIndex];

        instances.Add(mapInstance);
    }

    private void InstantiateDecorations()
    {
        GameObject decorationInstance = Instantiate(levels[level].decoration, Vector3.zero, Quaternion.Euler(0, 0, 0));
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Buildings");
        // GameObject platformStand = GameObject.FindWithTag("PlatformStand");
        GameObject platform = GameObject.FindWithTag("Platform");
        GameObject floor = GameObject.FindWithTag("Floor");

        platform.GetComponent<Renderer>().material = levels[level].mapPlatformMaterial;
        floor.GetComponent<Renderer>().material = levels[level].decorationFloorMaterial;

        // platformStand.GetComponent<Renderer>().material = levels[level].mapStandMaterial;
        foreach (GameObject building in buildings)
        {
            building.GetComponent<Renderer>().material = levels[level].decorationBuildingsMaterial;
        }

        RenderSettings.fogColor = levels[level].fogColor;

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
        opponentCastleController.SetCastleMaterial(outlinedMaterials[materialIndex], outlinedMaterials[materialIndex]);

        playerCastleController.SetCastleParams(10 + towerHealth, true, "OpponentUnit", 2);
        playerCastleController.SetCastleMaterial(outlinedMaterials[playerMaterialIndex], outlinedMaterials[playerMaterialIndex]);

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

        Vector3 opponentCastlePosition = opponentCastleCoords.transform.position;
        GameObject opponentWithdrawZone = Instantiate(
            opponentWithdrawZonePrefab,
            new Vector3(-2.5f, opponentCastlePosition.y, opponentCastlePosition.z),
            Quaternion.identity
        );

        Vector3 playerCastlePosition = playerCastleCoords.transform.position;
        GameObject playerWithdrawZone = Instantiate(
            playerWithdrawZonePrefab,
            new Vector3(-2.5f, playerCastlePosition.y, playerCastlePosition.z),
            Quaternion.identity
        );

        WithdrawItems opponentWithdrawZoneController = opponentWithdrawZone.GetComponent<WithdrawItems>();
        opponentWithdrawZoneController.SetMaterial(outlinedMaterials[materialIndex]);

        WithdrawItems playerWithdrawZoneController = playerWithdrawZone.GetComponent<WithdrawItems>();
        playerWithdrawZoneController.SetMaterial(outlinedMaterials[playerMaterialIndex]);

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
        opponentAIController.SetOpponentMaterial(outlinedMaterials[materialIndex]);

        int currentLevel = PlayerPrefs.GetInt(SerializableFields.Level, 0);
        int nameIndex =  currentLevel % GlobalConstants.enemyNames.Length;
        opponentAIController.SetPlayerName(GlobalConstants.enemyNames[nameIndex]);

        PlayerController playerController = playerContainer.GetComponent<PlayerController>();
        playerController.SetPlayerMaterial(outlinedMaterials[playerMaterialIndex]);
        playerController.SetPlayerName("Player");

        instances.Add(playerContainer);
        instances.Add(opponentContainer);
    }

    private void InstantiateBlockSpawner()
    {
        GameObject blockSpawnerInstance = Instantiate(levels[level].stackableBoxSpawners);
        StackableBoxSpawnerController stackableBoxSpawnerController = blockSpawnerInstance.GetComponent<StackableBoxSpawnerController>();

        stackableBoxSpawnerController.SetOpponentMaterial(outlinedMaterials[materialIndex]);
        stackableBoxSpawnerController.SetPlayerMaterial(outlinedMaterials[playerMaterialIndex]);
        instances.Add(blockSpawnerInstance);

    }
}
