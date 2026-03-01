using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private ObjectSound _sound;

    [SerializeField] private TextMeshProUGUI txtItemName;
    [SerializeField] private TextMeshProUGUI txtItemDesc;
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
    [SerializeField] private TextLanguageSwitch _languageSwitchForName;
    [SerializeField] private TextLanguageSwitch _languageSwitchForDescription;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
        _currenciesWallet = FindAnyObjectByType<CurrenciesWallet>();
        _baseColor = txtPrice.color;
    }

    public void Init(ItemData newItemData)
    {
        itemData = newItemData;
        itemData.ApplyButton(this);
        SetNewValues();
    }


    public void SetNewValues()
    {
        if (itemData == null) return;

        if (!itemData.IsUnlocked)
        {
            image.enabled = false;
            txtItemName.text = "???";
            txtItemDesc.text = "...";
            txtPrice.text = "???";

            txtUpgradeCounter.text = "??/??";
            return;
        }

        if(itemData.Name != null)
        _languageSwitchForName.GetList(itemData.Name);
        if(itemData.Description != null)
        _languageSwitchForDescription.GetList(itemData.Description);

        image.sprite = itemData.Sprite;
        image.enabled = true;
        txtUpgradeCounter.text = $"{itemData.CurrentLevelOfUpgrade}/{itemData.MaxLevelOfUpgrade}";
        if (itemData.CurrentLevelOfUpgrade >= itemData.MaxLevelOfUpgrade)
        {
            txtPrice.text = "sold";
        }
        else
        {
            txtPrice.text = MoneyToText(itemData.PriceCurrent) +"$";
            
        }
    }

    public string MoneyToText(long newCount)
    {
        string final = ""+newCount;
        if(newCount>=1000000000000)
        {
            newCount/=1000000000;
            final = newCount/1000+"";
            if (newCount%1000/10>0)
            {
                final+="."+ newCount%1000/10;
            }
            final +="t";
        }
        else if(newCount>=1000000000)
        {
            newCount/=1000000;
            final = newCount/1000+"";
            if (newCount%1000/10>0)
            {
                final+="."+ newCount%1000/10;
            }
            final +="b";
        }
        else if(newCount>=1000000)
        {
            newCount/=1000;
            final = newCount/1000+"";
            if (newCount%1000/10>0)
            {
                final+="."+ newCount%1000/10;
            }
            final +="m";
        }
        else if(newCount>=1000)
        {
            final = newCount/1000+"";
            if (newCount%1000/10>0)
            {
                final+="."+ newCount%1000/10;
            }
            final +="k";
        }
        return final;
    }

    public void ButtonClick()
    {
        if (itemData == null)
        {
            Debug.LogError("itemData == null");
            return;
        }

        if(itemData.IsUnlocked)
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
            if (_currenciesWallet.InEnough(itemData.PriceCurrent))
            {
                txtPrice.color = _baseColor;
            }
            else if (itemData.CurrentLevelOfUpgrade >= itemData.MaxLevelOfUpgrade)
            {
                txtPrice.color = _soldColor;
                yield break;
            }
            else
            {
                txtPrice.color = _notEnoughColor;
            }
            
            yield return new WaitForSeconds(_priceCheckerCooldown);
            yield return null;
        }
    }

    void OnDisable()
    {
        StopCoroutine(PriceChecker());
    }

    void OnEnable()
    {
       StartCoroutine(PriceChecker());
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
        StopCoroutine(PriceChecker());
    }
}