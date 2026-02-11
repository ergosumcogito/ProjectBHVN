using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    public Vector2 MovementInput { get; private set; }

    private Vector2 overrideInput = Vector2.zero;
    
    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
        _inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }
    
    private void Update()
    {
        // Override movement input if set (mobile joystick)
        MovementInput = overrideInput != Vector2.zero ? overrideInput : _inputActions.Player.Move.ReadValue<Vector2>();

    }
    
    public void OverrideMovementInput(Vector2 input)
    {
        overrideInput = input;
    }

}