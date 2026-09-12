using System;
using System.Collections;
using UnityEngine;

public class EnemyPlayerDetect : MonoBehaviour
{
    [Header("Detect Components")]
    [Space]
    [SerializeField] private bool canAtack = false;
    public bool CanAtack { get => canAtack;  }
    [Space]
    [SerializeField] public PlayerController player;
    [SerializeField] private CircleCollider2D _CircleColliderAtack;
    [SerializeField] private float colliderDelay = 1.0f;
    [Space]
    [SerializeField] private bool newUpdatePointposition;
    public bool NewUpdatePointposition { get => newUpdatePointposition; set => newUpdatePointposition = value; }
    

    private void Start()
    {
        _CircleColliderAtack = GetComponent<CircleCollider2D>();
        newUpdatePointposition = false;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {       
        if (collision.CompareTag("Player"))
        {           
            StartCoroutine(ColliderCorutine());            
        }                
    }
    private IEnumerator ColliderCorutine()
    {
        canAtack = true;
        _CircleColliderAtack.enabled = false;
        yield return new WaitForSeconds(colliderDelay);
        _CircleColliderAtack.enabled = true;
        canAtack = false;
        newUpdatePointposition = true;
    }
}

