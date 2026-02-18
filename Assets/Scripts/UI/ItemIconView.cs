using UnityEngine;
using UnityEngine.UI;

public class ItemIconView : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public void Setup(Sprite icon)
    {
        iconImage.sprite = icon;
    }
}