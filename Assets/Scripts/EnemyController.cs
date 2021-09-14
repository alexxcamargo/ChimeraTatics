using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    public EnemyState currentState;
    public GameObject target;
    [SerializeField]
    private Tilemap _groundTileMap;
    [SerializeField]
    private Tilemap _collisionTileMap;
    bool canWalk = true;

    public bool enemyPosUp, enemyPosInLine, enemyPosDown, enemyPosRight;

    public enum EnemyState { Ready, EnableToAttack, Busy, Attack, Dead }
    

    private void Awake()
    {
        currentState = EnemyState.Ready;
    }

    public EnemyState GetCurrentState()
    {
        return currentState;
    }

    public void SetState(EnemyState newState)
    {
        currentState = newState;
    }
    

    public void RoundEnemy()
    {
        if (GetCurrentState() != EnemyState.Attack)
        {
            SetState(EnemyState.Attack);
            StartCoroutine(MoveEnemy());
        }
    }

    private IEnumerator MoveEnemy()
    {
        Vector3Int nextPos = new Vector3Int(0,0,0);
        
        while (canWalk)
        {
            Vector3Int enemyPostion = _groundTileMap.WorldToCell(transform.position);
            Vector3Int posPlayer = _groundTileMap.WorldToCell(target.transform.position);

            // Move UP
            if (posPlayer.y > enemyPostion.y)
            {
                nextPos = new Vector3Int(enemyPostion.x, enemyPostion.y + 1, 0);

                if (VerifyNextStep(posPlayer, nextPos))
                {
                    transform.position += new Vector3(0, 1, 0);
                    enemyPostion.y++;
                    yield return new WaitForSeconds(.5f);
                }
            }


            // Move Down
            if (posPlayer.y < enemyPostion.y)
            {

                nextPos = new Vector3Int(enemyPostion.x, enemyPostion.y - 1, 0);

                if (VerifyNextStep(posPlayer, nextPos))
                {
                    transform.position += new Vector3(0, -1, 0);
                    enemyPostion.y--;
                    yield return new WaitForSeconds(.5f);
                }
            }
            
            // Move Right
            if (posPlayer.x > enemyPostion.x)
            {
                nextPos = new Vector3Int(enemyPostion.x + 1, enemyPostion.y, 0);

                if (VerifyNextStep(posPlayer, nextPos))
                {
                    transform.position += new Vector3(1, 0, 0);
                    enemyPostion.x++;
                    yield return new WaitForSeconds(.5f);
                }
            }

            // Move Left
            if (posPlayer.x < enemyPostion.x)
            {
                nextPos = new Vector3Int(enemyPostion.x - 1, enemyPostion.y, 0);

                if (VerifyNextStep(posPlayer, nextPos))
                {
                    transform.position += new Vector3(-1, 0, 0);
                    enemyPostion.x--;
                    yield return new WaitForSeconds(.5f);
                }
            }

            if (enemyPostion.x == posPlayer.x && enemyPostion.y == posPlayer.y)
            {
                canWalk = false;
            }
        }
    }

    /// <summary>
    /// If the next step is the same position of the player stop move
    /// </summary>
    /// <returns></returns>
    bool VerifyNextStep(Vector3Int posPlayer, Vector3Int nextPos)
    {

        if (posPlayer == nextPos)
        {
            canWalk = false;
        }

        // Verify if the next position is a grid position or a collision position
        if (!_groundTileMap.HasTile(nextPos) || _collisionTileMap.HasTile(nextPos))
            return false;

        return posPlayer != nextPos;
    }


}
