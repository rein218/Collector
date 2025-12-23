using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Chelix : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private ChelixAnimationController _animationController;
    private Coin currentGoalCoin;
    [Header("Variables")]
    [SerializeField] private float moveSpeed = 0.5f;

    [SerializeField] private float distanceToTriggerGoal = 0.65f;
    private float distanceToGoal;

    private ChelixState currentState = ChelixState.Idle;

    private void Awake()
    {
        DoStateAction();
    }


    private void DoStateAction()
    {
        StopAllCoroutines();
        switch (currentState)
        {
            case ChelixState.Idle:
                StartCoroutine(IdleIE());
                _animationController.StopMoving();
                break;

            case ChelixState.MovingToGoal:
                StartCoroutine(MoveToGoalIE());
                break;

            case ChelixState.Sleeping:
                StartCoroutine(SleepIE());
                _animationController.StopMoving();
                break;
        }
        Debug.Log(currentState);
    }

    private IEnumerator MoveToGoalIE()
    {
        distanceToGoal = Vector3.Distance(transform.position, currentGoalCoin.transform.position);


        var sidetomove = (currentGoalCoin.transform.position - transform.position).normalized;
        if      (sidetomove.y<0 && sidetomove.x<0) _animationController.MoveTopLeft();
        else if (sidetomove.y<0 && sidetomove.x>0) _animationController.MoveTopRight();
        else if (sidetomove.y>0 && sidetomove.x<0) _animationController.MoveBottomLeft();
        else                                       _animationController.MoveBottomRight();
        
        while (distanceToGoal > distanceToTriggerGoal)
        {
            Vector3 direction = (currentGoalCoin.transform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            distanceToGoal = Vector3.Distance(transform.position, currentGoalCoin.transform.position);

            yield return null;
        }


        yield return new WaitForSeconds(0.3f);


        InteractWithGoal(currentGoalCoin);

        currentState = ChelixState.Idle;
        DoStateAction();
    }

    private IEnumerator IdleIE()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("hehe");
        SetNewGoal();

        DoStateAction();
    }

    private IEnumerator SleepIE()
    {
        while (currentState == ChelixState.Sleeping)
        {
            Debug.Log("Chelix is sleeping");
            yield return new WaitForSeconds(2);
        }

        yield return new WaitForSeconds(0.5f);

        SetNewGoal();

        DoStateAction();
    }

    public void SetNewGoal()
    {
        if (BusChelixCoins.Instance.CoinListIsEmpty())
        {
            currentState = ChelixState.Sleeping;
            DoStateAction();
            return;
        }

        Coin newGoalCoin = BusChelixCoins.Instance.FindGoalForChelix();

        Debug.Log($"newGoalCoin == {newGoalCoin}");
        if (newGoalCoin == currentGoalCoin || newGoalCoin == null)
        {
            currentGoalCoin = null;
            SetNewGoal();
        }
        
        Debug.Log($"currentGoalCoin == {currentGoalCoin}");

        currentGoalCoin = newGoalCoin;

        if (currentGoalCoin != null)
            currentState = ChelixState.MovingToGoal;
        else
            currentState = ChelixState.Sleeping;
    }

    private void InteractWithGoal(Coin coinToInteract)
    {
        _animationController.DoFlip();
        coinToInteract.Interact(true);

        Debug.Log("Chelix interacted with: " + coinToInteract.name); 

        currentState = ChelixState.Idle;

        SetNewGoal();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}