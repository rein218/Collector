using UnityEngine;

public class Coin : MonoBehaviour
{
    private int valueOfCoin = 1;
    public int ValueOfCoin => valueOfCoin;


    public void Init(int newValueOfCoin)
    {
        valueOfCoin = newValueOfCoin;
    }





}
