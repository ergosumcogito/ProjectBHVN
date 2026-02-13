using System;
using UnityEngine;

public class PlayerRuntimeCurrency : MonoBehaviour
{
    private PlayerProgress progress;

    public int Coins => progress.coins;

    public event Action<int> OnCoinsChanged;

    public void Init(PlayerProgress progress)
    {
        this.progress = progress;
        OnCoinsChanged?.Invoke(progress.coins);
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        progress.coins += amount;
        OnCoinsChanged?.Invoke(progress.coins);
    }

    public bool TrySpendCoins(int amount)
    {
        if (amount <= 0) return true;
        if (progress.coins < amount) return false;

        progress.coins -= amount;
        OnCoinsChanged?.Invoke(progress.coins);
        return true;
    }

    public bool CanAfford(int amount)
    {
        return progress.coins >= amount;
    }
}