using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core.PlayerLogic.Abilities;

public class AbilityUIButton : MonoBehaviour
{
    public enum AbilityType
    {
        Flash,
        Heal
    }

    [Header("Config")]
    [SerializeField] private AbilityType abilityType;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI cooldownText;

    private PlayerAbilitySlot abilitySlot;
    private Color _defaultColor;

    private void Awake()
    {
        if (buttonImage == null)
            buttonImage = button.GetComponent<Image>();

        _defaultColor = buttonImage.color;
    }

    private void OnEnable()
    {
        TryInitialize();
    }

    private void Update()
    {
        if (abilitySlot == null)
        {
            TryInitialize();
            return;
        }

        UpdateCooldownVisual();
    }

    private void TryInitialize()
    {
        abilitySlot = null;

        var slots = FindObjectsOfType<PlayerAbilitySlot>();
        
        // Debug.Log($"[{abilityType}] Found {slots.Length} ability slots in scene");


        foreach (var slot in slots)
        {
            if (abilityType == AbilityType.Flash &&
                slot.Ability is FlashAbility)
            {
                abilitySlot = slot;
                break;
            }

            if (abilityType == AbilityType.Heal &&
                slot.Ability is HealAbility)
            {
                abilitySlot = slot;
                break;
            }
        }
        
        // if (abilitySlot == null)
        //     Debug.LogWarning($"[{abilityType}] No ability slot found!");
    }

    public void OnClick()
    {
        if (abilityType == AbilityType.Flash)
            InputReader.Instance.TriggerFlash();

        if (abilityType == AbilityType.Heal)
            InputReader.Instance.TriggerHeal();
    }

    private void UpdateCooldownVisual()
    {
        float remaining = abilitySlot.CooldownRemaining;

        if (remaining > 0f)
        {
            button.interactable = false;

            // Fade button image manually
            var c = _defaultColor;
            c.a = 0.6f;
            buttonImage.color = c;

            cooldownText.gameObject.SetActive(true);
            cooldownText.text = Mathf.CeilToInt(remaining).ToString();
        }
        else
        {
            button.interactable = true;
            buttonImage.color = _defaultColor;

            cooldownText.gameObject.SetActive(false);
        }
    }
}