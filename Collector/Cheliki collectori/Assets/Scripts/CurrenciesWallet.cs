using UnityEngine;
using UnityEngine.Events;
using YG;

public class CurrenciesWallet : MonoBehaviour
{
    private int dollarsCount = 0;
    private int failsCount = 0;


    [SerializeField] public UnityEvent<int> changeDollarsCountEvent = new UnityEvent<int>();
    [SerializeField] public UnityEvent<int> changeFailsCountEvent = new UnityEvent<int>();

    public static CurrenciesWallet Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        //load saved
        Load();
        changeDollarsCountEvent?.Invoke(dollarsCount);
        changeFailsCountEvent?.Invoke(failsCount);
    }


    public void Save()
    {
        YG2.saves.dollarsCount = dollarsCount;
        YG2.saves.failsCount = failsCount;
        
    }

    public void Load()
    {
        dollarsCount = YG2.saves.dollarsCount;
        failsCount = YG2.saves.failsCount;
    }




    public void AddDollars(int dollarsToAdd)
    {
        if (dollarsToAdd <= 0)
        {
            Debug.LogError("dollarsToAdd <= 0");
            return;
        }

        dollarsCount += dollarsToAdd;

        Debug.Log($"{dollarsToAdd} dollars added");

        changeDollarsCountEvent?.Invoke(dollarsCount);
        Save();
    }

    public bool SpendDollars(int dollarToSpend)
    {
        if (dollarsCount - dollarToSpend >= 0)
        {
            dollarsCount -= dollarToSpend;

            Debug.Log($"{dollarToSpend} dollars spent");

            changeDollarsCountEvent?.Invoke(dollarsCount);
            Save();
            return true;
        }

        return false;
    }

    public void AddFail()
    {
        failsCount += 1;

        Debug.Log($"1 fail currency added");

        changeFailsCountEvent?.Invoke(failsCount);
        Save();
    }

    public bool SpendFails(int dollarToSpend)
    {
        if (dollarsCount - dollarToSpend >= 0)
        {
            dollarsCount -= dollarToSpend;

            Debug.Log($"{dollarToSpend} fails currency spent");

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

