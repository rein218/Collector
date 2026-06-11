using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;

public class Chelix : MonoBehaviour, ISpawnable
{
    [Header("Links")]
    [SerializeField] private ChelixAnimationController _animationController;
    private Coin currentGoalCoin;
    [Header("Variables")]
    
    [SerializeField] private float distanceToTriggerGoal = 0.65f;

    [SerializeField] private float spawnTime = 0.3f;
    private float distanceToGoal;
    public string Id {get; private set;}

    private ChelixState currentState = ChelixState.Spawning;

    public void Init(string id)
    {
        Id = id;
        DoStateAction();
    }


    private void DoStateAction()
    {
        StopAllCoroutines();
        switch (currentState)
        {
            case ChelixState.Spawning:
                StartCoroutine(Spawning());
                break;
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
    }

    private IEnumerator Spawning()
    {
        yield return new WaitForSeconds(spawnTime);
        currentState = ChelixState.Idle;
        DoStateAction();
    }

    private IEnumerator MoveToGoalIE()
    {
        distanceToGoal = Vector3.Distance(transform.position, currentGoalCoin.transform.position);
        var speed = GameManager.Instance.GetCurrentHelperSpeed(Id);

        var sidetomove = (currentGoalCoin.transform.position - transform.position).normalized;
        if      (sidetomove.y<0 && sidetomove.x<0) _animationController.MoveTopLeft();
        else if (sidetomove.y<0 && sidetomove.x>0) _animationController.MoveTopRight();
        else if (sidetomove.y>0 && sidetomove.x<0) _animationController.MoveBottomLeft();
        else                                       _animationController.MoveBottomRight();
        
        while (distanceToGoal > distanceToTriggerGoal)
        {
            Vector3 direction = (currentGoalCoin.transform.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);

            distanceToGoal = Vector3.Distance(transform.position, currentGoalCoin.transform.position);

            yield return null;
        }


        yield return new WaitForSeconds(0.15f);
        if(!currentGoalCoin.isOccupied) InteractWithGoal(currentGoalCoin);

        currentState = ChelixState.Idle;
        DoStateAction();
    }

    private IEnumerator IdleIE()
    {
        yield return new WaitForSeconds(2);

        SetNewGoal();

        DoStateAction();
    }

    private IEnumerator SleepIE()
    {
        while (currentState == ChelixState.Sleeping)
        {
            yield return new WaitForSeconds(3);
        }

        yield return new WaitForSeconds(0.5f);

        SetNewGoal();

        DoStateAction();
    }

    public void SetNewGoal()
    {
        var newCoin = CoinRegistry.Instance.GetRandomAvailableCoinForHelper();
        if (newCoin  == null)
        {
            SleepIE();
            return;
        }


        currentGoalCoin = newCoin;

        if (currentGoalCoin != null)
            currentState = ChelixState.MovingToGoal;
        else
            currentState = ChelixState.Sleeping;
            
    }
    /*
    public void SetNewSpeed(float newSpeed)
    {
        speedMod =  speedMod + newSpeed;
        currMoveSpeed = moveSpeed+(moveSpeed*speedMod/100);
    }
    */
    private void InteractWithGoal(Coin coinToInteract)
    {
        _animationController.DoFlip();
        coinToInteract.Interact();

        currentState = ChelixState.Idle;

        SetNewGoal();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}