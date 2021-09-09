using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMeeleController : MonoBehaviour
{
    [SerializeField]
    private Tilemap _groundTileMap;
    [SerializeField]
    private Tilemap _collisionTileMap;
    private PlayerInput _playerInput;
    public int steps;
    private int _countSteps = 0;
    public PlayerState currentState;
    public string playerName;
    public List<EnemyController> enemiesToAttack;
    public bool alreadyMoved;

    public enum PlayerState { Ready, Selected, Busy, Attack }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        currentState = PlayerState.Ready;
        enemiesToAttack = new List<EnemyController>();
    }
       
    private void Start()
    {
        _playerInput.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());      
    }

    public void EnableInput()
    {
        UIController._instance.SetStepsLeftMessage("Steps left: " + (steps - _countSteps));
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
        
        alreadyMoved = true;
        _countSteps++;
        UIController._instance.SetStepsLeftMessage("Steps left: " + (steps - _countSteps));
        return true;
        
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPostion = _groundTileMap.WorldToCell(transform.position + (Vector3)direction);

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

    public int GetSteps()
    {
        return _countSteps;
    }
}
