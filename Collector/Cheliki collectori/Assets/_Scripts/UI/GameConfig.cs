using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Item/GameConfig")]
public class GameConfig : ScriptableObject
{
    public List<ShopItemConfig> allItems;
    private Dictionary<string, ShopItemConfig> _allItems;


    //public ShopItemConfig GetItemConfig(ShopItemConfig config) => _allItems.FirstOrDefault(i => i.Value == config).Value;
    public ShopItemConfig GetItemConfig(string id) => _allItems[id];
    
    public IEnumerable<ShopItemConfig> GetAllItems()
    {
        foreach (var item in _allItems)
                yield return item.Value;
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        CheckDuplicates();
        UpdateDict();
    }

    private void CheckDuplicates()
    {
        var ids = new HashSet<string>();
        foreach (var item in allItems)
        {
            if (!ids.Add(item.Id))
            {
                Debug.LogError($"Duplicate ItemConfig ID: {item.Id} in {item.name}");
            }

        }
    }

    private void UpdateDict()
    {
        _allItems = new Dictionary<string, ShopItemConfig>();
        foreach (var item in allItems)
        {
            _allItems.Add(item.Id,item);
        }
    }

    [ContextMenu("FindAllItems")]
    public void FindAllItems()
    {
        allItems.Clear();
        string[] guids = AssetDatabase.FindAssets("t:ShopItemConfig");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ShopItemConfig item = AssetDatabase.LoadAssetAtPath<ShopItemConfig>(path);
            if (item != null)
            {
                allItems.Add(item);
            }
        }

        EditorUtility.SetDirty(this);
        Debug.Log($"Найдено {allItems.Count} элементов ShopItemConfig");
    }
    [ContextMenu("FindAllItemNotInList")]
    public void FindAllItemNotInList()
    {
        string[] guids = AssetDatabase.FindAssets("t:ShopItemConfig");
        int count =0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ShopItemConfig item = AssetDatabase.LoadAssetAtPath<ShopItemConfig>(path);
            if (item != null && !allItems.Contains(item))
            {
                allItems.Add(item);
                count++;
            }
        }
        Debug.Log($"Найдено {count} недостающих элементов ShopItemConfig");
    }

    [ContextMenu("ClearList")]
    public void ClearList()
    {
        allItems.Clear();
    }


    #endif

}
