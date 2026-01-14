using System.Collections;
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
    [SerializeField] private TextMeshProUGUI txtUpgradeCounter;
    [SerializeField] private TextMeshProUGUI txtUpgradeValue;

    public ItemData itemData { get; protected private set; }

    private Color txtPriceColorDefault;
    [SerializeField] private Color txtPriceColorError = Color.red;
    [SerializeField] private float timeForPriceError = 2;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);

        txtPriceColorDefault = txtPrice.color;
        txtUpgradeValue.transform.parent.gameObject.SetActive(false);
    }

    public void Init(ItemData newItemData)
    {
        itemData = newItemData;
        

        if (itemData is ItemUpgradeData itemUpgradeData)
        {
            txtUpgradeValue.transform.parent.gameObject.SetActive(true);
            txtUpgradeValue.text = $"{itemUpgradeData.SpecialModifier}";
        }

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
        txtPrice.text = $"{itemData.PriceCurrent}$";
        txtUpgradeCounter.text = $"{itemData.UpgradeCurrentValue}/{itemData.UpgradeMaxValue}";
    }

    public void ButtonClick()
    {
        if (itemData == null)
        {
            Debug.LogError("itemData == null");
            return;
        }

        if (itemData.ButtonClick())
            SetNewValues();
        else
            StartCoroutine(PriceRedIndicator());
    }

    private IEnumerator PriceRedIndicator()
    {
        txtPrice.color = txtPriceColorError;

        yield return new WaitForSeconds(timeForPriceError);

        txtPrice.color = txtPriceColorDefault;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }
}