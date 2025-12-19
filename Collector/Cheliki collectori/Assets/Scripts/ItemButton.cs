using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private Button button;

    [SerializeField] private TextMeshProUGUI txtItemName;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private TextMeshProUGUI txtUpgrade;

    [SerializeField] private ItemSO itemSO;
    private ItemData itemData;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);

        itemData = itemSO.ItemData;
        SetNewValues();
    }

    //public void Init(ItemData newItemData)
    //{
    //    itemData = newItemData;
    //}

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

        if (!itemData.IsUpgradedFull() && CurrenciesWallet.Instance.SpendDollars(itemData.PriceCurrent))
        {
            if (itemData.Upgrade()) SetNewValues();
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}