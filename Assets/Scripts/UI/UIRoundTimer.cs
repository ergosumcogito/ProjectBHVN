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
        isActive = true;
    }

    private void StopTimer()
    {
        isActive = false;
    }

    private void Update()
    {
        if (!isActive) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime < 0) remainingTime = 0;

        int seconds = Mathf.FloorToInt(remainingTime);
        timerText.text = seconds.ToString();
    }
}