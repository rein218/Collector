using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class CurrenciesWallet : MonoBehaviour
{
    private long allTimeDollarsCount = 0;
    private long dollarsCount = 0;
    private long failsCount = 0;
    private bool doubleReward = false;


    [SerializeField] public UnityEvent<long> changeDollarsCountEvent = new UnityEvent<long>();
    [SerializeField] public UnityEvent<long> changeAllTimeDollarsCountEvent = new UnityEvent<long>();
    [SerializeField] public UnityEvent<long> changeFailsCountEvent = new UnityEvent<long>();

    public static CurrenciesWallet Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void Start()
    {
        Load();
        changeDollarsCountEvent?.Invoke(dollarsCount);
        changeAllTimeDollarsCountEvent?.Invoke(allTimeDollarsCount);
        changeFailsCountEvent?.Invoke(failsCount);
    }


    public void MakeItDouble(float timeDouble)
    {
        StartCoroutine(timerForDouble());
        IEnumerator timerForDouble()
        {
            doubleReward = true;
            yield return new WaitForSeconds(timeDouble);
        }
    }


    public void Save()
    {
        YG2.saves.dollarsCount = dollarsCount;
        YG2.saves.failsCount = failsCount;
        YG2.saves.allTimeDollarsCount = allTimeDollarsCount;
    }

    public void Load()
    {
        dollarsCount = YG2.saves.dollarsCount;
        failsCount = YG2.saves.failsCount;
        allTimeDollarsCount = YG2.saves.allTimeDollarsCount;
    }


    public bool InEnough(int dollarToSpend)
    {
        if (dollarsCount >= dollarToSpend)
        {
            return true;
        }
        return false;
    }

    #if UNITY_EDITOR 
    [ContextMenu("AddDolllars")]
    public void AddDollars()
    {
        AddDollars(5000);
    }

    [ContextMenu("AddDolllars2")]
    public void AddDollars2()
    {
        AddDollars(50000);
    }

    #endif

    
    public void AddDollars(int dollarsToAdd)
    {
        if (dollarsToAdd <= 0)
        {
            return;
        }
        if(doubleReward)
        dollarsCount = dollarsCount+ dollarsToAdd*2;
        else
        dollarsCount += dollarsToAdd;
        allTimeDollarsCount+= dollarsToAdd;
        changeDollarsCountEvent?.Invoke(dollarsCount);
        changeAllTimeDollarsCountEvent?.Invoke(allTimeDollarsCount);
        Save();
    }

    public bool SpendDollars(int dollarToSpend)
    {
        if (dollarsCount - dollarToSpend >= 0)
        {
            dollarsCount -= dollarToSpend;


            changeDollarsCountEvent?.Invoke(dollarsCount);
            Save();
            return true;
        }
        return false;
    }

    public void AddFail()
    {
        failsCount += 1;


        changeFailsCountEvent?.Invoke(failsCount);
        Save();
    }

    public bool SpendFails(int dollarToSpend)
    {
        if (dollarsCount - dollarToSpend >= 0)
        {
            dollarsCount -= dollarToSpend;


            changeFailsCountEvent?.Invoke(failsCount);
            Save();
            return true;
        }
        
        return false;
    }



    private void OnDestroy()
    {
        changeDollarsCountEvent.RemoveAllListeners();
        changeFailsCountEvent.RemoveAllListeners();
    }
    
}