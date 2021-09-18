using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Raycast to get collision with enemies and change the list of enemies can be attack
/// </summary>
public class MeeleAttackEnemy : MonoBehaviour
{
    public RaycastHit2D hitDown, hitUp, hitLeft, hitRight;
    private PlayerController lastPlayerDown, lastPlayerUp, lastPlayerLeft, lastPlayerRight;
    private EnemyController _enemyMeelController;
    public LayerMask enemieLayerMask;
    public float distanceToAttack;
    public bool drawnLines;

    void Awake()
    {
        _enemyMeelController = GetComponentInParent<EnemyController>();
    }

    void Update()
    {
        hitDown = Physics2D.Raycast(transform.position, Vector2.down, distanceToAttack, enemieLayerMask);
        hitUp = Physics2D.Raycast(transform.position, Vector2.up, distanceToAttack, enemieLayerMask);
        hitRight = Physics2D.Raycast(transform.position, Vector2.right, distanceToAttack, enemieLayerMask);
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceToAttack, enemieLayerMask);


        // For each RaycastHit2D that collide with the enemy we change the list of the enemies of player can attack
        if (hitDown.collider != null)
        {
            lastPlayerDown = hitDown.collider.gameObject.GetComponent<PlayerController>();
            lastPlayerDown.SetState(PlayerController.PlayerState.OnTarget);
            ChangeListEnemiesPlayer(lastPlayerDown, true);
        }
        else
        {
            if (lastPlayerDown != null)
            {
                lastPlayerDown.SetState(PlayerController.PlayerState.Ready);
                ChangeListEnemiesPlayer(lastPlayerDown, false);
                lastPlayerDown = null;
            }
        }

        if (hitUp.collider != null)
        {
            lastPlayerUp = hitUp.collider.gameObject.GetComponent<PlayerController>();
            lastPlayerUp.SetState(PlayerController.PlayerState.OnTarget);
            ChangeListEnemiesPlayer(lastPlayerUp, true);
        }
        else
        {
            if (lastPlayerUp != null)
            {
                lastPlayerUp.SetState(PlayerController.PlayerState.Ready);
                ChangeListEnemiesPlayer(lastPlayerUp, false);
                lastPlayerUp = null;
            }
        }
            

        if (hitRight.collider != null)
        {
            lastPlayerRight = hitRight.collider.gameObject.GetComponent<PlayerController>();
            lastPlayerRight.SetState(PlayerController.PlayerState.OnTarget);
            ChangeListEnemiesPlayer(lastPlayerRight, true);
        }
        else
        {
            if (lastPlayerRight != null)
            {
                lastPlayerRight.SetState(PlayerController.PlayerState.Ready);
                ChangeListEnemiesPlayer(lastPlayerRight, false);
                lastPlayerRight = null;
            }            
        }
            

        if (hitLeft.collider != null)
        {
            lastPlayerLeft = hitLeft.collider.gameObject.GetComponent<PlayerController>();
            lastPlayerLeft.SetState(PlayerController.PlayerState.OnTarget);
            ChangeListEnemiesPlayer(lastPlayerLeft, true);
        }
        else
        {
            if (lastPlayerLeft != null)
            {
                lastPlayerLeft.SetState(PlayerController.PlayerState.Ready);
                ChangeListEnemiesPlayer(lastPlayerLeft, false);
                lastPlayerLeft = null;
            }
        }

    }

    private void ChangeListEnemiesPlayer(PlayerController playerController, bool add)
    {
        if (add)
        {
            if(!_enemyMeelController.playerToAttack.Contains(playerController))
                _enemyMeelController.playerToAttack.Add(playerController);
        }
        else
        {
            if (_enemyMeelController.playerToAttack.Contains(playerController))
                _enemyMeelController.playerToAttack.Remove(playerController);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (drawnLines)
        {
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - distanceToAttack));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + distanceToAttack));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - distanceToAttack, transform.position.y));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + distanceToAttack, transform.position.y));
        }
    }
}
