using TMPro;
using UnityEngine;
using YG;

public class LeaderBoardUnlocker : MonoBehaviour
{
    [SerializeField] private int summToReach = 50000;
    [SerializeField] private int min = 1000;
    [SerializeField] private TMP_Text description;

    [SerializeField] private TMP_Text ammountText;
    [SerializeField] private GameObject ledearBoardButton;
    public void OnEnable()
    {
        EventBus.changeAllTimeDollarsCountEvent+=CheckStatus;
    }

    void OnDisable()
    {
        EventBus.changeAllTimeDollarsCountEvent-=CheckStatus;
    }

    public void CheckStatus(long ammount)
    { 
        ammountText.text = $"{FromIntToString(ammount)}/{FromIntToString(summToReach)}";
        if(ammount<min) Hide();
        else if (summToReach<=ammount)
        {
            EventBus.changeAllTimeDollarsCountEvent-=CheckStatus;
            Hide();
            UnlockLeaderBoard();
        }
        else
        {
            UnHide();
        }
        
    }
    

    public void UnHide()
    {
        description.gameObject.SetActive(true);
        ammountText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        description.gameObject.SetActive(false);
        ammountText.gameObject.SetActive(false);
    }

    public void UnlockLeaderBoard()
    {
        ledearBoardButton.SetActive(true);
    }

    public string FromIntToString(long newCount)
    {
        string final = ""+newCount;
        if(newCount>=1000000000000)
        {
            newCount/=1000000000;
            final = newCount/1000+"";
            if (newCount%1000/10>0)
            {
                final+="."+ newCount%1000/10;
            }
            final +="t";
        }
        else if(newCount>=1000000000)
        {
            newCount/=1000000;
            final = newCount/1000+"";
            if (newCount%1000/10>0)
            {
                final+="."+ newCount%1000/10;
            }
            final +="b";
        }
        else if(newCount>=1000000)
        {
            newCount/=1000;
            final = newCount/1000+"";
            if (newCount%1000/10>0)
            {
                final+="."+ newCount%1000/10;
            }
            final +="m";
        }
        else if(newCount>=1000)
        {
            final = newCount/1000+"";
            if (newCount%1000/10>0)
            {
                final+="."+ newCount%1000/10;
            }
            final +="k";
        }
        return final;
    }
}
