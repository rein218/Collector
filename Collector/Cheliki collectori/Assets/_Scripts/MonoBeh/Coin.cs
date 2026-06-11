using System;
using UnityEngine;

public class Coin : MonoBehaviour, ISpawnable
{
    private CoinMover _coinMover;
    public string id {get; private set;}
    public bool isOccupied {get; private set;}

    public void Init(string id)
    {
        this.id = id;

        _coinMover = GetComponent<CoinMover>();
        _coinMover.eventTossEnding+=GetSideOfCoin;
        isOccupied = true;
    }

    void OnDisable()
    {
        _coinMover.eventTossEnding-=GetSideOfCoin;
    }

    public void Interact()
    {
        if(id == null) return;
        if (_coinMover.IsMoving()) return;
        EventBus.OnCoinFlipStart?.Invoke(transform.position);
        
        _coinMover.StartMovement(GameManager.Instance.GetCurrentCoinMoveDuration(id), GameManager.Instance.GetCurrentDoubleFlipChance(id));
        isOccupied = true;
    }

    public void GetSideOfCoin()
    {
        if(id == null) return;
        var value = GameManager.Instance?.GetCurrentCoinValue(id);
        GameManager.Instance?.TryToAddDollars((int)value);
        EventBus.OnCoinFlipEnd?.Invoke((int)value, transform.position);
        isOccupied = false;
    }

    public void SetIsOcupied(bool newBool)
    {
        isOccupied = newBool;
    }
}

