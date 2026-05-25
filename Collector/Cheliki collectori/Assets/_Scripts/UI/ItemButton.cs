using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;

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

    public ItemConfig itemData { get; protected private set; }
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



    public void Init(ItemConfig newItemData)
    {
        itemData = newItemData;
        UpdateValues();
    }


    public void UpdateValues()
    {
        if (itemData == null) return;

        /*
        if (!itemData.IsUnlocked)
        {
            image.enabled = false;
            txtItemName.text = "???";
            txtItemDesc.text = "...";
            txtPrice.text = "???";

            txtUpgradeCounter.text = "??/??";
            return;
        }*/

        txtItemName.text = itemData.DisplayName;
        image.enabled = true;
        //txtUpgradeCounter.text = $"{itemData.CurrentLevelOfUpgrade}/{itemData.MaxLevelOfUpgrade}";
        txtPrice.text = MoneyToText((long)itemData.BaseCost) +"$";
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
        /*
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
            if(ItemsMenu.instance!=null) 
            ItemsMenu.instance.Save();
        }
        */
    }

}