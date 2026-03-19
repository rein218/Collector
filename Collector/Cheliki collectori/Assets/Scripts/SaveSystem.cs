using UnityEngine;
using YG;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    void Awake()
    {
        if(instance == null) 
        instance = this;
    }

    public void SetDefaultSaves()
    {
        YG2.SetDefaultSaves();
        YG2.saves.SetDefault();
        YG2.SaveProgress();
    }

}
