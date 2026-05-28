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

    public ItemConfig itemData { get; protected private set; }

    [Header("price visual")]
    [SerializeField] private float _priceCheckerCooldown = 0.25f;
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _notEnoughColor;
    [SerializeField] private Color _soldColor;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
        EventBus.OnStateChanged+=UpdateValues;
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
        if (GameManager.instance.IsItemUnlocked(itemData.itemType))
        {
            UpdateData();
            RecountPrice();
            Debug.Log("Show " + itemData.itemType.ToString());
        }
        else
        {
            Debug.Log("Hide " + itemData.itemType.ToString());
            Hide();
        }
    }

    private void UpdateData()
    {
        txtItemName.text = itemData.itemType.ToString();
        image.enabled = true;
        txtItemName.text = "idk";
        txtItemDesc.text = "idk";
        txtPrice.text = "idk";

        txtUpgradeCounter.text = "idk/idk";
    }

    private void Hide()
    {
        image.enabled = false;
        txtItemName.text = "???";
        txtItemDesc.text = "...";
        txtPrice.text = "???";
        txtUpgradeCounter.text = "??/??";
    }

    private void RecountPrice()
    {
        if (itemData == null) return;
        var cost = GameManager.instance?.GetActualPrice(itemData.itemType);
        txtPrice.text = MoneyToText((long)cost) +"$";
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

        if(GameManager.instance.IsItemUnlocked(itemData.itemType))
        {

            _sound.PlaySound(); 
        }
    }


}