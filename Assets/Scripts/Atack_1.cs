using System;
using System.Collections;
using UnityEngine;

public class Atack_1 : MonoBehaviour
{
    /*[SerializeField] private Vector2 hitPower;
    [SerializeField] private Vector2 hitDefault;
    [SerializeField] private float HitDelay = 0.5f;
    [SerializeField] private float direcctionDamage;
    private Rigidbody2D _rb;
    //private Transform _enemyGO;*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
        //Transform  enemyGO = collision.GetComponent<Transform>();
        //_rb = enemyRb;
        //_enemyGO = enemyGO;
        //if (_rb == null) return;
        //if (_enemyGO == null) return;
        if (collision.CompareTag("Enemy"))
        {
            
            Debug.Log("Hit Enemy");
            collision.GetComponent<EnemyController_1>().KnowcBack(transform.position.x);
            //StartCoroutine(AtackCoroutine());
           
        }
   

    }

    /* private IEnumerator AtackCoroutine()
    {
        EnemyKnoked(direcctionDamage);
        //_rb.linearVelocity = new Vector2(hitPower.x, hitPower.y);
        yield return new WaitForSeconds(HitDelay);
        _rb.linearVelocity = new Vector2(hitDefault.x,hitDefault.y);
    }

   private void EnemyKnoked(float sourgeDamageXPosition)
    {
        sourgeDamageXPosition = transform.position.x;
        float direction;
        if (_enemyGO.position.x > sourgeDamageXPosition)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
            _rb.linearVelocity = new Vector2(hitPower.x * direction, hitPower.y);
    }*/

}
