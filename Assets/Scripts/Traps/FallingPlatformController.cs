using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatformController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D[] _BoxCollider;
    [Space]
    [Header("Platform Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    private Vector3[] _WayPoints;
    private int _WayPointsIndex;
    private bool canMove = false;
    [Space]
    [Header("Fall Platform Settings")]
    [SerializeField] private bool canFall;
    [SerializeField] private float fallDelay;
    [Space]
    [SerializeField] private float impactSpeed;
    [SerializeField] private float impactDuration;
    private float impactTimer;
    private bool impactHappened;
    [Space]
    [Header("RespwnSettings")]
    [SerializeField] private bool canRespwn = true;
    [SerializeField] private float respwnDelay = 2f;
    [SerializeField] private float respwnAfterFallTime = 3f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _BoxCollider = GetComponents<BoxCollider2D>();
    }
    private void Start()
    {
        SetUpWayPoints();
        float randomDelay = Random.Range(0, 0.6f);
        Invoke(nameof(ActivatePlatform), randomDelay);
    }
    private void Update()
    {
        HandleImpact();
        HandleMovenment();
    }
    private void ActivatePlatform() => canMove = true;
    private void SetUpWayPoints()
    {
        _WayPoints = new Vector3[2];
        float yOfset = distance / 2;
        _WayPoints[0] = transform.position + new Vector3 (0, yOfset, 0);
        _WayPoints[1] = transform.position + new Vector3 (0, -yOfset, 0);
    }
    private void HandleMovenment()
    {
        if (!canMove) return;
        transform.position = Vector2.MoveTowards(transform.position, _WayPoints[_WayPointsIndex], speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _WayPoints[_WayPointsIndex]) < 0.1)
        {
            _WayPointsIndex++;
            if(_WayPointsIndex >= _WayPoints.Length)
            {
                _WayPointsIndex = 0;
            }
        }
    }
    private void HandleImpact()
    {
        if(impactTimer< 0)
        {
            return;
        }
        impactTimer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,transform.position +(Vector3.down *10),impactSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player == null) return;

        if (!canFall) return;

        if (impactHappened) return;

        Invoke(nameof(SwithOffPlatform), fallDelay);      
        
        impactTimer = impactDuration;

        impactHappened = true;
    }
    private void SwithOffPlatform()
    {
        _animator.SetTrigger("Deactivate");
        canMove = false;

        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.gravityScale = 3.5f;
        _rigidbody.linearDamping = 0.05f;
        
        foreach(BoxCollider2D collider  in _BoxCollider)
        {
            collider.enabled = false;
        }
        if (!canFall) return ;

        if (canRespwn)
        {
            GameObject fallingPlattForm = GameManager.instance.fallingPlatformPrefab;
            Vector3 respwnposition = transform.position;
            GameManager.instance.CreateObject(GameManager.instance.fallingPlatformPrefab, respwnposition, respwnDelay);
            Invoke(nameof(DestroyPlatform), respwnDelay);
        }
        else
        {
            Invoke(nameof(DestroyPlatform), respwnAfterFallTime);
        }
    }
    private void DestroyPlatform()
    {
       Destroy(gameObject);
    }
}
