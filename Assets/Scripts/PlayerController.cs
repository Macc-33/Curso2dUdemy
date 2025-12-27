using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private GaderInput m_gaderInput;
    private Transform m_transform;
   [SerializeField] private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_gaderInput = GetComponent<GaderInput>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        m_rigidbody.linearVelocity = new Vector2(speed * m_gaderInput.ValueX , m_rigidbody.linearVelocityY);
    }
}
