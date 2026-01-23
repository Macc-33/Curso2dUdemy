using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private bool diamondHaveRandomLook;
   


    [SerializeField] private PlayerController _playerController;
    public PlayerController PlayerController { get => _playerController; }

    [SerializeField] private int _diamondCollected;
    public int DiamondCollected { get => _diamondCollected; }
    public bool DiamondHaveRandomLook1 { get => diamondHaveRandomLook; set => diamondHaveRandomLook = value; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

    }
    public void AddDiamond() => _diamondCollected++;
    public void DiamondHaveRandomLook() => DiamondHaveRandomLook1 = true;
  
  
    
  
}
