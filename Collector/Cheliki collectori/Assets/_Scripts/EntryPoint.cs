using System.Collections;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private GameState _gameState;
    [SerializeField] private ItemsMenu _itemsMenu;
    public IEnumerator Start()
    {
        _gameState = new GameState();
        _itemsMenu.Init(_gameState);
        yield return null;
    }

}
