using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    private int coinsCount = 0;

    [SerializeField] private TextMeshProUGUI txtCoinCounter;

    
    void Awake()
    {
        //load saved
        UpdateCoinCounterText();
    }

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
