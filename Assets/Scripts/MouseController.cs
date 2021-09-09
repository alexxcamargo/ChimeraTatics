using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{   
    private Camera mainCamera;
    private PlayerInput playerInput;
    private PlayerMeeleController lastPlayerSelect;
    public MouseState currentState;
    
    public enum MouseState { Ready, Selected, Attack }

    private void Awake()
    {
        playerInput = new PlayerInput();
        mainCamera = Camera.main;
        currentState = MouseState.Ready;
    }
    
    void Start()
    {
        playerInput.Main.MouseButtons.performed += ctx => OnClickActions();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }


    public void ChangeMouseState(MouseState mouseState)
    {
        currentState = mouseState;
    }

    private void OnClickActions()
    {
        // Get the  Coordinates where the Mouse is on the screen
        Ray ray = mainCamera.ScreenPointToRay(playerInput.Main.MousePosition.ReadValue<Vector2>());
        RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);
        
        if (hits2D.collider != null)
        {
            if(hits2D.collider.tag == "Player")
            {
                PlayerActions(hits2D);
            }

            if (hits2D.collider.tag == "AI" && currentState == MouseState.Attack)
            {
                AiAction(hits2D);
            }
        }
        else
        {
            ChangeMouseState(MouseState.Ready);
            UIController._instance.ClearStepsLeftMessage();
            UIController._instance.ClearHealthMessage();
            UIController._instance.SetTxtMessagePlayerToMove();
        }
    }

    private void AiAction(RaycastHit2D hits2D)
    {
        if (currentState == MouseState.Attack)
        {
            if (lastPlayerSelect == null)
            {
                UIController._instance.SetTxtMessage("Select a player to attack!");
                return;
            }

            EnemyController enemyController = hits2D.collider.gameObject.GetComponent<EnemyController>();

            if (enemyController.GetCurrentState() == EnemyController.EnemyState.EnableToAttack)
            {
                if (lastPlayerSelect.enemiesToAttack.Contains(enemyController))
                {
                    UIController._instance.SetTxtMessage("You attacked Enemy !!");

                    if (lastPlayerSelect.alreadyMoved)
                    {
                        lastPlayerSelect.SetState(PlayerMeeleController.PlayerState.Busy);
                    }

                    if (enemyController.GetComponent<HealthController>().Damage(3) <= 0)
                    {
                        enemyController.SetState(EnemyController.EnemyState.Dead);
                    }
                }
                else
                {
                    UIController._instance.SetTxtMessage("Enemy out of range");
                }
            }

            if (enemyController.GetCurrentState() == EnemyController.EnemyState.Ready)
            {
                UIController._instance.SetTxtMessage("Enemy out of range");
            }
        }
    }


    /// <summary>
    /// Set Texts on panel, and allow move and attack
    /// </summary>
    /// <param name="hits2D"></param>
    private void PlayerActions(RaycastHit2D hits2D)
    {
        ChangeMouseState(MouseState.Selected);
        if (lastPlayerSelect != null)
        {
            lastPlayerSelect.DisableInput();
            lastPlayerSelect = null;
        }

        PlayerMeeleController playerMeeleController = hits2D.collider.gameObject.GetComponent<PlayerMeeleController>();

        UIController._instance.SetCurrentHealth(playerMeeleController.GetComponent<HealthController>().GetCurrentHealth());

        if (playerMeeleController.GetCurrentState() == PlayerMeeleController.PlayerState.Busy)
        {
            UIController._instance.ClearStepsLeftMessage();
            UIController._instance.SetTxtMessage(playerMeeleController.playerName + " Is Busy please select another one");
            return;
        }

        UIController._instance.SetTxtMessage(playerMeeleController.playerName + " Selected");
        

        if (playerMeeleController.GetCurrentState() == PlayerMeeleController.PlayerState.Ready)
        {
            playerMeeleController.EnableInput();
            lastPlayerSelect = playerMeeleController;
        }
    }
}
