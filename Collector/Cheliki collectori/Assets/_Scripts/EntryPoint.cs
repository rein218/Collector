using System.Collections;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private GameState _gameState;
    public IEnumerator Start()
    {
        _gameState = new GameState();
        yield return null;
    }

}
