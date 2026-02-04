using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemSorter : MonoBehaviour
{
    [System.Serializable]
    public class SortableItem
    {
        public ItemData itemData;
        public GameObject gameObject;
        public int originalIndex; // Для сохранения исходного порядка
    }

    [SerializeField] private List<SortableItem> itemsToSort = new List<SortableItem>();
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] private bool sortOnStart = true;
    private Dictionary<ItemData, GameObject> itemDictionary;

    void Start()
    {
        if (layoutGroup == null)
            layoutGroup = GetComponent<VerticalLayoutGroup>();
            
        if (sortOnStart)
            SortItems();
    }

    // Конвертация из Dictionary в SortableItem с сохранением порядка
    public void InitializeFromDictionary(Dictionary<ItemData, GameObject> dictionary)
    {
        itemsToSort.Clear();
        int index = 0;
        
        foreach (var kvp in dictionary)
        {
            itemsToSort.Add(new SortableItem
            {
                itemData = kvp.Key,
                gameObject = kvp.Value,
                originalIndex = index++
            });
        }
    }
    
    // Основной метод сортировки
    public void SortItems()
    {
        if (itemsToSort.Count <= 1) return;
        
        // Создаем две группы с сохранением исходного порядка внутри каждой
        List<SortableItem> unlockedItems = new List<SortableItem>();
        List<SortableItem> lockedItems = new List<SortableItem>();
        
        // Разделяем на группы, сохраняя исходный порядок через originalIndex
        foreach (var item in itemsToSort)
        {
            if (item.itemData != null && item.itemData.IsUnlocked)
            {
                unlockedItems.Add(item);
            }
            else
            {
                lockedItems.Add(item);
            }
        }
        
        // Сортируем каждую группу по исходному индексу (для сохранения порядка)
        unlockedItems = unlockedItems.OrderBy(x => x.originalIndex).ToList();
        lockedItems = lockedItems.OrderBy(x => x.originalIndex).ToList();
        
        // Объединяем: сначала unlocked, потом locked
        List<SortableItem> sortedItems = new List<SortableItem>();
        sortedItems.AddRange(unlockedItems);
        sortedItems.AddRange(lockedItems);
        
        // Применяем новый порядок в иерархии
        ApplySortingToHierarchy(sortedItems);
        
        // Обновляем словарь если нужно
        UpdateDictionaryFromList(sortedItems);
    }
    
    private void ApplySortingToHierarchy(List<SortableItem> sortedItems)
    {
        // Убеждаемся, что все объекты - дети этого GameObject
        foreach (var item in sortedItems)
        {
            if (item.gameObject.transform.parent != transform)
            {
                item.gameObject.transform.SetParent(transform);
            }
        }
        
        // Устанавливаем порядок в иерархии
        for (int i = 0; i < sortedItems.Count; i++)
        {
            sortedItems[i].gameObject.transform.SetSiblingIndex(i);
        }
        
        // Обновляем Layout
        if (layoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
    
    private void UpdateDictionaryFromList(List<SortableItem> sortedList)
    {
        itemDictionary = new Dictionary<ItemData, GameObject>();
        foreach (var item in sortedList)
        {
            if (item.itemData != null)
            {
                itemDictionary[item.itemData] = item.gameObject;
            }
        }
    }
    
    // Метод для обновления статуса одного ItemData
    public void UpdateItemStatus(ItemData itemData, bool isUnlocked)
    {
        var item = itemsToSort.Find(x => x.itemData == itemData);
        if (item != null)
        {
            // Обновляем статус и пересортируем
            SortItems();
        }
    }
    
    // Метод для добавления нового элемента
    public void AddItem(ItemData itemData, GameObject gameObject)
    {
        itemsToSort.Add(new SortableItem
        {
            itemData = itemData,
            gameObject = gameObject,
            originalIndex = itemsToSort.Count // Новый элемент в конец исходного порядка
        });
        itemData.ApplySorter(this);
        SortItems();
    }
    
    // Получить текущий порядок (для отладки)
    public List<ItemData> GetCurrentOrder()
    {
        return itemsToSort.Select(x => x.itemData).ToList();
    }

}
