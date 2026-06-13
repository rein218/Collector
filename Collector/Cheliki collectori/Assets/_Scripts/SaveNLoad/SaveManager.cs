using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    [SerializeField] private float saveTimer = 15;
    private float currTime = 0;
    private GameState _gameState; 
    


    void Awake()
    {
        if(Instance == null)
        Instance = this; 
        _gameState = new GameState();
    }
    void OnEnable()
    {
        EventBus.OnStateChanged+=Save;
        StartCoroutine(SaveLoop());
    }

    void OnDisable()
    {
        EventBus.OnStateChanged-=Save;
    }

    public IEnumerator SaveLoop()
    {
        while (true)
        {
            currTime+=Time.deltaTime;
            if(saveTimer<currTime)
            {
                Save();
                currTime= 0;
            }
            yield return null;
        }
    }

    public void Save()
    {
        currTime= 0;
        string str = JsonUtility.ToJson(_gameState);
        YG2.saves.saveFile = str;
        YG2.SaveProgress();
    }

    public GameState Load()
    {
        if(YG2.saves.saveFile == "") _gameState = new GameState();
        else _gameState = JsonUtility.FromJson<GameState>(YG2.saves.saveFile);  

        return _gameState;
    }

    [ContextMenu("ResetProgress")]
    public void ResetProgress()
    {
        YG2.saves.saveFile = "";
        YG2.SaveProgress();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public string saveFile;
    }
}

