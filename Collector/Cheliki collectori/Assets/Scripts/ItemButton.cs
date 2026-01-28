using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

public class ItemButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private ObjectSound _sound;

    [SerializeField] private TextMeshProUGUI txtItemName;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private TextMeshProUGUI txtUpgradeCounter;
    [SerializeField] private TextMeshProUGUI txtUpgradeValue;

    public ItemData itemData { get; protected private set; }
    [Header("костыль")]
    [SerializeField] private CurrenciesWallet _currenciesWallet;
    [Header("price visual")]
    [SerializeField] private float _priceCheckerCooldown = 0.25f;
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _notEnoughColor;
    [SerializeField] private Color _soldColor;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
        _currenciesWallet = FindAnyObjectByType<CurrenciesWallet>();
        _baseColor = txtPrice.color;
        
       // txtUpgradeValue.transform.parent.gameObject.SetActive(false);
    }

    public void Init(ItemData newItemData)
    {
        itemData = newItemData;
        
    /*
        if (itemData is ItemUpgradeData itemUpgradeData)
        {
            txtUpgradeValue.transform.parent.gameObject.SetActive(true);
            txtUpgradeValue.text = $"{itemUpgradeData.SpecialModifier}";
        }
        */

        SetNewValues();
        StartCoroutine(PriceChecker());
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
        txtUpgradeCounter.text = $"{itemData.UpgradeCurrentValue}/{itemData.UpgradeMaxValue}";
        if (itemData.UpgradeCurrentValue >= itemData.UpgradeMaxValue)
        {
            txtPrice.text = "sold";
        }
        else
        {
            txtPrice.text = $"{itemData.PriceCurrent}$";
        }
    }

    public void ButtonClick()
    {
        if (itemData == null)
        {
            Debug.LogError("itemData == null");
            return;
        }

        if (itemData.ButtonClick())
        {
            SetNewValues();
             _sound.PlaySound();
        }
        
    }

    private IEnumerator PriceChecker()
    {   
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            if (_currenciesWallet.Check(itemData.PriceCurrent))
            {
                txtPrice.color = _baseColor;
            }
            else
            {
                txtPrice.color = _notEnoughColor;
            }

            if (itemData.UpgradeCurrentValue >= itemData.UpgradeMaxValue)
            {
                txtPrice.color = _soldColor;
                yield break;
            }
            yield return new WaitForSeconds(_priceCheckerCooldown);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }
}