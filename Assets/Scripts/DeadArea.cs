using UnityEngine;

public class DeadArea : MonoBehaviour
{
    [SerializeField] private PlayerController player;


    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<PlayerController>();

        if (collision.CompareTag("Player")) player.Die();
    }
}
