using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

/// <summary>
/// Control the rounds of enemies and player
/// </summary>
public class RoundController : MonoBehaviour
{   
    public List<PlayerController> listPlayerControllers;
    public List<EnemyController> listEnemyControllers;
    private List<PlayerController> listPlayerControllersAlive;
    private List<EnemyController>  listEnemyControllersAlive;
    public static RoundController _instance = null;
    [SerializeField]
    private RoundState currentState;

    public enum RoundState { Player, Enemy }

    private void Awake()
    {
        _instance = this;
        SetCurrentState(RoundState.Player);
    }


    public RoundState GetCurrentState()
    {
        return currentState;
    }


    public void SetCurrentState(RoundState state)
    {
        currentState = state;
    }

    public bool GetPlayersReady()
    {
        return listPlayerControllers.FindAll(p => p.GetCurrentState() == PlayerController.PlayerState.Ready).Count == 0;
    }

    public List<PlayerController> GetPlayerAlive()
    {
        return listPlayerControllers.FindAll(p => p.GetCurrentState() != PlayerController.PlayerState.Dead);
    }

    public void EnemyRound()
    {
        SetCurrentState(RoundState.Enemy);

        listEnemyControllersAlive = listEnemyControllers.FindAll(
            e => e.GetCurrentState() == EnemyController.EnemyState.Ready || 
                 e.GetCurrentState() == EnemyController.EnemyState.EnableToAttack);
        
        if (listEnemyControllersAlive.Count > 0)
        {
            listEnemyControllersAlive[0].StartRoundEnemy();
        }
        else
        {
            PlayerRound();
        }
    }

    void PlayerRound()
    {
        listPlayerControllersAlive = listPlayerControllers.FindAll(p => p.GetCurrentState() != PlayerController.PlayerState.Dead);

        listEnemyControllersAlive = listEnemyControllers.FindAll(x => x.GetCurrentState() != EnemyController.EnemyState.Dead);


        if (listPlayerControllersAlive.Count == 0)
        {
            UIController._instance.OnClickClearHUD(false);
            UIController._instance.SetTxtMessage("Game Over");
            return;
        }

        listPlayerControllersAlive.ForEach(p=> p.ResetSteps());

        if (listEnemyControllersAlive.Count == 0)
        {
            UIController._instance.OnClickClearHUD(false);
            UIController._instance.SetTxtMessage("You Win");
            return;
        }

        SetCurrentState(RoundState.Player);
        listPlayerControllersAlive.ForEach(p => p.SetState(PlayerController.PlayerState.Ready));
        //UIController._instance.OnClickClearHUD();
    }




}
