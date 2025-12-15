using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        //load saved
        UpdateCoinCounterText();
    }


    private int coinsCount = 0;
    [SerializeField] private TextMeshProUGUI txtCoinCounter;



    public void AddCoins(int coinsToAdd)
    {
        coinsCount += coinsToAdd;

        Debug.Log($"{coinsToAdd} coins added");

        UpdateCoinCounterText();
    }

    public void SpendCoins(int coinsToSpend)
    {
        if (coinsCount - coinsToSpend >= 0)
        {
            coinsCount -= coinsToSpend;

            Debug.Log($"{coinsToSpend} coins spent");

            UpdateCoinCounterText();
        }
    }

    private void UpdateCoinCounterText()
    {
        txtCoinCounter.text = $"{coinsCount}";
    }


}
