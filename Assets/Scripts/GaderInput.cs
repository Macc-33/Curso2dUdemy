using UnityEngine;
using UnityEngine.InputSystem;
public class GaderInput : MonoBehaviour
{
    private Controls controls;
    [SerializeField] private float _valueX;

    public float ValueX { get => _valueX;  }

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
        _valueX = context.ReadValue<float>();
    }
    private void StopMove(InputAction.CallbackContext context)
    {
        _valueX = 0;
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
