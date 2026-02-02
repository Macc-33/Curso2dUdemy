using UnityEngine;

public class DoorIn : MonoBehaviour
{
    private static readonly int idOpenDoor = Animator.StringToHash("OpenDoor");
    private Animator animator => GetComponent<Animator>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!GameManager.instance.hasCheckPointActive) return;
        if (!collision.CompareTag("Player")) return;
        animator.SetTrigger(idOpenDoor);
        collision.GetComponent<PlayerController>().DoorIn();
        collision.transform.position = new Vector3(transform.position.x,collision.transform.position.y,collision.transform.position.z);

    }

}
