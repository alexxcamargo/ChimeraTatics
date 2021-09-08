using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyState currentState;    
    
    public enum EnemyState { Ready, EnableToAttack, Busy, Attack }

    private void Awake()
    {
        currentState = EnemyState.Busy;
    }

    public EnemyState GetState()
    {
        return currentState;
    }

    public void SetState(EnemyState newState)
    {
        currentState = newState;
    }
}
