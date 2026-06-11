using TMPro;
using UnityEngine;

public class CurrencyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCounter;

    void OnEnable()
    {
        EventBus.changeDollarsCountEvent+=UpdateCounterText;
    }

    void OnDisable()
    {
        EventBus.changeDollarsCountEvent-=UpdateCounterText;
    }

    public void UpdateCounterText(long newCount)
    {
        txtCounter.text = Utils.MoneyToText(newCount);
    }

}