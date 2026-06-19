using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using YG;

public class RewarderAD : MonoBehaviour
{
    [SerializeField] private TMP_Text CoolRewardText;
    [SerializeField] private TMP_Text LameRewardText;
    [SerializeField] private GameObject particles1;
    [SerializeField] private GameObject particles2;
    [SerializeField] private ObjectSound objectSound;
    private long sum = 10;
    private int mod = 2;
    void OnEnable()
    {
        sum = GameManager.Instance.GetAverageIncome();
        if(sum<15) sum = 15; 
        mod = Random.Range(2,5);
        CoolRewardText.text = Utils.MoneyToText(sum*mod)+"$";
        LameRewardText.text = Utils.MoneyToText(sum)+"$";
    }

    public void GetCoolSum()
    {
        YG2.RewardedAdvShow("justMoney", () =>
        {
            GameManager.Instance.TryToAddDollars((int)sum*mod, false);
            var pos = this.gameObject.transform.position;
            Instantiate(particles1).transform.position = pos;
            Instantiate(particles2).transform.position = pos;
            objectSound.PlaySound();
            EventBus.OnCoinFlipEnd((int)sum*mod,pos);
        });
        this.gameObject.SetActive(false);
    }

    public void GetBoringSumm()
    {
        GameManager.Instance.TryToAddDollars((int)sum, false);
        Instantiate(particles1).transform.position = this.gameObject.transform.position;
        objectSound.PlaySound();
        this.gameObject.SetActive(false);
    }
}
