using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    //private List<Coin> coinsOnField = new List<Coin>();

    [SerializeField] private GameObject coinPrefab;


    public void SpawnNewCoin(int newValueOfCoin) 
    {
        if (coinPrefab == null)
        {
            Debug.LogError("Coin prefab == null");
            return;
        }

        GameObject newCoinObj = Instantiate(coinPrefab, GenerateNewPosition(), Quaternion.identity);
        Coin newCoin = newCoinObj.GetComponent<Coin>();
        newCoin.Init(newValueOfCoin);

    }

    private Vector3 GenerateNewPosition()
    {
        float posX, posY;

        //needs to find bounds insted 20

        posX = Random.Range(-5, 5); 
        posY = Random.Range(-5, 5);

        Vector3 newPos = new(posX, posY, 0);

        return newPos;
    }
}
