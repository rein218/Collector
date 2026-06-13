
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public List<SpawnerObj> prefabsObjs;

    [SerializeField] private float _spawnPosRadius = 3f;
    void OnEnable() => EventBus.OnItemsChanged += OnItemChanged;
    void OnDisable() => EventBus.OnItemsChanged -= OnItemChanged;

    public void Init()
    {
        foreach (var pref in prefabsObjs)
        {
            OnItemChanged(pref.id);
        }
        SpawnFirstCoing(); 
    }

    private void SpawnFirstCoing()
    {
        if(prefabsObjs.Count<1) return;
        var gm = Instantiate(prefabsObjs[0].prefab, TableBorders.position, Quaternion.identity);
        gm.GetComponent<ISpawnable>().Init(prefabsObjs[0].id);
        if(gm.TryGetComponent<Coin>(out var c))
        {
            CoinRegistry.Instance?.Register(c);
        }
    }

    private void OnItemChanged(string itemId)
    {
        SpawnerObj obj = prefabsObjs.FirstOrDefault(i=> i.id == itemId);
        if (obj != null)
        {
            int count = GameManager.Instance.GetItemLevel(itemId);
            int diff = count - obj.count;
            if(diff>0)
            {
                for (int i = 0; diff>i;i++)
                {
                    SpawnNewObj(obj.prefab, itemId);
                    obj.count++;
                }
            }
        }
    }
    
    private void SpawnNewObj(GameObject prefab, string itemId) 
    {
        var gm = Instantiate(prefab, GeneratePositionInMiddle(), Quaternion.identity);
        gm.GetComponent<ISpawnable>().Init(itemId);
        if(gm.TryGetComponent<Coin>(out var c))
        {
            CoinRegistry.Instance?.Register(c);
        }
    }

    private Vector3 GenerateNewPosition()
    {
        float posX, posY;

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
}


[Serializable]
public class SpawnerObj
{
    public GameObject prefab;
    public string id;
    public int count;
}
