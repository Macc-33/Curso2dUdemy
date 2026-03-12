using System;
using System.Collections;
using UnityEngine;

public class EnemyPlayerDetect : MonoBehaviour
{
    [SerializeField] public bool canAtack = false;
    [SerializeField] public PlayerController player;
    [SerializeField] private CircleCollider2D _CircleCollider;
    [SerializeField] private float colliderDelay = 1.0f;
    private EnemyController_1 _Enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        _CircleCollider = GetComponent<CircleCollider2D>();
        _Enemy = GetComponentInParent<EnemyController_1>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            //_Enemy.player = player.transform;
            StartCoroutine(ColliderCorutine());            
        }
                
    }

    private IEnumerator ColliderCorutine()
    {
        canAtack = true;
        _CircleCollider.enabled = false;
        yield return new WaitForSeconds(colliderDelay);
        _CircleCollider.enabled = true;
        canAtack = false;
    }
}
