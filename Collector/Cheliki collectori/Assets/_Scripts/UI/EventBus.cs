using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static Action<double> OnGoldChanged;
    public static Action<string, int> OnUpgradeLevelChanged; // upgradeId, newLevel
    // Можно событие о разблокировке (доступности) кнопок
    public static Action<string, bool> OnUpgradeAvailableChanged; // upgradeId, available
}
