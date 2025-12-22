using UnityEngine;

public class Coin : MonoBehaviour
{
    private ItemSO itemSO;

    public bool isOccupied { get; private set; } = false;

    private RandomMover randomMover;


    private void Awake()
    {
        randomMover = GetComponent<RandomMover>();
    }

    public void Init(ItemSO newItemSO)
    {
        itemSO = newItemSO;
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
        randomMover.StartMovement();


        if (GetRandomSideOfCoin())
        {
            CurrenciesWallet.Instance.AddDollars(itemSO.ItemData.SpecialCurrentValue);
        }
        else
        {
            CurrenciesWallet.Instance.AddFail();
        }
    }


}
