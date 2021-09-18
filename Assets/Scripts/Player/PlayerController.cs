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
    public int MaxDamageOnAttack;

    [SerializeField]
    private Tilemap _groundTileMap;
    [SerializeField]
    private Tilemap _collisionTileMap;
    private PlayerInput _playerInput;
    public int steps; 
    private SpriteRenderer spriteRenderer;
    public GameObject rangeAttack,magicHitBox;
    private int _countSteps, stepsLeft;
    public Vector2 currentPositonGrid;
    private Animator animator;
    
    public PlayerState currentState;
    private AnimationState currentAnimationState;
    public Type playerType;
    public string playerName;
    public List<EnemyController> enemiesToAttack;
    public bool alreadyMoved, alreadyAttack;
    public Sprite imgPlayer;

    private MeeleAttack meeleAttack;

    public enum PlayerState { Ready, Selected, Defense, Attack, Dead, OnTarget }

    public enum AnimationState { Idle, Defense, Dead }

    public enum Type { Melee, Magic }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        currentState = PlayerState.Ready;
        enemiesToAttack = new List<EnemyController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        stepsLeft = steps;

        if (playerType == Type.Melee)
        {
            meeleAttack = GetComponentInChildren<MeeleAttack>();
            meeleAttack.gameObject.SetActive(false);
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
        animator.Play(Enum.GetName(typeof(AnimationState), currentAnimationState));
    }

    public void EnableInput()
    {
        if (stepsLeft > 0)
        {
            UIController._instance.SetStepsLeftMessage((stepsLeft - _countSteps).ToString());
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
                spriteRenderer.flipX = (direction.x >= 0);
                transform.position += (Vector3)direction;
            }
        }
    }

    private bool VerifySteps()
    {
        if (_countSteps >= stepsLeft)
        {
            _countSteps = 0;
            DisableInput();
            return false;
        }

        if (GetCurrentState() == PlayerState.Selected)
        {
            alreadyMoved = true;
            _countSteps++;
            UIController._instance.SetStepsLeftMessage((stepsLeft - _countSteps).ToString());
            return true;
        }

        return false;
        
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPostion = _groundTileMap.WorldToCell(transform.position + (Vector3)direction);

        // Verify if the next position is a grid position or a collision position
        if (!_groundTileMap.HasTile(gridPostion) || _collisionTileMap.HasTile(gridPostion))
           return false;
        
        currentPositonGrid = new Vector2(gridPostion.x, gridPostion.y);    

        return true; 
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }

    public void SetState(PlayerState playerState)
    {
        if (GetCurrentState() == PlayerState.Dead || playerState == PlayerState.Dead)
        {
            currentAnimationState = AnimationState.Dead;
            currentState = playerState;
            return;
        }

        currentState = playerState;

        if (playerState == PlayerState.Ready)
        {
            currentAnimationState = AnimationState.Idle;
        }

        if (playerState == PlayerState.Defense)
        {
            currentAnimationState = AnimationState.Defense;
        }

        if (playerState == PlayerState.Dead)
        {
            currentAnimationState = AnimationState.Dead;
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
        meeleAttack.gameObject.SetActive(active);
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
        stepsLeft = steps;
    }

    /// <summary>
    /// When Rounds 
    /// </summary>
    public void ResetSteps()
    {
        SetStepsLeft(steps);
        _countSteps = 0;
    }
    
}
