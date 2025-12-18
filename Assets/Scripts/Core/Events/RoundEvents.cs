using System;
using UnityEngine;

// --------------------
// --- Round Events ---
// --------------------
// Stores events
// Allows subscription to event independently fromRoundSystem

public static class RoundEvents
{
    public static Action<float> OnRoundStart;
    public static Action OnRoundEnd;
    public static Action OnRoundSurvived;
    public static Action OnRoundFailed;
    public static Action OnPlayerDied;

    public static void Log(string message)
    {
        Debug.Log("[RoundEvent] " + message);
    }
}