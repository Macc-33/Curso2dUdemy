using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static event System.Action<PlayerController> OnPlayerSpawned;

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerRespawnPoint;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float respawnPlayerDelay;

    public PlayerController PlayerController { get => _playerController; }

    [Header("Respwn Settings")]
    public bool hasCheckPointActive = false;
    public Vector3 checkPointRespwnPosition;

    [Header("DiamondItems")]
    [SerializeField] private bool diamondHaveRandomLook;
    [SerializeField] private int _diamondCollected;
    [SerializeField] private int totalDiamonds;

    [Header("Traps")]
    public GameObject arrowPrefab;
    public GameObject fallingPlatformPrefab;
    public int DiamondCollected { get => _diamondCollected; }
    public bool DiamondHaveRandomLook1 { get => diamondHaveRandomLook; set => diamondHaveRandomLook = value; }

 

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        GameObject[] diamonds = GameObject.FindGameObjectsWithTag("Diamond");
        totalDiamonds = diamonds.Length;
    }
    public void RespwnPlayer()
    {
        if (hasCheckPointActive) playerRespawnPoint.position = checkPointRespwnPosition;
        StartCoroutine(RespwnPlayerCoroutine());
    }
    IEnumerator RespwnPlayerCoroutine()
    {
      

        if (!hasCheckPointActive)
            yield return new WaitForSeconds(respawnPlayerDelay);

        GameObject newPlayer =
            Instantiate( playerPrefab, playerRespawnPoint.position,Quaternion.identity);

        newPlayer.name = "Player";

        _playerController =newPlayer.GetComponent<PlayerController>();

       
        OnPlayerSpawned?.Invoke(_playerController);

    }
    public void CreateObject(GameObject prefab, Vector3 position, float delay)
    {
        StartCoroutine(CreateObjectRoutine(prefab,position,delay));
    }

    private IEnumerator CreateObjectRoutine(GameObject prefab, Vector3 position, float delay)
    {
        
        yield return new WaitForSeconds(delay);
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
    }

    public void AddDiamond() => _diamondCollected++;
    public void DiamondHaveRandomLook() => DiamondHaveRandomLook1 = true;
    
}
