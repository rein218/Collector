using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class RandomMover : MonoBehaviour
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
        Vector2 randomOffset = Random.insideUnitCircle * _movementRadius;
        Vector3 targetPos = startPos + new Vector3(randomOffset.x, randomOffset.y, 0);
        

        moveCoroutine = StartCoroutine(MoveToPosition(startPos, targetPos, _moveDuration));
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
            Debug.Log("t "+ t + " tLerp " +tLerp);

            //calculate move
            Vector3 currentMovePos = Vector3.Lerp(startPos, targetPos, t);

            //calculate tossing
            float currHeight = Mathf.Lerp(0,_tossHeight, tLerp);
            Vector3 currentTossPos = new Vector3(0,currHeight,0);

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