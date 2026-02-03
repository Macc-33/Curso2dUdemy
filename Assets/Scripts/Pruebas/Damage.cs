using UnityEngine;

public class Damage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().KnowcBack(transform.position.x);
            Debug.Log(collision.name);
        }
    }
}
