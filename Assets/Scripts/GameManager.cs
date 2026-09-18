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
    [SerializeField] private Transform startPlayerPoint;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float respawnPlayerDelay;

    public PlayerController PlayerController { get => _playerController; }

    [Header("Respwn Settings")]
    public bool hasCheckPointActive = false;
    public Vector3 checkPointRespwnPosition;

    [Header("SoulItems")]
    [SerializeField] private bool soulsHaveRandomLook;
    [SerializeField] private int _soulCollected;
    [SerializeField] private int totalSouls;

    [Header("Traps")]
    public GameObject arrowPrefab;
    public GameObject fallingPlatformPrefab;
    public int SoulCollected { get => _soulCollected; }
    public bool SoulHaveRandomLook1 { get => soulsHaveRandomLook; set => soulsHaveRandomLook = value; }

 

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        GameObject[] soul = GameObject.FindGameObjectsWithTag("Soul");
        totalSouls = soul.Length;
    }
    public void RespwnPlayer()
    {
        if (hasCheckPointActive) playerRespawnPoint.position = checkPointRespwnPosition;
        else
        {
            playerRespawnPoint = startPlayerPoint;
        }
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

    public void AddSoul() => _soulCollected++;
    public void SoulHaveRandomLook() => SoulHaveRandomLook1 = true;
    
}
