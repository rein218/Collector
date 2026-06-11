using System.Collections.Generic;
using UnityEngine;

public class CoinTextBus : MonoBehaviour
{
    [SerializeField] private ObjectPool _objectPool;
    
    
    private void OnEnable()
    {
       EventBus.OnCoinFlipEnd+=OnCoinFlipped;
    }


    void OnDisable()
    {
        EventBus.OnCoinFlipEnd-=OnCoinFlipped;
    }

    private void OnCoinFlipped(int value, Vector2 position)
    {
        if (!_objectPool.CanGetObject()) return;

        GameObject textObject = _objectPool.Get();
        if (textObject == null) return;
        
        textObject.transform.position = position;
        AmountText amountText = textObject.GetComponent<AmountText>();
        amountText.PlayAnim(value);
        amountText.OnComplete+=ReturnText;
    }

    public void ReturnText(AmountText amountText)
    {
        _objectPool.Return(amountText.gameObject);
        amountText.OnComplete-=ReturnText;
    }
}
