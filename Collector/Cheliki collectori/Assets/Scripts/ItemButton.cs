using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private Button button;

    [SerializeField] private TextMeshProUGUI txtItemName;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private TextMeshProUGUI txtUpgrade;

    private ItemData itemData;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
    }

    public void Init(ItemData newItemData)
    {

        itemData = newItemData;
        SetNewValues();
    }

    public void SetNewValues()
    {
        if (itemData == null)
        {
            Debug.LogError("itemData == null");
            return;
        }

        txtItemName.text = $"{itemData.ItemName}";
        image.sprite = itemData.Sprite;
        txtPrice.text = $"{itemData.PriceCurrent}";
        txtUpgrade.text = $"{itemData.UpgradeCurrentValue}/{itemData.UpgradeMaxValue}";
    }

    public void ButtonClick()
    {
        if (itemData == null)
        {
            Debug.LogError("itemData == null");
            return;
        }

        if (itemData.ButtonClick()) SetNewValues();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}