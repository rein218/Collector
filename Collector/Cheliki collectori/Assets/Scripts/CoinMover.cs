using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using DG.Tweening;

public class CoinMover : MonoBehaviour
{
    [Header("Animations settings")]
    [SerializeField] private CoinAnimationController _animationController;
    [SerializeField] private float _minAnimSpeed = 0f;
    [SerializeField] private float _maxAnimSpeed = 3f;

    [Header("Sound Settings")]
    [SerializeField] private ObjectSound _soundFlung;
    [SerializeField] private ObjectSound _soundDrop;

    [Header("Move Settings")]
    [SerializeField] private float _movementRadius = 2f;
    [SerializeField] private float defaultMoveDuration = 2f;
    private float _moveDuration;
    [Header("Toss Settings")]
    [SerializeField] private float _tossHeight = 2f;
    private bool _isTossed = false;

    [SerializeField] private UnityEvent eventTossEnding;

    public void Start()
    {
        StartCoroutine(Spawn());
    }
    public IEnumerator Spawn()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < 0.3)
        {
            float t = elapsedTime / 0.3f;
            Vector3 currentMovePos = Vector3.Lerp(new Vector3(0,25,0), new Vector3(0,0,0), t);
            _animationController.transform.localPosition = currentMovePos;
            yield return null;
            elapsedTime+=Time.deltaTime;
        }
        _animationController.transform.localPosition = Vector3.zero;
    }

    public void StartMovement()
    {
        if (_isTossed) return;
        _isTossed = true;
        var p =  FindNewPos();
        MoveToPos(p, _moveDuration);
    }

    public void SetMoveDuration(float newCoinMoveUpgrade)
    {
        _moveDuration = defaultMoveDuration - newCoinMoveUpgrade;

    }

    private Vector3 FindNewPos()
    {
        Vector2 randomOffset = Random.insideUnitCircle * _movementRadius;
        Vector3 destination =  new Vector3(transform.position.x + randomOffset.x, transform.position.y + randomOffset.y, 0);
        
        if (destination.x > TableBorders.rightBorder)  destination.x = TableBorders.rightBorder - 0.1f;
        if (destination.x < TableBorders.leftBorder)   destination.x = TableBorders.leftBorder + 0.1f;
        if (destination.y > TableBorders.topBorder)    destination.y = TableBorders.topBorder - 0.1f; 
        if (destination.y < TableBorders.bottomBorder) destination.y = TableBorders.bottomBorder + 0.1f;

        return destination;
    }
    private void MoveToPos(Vector3 targetPos, float duration)
    {
        Sequence tossSequence = DOTween.Sequence();
        Tweener speedUp = DOTween.To(_animationController.ChangeSpeed, _minAnimSpeed, _maxAnimSpeed, duration*0.6f).SetEase(Ease.OutSine);
        Tweener speedDown = DOTween.To(_animationController.ChangeSpeed, _maxAnimSpeed , _minAnimSpeed, duration*0.4f).SetEase(Ease.OutSine);


        tossSequence.Append(_animationController.transform.DOLocalMoveY(_tossHeight, duration*0.6f).SetEase(Ease.OutCubic));
        tossSequence.Join(speedUp);
        tossSequence.Append(_animationController.transform.DOLocalMoveY(0, duration*0.4f).SetEase(Ease.InExpo));
        tossSequence.Join(speedDown);
        tossSequence.Insert(0,transform.DOMove(targetPos, duration).SetEase(Ease.OutQuart));

        
        
        

        

        _animationController.StartRotation();
        _soundFlung.PlaySound();

        tossSequence.Play().OnComplete(() => 
            {
                _soundDrop.PlaySound();
                _animationController.EndRotation();
                _isTossed = false;
                eventTossEnding?.Invoke();
            });
    }

    public bool IsMoving()
    {
        return _isTossed;
    }
}