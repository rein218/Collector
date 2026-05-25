using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using YG;

public class AdController : MonoBehaviour
{
    [SerializeField] private float _adDefaultCooldown;
    [SerializeField] private float _rewardedTime;
    [SerializeField] private CurrenciesWallet _wallet;
    [SerializeField] private string rewardID;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private List<GameObject> requareToBeClosed;

    void Start()
    {
        StartCoroutine(adCycle());
    }

    public IEnumerator adCycle()
    {
        float time = _adDefaultCooldown;
        if(YG2.interAdvInterval>1)
        time = YG2.interAdvInterval;

        while (true)
        {
            
            yield return new WaitForSeconds(time);

            while(CheckIfOpen())
            {
                yield return new WaitForSeconds(3);
            }

            panel.SetActive(true);
            timerText.text = "2";
            yield return new WaitForSeconds(1);
            timerText.text = "1";
            yield return new WaitForSeconds(1);
            panel.SetActive(false);
            timerText.text = "0";
            ShowAd();
        }
        
    }

    public bool CheckIfOpen()
    {
        foreach (var ch in requareToBeClosed)
        {
            if(ch.active)
            {
                return true;
            }
        }
        return false;
    }

    [ContextMenu("Show ad")]
    public void ShowAd()
    {
        YG2.InterstitialAdvShow();
    }

    [ContextMenu("Show rewarded ad")]
    public void RewardAdvShow()
    {
        YG2.RewardedAdvShow(rewardID, () =>
        {
			if (true)
				_wallet.MakeItDouble(_rewardedTime);
        });
    }
}
