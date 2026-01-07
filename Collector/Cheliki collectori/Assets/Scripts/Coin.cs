using UnityEngine;

public class Coin : MonoBehaviour
{
    private ItemData itemData;

    public bool isOccupied { get; private set; } = false;
    public int coinValue { get; private set; }

    private CoinMover coinMover;


    private void Awake()
    {
        coinMover = GetComponent<CoinMover>();
    }

    public void SetNewCoinValue(int newCoinValue)
    {
        coinValue = newCoinValue;
    }

    public void Interact(bool isInteractedByNPC = false)
    {
        if (!coinMover.IsMoving()) coinMover.StartMovement();


        if (isInteractedByNPC) isOccupied = false;
    }

    public void GetSideOfCoin()
    {
        if (Random.Range(0, 2) > 0)
        {
            CurrenciesWallet.Instance.AddDollars(coinValue);
        }
        else
        {
            CurrenciesWallet.Instance.AddFail();
        }
    }

    public void SetIsOcupied(bool newBool)
    {
        isOccupied = newBool;
    }


}
