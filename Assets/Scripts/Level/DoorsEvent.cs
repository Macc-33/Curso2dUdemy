using UnityEngine;
using UnityEngine.Tilemaps;


public class DoorsEvent : MonoBehaviour
{
    [SerializeField] private GameObject entranceDoor;
    [SerializeField] private Animator animatorEntranceDoor;
    private int idOpenDoor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        idOpenDoor = Animator.StringToHash("OpenDoor");
        entranceDoor = GameObject.FindGameObjectWithTag("EntranceDoor");
        animatorEntranceDoor = entranceDoor.GetComponent<Animator>();
    }
   public void DoorOut()
    {
        if (!GameManager.instance.hasCheckPointActive)
        {
            animatorEntranceDoor.SetTrigger(idOpenDoor);
            Debug.Log("abriendo");
        }
   
    }
}
