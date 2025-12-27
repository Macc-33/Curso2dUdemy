using UnityEngine;
using UnityEngine.InputSystem;
public class GaderInput : MonoBehaviour
{
    private Controls controls;
    [SerializeField] private float _valueX;

    public float ValueX { get => _valueX;  }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        controls = new Controls();
    }
    private void OnEnable()
    {
        controls.Player.Move.performed += StarMove;
        controls.Player.Move.canceled += StopMove;
        controls.Player.Enable();
    }
    private void StarMove(InputAction.CallbackContext context)
    {
        _valueX = context.ReadValue<float>();
    }
    private void StopMove(InputAction.CallbackContext context)
    {
        _valueX = 0;
    }
    private void OnDisable()
    {
        controls.Player.Move.performed -= StarMove;
        controls.Player.Move.canceled -= StopMove;
        controls.Player.Disable();  
    }
}
