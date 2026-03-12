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
    [SerializeField] private EnemyPlayerDetect _enemyPlayerDetect;
    [SerializeField] public Transform player;
    [Space]
    [Header("HitBack Settings ")]
    [SerializeField] private bool isKnocked;
    [SerializeField] private Vector2 knockedPower;
    [SerializeField] private Vector2 defaulKnockedPower;
    [SerializeField] private float knockedDuration;
    [Space]
    [Header("Animations Settings ")]
    private int idEnemyRun = Animator.StringToHash("EnemyRun");
    private int idKnockBack = Animator.StringToHash("HitBack");

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _enemyPlayerDetect = GetComponentInChildren<EnemyPlayerDetect>();       
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
        Flip();        
        if (!canMove) return; 
        if(isKnocked) return;
        Movenment();  
        Atack();
        //Debug.Log();
    }
    private void Flip()
    {
        /*if (moveDirection == -1) spriteRenderer.flipX = true;
        if (moveDirection == 1) spriteRenderer.flipX = false;*/

        float direction;

        if (canAtack && player != null)
        {
            direction = player.position.x - transform.position.x;
        }
        else
        {
            direction = myWayPointsPosition[indexWayPoints].x - transform.position.x;
        }

        if (direction > 0)
        {
            StartCoroutine(FlipDelayCorutine(flipDelay));
            _SpriteRenderer.flipX = true;
        }                       
        else if (direction < 0)
        {
            StartCoroutine(FlipDelayCorutine(flipDelay));
            _SpriteRenderer.flipX = false;
        }
            
    }
    private IEnumerator FlipDelayCorutine(float delay)
    {
        yield return new WaitForSeconds(1);
    }
    private void Movenment()
    {
        if(isKnocked) return;
        canAtack = _enemyPlayerDetect.canAtack;
        if (canAtack == true) return;

        float direction = Mathf.Sign(myWayPointsPosition[indexWayPoints].x - transform.position.x);

        _Rigidbody.linearVelocity = new Vector2(direction * normalSpeed, _Rigidbody.linearVelocity.y);

        //transform.position = Vector2.MoveTowards(transform.position, myWayPointsPosition[indexWayPoints], normalSpeed * Time.deltaTime);                       
        //transform.position = Vector2.MoveTowards(transform.position, player.transform.position , speed * Time.deltaTime) ;        
    }
    private void FixedUpdate()
    {
        StopBetweenPoints();
        SetAnimationValues();
    }

    private void StopBetweenPoints()
    {
        if (canAtack) return;
        if(isKnocked) return ;
        if (Vector2.Distance(transform.position, myWayPointsPosition[indexWayPoints]) < 0.1)
        {
            if (indexWayPoints == myWayPointsPosition.Length - 1 || indexWayPoints == 0)
            {

                StartCoroutine(StopMovenment(waitForMove));
              
            }
            indexWayPoints = indexWayPoints + moveDirection;
        }
    }

    IEnumerator StopMovenment(float delayTime)
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
    }
    private void Atack()
    {
        if (!canAtack) return;
        if (isKnocked) return;

        float direction = Mathf.Sign(player.position.x - transform.position.x);

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
