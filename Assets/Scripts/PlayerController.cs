using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components del player controller
    private Rigidbody2D m_rigidbody;
    private GaderInput m_gaderInput;
    private Transform m_transform;
    private Animator m_animator;

    [Header("Move and jumps settings")]
    [SerializeField] private float speed;
    private int direction = 1;   
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJump;
    [SerializeField] private int counterExtraJump;
    private int idSpeed;

    [Header("Ground settings")]
    [SerializeField] private Transform rFoot;
    [SerializeField] private Transform lFoot;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;
    private int idIsGrounded;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idSpeed = Animator.StringToHash("Speed"); // animator.strigtohash es para comvertir cualquier string de texto en un valor numerico y asi no consume tanto recurso 
        idIsGrounded = Animator.StringToHash("IsGrounded");
        m_gaderInput = GetComponent<GaderInput>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_transform = GetComponent<Transform>();
        m_animator = GetComponent<Animator>();
        lFoot = GameObject.Find("L_Foot").GetComponent<Transform>();
        rFoot = GameObject.Find("R_Foot").GetComponent<Transform>();
        counterExtraJump = extraJump;
    }

    private void Update()
    {
        SetAnimatorValues();
        
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
        Jump();
        CheckGround();
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
    private void Jump()
    {
        if(m_gaderInput.IsJumping)
        {
            if(isGrounded)
               m_rigidbody.linearVelocity = new Vector2(speed * m_gaderInput.ValueX, jumpForce);
            if(counterExtraJump > 0)
            {
                m_rigidbody.linearVelocity = new Vector2(speed * m_gaderInput.ValueX, jumpForce);
                counterExtraJump--;
            }
        }
        m_gaderInput.IsJumping = false;
    }
    private void CheckGround()
    {
        RaycastHit2D lFootRay = Physics2D.Raycast(lFoot.position, Vector2.down,rayLength,groundLayer);
        RaycastHit2D rFootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLength, groundLayer);
        if(lFootRay || rFootRay)
        {
            isGrounded = true;
            counterExtraJump = extraJump;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void SetAnimatorValues()
    {
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody.linearVelocityX));
        m_animator.SetBool(idIsGrounded, isGrounded);
    }
}
