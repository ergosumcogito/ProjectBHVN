using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text price;
    [SerializeField] private Transform statsContainer;
    [SerializeField] private TMP_Text statLinePrefab;
    [SerializeField] private Button buyButton;

    private ItemData itemData;

    public void Init(ItemData data)
    {
        itemData = data;

        icon.sprite = data.icon;
        title.text = data.itemName;
        price.text = data.price.ToString();

        BuildStats(data);
        buyButton.onClick.AddListener(OnBuyClicked);
    }

    private void BuildStats(ItemData data)
    {
        // cleaning
        foreach (Transform child in statsContainer)
            Destroy(child.gameObject);

        foreach (var modifier in data.modifiers)
        {
            var line = Instantiate(statLinePrefab, statsContainer);
            line.text = FormatModifier(modifier);
        }
    }

    private string FormatModifier(StatModifier modifier)
    {
        // formatting
        string sign = modifier.value > 0 ? "+" : "";
        return $"{sign}{modifier.value} {modifier.stat}";
    }

    private void OnBuyClicked()
    {
        Debug.Log($"Buy: {itemData.itemName}");
        // here goes buy logic
    }
}