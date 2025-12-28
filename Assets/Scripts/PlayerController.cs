using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody2D m_rigidbody;
    private GaderInput m_gaderInput;
    private Transform m_transform;
    private Animator m_animator;

    //Values

   [SerializeField] private float speed;
    private int direction = 1;
    private int idSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idSpeed = Animator.StringToHash("Speed");
        m_gaderInput = GetComponent<GaderInput>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_transform = GetComponent<Transform>();
        m_animator = GetComponent<Animator>();
    }

  

    private void Update()
    {
        SetAnimatorValues();
        
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        Flip();
        m_rigidbody.linearVelocity = new Vector2(speed * m_gaderInput.ValueX, m_rigidbody.linearVelocityY); //Movimiento en eje X del personaje 

    }

    private void Flip()
    {
       if(m_gaderInput.ValueX * direction < 0)
        {
            m_transform.localScale = new Vector3(-m_transform.localScale.x, 1, 1);
            direction *= -1;
        }
    }
    private void SetAnimatorValues()
    {
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody.linearVelocityX));
    }
}
