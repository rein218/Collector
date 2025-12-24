using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject chelixPrefab;

    private GameObject SpawnNewObj(GameObject prefab) 
    {
        return Instantiate(prefab, GenerateNewPosition(), Quaternion.identity);
    }

    private Vector3 GenerateNewPosition()
    {
        float posX, posY;

        //needs to find bounds instead 20

        posX = Random.Range(-5.0f, 5.0f); 
        posY = Random.Range(-5.0f, 5.0f);

        Vector3 newPos = new(posX, posY, 0);

        return newPos;
    }


    public void SpawnNewCoin(ItemData itemData)
    {
        GameObject newCoinObj = SpawnNewObj(coinPrefab);
        Coin newCoin = newCoinObj.GetComponent<Coin>();
        newCoin.Init(itemData);

        BusChelixCoins.Instance.AddToCoinList(newCoin);
    }

    public void SpawnNewChelix()
    {
        GameObject newChelixObj = SpawnNewObj(chelixPrefab);
        Chelix newChelix = newChelixObj.GetComponent<Chelix>();

        BusChelixCoins.Instance.AddToChelixList(newChelix);
    }
}
