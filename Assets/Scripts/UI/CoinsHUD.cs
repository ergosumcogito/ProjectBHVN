using TMPro;
using UnityEngine;

public class CoinsHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;

    private PlayerRuntimeCurrency currency;

    public void Init(PlayerRuntimeCurrency currency)
    {
        if (this.currency != null)
            this.currency.OnCoinsChanged -= UpdateView;
        
        this.currency = currency;

        currency.OnCoinsChanged += UpdateView;
        UpdateView(currency.Coins);
    }

    private void OnDisable()
    {
        if (currency != null)
            currency.OnCoinsChanged -= UpdateView;
    }

    private void UpdateView(int amount)
    {
        coinsText.text = amount.ToString();
    }
}