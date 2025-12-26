using UnityEngine;

public class Coin : MonoBehaviour
{
    private ItemData itemData;

    public bool isOccupied { get; private set; } = false;

    private CoinMover coinMover;


    private void Awake()
    {
        coinMover = GetComponent<CoinMover>();
    }

    public void Init(ItemData newItemData)
    {
        itemData = newItemData;
    }

    public void Interact(bool isInteractedByNPC = false)
    {
        //animation

        Toss();
        



        if (isInteractedByNPC) isOccupied = false;
    }

    private bool GetRandomSideOfCoin()
    {
        if (Random.Range(0, 2) > 0) return true;

        return false;
    }

    private void Toss()
    {
        coinMover.StartMovement();


        if (GetRandomSideOfCoin())
        {
            CurrenciesWallet.Instance.AddDollars(itemData.SpecialCurrentValue);
        }
        else
        {
            CurrenciesWallet.Instance.AddFail();
        }
    }


}
