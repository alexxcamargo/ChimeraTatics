using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    public RaycastHit2D hitDown, hitUp, hitLeft, hitRight;
    private EnemyController lastEnemyDown, lastEnemyUp, lastEnemyLeft, lastEnemyRight;
    private PlayerMeeleController _playerMeeleController;
    public LayerMask chaoLayerMask;
    public float distanceToAttack;
    public bool drawnLines;

    void Awake()
    {
        _playerMeeleController = GetComponentInParent<PlayerMeeleController>();
    }

    void Update()
    {
        hitDown = Physics2D.Raycast(transform.position, Vector2.down, distanceToAttack, chaoLayerMask);
        hitUp = Physics2D.Raycast(transform.position, Vector2.up, distanceToAttack, chaoLayerMask);
        hitRight = Physics2D.Raycast(transform.position, Vector2.right, distanceToAttack, chaoLayerMask);
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceToAttack, chaoLayerMask);
        
        
        if (hitDown.collider != null)
        {
            lastEnemyDown = hitDown.collider.gameObject.GetComponent<EnemyController>();
            hitDown.collider.gameObject.GetComponent<EnemyController>().SetState(EnemyController.EnemyState.EnableToAttack);
            ChangeListEnemiesPlayer(lastEnemyDown, true);
        }
        else
        {
            if (lastEnemyDown != null)
            {
                lastEnemyDown.SetState(EnemyController.EnemyState.Ready);
                ChangeListEnemiesPlayer(lastEnemyDown, false);
                lastEnemyDown = null;
            }
        }

        if (hitUp.collider != null)
        {
            lastEnemyUp = hitUp.collider.gameObject.GetComponent<EnemyController>();
            hitUp.collider.gameObject.GetComponent<EnemyController>().SetState(EnemyController.EnemyState.EnableToAttack);
            ChangeListEnemiesPlayer(lastEnemyUp, true);
        }
        else
        {
            if (lastEnemyUp != null)
            {
                lastEnemyUp.SetState(EnemyController.EnemyState.Ready);
                ChangeListEnemiesPlayer(lastEnemyUp, false);
                lastEnemyUp = null;
            }
        }
            

        if (hitRight.collider != null)
        {
            lastEnemyRight = hitRight.collider.gameObject.GetComponent<EnemyController>();
            hitRight.collider.gameObject.GetComponent<EnemyController>().SetState(EnemyController.EnemyState.EnableToAttack);
            ChangeListEnemiesPlayer(lastEnemyRight, true);
        }
        else
        {
            if (lastEnemyRight != null)
            {
                lastEnemyRight.SetState(EnemyController.EnemyState.Ready);
                ChangeListEnemiesPlayer(lastEnemyRight, false);
                lastEnemyRight = null;
            }            
        }
            

        if (hitLeft.collider != null)
        {
            lastEnemyLeft = hitLeft.collider.gameObject.GetComponent<EnemyController>();
            hitLeft.collider.gameObject.GetComponent<EnemyController>().SetState(EnemyController.EnemyState.EnableToAttack);
            ChangeListEnemiesPlayer(lastEnemyLeft, true);
        }
        else
        {
            if (lastEnemyLeft != null)
            {
                lastEnemyLeft.SetState(EnemyController.EnemyState.Ready);
                ChangeListEnemiesPlayer(lastEnemyLeft, false);
                lastEnemyLeft = null;
            }
        }
            

    }

    private void ChangeListEnemiesPlayer(EnemyController enemyController, bool add)
    {
        if (add)
        {
            if(!_playerMeeleController.enemiesToAttack.Contains(enemyController))
                _playerMeeleController.enemiesToAttack.Add(enemyController);
        }
        else
        {
            if (_playerMeeleController.enemiesToAttack.Contains(enemyController))
                _playerMeeleController.enemiesToAttack.Remove(enemyController);
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
