using TMPro;
using UnityEngine;
using UnityEngine.Localization;
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

    public ShopItemConfig itemData { get; protected private set; }

    [Header("price visual")]
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _notEnoughColor;
    [SerializeField] private Color _soldColor;
    private LocalizedString localizedName;
    private LocalizedString localizedDesc;
    private string currname;
    private string currdesk;

    private bool _isVisible = true;

    public void Init(ShopItemConfig newItemData)
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
        itemData = newItemData;
        _baseColor = txtPrice.color;
    }


    void OnEnable()
    {
        EventBus.OnStateChanged+=UpdateValues;
        EventBus.changeDollarsCountEvent+=CheckPrice;
        localizedName = new LocalizedString("Main table", itemData.nameKey);
        localizedDesc = new LocalizedString("Main table", itemData.descKey);
        localizedName.StringChanged+=UpdateName;
        localizedDesc.StringChanged+=UpdateDesc;
        localizedName.RefreshString();
        localizedDesc.RefreshString();
        UpdateValues();
        CheckPrice(GameManager.Instance.GetMoneyCount());
    }
    
    void OnDisable()
    {
        EventBus.OnStateChanged-=UpdateValues;
        EventBus.changeDollarsCountEvent-=CheckPrice;
        localizedName.StringChanged-=UpdateName;
        localizedDesc.StringChanged-=UpdateDesc;
    } 

    public void UpdateValues()
    {
        if (itemData == null) return;
        if (GameManager.Instance.CheckRequirements(itemData.UnlockRequirements))
        {
            UpdateCover();
        }
        else
        {
            Hide();
        }
    }

    private void UpdateName(string str)
    {
        currname = str;
        UpdateValues();
    }

    private void UpdateDesc(string str)
    {
        currdesk = str;
        UpdateValues();
    }


    private void UpdateCover()
    {
        image.enabled = true;
        image.sprite = itemData.Icon;
        txtItemName.text = currname;
        txtItemDesc.text = currdesk;


        var currLevel = GameManager.Instance?.GetCurrLevel(itemData.Id);
        if(currLevel>=itemData.MaxLevel)
        {
            txtUpgradeCounter.text = "max";
            txtPrice.color = _soldColor;
            txtPrice.text = "sold";
        }
        else
        {
            txtUpgradeCounter.text = GameManager.Instance?.GetCurrLevel(itemData.Id)+"/"+itemData.MaxLevel;
            var cost = GameManager.Instance?.GetActualPrice(itemData.Id);
            txtPrice.text = Utils.MoneyToText((long)cost) +"$";
        }
        _isVisible = true;
    }

    private void Hide()
    {
        image.enabled = false;
        txtItemName.text = "???";
        txtItemDesc.text = "...";
        txtPrice.text = "???";
        txtUpgradeCounter.text = "??/??";
        _isVisible = false;
    }

    private void CheckPrice(long currMoney)
    {
        if (IsVisible() == false) return;

        var cost = GameManager.Instance?.GetActualPrice(itemData.Id);
        if (currMoney >= cost)
        {
            txtPrice.color = _baseColor;
        }
        else
        {
            txtPrice.color = _notEnoughColor;
        }
    }
    
    public void ButtonClick()
    {
        if (IsVisible() == false) return;
        if (GameManager.Instance.LevelUpItem(itemData.Id))
        {
            _sound.PlaySound(); 
        }
    }



    public bool IsVisible()
    {
        if (itemData.Id == null) return false; 
        if (_isVisible == false) return false; 
        return true;
    }

}