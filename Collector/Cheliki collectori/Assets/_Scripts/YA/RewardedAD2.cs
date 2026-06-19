using TMPro;
using UnityEngine;
using YG;

public class RewardedAD2 : MonoBehaviour
{
    [SerializeField] private ObjectSound objectSound;
    
    public void GetCoolSum()
    {
        YG2.RewardedAdvShow("mult", () =>
        {
            GameManager.Instance.SetMult(4, 15);
            var pos = this.gameObject.transform.position;
            objectSound.PlaySound();
            EventBus.OnMultiplyTrigger(15);
        });
        this.gameObject.SetActive(false);
    }

    public void GetBoringSumm()
    {
        GameManager.Instance.SetMult(2, 10);
        EventBus.OnMultiplyTrigger(10);
        objectSound.PlaySound();
        this.gameObject.SetActive(false);
    }
}
