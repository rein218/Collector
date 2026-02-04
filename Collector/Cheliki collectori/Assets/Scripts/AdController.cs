using System.Collections;
using UnityEngine;
using YG;

public class AdController : MonoBehaviour
{
    [SerializeField] private float _adDefaultCooldown;
    [SerializeField] private float _rewardedTime;
    [SerializeField] private CurrenciesWallet _wallet;
    [SerializeField] private string rewardID;
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
