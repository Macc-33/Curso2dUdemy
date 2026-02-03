using System.Collections;
using UnityEngine;

public class SawController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform[] myWayPoints;
    [SerializeField] private Vector2[] myWayPointsPosition;
    [SerializeField] private int indexWayPoints = 1;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float waitForMove = 1;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private int moveDirection = 1;
    private int idSawActive = Animator.StringToHash("SawActive");


    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateWayPoint();
        spriteRenderer.flipX = true;
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
        animator.SetBool(idSawActive, canMove);
        if (!canMove) return;

        transform.position = Vector2.MoveTowards(transform.position, myWayPointsPosition[indexWayPoints],speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, myWayPointsPosition[indexWayPoints]) < 0.1)
        {
            if (indexWayPoints == myWayPointsPosition.Length -1 || indexWayPoints == 0)
            {
                moveDirection = moveDirection * -1;
                StartCoroutine(StopMovenment(waitForMove));
            }
            indexWayPoints = indexWayPoints + moveDirection;
        }
        

     
    }
    IEnumerator StopMovenment(float delayTime)
    {
        canMove = false;
        yield return new WaitForSeconds(delayTime);
        canMove = true;
        spriteRenderer.flipX = !spriteRenderer.flipX; //cuando la sierra llega a un extremo hace el cambio de direccion 
    }
}
