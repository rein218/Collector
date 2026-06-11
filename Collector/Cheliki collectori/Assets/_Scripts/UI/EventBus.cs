using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static Action<long> changeDollarsCountEvent;
    public static Action<long> changeAllTimeDollarsCountEvent;
    public static Action<long> changeFailsCountEvent;
    
    public static Action OnStateChanged;
    public static Action<string> OnItemsChanged;


    public static Action<Vector2> OnCoinFlipStart;
    public static Action<int, Vector2> OnCoinFlipEnd;


    public static Action OnAvailableCoinsChanged;
}
