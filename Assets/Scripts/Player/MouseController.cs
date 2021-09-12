using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Select the Characters and attack the enemies, change the states of Playable characters and enemies
/// </summary>
public class MouseController : MonoBehaviour
{   
    private Camera mainCamera;
    private PlayerInput playerInput;
    private PlayerController lastPlayerSelect;
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
                AttackEnemies(hits2D);
            }

            if (hits2D.collider.tag == "MageHitBox")
            {
                ChangeMouseState(MouseState.Ready);
                lastPlayerSelect.ActiveMagicHitBox(false);
                lastPlayerSelect.ActiveRange(false);
                UIController._instance.OnClickClearHUD();
                OnClickActions();
            }
        }
        else
        {
            // Verify if the point of click is not an UIElement
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                ChangeMouseState(MouseState.Ready);
                UIController._instance.OnClickClearHUD();
                
                if (lastPlayerSelect != null)
                {
                    lastPlayerSelect.ActiveRange(false);
                    if (lastPlayerSelect.GetCurrentState() != PlayerController.PlayerState.Defense)
                    {
                        lastPlayerSelect.SetState(PlayerController.PlayerState.Ready);
                        if (lastPlayerSelect.alreadyMoved)
                        {
                            lastPlayerSelect.steps = 0;
                        }
                    }
                }
            }
            else
            {
                if (lastPlayerSelect != null)
                {

                    if (EventSystem.current.currentSelectedGameObject != null)
                    {
                        if (EventSystem.current.currentSelectedGameObject.name == "btnDefense")
                        {
                            lastPlayerSelect.SetState(PlayerController.PlayerState.Defense );
                            UIController._instance.OnClickClearHUD();
                        }
                        else
                        {
                            lastPlayerSelect.ActiveRange(true);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Attack enemies and update status of enemies and playable characters 
    /// </summary>
    /// <param name="hits2D">The Enemie you've selected</param>
    private void AttackEnemies(RaycastHit2D hits2D)
    {
        if (currentState == MouseState.Attack)
        {
            if (lastPlayerSelect == null)
            {
                UIController._instance.SetTxtMessage("Select a player to attack!");
                return;
            }

            EnemyController enemyController = hits2D.collider.gameObject.GetComponent<EnemyController>();

            // The enemies are "Enable To Attack" when the hitbox of the player hit him
            if (enemyController.GetCurrentState() == EnemyController.EnemyState.EnableToAttack)
            {
                // Verify if this enemy You're trying to attack is on the list of the enemies your Character are able to attack
                if (lastPlayerSelect.enemiesToAttack.Contains(enemyController))
                {
                    UIController._instance.SetTxtMessage("You attacked Enemy !!");
                    lastPlayerSelect.ActiveRange(false);

                    if (lastPlayerSelect.alreadyMoved)
                    {
                        lastPlayerSelect.SetState(PlayerController.PlayerState.Defense);
                        UIController._instance.OnClickClearHUD();
                    }

                    if (enemyController.GetComponent<HealthController>().Damage(3) <= 0)
                    {
                        enemyController.SetState(EnemyController.EnemyState.Dead);
                        enemyController.gameObject.SetActive(false);
                    }
                }
                else
                {
                    UIController._instance.SetTxtMessage("Enemy out of range");
                }
            }else if (enemyController.GetCurrentState() == EnemyController.EnemyState.Ready)
            {
                UIController._instance.SetTxtMessage("Enemy out of range");
            }
        }
    }


    /// <summary>
    /// Move and attack with the character
    /// </summary>
    /// <param name="hits2D">The Character you've selected</param>
    private void PlayerActions(RaycastHit2D hits2D)
    {
        ChangeMouseState(MouseState.Selected);

        if (lastPlayerSelect != null)
        {
            lastPlayerSelect.DisableInput();
            lastPlayerSelect = null;
        }

        PlayerController playerController = hits2D.collider.gameObject.GetComponent<PlayerController>();
        
        if (playerController.GetCurrentState() == PlayerController.PlayerState.Defense)
        {
            UIController._instance.OnClickDefenseCharacter(playerController.playerName, playerController.imgPlayer);

            if (playerController.GetTypePlayer() == PlayerController.Type.Magic)
                playerController.ActiveMagicHitBox(false);

            return;
        }

        // We activate the mage Hit Box when the mage is selected, because if this box is always active the player cannot select characters below the hit box
        if(playerController.GetTypePlayer() == PlayerController.Type.Magic)
            playerController.ActiveMagicHitBox(true);

        playerController.SetState(PlayerController.PlayerState.Selected);
        UIController._instance.OnClickCharacter(playerController.GetComponent<HealthController>().GetCurrentHealth(), playerController.playerName);
        playerController.EnableInput();
        lastPlayerSelect = playerController;
        
    }
}
