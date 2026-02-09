using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(RectTransform))]
public class FloatingAnimation : MonoBehaviour
{
    [SerializeField] private float range = 20f; 
    [SerializeField] private float time = 1.5f;  

    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 startPos = rt.anchoredPosition;
        
       
        rt.DOAnchorPos(startPos + Random.insideUnitCircle * range, time)
          .SetEase(Ease.InOutSine)
          .SetLoops(-1, LoopType.Yoyo)
          .SetDelay(Random.Range(0, 0.5f)); 
    }

}
