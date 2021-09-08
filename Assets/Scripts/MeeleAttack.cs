using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    public RaycastHit2D hitDown, hitUp, hitLeft, hitRight;
    private EnemyController lastEnemyDown, lastEnemyUp, lastEnemyLeft, lastEnemyRight;

    public LayerMask chaoLayerMask;
    public float distanceToAttack;
    public bool hit;
    public bool drawnLines;
   
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
        }
        else
        {
            if (lastEnemyDown != null)
            {
                lastEnemyDown.SetState(EnemyController.EnemyState.Busy);
                lastEnemyDown = null;
            }
        }
            

        if (hitUp.collider != null)
        {
            lastEnemyUp = hitUp.collider.gameObject.GetComponent<EnemyController>();
            hitUp.collider.gameObject.GetComponent<EnemyController>().SetState(EnemyController.EnemyState.EnableToAttack);
        }
        else
        {
            if (lastEnemyUp != null)
            {
                lastEnemyUp.SetState(EnemyController.EnemyState.Busy);
                lastEnemyUp = null;
            }
        }
            

        if (hitRight.collider != null)
        {
            lastEnemyRight = hitRight.collider.gameObject.GetComponent<EnemyController>();
            hitRight.collider.gameObject.GetComponent<EnemyController>().SetState(EnemyController.EnemyState.EnableToAttack);
        }
        else
        {
            if (lastEnemyRight != null)
            {
                lastEnemyRight.SetState(EnemyController.EnemyState.Busy);
                lastEnemyRight = null;
            }            
        }
            

        if (hitLeft.collider != null)
        {
            lastEnemyLeft = hitLeft.collider.gameObject.GetComponent<EnemyController>();
            hitLeft.collider.gameObject.GetComponent<EnemyController>().SetState(EnemyController.EnemyState.EnableToAttack);
        }
        else
        {
            if (lastEnemyLeft != null)
            {
                lastEnemyLeft.SetState(EnemyController.EnemyState.Busy);
                lastEnemyLeft = null;
            }
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
