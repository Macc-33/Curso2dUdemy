using UnityEngine;

public class ExtraDamage : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private Vector2 extraKnockedPower;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = null;
        PlayerHealth playerHealth = null;

        if (collision.CompareTag("Player"))
        {
            playerController = collision.GetComponent<PlayerController>();
            playerHealth = collision.GetComponent<PlayerHealth>();
        }
        else if (collision.CompareTag("PlayerDamage_1"))
        {
            playerController = collision.GetComponentInParent<PlayerController>();
            playerHealth = collision.GetComponentInParent<PlayerHealth>();
        }
        else
        {
            return;
        }

        if (playerController == null || playerHealth == null)
            return;

        playerController.KnockedForce = extraKnockedPower;

        playerHealth.TakeDamage( damage, transform.position.x );
    }
}
