using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private ItemData itemData;
    public ItemData ItemData => itemData;
}