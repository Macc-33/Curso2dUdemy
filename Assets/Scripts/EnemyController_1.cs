using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class EnemyController_1 : MonoBehaviour
{
    [Header("Enemy Settings")]
    private Rigidbody2D _Rigidbody;
    private SpriteRenderer _SpriteRenderer;
    private Animator _Animator;
    private int moveDirection = 1;
    [Space]
    [Header("Run,Walk and Idle settings")]
    [SerializeField] private float normalSpeed;
    [SerializeField] private float flipDelay;
    [SerializeField] private float waitForMove = 1;
    [SerializeField] private Transform[] myWayPoints;
    [SerializeField] private Vector2[] myWayPointsPosition;
    [SerializeField] private int indexWayPoints = 1;
    [SerializeField] private bool canMove = true;
    [Space]
    [Header("Atack Settings")]
    [SerializeField] private float atackSpeed;
    [SerializeField] private bool canAtack;
    [SerializeField] private bool isAtack;
    [SerializeField] private PlayerController _player;
    [SerializeField] private EnemyPlayerDetect _enemyPlayerDetect;
    [SerializeField] private DamageEnemy_1 _damageEnemy_1;
    [SerializeField] private Transform playerPoint;
    [Space]
    [Header("HitBack Settings ")]
    [SerializeField] public bool isKnocked;
    [SerializeField] private Vector2 knockedPower;
    [SerializeField] private Vector2 defaulKnockedPower;
    [SerializeField] private float knockedDuration;
    [Space]
    [Header("Animations Settings ")]
    private int idEnemyRun = Animator.StringToHash("EnemyRun");
    private int idKnockBack = Animator.StringToHash("HitBack");
    private int idOnAtack = Animator.StringToHash("OnAtack");
    private enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Knockback
    }
    [Header("FSM Settings ")]   
    [SerializeField] private EnemyState currentState;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _player = FindAnyObjectByType<PlayerController>();
        _enemyPlayerDetect = GetComponentInChildren<EnemyPlayerDetect>();
        _damageEnemy_1 = GetComponentInChildren<DamageEnemy_1>();
        playerPoint = _player.Transform;
    }
    private void OnEnable()
    {
        GameManager.OnPlayerSpawned += SetPlayerReference;
    }
    private void OnDisable()
    {
        GameManager.OnPlayerSpawned -= SetPlayerReference;
    }
    private void SetPlayerReference(PlayerController newPlayer)
    {
        _player = newPlayer;

        playerPoint = _player.transform;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       canMove= true;
       canAtack = false;        
       UpdateWayPoint();
       indexWayPoints = 1;
       transform.position = myWayPointsPosition[0];
       currentState = EnemyState.Patrol;
    }
    private void UpdateWayPoint()
    {
        myWayPointsPosition = new Vector2[myWayPoints.Length];

        for (int i = 0; i < myWayPoints.Length; i++)
        {
            myWayPointsPosition[i] = myWayPoints[i].position;
        }
    }
    // Update is called once per frame
    void Update()
    {
        HandleStateTransitions();
    }
    private void HandleStateTransitions()
    {
        // Verificar si el enemigo está en knockback
        if (isKnocked)
        {
            ChangeState(EnemyState.Knockback);
            return;
        }

        // Verificar si el enemigo puede atacar al player
        canAtack = _enemyPlayerDetect.CanAtack;

        if (canAtack)
        {
            ChangeState(EnemyState.Attack);
            return;
        }
      
        // Verificar si el player está dentro del rango de persecución
        if (playerPoint != null)
        {
            float distanceToPlayer = Vector2.Distance(playerPoint.position, transform.position);

            if (distanceToPlayer < 3f) // Rango de chase
            {
                ChangeState(EnemyState.Chase);
                return;
            }
            else // Player fuera del rango
            {
                // Actualizar waypoints si el player salió de la zona
                // NUEVO: player salió del área
                if (_enemyPlayerDetect.NewUpdatePointposition)
                {
                    UpdateWayPoint();
                    _enemyPlayerDetect.NewUpdatePointposition = false;
                    indexWayPoints = GetClosestWaypoint();
                    indexWayPoints = Mathf.Clamp(indexWayPoints, 0, myWayPointsPosition.Length - 1);
                }

                ChangeState(EnemyState.Patrol); // Volver al patrullaje
                return;
            }
        }
        // Si no hay player
        ChangeState(EnemyState.Patrol);

        // Verificar si el player está dentro del rango de persecución
        if (playerPoint != null)
        {
            float distance = Mathf.Abs( playerPoint.position.x - transform.position.x);

            if (distance < 5f)
            {
                ChangeState(EnemyState.Chase);
                return;
            }
        }

        // Si no se cumplen las condiciones anteriores, el enemigo patrulla
        if (canMove)
            ChangeState(EnemyState.Patrol);
        else
            ChangeState(EnemyState.Idle);
    }
    private int GetClosestWaypoint()
    {
        float closestDistance = Mathf.Infinity;

        int closestIndex = 0;

        for (int i = 0; i < myWayPointsPosition.Length; i++)
        {
            float distance = Vector2.Distance( transform.position, myWayPointsPosition[i]);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;

            case EnemyState.Patrol:
                HandlePatrolState();
                break;

            case EnemyState.Chase:
                HandleChaseState();
                break;

            case EnemyState.Attack:
                HandleAttackState();
                break;

            case EnemyState.Knockback:
                HandleKnockbackState();
                break;
        }

        Flip();
        SetAnimationValues();
    }

    private void HandleIdleState() // El enemigo se queda quieto
    {
        _Rigidbody.linearVelocity =  new Vector2(0,_Rigidbody.linearVelocity.y);
    }
    private void HandlePatrolState() // El enemigo se mueve entre los puntos de patrulla
    {
        canMove = true; // asegurar que puede moverse
        EnemyRutine();
        StopBetweenPoints();
    }
    void EnemyRutine()
    {
        if (myWayPointsPosition.Length == 0) return;
        if (isKnocked) return;
        canAtack = _enemyPlayerDetect.CanAtack;
        if (canAtack == true) return;

        float direction = Mathf.Sign(myWayPointsPosition[indexWayPoints].x - transform.position.x);

        _Rigidbody.linearVelocity = new Vector2(direction * normalSpeed, _Rigidbody.linearVelocity.y);
    }
    private void StopBetweenPoints()
    {
        if (canAtack) return;
        if(isKnocked) return ;
        if (myWayPointsPosition.Length == 0) return;
        if (Vector2.Distance(transform.position, myWayPointsPosition[indexWayPoints]) < 0.3)
        {
            if (indexWayPoints == myWayPointsPosition.Length - 1 || indexWayPoints == 0)
            {

                StartCoroutine(StopBetweenPointsRoutine(waitForMove));
              
            }
            indexWayPoints = indexWayPoints + moveDirection;
            indexWayPoints = Mathf.Clamp(indexWayPoints, 0, myWayPointsPosition.Length - 1);
        }
    }
    private void HandleChaseState() // El enemigo persigue al jugador
    {
        if (playerPoint == null)return;

        float direction = Mathf.Sign(playerPoint.position.x - transform.position.x);

        _Rigidbody.linearVelocity = new Vector2(direction * normalSpeed,_Rigidbody.linearVelocity.y);
    }
    private void HandleAttackState()
    {
        if (playerPoint == null) return;

        isAtack = _damageEnemy_1.IsAtack;

        float direction = Mathf.Sign(playerPoint.position.x - transform.position.x );

        _Rigidbody.linearVelocity = new Vector2( direction * atackSpeed, _Rigidbody.linearVelocity.y);
    }
    private void HandleKnockbackState()
    {
        // No movement here
    }
    private void Flip()
    {
        if (myWayPointsPosition.Length == 0) return;
        float direction;

        if (canAtack && playerPoint != null)
            direction = playerPoint.position.x - transform.position.x;
        else
            direction = myWayPointsPosition[indexWayPoints].x - transform.position.x;

        _SpriteRenderer.flipX = direction > 0;
    }
    private void SetAnimationValues()
    {
        _Animator.SetBool(idEnemyRun, canMove);
        _Animator.SetBool(idKnockBack, isKnocked);  
        _Animator.SetBool(idOnAtack, isAtack);
    }
    IEnumerator StopBetweenPointsRoutine(float delayTime)
    {
        canMove = false;
        moveDirection = moveDirection * -1;
        yield return new WaitForSeconds(delayTime);
        canMove = true;
        //spriteRenderer.flipX = !spriteRenderer.flipX; //cuando la sierra llega a un extremo hace el cambio de direccion 
    }
    public void KnowcBack(float sourceDamageXPosition)
    {     
        float direction = 1;

        if (transform.position.x < sourceDamageXPosition)
            direction = -1;

        StartCoroutine(KnockBackRutine());
        _Rigidbody.linearVelocity = new Vector2(knockedPower.x * direction, knockedPower.y);
    }
    private IEnumerator KnockBackRutine()
    {
        isKnocked = true;        
        yield return new WaitForSeconds(knockedDuration);
        isKnocked = false;
    }
 
 


}
