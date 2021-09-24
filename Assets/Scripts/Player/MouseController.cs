using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Select the Characters and attack the enemies, change the states of Players characters and enemies
/// </summary>
public class MouseController : MonoBehaviour
{   
    private Camera _mainCamera;
    private PlayerInput _playerInput;
    private PlayerController _lastPlayerSelect;
    private MouseState _currentState;
    
    public enum MouseState { Ready, Selected, Attack }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _mainCamera = Camera.main;
        _currentState = MouseState.Ready;
    }
    
    void Start()
    {
        _playerInput.Main.MouseButtons.performed += ctx => OnClickActions();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }
    
    public void ChangeMouseState(MouseState mouseState)
    {
        _currentState = mouseState;
    }

    private void OnClickActions()
    {
        
        if (RoundController._instance.GetCurrentState() == RoundController.RoundState.Player)
        {
            // Get the  Coordinates where the Mouse is on the screen
            Ray ray = _mainCamera.ScreenPointToRay(_playerInput.Main.MousePosition.ReadValue<Vector2>());
            RaycastHit2D hits2D = Physics2D.GetRayIntersection(ray);


            // Verify if where the mouse Click is a collider
            if (hits2D.collider != null)
            {
                if (hits2D.collider.tag == "Player")
                {
                    PlayerActions(hits2D);
                }

                if (hits2D.collider.tag == "AI" && _currentState == MouseState.Attack)
                {
                    AttackEnemies(hits2D);
                }

                if (hits2D.collider.tag == "MageHitBox")
                {
                    ChangeMouseState(MouseState.Ready);
                    DisableHitBox(_lastPlayerSelect, false);
                    _lastPlayerSelect.ActiveRange(false);
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

                    if (_lastPlayerSelect != null)
                    {
                        _lastPlayerSelect.ActiveRange(false);
                        if (_lastPlayerSelect.GetCurrentState() != PlayerController.PlayerState.Defense)
                        {
                            _lastPlayerSelect.SetState(PlayerController.PlayerState.Ready);
                            if (_lastPlayerSelect.alreadyMoved)
                            {
                                _lastPlayerSelect.SetStepsLeft(0);
                            }
                        }
                    }
                }
                else
                {
                    if (_lastPlayerSelect != null)
                    {

                        if (EventSystem.current.currentSelectedGameObject != null)
                        {
                            if (EventSystem.current.currentSelectedGameObject.name == "btnDefense")
                            {
                                _lastPlayerSelect.SetState(PlayerController.PlayerState.Defense);
                                UIController._instance.OnClickClearHUD();

                                DisableHitBox(_lastPlayerSelect, false);

                                if (RoundController._instance.GetPlayersReady())
                                {
                                    RoundController._instance.EnemyRound();
                                }
                            }
                            else
                            {
                                _lastPlayerSelect.ActiveRange(true);
                            }
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
        if (_currentState == MouseState.Attack)
        {
            if (_lastPlayerSelect == null)
            {
                UIController._instance.SetTxtMessage("Select a player to attack!");
                return;
            }

            EnemyController enemyController = hits2D.collider.gameObject.GetComponent<EnemyController>();

            // The enemies are "Enable To Attack" when the hitbox of the player hit him
            if (enemyController.GetCurrentState() == EnemyController.EnemyState.EnableToAttack)
            {
                // Verify if this enemy You're trying to attack is on the list of the enemies your Character are able to attack
                if (_lastPlayerSelect.enemiesToAttack.Contains(enemyController))
                {
                    _lastPlayerSelect.ActiveRange(false);

                    _lastPlayerSelect.SetState(PlayerController.PlayerState.Defense);
                    UIController._instance.OnClickClearHUD(false);

                    int damage = _lastPlayerSelect.GetDamage();

                    if (enemyController.GetComponent<HealthController>().Damage(damage) <= 0)
                    {
                        enemyController.SetState(EnemyController.EnemyState.Dead);
                    }
                    else
                    {
                        enemyController.SetState(EnemyController.EnemyState.Ready);
                    }

                    UIController._instance.SetTxtMessage("An enemy loses " + damage + " hitpoints due to your attack.");
                    ChangeMouseState(MouseState.Ready);

                    DisableHitBox(_lastPlayerSelect, false);


                    if (RoundController._instance.GetPlayersReady())
                    {
                        RoundController._instance.EnemyRound();
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

        if (_lastPlayerSelect != null)
        {
            _lastPlayerSelect.DisableInput();
            _lastPlayerSelect = null;
        }

        PlayerController playerController = hits2D.collider.gameObject.GetComponent<PlayerController>();
        
        if (playerController.GetCurrentState() == PlayerController.PlayerState.Defense)
        {
            UIController._instance.OnClickDefenseCharacter(playerController.playerName, playerController.imgPlayer);
            DisableHitBox(playerController, false);
            return;
        }

        DisableHitBox(playerController, true);

        playerController.SetState(PlayerController.PlayerState.Selected);
        UIController._instance.OnClickCharacter(playerController.GetComponent<HealthController>().GetCurrentHealth(), playerController.playerName, playerController.steps);
        playerController.EnableInput();
        _lastPlayerSelect = playerController;
        
    }


    /// <summary>
    /// We activate the mage Hit Box when the mage is selected, because if this box is always active the player cannot select characters below the hit box.
    /// And The hit box and raycasts from melee change the state of Enemies
    /// </summary>
    /// <param name="playerController"></param>
    /// <param name="enable"></param>
    private void DisableHitBox(PlayerController playerController, bool enable)
    {
        if (playerController.GetTypePlayer() == PlayerController.Type.Magic)
            playerController.ActiveMagicHitBox(enable);


        if (playerController.GetTypePlayer() == PlayerController.Type.Melee)
            playerController.ActiveMeleeRaycast(enable);
    }
}
