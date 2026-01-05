using System.Collections;
using Core;
using UnityEngine;

// --------------------
// --- Round System ---
// --------------------
// Handles the state of the round
// Manages round timer
// Notifies everyone about round state


public enum RoundState
{
    Inactive,
    Running,
    Paused,
    Ended
}

public class RoundSystem : MonoBehaviour
{
    [SerializeField] private RoundData _roundData;

    private RoundState currentState = RoundState.Inactive;
    private float timer;
    private Coroutine roundCoroutine;
    
    public void StartRound()
    {
        if (currentState == RoundState.Running) return;

        timer = _roundData.duration;
        currentState = RoundState.Running;

       // RoundEvents.Log("Round started! Duration: " + timer + "s"); // TODO debug log
        RoundEvents.OnRoundStart?.Invoke(timer);

        // Start timer with coroutine
        roundCoroutine = StartCoroutine(RoundTimerCoroutine());
    }

    private IEnumerator RoundTimerCoroutine()
    {
        while (timer > 0 && currentState == RoundState.Running)
        {
            if (Time.timeScale > 0) // Check for pause
            {
                timer -= Time.deltaTime;
                // RoundEvents.Log($"Round Timer: {timer:F1}"); // TODO debug log
            }
            yield return null;
        }

        if (currentState == RoundState.Running)
        {
            EndRound(true); // Round completed
        }
    }

    public void PauseRound()
    {
        if (currentState != RoundState.Running) return;
        currentState = RoundState.Paused;
        RoundEvents.Log("Round paused");
    }

    public void ResumeRound()
    {
        if (currentState != RoundState.Paused) return;
        currentState = RoundState.Running;
        RoundEvents.Log("Round resumed");
    }

    public void EndRound(bool survived)
    {
        if (currentState == RoundState.Ended || currentState == RoundState.Inactive) return;

        currentState = RoundState.Ended;

        if (roundCoroutine != null)
            StopCoroutine(roundCoroutine);

        RoundEvents.Log("Round ended. Survived: " + survived);

        RoundEvents.OnRoundEnd?.Invoke();

        if (survived)
        {
            RoundEvents.OnRoundSurvived?.Invoke();
        }
        else
        {
            RoundEvents.OnRoundFailed?.Invoke();
        }
    }

    public void OnPlayerDeath()
    {
        EndRound(false);
    }
    
    private void OnEnable()
    {
        RoundEvents.OnPlayerDied += OnPlayerDeath;
    }

    private void OnDisable()
    {
        RoundEvents.OnPlayerDied -= OnPlayerDeath;
    }
    
}
