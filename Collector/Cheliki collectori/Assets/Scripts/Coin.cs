using UnityEngine;

public class Coin : MonoBehaviour
{
    private int valueOfCoin = 1;
    public int ValueOfCoin => valueOfCoin;

    private bool isOccupied = false;
    public bool IsOccupied => isOccupied;

    private RandomMover randomMover;


    private void Awake()
    {
        randomMover = GetComponent<RandomMover>();
    }

    public void Init(int newValueOfCoin)
    {
        valueOfCoin = newValueOfCoin;
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
            CoinCounter.Instance.AddCoins(valueOfCoin);
        }
        else
        {
            //CoinCounter.Instance.AddFail();
        }
    }


}
