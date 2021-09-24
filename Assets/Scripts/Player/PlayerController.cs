using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Move the player and update States
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Range(20, 100)]
    public int MaxDamageOnAttack; // Damage that player can do

    public Type playerType;
    public int steps;
    public GameObject rangeAttack,magicHitBox;
    public bool alreadyMoved;
    public string playerName;
    public List<EnemyController> enemiesToAttack;
    public Sprite imgPlayer;

    [SerializeField]
    private Tilemap _groundTileMap;
    [SerializeField]
    private Tilemap _collisionTileMap;

    private PlayerInput _playerInput;
    private int _countSteps, _stepsLeft;
    private Animator _animator;
    private PlayerState _currentState;
    private AnimationState _currentAnimationState;
    private MeeleAttack _meleeAttack;
    private SpriteRenderer _spriteRenderer;
    

    public enum PlayerState { Ready, Selected, Defense, Dead }

    public enum AnimationState { Idle, Defense, Dead }

    public enum Type { Melee, Magic }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _currentState = PlayerState.Ready;
        enemiesToAttack = new List<EnemyController>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _stepsLeft = steps;

        if (GetTypePlayer() == Type.Melee)
        {
            _meleeAttack = GetComponentInChildren<MeeleAttack>();
            _meleeAttack.gameObject.SetActive(false);
        }
    }
       
    private void Start()
    {
        _playerInput.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());      
    }

    void FixedUpdate()
    {
        ChangeAnimation();
    }

    void ChangeAnimation()
    {
        _animator.Play(Enum.GetName(typeof(AnimationState), _currentAnimationState));
    }

    public void EnableInput()
    {
        if (_stepsLeft > 0)
        {
            UIController._instance.SetStepsLeftMessage((_stepsLeft - _countSteps).ToString());
        }
        
        UIController._instance.SetImgHUD(imgPlayer);
        _playerInput.Enable();
    }

    public void DisableInput()
    {
        _playerInput.Disable();
    }

    private void Move(Vector2 direction)
    {
        if (CanMove(direction))
        {
            if (VerifySteps())
            {
                _spriteRenderer.flipX = (direction.x >= 0);
                transform.position += (Vector3)direction;
            }
        }
    }

    /// <summary>
    /// Allow player Move if he has a steps  
    /// </summary>
    /// <returns></returns>
    private bool VerifySteps()
    {
        if (_countSteps >= _stepsLeft)
        {
            _countSteps = 0;
            DisableInput();
            return false;
        }

        if (GetCurrentState() == PlayerState.Selected)
        {
            alreadyMoved = true;
            _countSteps++;
            UIController._instance.SetStepsLeftMessage((_stepsLeft - _countSteps).ToString());
            return true;
        }

        return false;
    }


    /// <summary>
    /// Not allow player move in collision tile
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPostion = _groundTileMap.WorldToCell(transform.position + (Vector3)direction);

        // Verify if the next position is a grid position or a collision position
        if (!_groundTileMap.HasTile(gridPostion) || _collisionTileMap.HasTile(gridPostion))
           return false;
        
        return true; 
    }

    public PlayerState GetCurrentState()
    {
        return _currentState;
    }

    public void SetState(PlayerState playerState)
    {
        if (GetCurrentState() == PlayerState.Dead || playerState == PlayerState.Dead)
        {
            _currentAnimationState = AnimationState.Dead;
            _currentState = playerState;
            this.gameObject.SetActive(false);
            return;
        }

        _currentState = playerState;

        if (playerState == PlayerState.Ready)
        {
            _currentAnimationState = AnimationState.Idle;
        }

        if (playerState == PlayerState.Defense)
        {
            _currentAnimationState = AnimationState.Defense;
        }
    }

    public Type GetTypePlayer()
    {
        return playerType;
    }

    public void ActiveMagicHitBox(bool active)
    {
        magicHitBox.SetActive(active);
    }
    
    public void ActiveMeleeRaycast(bool active)
    {
        _meleeAttack.gameObject.SetActive(active);
    }
    
    public void ActiveRange(bool active)
    {
        rangeAttack.SetActive(active);
    }

    public int GetSteps()
    {
        return _countSteps;
    }

    /// <summary>
    /// Make a Random number based on max damage to hit a Enemy
    /// </summary>
    /// <returns></returns>
    public int GetDamage()
    {
        return Random.Range(20, MaxDamageOnAttack);
    }
    
    public void SetStepsLeft(int steps)
    {
        _stepsLeft = steps;
    }

    /// <summary>
    /// When Rounds ends Reset the count steps
    /// </summary>
    public void ResetSteps()
    {
        SetStepsLeft(steps);
        _countSteps = 0;
    }
    
}
