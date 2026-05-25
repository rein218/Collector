using System;
using UnityEngine;

public class TestUnlockReq : MonoBehaviour
{
    [SerializeField] private ItemConfig _itemConfig;
    [SerializeField] private UpgradeConfig _upgradeConfig;

    public void Start()
    {
        GameState g = new GameState();

        ItemState itemState = new ItemState();



        //g.Items.Add(_itemConfig);

    }

    
}
