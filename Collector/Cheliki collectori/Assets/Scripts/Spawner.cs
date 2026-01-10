using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject coinBronzePrefab,
                                coinSilverPrefab,
                                coinGoldPrefab;
    [SerializeField] GameObject chelixPrefab;

    private GameObject SpawnNewObj(GameObject prefab) 
    {
        return Instantiate(prefab, GenerateNewPosition(), Quaternion.identity);
    }

    private Vector3 GenerateNewPosition()
    {
        float posX, posY;

        //needs to find bounds instead 20

        posX = Random.Range(BoundsOfActiveSpace.leftBorder, BoundsOfActiveSpace.rightBorder); 
        posY = Random.Range(BoundsOfActiveSpace.bottomBorder, BoundsOfActiveSpace.topBorder);

        Vector3 newPos = new(posX, posY, 0);

        return newPos;
    }


    public void SpawnNewCoin(ItemData itemData)
    {
        GameObject newCoinObj;
        switch (itemData.ItemName)
        {
            case ItemName.NewCoinBronze:
                newCoinObj = SpawnNewObj(coinBronzePrefab);
                break;

            case ItemName.NewCoinSilver:
                newCoinObj = SpawnNewObj(coinSilverPrefab);
                break;

            case ItemName.NewCoinGold:
                newCoinObj = SpawnNewObj(coinGoldPrefab);
                break;
            
            default:
                Debug.LogError("Trying to spawn coin without correct itemData");
                return;
        }
        
        Coin newCoin = newCoinObj.GetComponent<Coin>();
        newCoin.SetNewCoinValue((int)itemData.SpecialCurrentValue);

        BusChelixCoins.Instance.AddToCoinsXList(newCoin, itemData.ItemName);
    }

    public void SpawnNewChelix()
    {
        GameObject newChelixObj = SpawnNewObj(chelixPrefab);
        Chelix newChelix = newChelixObj.GetComponent<Chelix>();

        BusChelixCoins.Instance.AddToChelixList(newChelix);
    }
}
