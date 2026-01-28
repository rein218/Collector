using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.ComponentModel.Design;

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
    private Coroutine moveCoroutine;

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
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        
        var p =  FindNewPos();
        
        moveCoroutine = StartCoroutine(MoveToPosition(transform.position, p, _moveDuration));
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

    private IEnumerator MoveToPosition(Vector3 startPos, Vector3 targetPos, float duration)
    {
        _animationController.StartRotation();
        _soundFlung.PlaySound();
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            if (t>1) t=1; if (t<0) t=0; 
            t = t * t * (3f - 2f * t); //smooth
            float tLerp = Mathf.Sin(t * Mathf.PI); //stuck in the middle

            //calculate move
            Vector3 currentMovePos = Vector3.Lerp(startPos, targetPos, t);

            //calculate tossing
            float currHeight = Mathf.Lerp(0,_tossHeight, tLerp);
            Vector3 currentTossPos = new Vector3(0, currHeight,0);

            //animations
            float currentSpeed = Mathf.Lerp(_minAnimSpeed,_maxAnimSpeed, tLerp);
            currentSpeed=currentSpeed*(defaultMoveDuration/_moveDuration);
            _animationController.ChangeSpeed(currentSpeed);
            
            //change pos
            _animationController.transform.localPosition = currentTossPos;
            transform.position = currentMovePos;
            

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _soundDrop.PlaySound();
        _animationController.EndRotation();
        transform.position = targetPos;
        moveCoroutine = null;

        eventTossEnding?.Invoke();
    }
    

    public bool IsMoving()
    {
        return moveCoroutine != null;
    }
}