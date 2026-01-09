using UnityEngine;
using UnityEngine.InputSystem;
public class GaderInput : MonoBehaviour
{
    private Controls controls;
    [SerializeField] private Vector2 _value;

    public Vector2 Value { get => _value;  }

    [SerializeField] private bool _isJumping;
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        controls = new Controls();
    }
    private void OnEnable()
    {
        controls.Player.Move.performed += StarMove;
        controls.Player.Move.canceled += StopMove;
        controls.Player.Jump.performed += StarJump;
        controls.Player.Jump.canceled += StopJump;
        controls.Player.Enable();
    }
    private void StarMove(InputAction.CallbackContext context)
    {
        _value = context.ReadValue<Vector2>().normalized; //mathf.rountoint es para redondear el porcesador del gamepad
    }
    private void StopMove(InputAction.CallbackContext context)
    {
        _value = Vector2.zero;
    }

    private void StarJump(InputAction.CallbackContext context)
    {
        _isJumping = true;
    }
    private void StopJump(InputAction.CallbackContext context)
    {
        _isJumping = false;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= StarMove;
        controls.Player.Move.canceled -= StopMove;
        controls.Player.Jump.performed -= StarJump;
        controls.Player.Jump.canceled -= StopJump;
        controls.Player.Disable();  
    }
}
