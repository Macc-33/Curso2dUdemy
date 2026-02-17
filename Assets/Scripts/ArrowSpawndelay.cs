using System.Collections;
using UnityEngine;

public class ArrowSpawndelay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private float delay;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null ) return; 
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(OffRenderer());
        }

    }
    IEnumerator OffRenderer()
    {
        m_SpriteRenderer.enabled = false;
        yield return new WaitForSeconds(delay);
        m_SpriteRenderer.enabled = true;
    }
}
