using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class JoystickSpawner : MonoBehaviour
{
    [SerializeField] private Joystick joystickPrefab;
    [SerializeField] private Canvas canvas;

    private Joystick currentJoystick;
    private int activeFingerId = -1;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable(); // for testing in editor
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            // 1️⃣ Touch began → spawn joystick on left half
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began &&
                touch.screenPosition.x < Screen.width / 2f)
            {
                SpawnJoystick(touch);
            }

            // 2️⃣ Update joystick position while finger is active
            if (touch.finger.index == activeFingerId &&
                (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                 touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary))
            {
                if (currentJoystick != null)
                    currentJoystick.UpdateJoystick(touch.screenPosition, canvas.worldCamera);
            }

            // 3️⃣ Touch ended → reset and destroy joystick
            if (touch.finger.index == activeFingerId &&
                (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                 touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled))
            {
                if (currentJoystick != null)
                    currentJoystick.ResetJoystick();

                DestroyJoystick();
            }
        }
    }

    private void SpawnJoystick(Touch touch)
    {
        if (currentJoystick != null)
            return;

        activeFingerId = touch.finger.index;

        currentJoystick = Instantiate(joystickPrefab, canvas.transform);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            touch.screenPosition,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        currentJoystick.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }

    private void DestroyJoystick()
    {
        if (currentJoystick != null)
            Destroy(currentJoystick.gameObject);

        currentJoystick = null;
        activeFingerId = -1;
    }
}
