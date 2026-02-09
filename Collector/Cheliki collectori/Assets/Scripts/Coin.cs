using UnityEngine;

public class Coin : MonoBehaviour
{
    private ItemData itemData;

    public bool isOccupied { get; private set; } = false;
    public int coinValue { get; private set; }

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

    public void SetNewCoinValue(int newCoinValue)
    {
        coinValue += newCoinValue;
    }

    public void SetNewMoveDuration(float newCoinMoveUpgrade)
    {
        coinMover.SetMoveDuration(newCoinMoveUpgrade);
    }

    public void Interact(bool isInteractedByNPC = false)
    {
        if (coinMover.IsMoving()) return;
        OnCoinFlipStart?.Invoke(coinValue, transform.position);
        coinMover.StartMovement();
        if (isInteractedByNPC) isOccupied = false;
    }

    public void GetSideOfCoin()
    {
        CurrenciesWallet.Instance.AddDollars(coinValue);
        OnCoinFlipEnd?.Invoke(coinValue, transform.position);
        // CurrenciesWallet.Instance.AddFail();
    }

    public void SetIsOcupied(bool newBool)
    {
        isOccupied = newBool;
    }


}
