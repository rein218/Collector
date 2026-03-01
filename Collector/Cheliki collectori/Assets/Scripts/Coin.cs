using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private ItemData itemData;

    public bool isOccupied { get; private set; } = false;
    public int baseCoinValue;
    private int currCoinValue =0;
    private int coinValueMod =0;

    public delegate void FlipStart(int coinValue, Vector2 position);
    public event FlipStart OnCoinFlipStart;

    public delegate void FlipEnd(int coinValue, Vector2 position);
    public event FlipEnd OnCoinFlipEnd;

    private CoinMover coinMover;
  

    private void Awake()
    {
        coinMover = GetComponent<CoinMover>();
        coinMover.SetMoveDuration(0);
    }

    public void SetNewCoinValue(float newCoinValue)
    {
        baseCoinValue = (int)(newCoinValue/1);
        coinValueMod =  Mathf.RoundToInt(newCoinValue%1/0.0005f*25);
        currCoinValue = baseCoinValue + (baseCoinValue*coinValueMod/100);
    }

    public void SetNewCoinValueModifier(float newCoinValue)
    {
        baseCoinValue = (int)(newCoinValue/1);
        coinValueMod = Mathf.RoundToInt(newCoinValue%1/0.0005f*25);
         Debug.Log((newCoinValue%1/0.001f*50)+ " " +coinValueMod);
        currCoinValue = baseCoinValue + (baseCoinValue*coinValueMod/100);
    }

    public void SetNewMoveDuration(float newCoinMoveUpgrade)
    {
        coinMover.SetMoveDuration(newCoinMoveUpgrade);
    }

    public void Interact(bool isInteractedByNPC = false)
    {
        if (coinMover.IsMoving()) return;
        OnCoinFlipStart?.Invoke(currCoinValue, transform.position);
        coinMover.StartMovement();
        if (isInteractedByNPC) isOccupied = false;
    }

    public void GetSideOfCoin()
    {
        CurrenciesWallet.Instance.AddDollars(currCoinValue);
        OnCoinFlipEnd?.Invoke(currCoinValue, transform.position);
        // CurrenciesWallet.Instance.AddFail();
    }

    public void SetIsOcupied(bool newBool)
    {
        isOccupied = newBool;
    }


}
