using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemConfig : ScriptableObject
{
    [SerializeField, ReadOnly(true)] private string id; 
    public string Id => id;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString().Substring(0, 8); 
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif

    public ItemName itemType;
    public Sprite Icon;
    public float BaseCost;
    public float CostAdd;
    public float CostAddMultiplier = 1.05f;
    public List<UnlockRequirement> UnlockRequirements;
}
