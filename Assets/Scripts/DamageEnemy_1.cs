using System;
using System.Collections;
using UnityEngine;

public class DamageEnemy_1 : MonoBehaviour
{
    [Header("Damage Collider Components")]
    [Space]
    [SerializeField] private bool _isAtack ;       
    public bool IsAtack { get => _isAtack; set => _isAtack = value; }
    [Space]
    [SerializeField] private float atackDelay;
    [SerializeField] public EnemyController_1 _enemyController;
    private PlayerController _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _enemyController.GetComponentInParent<EnemyController_1>();
        _isAtack = false;
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player = collision.GetComponent<PlayerController>();
        if (collision.CompareTag("Player") && !_enemyController.isKnocked)
        {
            if (_isAtack) return;
            StartCoroutine(OnAtackCoroutine());
            collision.GetComponent<PlayerController>().KnowcBack(transform.position.x);
        }     
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        _player = collision.GetComponent<PlayerController>();
        if (collision.CompareTag("Player") && !_enemyController.isKnocked)
        {           
            if (_isAtack) return;
            StartCoroutine(OnAtackCoroutine());
            collision.GetComponent<PlayerController>().KnowcBack(transform.position.x);           
        }
    }
    private IEnumerator OnAtackCoroutine()
    {      
            _isAtack = true;
        yield return new WaitForSeconds(atackDelay);
            _isAtack = false;      
    }

}
