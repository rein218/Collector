using UnityEngine;

public class Utils : MonoBehaviour
{
    public static string MoneyToText(long newCount)
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
