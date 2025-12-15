using Unity.VisualScripting;
using UnityEngine;

public class Chelix : MonoBehaviour
{
    private Coin currentGoalCoin;
    [SerializeField] private float moveSpeed = 0.5f;

    [SerializeField] private float distanceToTriggerGoal = 0.65f;
    private float distanceToGoal;

    private ChelixState currentState = ChelixState.Idle;


    private void Update()
    {
        DoStateAction();
    }

    private void DoStateAction()
    {
        switch (currentState)
        {
            case ChelixState.Idle:
                Debug.Log(currentState);
                SetNewGoal();
                break;

            case ChelixState.MovingToGoal:
                MoveToGoalPos();
                break;

            case ChelixState.Sleeping:
                //
                break;
        }
    }

    public void SetNewGoal()
    {
        Coin newGoalCoin = BusChelixCoins.Instance.FindGoalForChelix();
        if (newGoalCoin == currentGoalCoin || newGoalCoin == null) SetNewGoal();

        currentGoalCoin = newGoalCoin;

        if (currentGoalCoin != null)
            currentState = ChelixState.MovingToGoal;
        else
            currentState = ChelixState.Sleeping;

        Debug.Log(currentState);
    }

    private void MoveToGoalPos()
    {
        Vector3 direction = (currentGoalCoin.transform.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        distanceToGoal = Vector3.Distance(transform.position, currentGoalCoin.transform.position);

        Debug.Log(distanceToGoal);

        if (distanceToGoal <= distanceToTriggerGoal) InteractWithGoal(currentGoalCoin);
    }

    private void InteractWithGoal(Coin coinToInteract)
    {
        coinToInteract.Interact(true);

        Debug.Log("Chelix interacted with: " + coinToInteract.name); 

        currentState = ChelixState.Idle;

        SetNewGoal();
    }
}