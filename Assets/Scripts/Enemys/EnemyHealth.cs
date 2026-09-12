using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private bool isDead;
    public bool IsDead => isDead;  // Sistema de encapsulacion para obtener valores sin tocar valores privados 

    private void Start()
    {
         currentHealth = maxHealth;
        isDead = false;
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;

        Debug.Log("Enemy Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        Debug.Log("Enemy Dead");
    }
}
