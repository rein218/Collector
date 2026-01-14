using UnityEngine;
using YG;
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
    public void SpawnNewCoin(ItemData itemData, bool yes = true)
    {
        GameObject newCoinObj;
        switch (itemData.ItemName)
        {
            case ItemName.NewCoinBronze:
                if(yes) YG2.saves.bronzeCount++;
                newCoinObj = SpawnNewObj(coinBronzePrefab);
                break;

            case ItemName.NewCoinSilver:
                if(yes) YG2.saves.siverCount++;
                newCoinObj = SpawnNewObj(coinSilverPrefab);

                break;

            case ItemName.NewCoinGold:
                if(yes) YG2.saves.goldCount++;
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

    public void SpawnNewChelix( bool yes = true)
    {
        if(yes) YG2.saves.chelixCount++;
        GameObject newChelixObj = SpawnNewObj(chelixPrefab);
        Chelix newChelix = newChelixObj.GetComponent<Chelix>();
        BusChelixCoins.Instance.AddToChelixList(newChelix);
    }
}
