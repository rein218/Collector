using UnityEngine;
using YG;
public class Spawner : MonoBehaviour
{
    
    [SerializeField] GameObject coinBronzePrefab,
                                coinSilverPrefab,
                                coinGoldPrefab;
    [SerializeField] GameObject chelixPrefab;

    [SerializeField] private float _spawnPosRadius = 3f;
    //костыль для обучения
    [SerializeField] private CameraController _сameraController;
    public delegate void CoinWasSpanwed(GameObject gameObject, Coin script);
    public event CoinWasSpanwed OnCoinSpawn;
    private GameObject SpawnNewObj(GameObject prefab) 
    {
        return Instantiate(prefab, GeneratePositionInMiddle(), Quaternion.identity);
    }

    private Vector3 GenerateNewPosition()
    {
        float posX, posY;

        //needs to find bounds instead 20

        posX = Random.Range(TableBorders.leftBorder, TableBorders.rightBorder); 
        posY = Random.Range(TableBorders.bottomBorder, TableBorders.topBorder);

        Vector3 newPos = new(posX, posY, 0);

        return newPos;
    }

    private Vector3 GeneratePositionInMiddle()
    {
        Vector2 randomOffset = Random.insideUnitCircle * _spawnPosRadius;
        Vector3 destination =  new Vector3(TableBorders.position.x + randomOffset.x, TableBorders.position.y + randomOffset.y, 0);
        
        if (destination.x > TableBorders.rightBorder)  destination.x = TableBorders.rightBorder - 0.1f;
        if (destination.x < TableBorders.leftBorder)   destination.x = TableBorders.leftBorder + 0.1f;
        if (destination.y > TableBorders.topBorder)    destination.x = TableBorders.topBorder - 0.1f; 
        if (destination.y < TableBorders.bottomBorder) destination.x = TableBorders.bottomBorder + 0.1f;

        return destination;
    }

    public void SpawnStartCoin(ItemData itemData)
    {
        GameObject newCoinObj;
        newCoinObj = Instantiate(coinBronzePrefab, TableBorders.position, Quaternion.identity);
        Coin newCoin = newCoinObj.GetComponent<Coin>();
        newCoin.SetNewCoinValue((int)itemData.SpecialCurrentValue);
        BusChelixCoins.Instance.AddToCoinsXList(newCoin, itemData.ItemType);

        _сameraController.FirstStart(newCoin);
        OnCoinSpawn?.Invoke(newCoinObj, newCoin);
    }
    public void SpawnNewCoin(ItemData itemData, bool yes = true)
    {
        GameObject newCoinObj;
        switch (itemData.ItemType)
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
        OnCoinSpawn?.Invoke(newCoinObj, newCoin);
        BusChelixCoins.Instance.AddToCoinsXList(newCoin, itemData.ItemType);
    }

    public void SpawnNewChelix( bool yes = true)
    {
        if(yes) YG2.saves.chelixCount++;
        GameObject newChelixObj = SpawnNewObj(chelixPrefab);
        Chelix newChelix = newChelixObj.GetComponent<Chelix>();
        BusChelixCoins.Instance.AddToChelixList(newChelix);
    }
}
