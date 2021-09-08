using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMeeleController : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundTileMap;
    [SerializeField]
    private Tilemap collisionTileMap;
    private PlayerInput playerInput;
    public int steps;
    private int countSteps = 0;
    public PlayerState currentState;

    public enum PlayerState { Ready, Selected, Busy, Attack }

    private void Awake()
    {
        playerInput = new PlayerInput();
        currentState = PlayerState.Ready;
    }
       
    private void Start()
    {
        playerInput.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());      
    }

    public void EnableInput()
    {
        playerInput.Enable();
    }

    public void DisableInput()
    {
        playerInput.Disable();
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
        if (countSteps >= steps)
        {
            countSteps = 0;
            currentState = PlayerState.Busy;
            DisableInput();
            return false;
        }

        countSteps++;
        return true;
        
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPostion = groundTileMap.WorldToCell(transform.position + (Vector3)direction);

        if (!groundTileMap.HasTile(gridPostion) || collisionTileMap.HasTile(gridPostion))
            return false;

        return true; 
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }
}
