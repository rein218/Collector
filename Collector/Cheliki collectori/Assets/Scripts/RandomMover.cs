using UnityEngine;
using System.Collections;

public class RandomMover : MonoBehaviour
{
    [SerializeField] private float movementRadius = 2f;
    [SerializeField] private float moveDuration = 2f;
    
    private Coroutine moveCoroutine;
    

    public void StartMovement()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        

        Vector3 startPos = transform.position;
        Vector2 randomOffset = Random.insideUnitCircle * movementRadius;
        Vector3 targetPos = startPos + new Vector3(randomOffset.x, randomOffset.y, 0);
        

        moveCoroutine = StartCoroutine(MoveToPosition(startPos, targetPos, moveDuration));
    }
    
    private IEnumerator MoveToPosition(Vector3 startPos, Vector3 targetPos, float duration)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // Smooth interpolation
            t = t * t * (3f - 2f * t);
            
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPos;
        moveCoroutine = null;
    }
    

    public bool IsMoving()
    {
        return moveCoroutine != null;
    }
}