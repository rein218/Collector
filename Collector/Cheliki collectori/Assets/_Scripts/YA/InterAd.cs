using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Components;
using YG;

public class InterAd : MonoBehaviour
{    
    [SerializeField] private LocalizeStringEvent localizeStringEvent;
    [SerializeField] private int TimeForTimer = 3;
    private float currentTimer = 3;

    void OnEnable()
    {
        if (localizeStringEvent == null)
            localizeStringEvent = GetComponent<LocalizeStringEvent>();

        StartCoroutine(TimerBeforeAd());
        
    }

    private IEnumerator TimerBeforeAd()
    {
        currentTimer = TimeForTimer;
        UpdateText();
        while (currentTimer>0)
        {
            yield return new WaitForSeconds(1f);
            currentTimer--;
            UpdateText();
        }
        YG2.InterstitialAdvShow();
        this.gameObject.SetActive(false);
    }

    void UpdateText()
    {
        localizeStringEvent.StringReference.Arguments = new object[] { currentTimer };
            localizeStringEvent.StringReference.RefreshString();

    }

    void OnDisable()
    {
        StopCoroutine(TimerBeforeAd());
    }
}
