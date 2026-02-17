using UnityEngine;

public class TrampolineController : MonoBehaviour
{

    [SerializeField] private Animator m_Animator;
    [SerializeField] private float forceDirection;
    [SerializeField] private float duration = 0.5f;

    private  int idTrampolineActive = Animator.StringToHash("TrampolineActive");

    private void Awake()
    {
        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.gameObject.CompareTag("Player"))
            return;

        var player = collision.GetComponent<PlayerController>();
        if (player == null )
            return;

        player.Push(transform.up * forceDirection, duration);

        if (m_Animator != null)
            m_Animator.SetTrigger(idTrampolineActive);
           
    }

}
