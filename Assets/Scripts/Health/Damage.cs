using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] public bool omHit = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().KnockBack(transform.position.x);
            Debug.Log(collision.name);
        }
       /*if ( collision.CompareTag("PlayerDamage_1"))
        {
            collision.GetComponentInParent<PlayerController>().KnowcBack(transform.position.x);
            Debug.Log(collision.name);
        }*/
       
    }
}
