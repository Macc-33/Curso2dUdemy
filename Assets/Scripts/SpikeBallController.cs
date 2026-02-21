using UnityEngine;

public class SpikeBallController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _Rigidbody;
    [SerializeField] private float pushForce;

    private void Start()
    {
        Vector2 pushVector = new Vector2(pushForce,0);
        _Rigidbody.AddForce(pushVector, ForceMode2D.Impulse);
    }
}
