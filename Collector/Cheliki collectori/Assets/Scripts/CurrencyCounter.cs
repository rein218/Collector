using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CurrencyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCounter;


    public void UpdateCounterText(int newCount)
    {
        txtCounter.text = $"{newCount}";
    }

}