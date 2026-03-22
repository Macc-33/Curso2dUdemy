using System;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{     
    //Components del player controller
    [Header("Components")]
    private Rigidbody2D m_rigidbody;
    private GaderInput m_gaderInput;
    private Animator m_animator;
    [SerializeField] private Transform m_transform;
    public Transform Transform { get => m_transform; set => m_transform = value; }
    [Space]
    [Header("jumps settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJump;
    [SerializeField] private int counterExtraJump;
    [Space]
    [Header("Move settings")]
    [SerializeField] private bool canMove;
    [SerializeField] private float moveDelay;
    [SerializeField] private float speed;
    private int direction = 1;
    [Space]
    [Header("Ground settings")]
    [SerializeField] private Transform rFoot;
    [SerializeField] private Transform lFoot;
    RaycastHit2D lFootRay;
    RaycastHit2D rFootRay;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;
    [Space]
    [Header("Wall settings")]
    [SerializeField] private float checkWallDistance;
    [SerializeField] private bool isWallDetected;
    [SerializeField] private bool canWallSlide;
    [SerializeField] private float speedSlice;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private bool isWasJumping;
    [SerializeField] private float wallJumpDuration;
    [Space]
    [Header("Hit Settings")]
    [SerializeField] private bool isKnocked;
    public bool IsKnocked {  set => isKnocked = value; }
    [SerializeField] private bool canBeKnocked;
    [SerializeField] private Vector2 knockedPower;
    public Vector2 KnockedPower {  set => knockedPower = value; }

    [SerializeField] private Vector2 defaulKnockedPower;
    [SerializeField] private float knockedDuration;

    [Space]
    [Header("Atack Settings")]
    [SerializeField] CircleCollider2D m_ColliderDamage;
    [SerializeField] private float atackColliderDelay = 0.2f;
    [SerializeField] private bool canHit;
    [SerializeField] private float canAtackDelay;
    [SerializeField] private  bool isAtack ; // no la puedo volver privada por que no funciona 
    public bool IsAtack { get => isAtack; set => isAtack = value; }

    [Space]
    [Header("DeadVFX")]
    [SerializeField] private GameObject deathVfx;
    [Space]
    [Header("Animations settings")]
    private int idSpeed;
    private int idIsGrounded;
    private int idIsWallDetected;
    private int idKnockBack;
    private int idIdle;
    private int idDoorIn;
    private int idIsAtack;
    private enum PlayerState
    {
        Idle,
        Move,
        Jump,
        WallSlide,
        Attack
    }
    private PlayerState currentState;

    private void Awake()
    {
        m_gaderInput = GetComponent<GaderInput>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();  
        m_ColliderDamage = GetComponentInChildren<CircleCollider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = PlayerState.Idle;
        idSpeed = Animator.StringToHash("Speed"); // animator.strigtohash es para comvertir cualquier string de texto en un valor numerico y asi no consume tanto recurso 
        idIsGrounded = Animator.StringToHash("IsGrounded");
        idIsWallDetected = Animator.StringToHash("IsWallDetected");
        idKnockBack = Animator.StringToHash("KnockBack");
        idIdle = Animator.StringToHash("Idle");
        idDoorIn = Animator.StringToHash("DoorIn");
        idIsAtack = Animator.StringToHash("IsAtack");
        counterExtraJump = extraJump;
        m_ColliderDamage.enabled = false ; //Desactivar collider de daño
        isAtack = false;
        canHit = true;
        canMove = true;
        CheckPlayerRespwnStated();       
    }
    private void HandleStateTransitions()
    {
        if (!canMove) return;

        if (currentState == PlayerState.Attack && isAtack)
            return;

        // PRIORIDAD 1 — ATAQUE
        if (m_gaderInput.IsAtack && canHit)
        {
            ChangeState(PlayerState.Attack);
            return;
        }
        // PRIORIDAD 2 — salto
        if (m_gaderInput.IsJumping)
        {
            ChangeState(PlayerState.Jump);
            return;
        }
        // PRIORIDAD 3 — wall slide
        if (isWallDetected && !isGrounded)
        {
            ChangeState(PlayerState.WallSlide);
            return;
        }
        
        // PRIORIDAD 4 — si está en el aire salto 
        if (!isGrounded)
        {
            ChangeState(PlayerState.Jump);
            return;
        }

        // PRIORIDAD 5 — si está movimiento
        if (m_gaderInput.Value.x != 0)
        {
            ChangeState(PlayerState.Move);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }

        Debug.Log(currentState);
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }
    private void CheckPlayerRespwnStated()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager.instance es NULL");
            return;
        }
        if (GameManager.instance.hasCheckPointActive)
        {
            startInCheckPoint();            
        }
        else
        {
            canMove = false;
            StartCoroutine(CanMoveRotuine());           
        }
    }
    private void startInCheckPoint()
    {        
        m_animator.Play("Idle");
        StartCoroutine(CanMoveRotuine());
    }
    IEnumerator CanMoveRotuine()
    {        
        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }
    private void Update()
    {
        SetAnimatorValues();
    }
    private void SetAnimatorValues()
    {
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody.linearVelocityX));
        m_animator.SetBool(idIsGrounded, isGrounded);
        m_animator.SetBool(idIsWallDetected, isWallDetected);
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!canMove)
        {
            HandleWall();
            handelWallSlide();
            //SetAnimatorValues();
            return;
        }
        if (isKnocked) return;
        CheckCollision();
       HandleStateTransitions();

        switch (currentState)
        {
            case PlayerState.Idle:
                Move();
                break;

            case PlayerState.Move:
                Move();
                break;

            case PlayerState.Jump:
                Move();   // ← permite movimiento en el aire
                Jump();   // ← lógica de salto
                break;

            case PlayerState.WallSlide:
                handelWallSlide();
                break;

            case PlayerState.Attack:
                HandleAttackState();
                break;
        }
        //Move();
        //Jump();
        //Atack();                       
    }

    private void HandleAttackState()
    {
        Move(); // opcional, si quieres permitir movimiento durante ataque

        if (!isAtack && canHit)
        {
            StartCoroutine(AtackRutine());
            StartCoroutine(CanAtackRoutine());

            m_animator.SetTrigger(idIsAtack);

            m_gaderInput.IsAtack = false;
        }
    }

    private void CheckCollision()
    {
        HandleGround(); //Detectar suelo ....traducido
        HandleWall(); //Detectar paretes 
       // handelWallSlide();
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
    private void HandleWall()
    {
        isWallDetected = Physics2D.Raycast(m_transform.position, Vector2.right * direction, checkWallDistance, groundLayer);
    }
    private void handelWallSlide()
    {
        canWallSlide = isWallDetected;
        if (!canWallSlide)return;
        speedSlice = m_gaderInput.Value.y < 0 ? 1 : 0.5f;
        m_rigidbody.linearVelocity = new Vector2 (m_rigidbody.linearVelocity.x, m_rigidbody.linearVelocity.y * speedSlice);
    }
    private void Move()
    {
        if (!canMove) return;
        if (isWallDetected && !isGrounded) return;
        if (isWasJumping) return;
        Flip();
        m_rigidbody.linearVelocity = new Vector2(speed * m_gaderInput.Value.x, m_rigidbody.linearVelocityY); //Movimiento en eje X del personaje 
    }
    private void Flip()
    {
      if(m_gaderInput.Value.x * direction < 0)
       {
            HandleFlipDirection();
       }
    }
    private void HandleFlipDirection()
    {
        m_transform.localScale = new Vector3(-m_transform.localScale.x, 1, 1);
        direction *= -1;
    }
    private void Jump()
    {
        if(m_gaderInput.IsJumping)
        {
            if (isGrounded)
            {
                m_rigidbody.linearVelocity = new Vector2(speed * m_gaderInput.Value.x, jumpForce);
                canDoubleJump = true;
            }
            else if (isWallDetected) WallJump();
            else if (counterExtraJump > 0 && canDoubleJump) DobleJump();
        }  
        m_gaderInput.IsJumping = false;
    }
    private void Atack()
    {        
        if (canWallSlide && !isGrounded) return;
        if (isKnocked) return;
        if (m_gaderInput.IsAtack && canHit)
        {           
            m_animator.SetTrigger(idIsAtack);
            StartCoroutine(CanAtackRoutine());
            StartCoroutine(AtackRutine());

        }        
         //Debug.Log(atacking);
        m_gaderInput.IsAtack = false;
    }
    private IEnumerator CanAtackRoutine()
    {
        canHit = false;
       yield return new WaitForSeconds(canAtackDelay);
        canHit = true;
    }

    private IEnumerator AtackRutine()
    {
        isAtack = true;
        m_ColliderDamage.enabled = true; //Activar collider de daño
        yield return new WaitForSeconds(atackColliderDelay); //Tiempo de animacion de ataque
        m_ColliderDamage.enabled = false; //Desactivar collider de daño
        isAtack = false;
    }

    private void WallJump()
    {
        m_rigidbody.linearVelocity = new Vector2(wallJumpForce.x * -direction, wallJumpForce.y);
        HandleFlipDirection();
        StartCoroutine(WallJumpRutine());
    }
    IEnumerator WallJumpRutine()
    {
        isWasJumping = true;        
        yield return new WaitForSeconds(wallJumpDuration);
        isWasJumping = false;
    }
    private void DobleJump()
    {
        m_rigidbody.linearVelocity = new Vector2(speed * m_gaderInput.Value.x, jumpForce);
        counterExtraJump--;
    }
    public void KnowcBack(float sourceDamageXPosition)
    {
        float direction = 1;

        if(transform.position.x < sourceDamageXPosition)
            direction = -1;

        StartCoroutine(KnockBackRutine());
        m_rigidbody.linearVelocity = new Vector2 (knockedPower.x *  direction, knockedPower.y);
        //m_animator.SetTrigger(idKnockBack);
    }
    private IEnumerator KnockBackRutine()
    {
       isKnocked = true;
       m_animator.SetBool(idKnockBack, isKnocked);
       yield return new WaitForSeconds(knockedDuration);
       isKnocked = false;
       m_animator.SetBool(idKnockBack, isKnocked);
       knockedPower = new Vector2(defaulKnockedPower.x, defaulKnockedPower.y);
    }

    public void Die() 
    {
        GameObject vfxPrefab = Instantiate(deathVfx,m_transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void Push(Vector2 direction , float duration = 0)
    {
        StartCoroutine(PushCoroutine(direction,duration));
    }
    public IEnumerator PushCoroutine(Vector2 direction, float duration)
    {
        canDoubleJump = true;
        canMove = false;       
        m_rigidbody.linearVelocity = Vector2.zero;
        m_rigidbody.AddForce(direction, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);                
        canMove = true;
    }
    internal void DoorIn()
    {
        m_rigidbody.linearVelocity = Vector2.zero;
        m_animator.Play(idIdle);
        m_animator.SetBool(idDoorIn, true);
        canMove = false;
        StartCoroutine(DoorInRotuine());
    }
    private IEnumerator DoorInRotuine()
    {
      yield return new WaitForSeconds(moveDelay);
        SceneManager.LoadScene(0);
        canMove = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_transform.position, new Vector2(m_transform.position.x + checkWallDistance * direction, m_transform.position.y));
    }

}
