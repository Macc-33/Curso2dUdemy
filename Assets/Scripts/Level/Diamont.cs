using System;
using UnityEngine;

public class Diamont : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Rigidbody2D m_rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private DiamondType diamondType;
    private int idPickedDiamond;
    private int idDiamondIndex;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        idPickedDiamond = Animator.StringToHash("PickedDiamond");
        idDiamondIndex = Animator.StringToHash("DiamondIndex");
    }
    private void Start()
    {
        gameManager = GameManager.instance;
        SetRandomDiamond();
    }
    private void SetRandomDiamond()
    {
        if (!GameManager.instance.DiamondHaveRandomLook1)
        {
            UpdateDiamondType();

            return;
        }

        var randomDiamondIndex = UnityEngine.Random.Range(0, 4);
        animator.SetFloat(idDiamondIndex, randomDiamondIndex);
    }
    private void UpdateDiamondType()
    {
        animator.SetFloat(idDiamondIndex, (int)diamondType);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //spriteRenderer.enabled = false;
            m_rigidBody.simulated = false;
            gameManager.AddDiamond();
            animator.SetTrigger(idPickedDiamond);
        }
        if (collision.CompareTag("PlayerDamage_1"))
        {
            //spriteRenderer.enabled = false;
            m_rigidBody.simulated = false;
            gameManager.AddDiamond();
            animator.SetTrigger(idPickedDiamond);
        }
    }
}
