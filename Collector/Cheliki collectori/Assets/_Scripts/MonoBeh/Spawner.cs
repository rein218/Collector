using UnityEngine;
using YG;
public class Spawner : MonoBehaviour
{
    
    [SerializeField] GameObject coinBronzePrefab,
                                coinSilverPrefab,
                                coinGoldPrefab;
    [SerializeField] GameObject chelixPrefab;

    [SerializeField] private float _spawnPosRadius = 3f;
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

    public Coin SpawnStartCoin(ItemConfig itemData)
    {
        GameObject newCoinObj;
        newCoinObj = Instantiate(coinBronzePrefab, TableBorders.position, Quaternion.identity);
        Coin newCoin = newCoinObj.GetComponent<Coin>();
        OnCoinSpawn?.Invoke(newCoinObj, newCoin);

        return newCoin;
        
    }
    public void SpawnNewCoin(ItemConfig itemData, bool yes = true)
    {
    }

    public void SpawnNewChelix( bool yes = true)
    {
    }
}
