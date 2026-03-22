using System;
using System.Collections;
using UnityEngine;

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
    [SerializeField] private PlayerController _playerController;
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

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        //_playerController = FindObjectOfType<PlayerController>();
        _playerController = FindAnyObjectByType<PlayerController>();
        _enemyPlayerDetect = GetComponentInChildren<EnemyPlayerDetect>();
        _damageEnemy_1 = GetComponentInChildren<DamageEnemy_1>();
        playerPoint = _playerController.Transform;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       canMove= true;
       canAtack = false;        
       UpdateWayPoint();
       indexWayPoints = 1;
       transform.position = myWayPointsPosition[0];
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
        if (!canMove) return; 
        if(isKnocked) return;
        Movenment();  
        Atack();
        if (_enemyPlayerDetect.NewUpdatePointposition)
        {
            UpdateWayPoint();      
            _enemyPlayerDetect.NewUpdatePointposition = false;
        }
    }

    private void FixedUpdate()
    {
        Flip();
        StopBetweenPoints();
        SetAnimationValues();
    }
  
    private void Flip()
    {
        float direction;

        if (canAtack && playerPoint != null)
            direction = playerPoint.position.x - transform.position.x;
        else
            direction = myWayPointsPosition[indexWayPoints].x - transform.position.x;

        _SpriteRenderer.flipX = direction > 0;
    }
    private IEnumerator FlipDelayCorutine(float delay)
    {
        yield return new WaitForSeconds(1);
    }
    private void Movenment()
    {        
        EnemyRutine();       
    }
    void EnemyRutine()
    {
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
        if (Vector2.Distance(transform.position, myWayPointsPosition[indexWayPoints]) < 0.3)
        {
            if (indexWayPoints == myWayPointsPosition.Length - 1 || indexWayPoints == 0)
            {

                StartCoroutine(StopBetweenPointsRoutine(waitForMove));
              
            }
            indexWayPoints = indexWayPoints + moveDirection;
        }
    }

    IEnumerator StopBetweenPointsRoutine(float delayTime)
    {
        canMove = false;
        moveDirection = moveDirection * -1;
        yield return new WaitForSeconds(delayTime);
        canMove = true;
        //spriteRenderer.flipX = !spriteRenderer.flipX; //cuando la sierra llega a un extremo hace el cambio de direccion 
    }
    private void SetAnimationValues()
    {
        _Animator.SetBool(idEnemyRun, canMove);
        _Animator.SetBool(idKnockBack, isKnocked);  
        _Animator.SetBool(idOnAtack, isAtack);
    }
    private void Atack()
    {
        if (playerPoint == null) return;
        if (!canAtack) return;
        if (isKnocked) return;
        isAtack = _damageEnemy_1.IsAtack;
        float direction = Mathf.Sign(playerPoint.position.x - transform.position.x);

        _Rigidbody.linearVelocity = new Vector2(direction * atackSpeed, _Rigidbody.linearVelocity.y);     
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
