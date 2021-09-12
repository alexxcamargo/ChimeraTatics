using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyState currentState;    
    
    public enum EnemyState { Ready, EnableToAttack, Busy, Attack, Dead }
    

    private void Awake()
    {
        currentState = EnemyState.Ready;
    }

    public EnemyState GetCurrentState()
    {
        return currentState;
    }

    public void SetState(EnemyState newState)
    {
        currentState = newState;
    }
}
