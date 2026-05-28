using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Item/GameConfig")]
public class GameConfig : ScriptableObject
{
    public List<ItemConfig> allItems;
    public ItemConfig GetItemConfig(ItemName type) => allItems.FirstOrDefault(i => i.itemType == type);
    public ItemConfig GetItemConfig(string id) => allItems.FirstOrDefault(i => i.Id == id);
    public IEnumerable<ItemConfig> GetAllItems()
    {
        foreach (var item in allItems)
                yield return item;
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        var ids = new HashSet<string>();
        foreach (var item in allItems)
        {
            if (!ids.Add(item.Id))
                    Debug.LogError($"Duplicate ItemConfig ID: {item.Id} in {item.name}");
        }
    }
    #endif

}
