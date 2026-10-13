using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [Space]
    [SerializeField] private bool isInvulnerable;
    [SerializeField] private float invulnerabilityDuration = 1.1f;

    private PlayerController playerController;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        currentHealth = maxHealth;
    }

 
   /* public void TakeDamage(int damage)
    {
        if (playerController.IsDead)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerController.Die();
        }
    }*/
  
    public void TakeDamage(int damage, float sourceDamageXPosition)
    {
        if (playerController.IsDead)
            return;

        if (isInvulnerable)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerController.Die();
            return;
        }

        playerController.KnockBack(sourceDamageXPosition);

        StartCoroutine(InvulnerabilityRoutine());
    }
    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false;
    }
}
