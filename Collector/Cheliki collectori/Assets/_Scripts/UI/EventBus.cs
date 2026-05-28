using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static Action<string, int> OnUpgradeLevelChanged; 
    public static Action<string, bool> OnUpgradeAvailableChanged;


    public static Action<long> changeDollarsCountEvent;
    public static Action<long> changeAllTimeDollarsCountEvent;
    public static Action<long> changeFailsCountEvent;
    
    public static Action OnStateChanged;
}
