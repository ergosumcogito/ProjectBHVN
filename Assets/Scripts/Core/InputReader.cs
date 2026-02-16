using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public static InputReader Instance { get; private set; }
    
    private PlayerInputActions _inputActions;

    private Vector2 keyboardInput;
    private Vector2 joystickInput;
    
    private bool joystickActive = false;
    
    public Vector2 MovementInput =>
        joystickActive ? joystickInput : keyboardInput;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
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
        keyboardInput = context.ReadValue<Vector2>();
    }
    
    // Called by joystick system
    public void SetJoystickState(bool active)
    {
        joystickActive = active;

        if (!active)
            joystickInput = Vector2.zero;
    }
    
    public void SetJoystickInput(Vector2 input)
    {
        joystickInput = input;
    }

    public void ResetAllInput()
    {
        keyboardInput = Vector2.zero;
        joystickInput = Vector2.zero;
        joystickActive = false;
    }

}