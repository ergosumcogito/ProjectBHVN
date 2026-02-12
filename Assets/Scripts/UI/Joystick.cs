using UnityEngine;

public class Joystick : MonoBehaviour
{
    [Header("Joystick Settings")]
    public RectTransform joystickHandle;
    public float handleRange = 50f;

    private Vector2 input = Vector2.zero;

    public Vector2 GetInput() => input;

    // Update joystick handle based on screen position
    public void UpdateJoystick(Vector2 screenPosition, Camera cam)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            screenPosition,
            cam,
            out Vector2 localPoint
        );

        // Clamp handle movement
        localPoint = Vector2.ClampMagnitude(localPoint, handleRange);

        // Move the handle
        joystickHandle.anchoredPosition = localPoint;

        // Normalize input (-1 to 1)
        input = localPoint / handleRange;
    }

    // Reset joystick when touch ends
    public void ResetJoystick()
    {
        input = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }
}