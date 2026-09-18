using System;
using UnityEngine;

public class Souls : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Rigidbody2D m_rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private SoulType soulType;
    private int idPickedSoul;
    private int idSoulIndex;

    private void Awake()
    {
        //gameManager = GameManager.instance;
        m_rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        idPickedSoul = Animator.StringToHash("PickedDiamond");
        idSoulIndex = Animator.StringToHash("DiamondIndex");
    }
    private void Start()
    {
       
        SetRandomSoul();
    }
    private void SetRandomSoul()
    {
        if (!GameManager.instance.SoulHaveRandomLook1)
        {
            UpdateSoulType();

            return;
        }

        var randomSoulIndex = UnityEngine.Random.Range(0, 4);
        animator.SetFloat(idSoulIndex, randomSoulIndex);
    }
    private void UpdateSoulType()
    {
        animator.SetFloat(idSoulIndex, (int)soulType);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") ||
            collision.CompareTag("PlayerDamage_1"))
        {
            m_rigidBody.simulated = false;
            GameManager.instance.AddSoul();
            animator.SetTrigger(idPickedSoul);
        }
    }
}
