using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerRespwnPoint;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float respwnPlayerDelay;
    public PlayerController PlayerController { get => _playerController; }

    [Header("Respwn Settings")]
    public bool hasCheckPointActive = false ; 
    public Vector3 checkPointRespwnPosition;

    [Header("DiamondItems")]
    [SerializeField] private bool diamondHaveRandomLook;
    [SerializeField] private int _diamondCollected;
    public int DiamondCollected { get => _diamondCollected; }
    public bool DiamondHaveRandomLook1 { get => diamondHaveRandomLook; set => diamondHaveRandomLook = value; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void RespwnPlayer()
    {
        if(hasCheckPointActive) playerRespwnPoint.position = checkPointRespwnPosition;
        StartCoroutine(RespwnPlayerCoroutine());
    }  
    IEnumerator RespwnPlayerCoroutine()
    {
       if(!hasCheckPointActive)
        {
            yield return new WaitForSeconds(respwnPlayerDelay);
            GameObject newPlayer = Instantiate(playerPrefab, playerRespwnPoint.position, Quaternion.identity);
            newPlayer.name = ("Player");
            _playerController = newPlayer.GetComponent<PlayerController>();
        }
       else
        {
            GameObject newPlayer = Instantiate(playerPrefab, playerRespwnPoint.position, Quaternion.identity);
            newPlayer.name = ("Player");
            _playerController = newPlayer.GetComponent<PlayerController>();
        }
       
    }
    public void AddDiamond() => _diamondCollected++;
    public void DiamondHaveRandomLook() => DiamondHaveRandomLook1 = true;
    
}
