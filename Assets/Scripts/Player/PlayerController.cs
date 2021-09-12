using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;



/// <summary>
/// Move the player and update States
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tilemap _groundTileMap;
    [SerializeField]
    private Tilemap _collisionTileMap;
    private PlayerInput _playerInput;
    public int steps = 10; 
    private SpriteRenderer spriteRenderer;
    public GameObject rangeAttack,magicHitBox;
    private int _countSteps = 0;
    
    public PlayerState currentState;
    public Type playerType;
    public string playerName;
    public List<EnemyController> enemiesToAttack;
    public bool alreadyMoved, alreadyAttack;
    public Sprite imgPlayer;

    public enum PlayerState { Ready, Selected, Defense, Attack }

    public enum Type { Meele, Magic }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        currentState = PlayerState.Ready;
        enemiesToAttack = new List<EnemyController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
       
    private void Start()
    {
        _playerInput.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());      
    }

    public void EnableInput()
    {
        if (steps > 0)
        {
            UIController._instance.SetStepsLeftMessage((steps - _countSteps).ToString());
        }
        
        UIController._instance.SetImgPlayer(imgPlayer);
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
        if (_countSteps >= steps)
        {
            _countSteps = 0;
            DisableInput();
            return false;
        }

        if (GetCurrentState() == PlayerState.Selected)
        {
            alreadyMoved = true;
            _countSteps++;
            UIController._instance.SetStepsLeftMessage((steps - _countSteps).ToString());
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

        return true; 
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }

    public void SetState(PlayerState playerState)
    {
        currentState = playerState;
    }

    public Type GetTypePlayer()
    {
        return playerType;
    }

    public void ActiveMagicHitBox(bool active)
    {
        if (this.GetTypePlayer() == Type.Magic)
        {
            magicHitBox.SetActive(active);
        }
    }


    public void ActiveRange(bool active)
    {
        rangeAttack.SetActive(active);
    }

    public int GetSteps()
    {
        return _countSteps;
    }
}
