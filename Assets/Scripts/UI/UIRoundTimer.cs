using UnityEngine;
using TMPro;

public class UIRoundTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float remainingTime;
    private bool isActive;

    private void OnEnable()
    {
        RoundEvents.OnRoundStart += StartTimer;
        RoundEvents.OnRoundEnd += StopTimer;
    }

    private void OnDisable()
    {
        RoundEvents.OnRoundStart -= StartTimer;
        RoundEvents.OnRoundEnd -= StopTimer;
    }

    private void StartTimer(float duration)
    {
        remainingTime = duration;
        isActive = duration > 0f;

        timerText.enabled = isActive;

        if (isActive)
        {
            UpdateTimerText();
        }
    }

    private void StopTimer()
    {
        isActive = false;
    }

    private void Update()
    {
        if (!isActive) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime < 0)
        {
            remainingTime = 0;
        }

        UpdateTimerText();
    }
    
    private void UpdateTimerText()
    {
        int seconds = Mathf.FloorToInt(remainingTime);
        timerText.text = seconds.ToString();
    }
}