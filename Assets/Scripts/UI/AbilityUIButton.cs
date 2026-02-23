using UnityEngine;

public class AbilityUIButton : MonoBehaviour
{
    public enum AbilityType
    {
        Flash,
        Heal
    }

    [SerializeField] private AbilityType abilityType;

    public void OnClick()
    {
        if (abilityType == AbilityType.Flash)
            InputReader.Instance.TriggerFlash();

        if (abilityType == AbilityType.Heal)
            InputReader.Instance.TriggerHeal();
    }
}