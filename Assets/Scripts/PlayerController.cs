using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components del player controller
    [Header("Components")]
    [SerializeField]private Transform m_transform;
    private Rigidbody2D m_rigidbody;
    private GaderInput m_gaderInput;
    private Animator m_animator;

    [Header("jumps settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJump;
    [SerializeField] private int counterExtraJump;

    [Header("Move settings")]
    [SerializeField] private float speed;
    private int direction = 1;   

    [Header("Ground settings")]
    [SerializeField] private Transform rFoot;
    [SerializeField] private Transform lFoot;
    RaycastHit2D lFootRay;
    RaycastHit2D rFootRay;
    [SerializeField] private bool isGrounded;
    private bool canDoubleJump;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall settings")]
    [SerializeField] private float checkWallDistance;
    [SerializeField] private bool isWallDetected;

    [Header("Animations settings")]
    private int idSpeed;
    private int idIsGrounded;


    private void Awake()
    {
        m_gaderInput = GetComponent<GaderInput>();
        m_rigidbody = GetComponent<Rigidbody2D>();
      //  m_transform = GetComponent<Transform>();
        m_animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idSpeed = Animator.StringToHash("Speed"); // animator.strigtohash es para comvertir cualquier string de texto en un valor numerico y asi no consume tanto recurso 
        idIsGrounded = Animator.StringToHash("IsGrounded");       
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
        CheckCollision();
        Move();
        Jump();
    }

    private void CheckCollision()
    {
        HandleGround(); //Detectar suelo ....traducido
        HandleWall(); //Detectar paretes 
    }

    private void HandleWall()
    {
        isWallDetected = Physics2D.Raycast(m_transform.position, Vector2.right * direction, checkWallDistance, groundLayer);
    }

    private void HandleGround()
    {
        lFootRay = Physics2D.Raycast(lFoot.position, Vector2.down, rayLength, groundLayer);
        rFootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLength, groundLayer);
        if (lFootRay || rFootRay)
        {
            isGrounded = true;
            counterExtraJump = extraJump;
            canDoubleJump = false;
        }
        else
        {
            isGrounded = false;
        }
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
            if (isGrounded)
            {
               m_rigidbody.linearVelocity = new Vector2(speed * m_gaderInput.ValueX, jumpForce);
                canDoubleJump = true;
            }
            else if(counterExtraJump > 0 && canDoubleJump)
            {
                m_rigidbody.linearVelocity = new Vector2(speed * m_gaderInput.ValueX, jumpForce);
                counterExtraJump--;
            }
        }
        m_gaderInput.IsJumping = false;
    }
    private void SetAnimatorValues()
    {
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody.linearVelocityX));
        m_animator.SetBool(idIsGrounded, isGrounded);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_transform.position, new Vector2(m_transform.position.x + checkWallDistance * direction, m_transform.position.y));
    }
}
