using UnityEngine;
using System.Collections;

public class CoinMover : MonoBehaviour
{
    [Header("Animations settings")]
    [SerializeField] private CoinAnimationControllers _animationController;
    [SerializeField] private float _minAnimSpeed = 0f;
    [SerializeField] private float _maxAnimSpeed = 3f;
    [Header("Move Settings")]
    [SerializeField] private float _movementRadius = 2f;
    [SerializeField] private float _moveDuration = 2f;
    [Header("Toss Settings")]
    [SerializeField] private float _tossHeight = 2f;
    [SerializeField] private float _tossDuration = 2f;
    
    
    private Coroutine moveCoroutine;
    

    public void StartMovement()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + FindNewDirection();
        

        moveCoroutine = StartCoroutine(MoveToPosition(startPos, targetPos, _moveDuration));
    }

    private Vector3 FindNewDirection()
    {
        sbyte targetX = 0, targetY = 0;
        Vector2 randomOffset = Random.insideUnitCircle * _movementRadius;

        if (transform.position.x + randomOffset.x * _movementRadius > BoundsOfActiveSpace.rightBorder) targetX = -2;
        else if (transform.position.x - randomOffset.x *  _movementRadius < BoundsOfActiveSpace.leftBorder) targetX = 2;

        if (transform.position.y + randomOffset.y * _movementRadius > BoundsOfActiveSpace.topBorder) targetY = -2;
        else if (transform.position.y - randomOffset.y * _movementRadius < BoundsOfActiveSpace.bottomBorder) targetY = 2;

        return new Vector3(targetX + randomOffset.x, targetY + randomOffset.y, 0);
    }

    private IEnumerator MoveToPosition(Vector3 startPos, Vector3 targetPos, float duration)
    {
        _animationController.StartRotation();
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
            _animationController.ChangeSpeed(currentSpeed);
            
            //change pos
            transform.position = currentMovePos + currentTossPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _animationController.EndRotation();
        transform.position = targetPos;
        moveCoroutine = null;
    }
    

    public bool IsMoving()
    {
        return moveCoroutine != null;
    }
}