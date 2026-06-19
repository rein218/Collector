using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using YG;

public class AdController : MonoBehaviour
{
    public static AdController Instance;
    private void Awake()
    {
        if(Instance == null)
        Instance = this; 
    }

    [Header("inter ad")]
    [SerializeField] private bool skipNextAd = false;
    [SerializeField] private float _adDefaultCooldown = 90;
    [SerializeField] private GameObject interPanel;

    [Header("rewarded ad")]

    [SerializeField] private int baseRewardedPause = 30;
    [SerializeField] private int timeForTick = 10;
    [SerializeField] private int spawnChancePerTick = 7;
    [SerializeField] private GameObject RewardedAd;
    [Header("rewarded ad1")]
    [SerializeField] private int timeForTick1 = 25;
    [SerializeField] private int spawnChancePerTick1 = 15;
     [SerializeField] private GameObject RewardedAd1;
    [Header("dada")]
    [SerializeField] private List<GameObject> requareToBeClosed;

    public void Init()
    {
        if(GameManager.Instance.FirstStart())
        {
            skipNextAd = true;
        }
        else
        {
            baseRewardedPause = 0;
        }
        StartCoroutine(InterAdCycle());
        StartCoroutine(RewardedAdCycle());
        StartCoroutine(RewardedAdCycle1());
        YG2.onCloseRewardedAdv += SkipNextInter;
    }

    public void StartAD(int num)
    {
        if(num == 0) RewardedAd.SetActive(true);
        else RewardedAd1.SetActive(true);
    }

    public void SkipNextInter()
    {
        skipNextAd = true;  
    }

    void OnDisable() => StopAllCoroutines();

    private IEnumerator RewardedAdCycle()
    {
        yield return new WaitForSeconds(baseRewardedPause);
        while (true)
        {   
            yield return new WaitForSeconds(timeForTick);
            if(spawnChancePerTick>= Random.Range(0f,100f))
            {
                EventBus.OnAdTrigger?.Invoke(0);
            }
        }
    }
    private IEnumerator RewardedAdCycle1()
    {
        yield return new WaitForSeconds(baseRewardedPause);
        while (true)
        {   
            yield return new WaitForSeconds(timeForTick1);
            if(spawnChancePerTick1>= Random.Range(0f,100f))
            {
                EventBus.OnAdTrigger?.Invoke(1);
            }
        }
    }


    public IEnumerator InterAdCycle()
    {
        float time = _adDefaultCooldown;
        if(YG2.interAdvInterval>1)
        time = YG2.interAdvInterval;

        while (true)
        {
            
            yield return new WaitForSeconds(time);
            while (CheckIfOpen())
            {
                yield return new WaitForSeconds(3);
            }
            if(skipNextAd)
            {
                skipNextAd = false;
                Debug.Log("adskipped");
            }
            else
            {
                interPanel.SetActive(true);
            }
        }
        
    }

    public bool CheckIfOpen()
    {
        foreach (var ch in requareToBeClosed)
        {
            if(ch.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}
