using System.Collections;
using TMPro;
using UnityEngine;
using YG;

public class AdController : MonoBehaviour
{
    [SerializeField] private float _adDefaultCooldown;
    [SerializeField] private float _rewardedTime;
    [SerializeField] private CurrenciesWallet _wallet;
    [SerializeField] private string rewardID;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text timerText;

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
