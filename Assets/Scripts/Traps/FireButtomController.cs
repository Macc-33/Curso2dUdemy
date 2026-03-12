using UnityEngine;

public class FireButtomController : MonoBehaviour
{
    [SerializeField] private Animator _Animator;
    [SerializeField] private FireController _fireController;

    private void Awake()
    {
       _Animator = GetComponent<Animator>();
        _fireController = GetComponentInParent<FireController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // PlayerController player = collision.GetComponent<PlayerController>();

       // if (player != null) return;
        if (collision.CompareTag("Player"))
        {
            _Animator.SetTrigger("Active");
            _fireController.SwitchOffFire();
        }
        if (collision.CompareTag("PlayerDamage_1"))
        {
            _Animator.SetTrigger("Active");
            _fireController.SwitchOffFire();
        }

    }
}
