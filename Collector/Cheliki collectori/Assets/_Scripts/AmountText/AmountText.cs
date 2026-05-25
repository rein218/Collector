using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class AmountText : MonoBehaviour
{
    public delegate void AnimationComplete(AmountText amountText);
    public event AnimationComplete OnComplete;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private Image _dolarSign;
    [SerializeField] private float _animDurration = 1;
    [SerializeField] private float _animHeight= 1;
    public void PlayAnim(int value)
    {
        string final = ""+value;
        if(value>=1000000000)
        {
            value/=1000000;
            final = value/1000+"";
            if (value%1000/10>0)
            {
                final+="."+ value%1000/10;
            }
            final +="b";
        }
        else if(value>=1000000)
        {
            value/=1000;
            final = value/1000+"";
            if (value%1000/10>0)
            {
                final+="."+ value%1000/10;
            }
            final +="m";
        }
        else if(value>=1000)
        {
            final = value/1000+"";
            if (value%1000/10>0)
            {
                final+="."+ value%1000/10;
            }
            final +="k";
        }
        _value.text = final;
        transform.DOMoveY(transform.position.y+_animHeight, 1f*_animDurration);
        _value.DOFade(0,1f*_animDurration);
        _dolarSign.DOFade(0, 1f * _animDurration).OnComplete(()=>
        {
            transform.DOLocalMoveY(0,0);
            OnComplete?.Invoke(this);
            _value.DOFade(1,0);
            _dolarSign.DOFade(1, 0);
        });

        
    }

}
