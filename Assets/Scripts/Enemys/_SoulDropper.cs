using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;

public class _SoulDropper : MonoBehaviour
{
    [Header("Soul Drop Settings")]
    [SerializeField] private GameObject soulPrefab;
    [SerializeField] private int soulAmount = 3;
    [SerializeField] private float minDropForce = 2f;
    [SerializeField] private float maxDropForce = 4f;

    public void DropSouls()
    {
        for (int i = 0; i < soulAmount; i++)
        {
            GameObject soul = Instantiate(soulPrefab,transform.position,Quaternion.identity);

            Rigidbody2D soulRb = soul.GetComponent<Rigidbody2D>();

            if (soulRb != null)
            {
                Vector2 direction = new Vector2(Random.Range(-1f, 1f),Random.Range(0.7f, 1.3f)).normalized;

                float dropForce = Random.Range(minDropForce,maxDropForce);

                soulRb.AddForce(direction * dropForce,ForceMode2D.Impulse);
            }
        }
    }
}