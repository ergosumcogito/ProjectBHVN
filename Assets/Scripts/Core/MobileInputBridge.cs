using UnityEngine;

public class MobileInputBridge : MonoBehaviour
{
    // [SerializeField] private Joystick joystick;       
    // [SerializeField] private InputReader inputReader;
    // [SerializeField] private float deadZone = 0.1f;

    private void Awake()
    {
        // Hide joystick if not mobile
       // joystick.gameObject.SetActive(Application.isMobilePlatform || Application.isEditor);
    }

    private void Update()
    {
        // Vector2 joyInput = joystick.GetInput();
        //
        // // Apply dead zone
        // if (joyInput.magnitude < deadZone) joyInput = Vector2.zero;
        //
        // // Override InputReader's MovementInput on mobile
        // inputReader.OverrideMovementInput(joyInput);
    }
}