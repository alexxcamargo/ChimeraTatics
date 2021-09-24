using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Range(5, 30)]
    public int MaxDamageOnAttack;
    
    [SerializeField]
    private Tilemap _groundTileMap;
    [SerializeField]
    private Tilemap _collisionTileMap;

    public String name;
    public Sprite imgHud;

    private bool _canWalk = true;
    private Animator _animator;
    private AnimationState _currentAnimationState;
    private int _indexEnemyToAttack;
    private EnemyState _currentState;
    private PlayerController _target;


    public enum EnemyState { Ready, EnableToAttack, Busy, Attack, Dead }

    public enum AnimationState { Idle, Dead }


    private void Awake()
    {
        _currentState = EnemyState.Ready;
        _animator = GetComponentInChildren<Animator>();
        _currentAnimationState = AnimationState.Idle;
    }

    public EnemyState GetCurrentState()
    {
        return _currentState;
    }

    public void SetState(EnemyState newState)
    {
        if (GetCurrentState() == EnemyState.Dead || newState == EnemyState.Dead)
        {
            _currentAnimationState = AnimationState.Dead;
            _currentState = EnemyState.Dead;
            this.gameObject.SetActive(false);
            return;
        }

        _currentState = newState;
    }
    void FixedUpdate()
    {
        ChangeAnimation();
    }

    void ChangeAnimation()
    {
        _animator.Play(Enum.GetName(typeof(AnimationState), _currentAnimationState));
    }
    

    public void StartRoundEnemy()
    {
        if (GetCurrentState() != EnemyState.Attack && GetCurrentState() != EnemyState.Busy)
        {
            SetState(EnemyState.Attack);
            // Select Random Player To Attack
            List<PlayerController> lisPlayerAlive = RoundController._instance.GetPlayerAlive();
            _indexEnemyToAttack = Random.Range(0, lisPlayerAlive.Count);
            _target = lisPlayerAlive[_indexEnemyToAttack];

            UIController._instance.ShowHudEnemy(this.gameObject.GetComponent<HealthController>().GetCurrentHealth(), name, imgHud);
            
            StartCoroutine(MoveEnemy());
        }
    }

    private IEnumerator MoveEnemy()
    {
        Vector3Int nextPos = new Vector3Int(0,0,0);
        
        while (_canWalk)
        {
            Vector3Int enemyPostion = _groundTileMap.WorldToCell(transform.position);
            Vector3Int posPlayer = _groundTileMap.WorldToCell(_target.transform.position);

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
                _canWalk = false;
            }
        }

        // After walk Trying To Attack the player
        AttackPlayer();
        yield return new WaitForSeconds(.5f);
    }

    /// <summary>
    /// If the next step is the same position of the player stop move
    /// </summary>
    /// <returns></returns>
    bool VerifyNextStep(Vector3Int posPlayer, Vector3Int nextPos)
    {

        if (posPlayer == nextPos)
        {
            _canWalk = false;
        }

        // Verify if the next position is a grid position or a collision position
        if (!_groundTileMap.HasTile(nextPos) || _collisionTileMap.HasTile(nextPos))
            return false;

        return posPlayer != nextPos;
    }

    /// <summary>
    /// After Attack the player Set Enemy to busy and call Enemy round again to the next enemy Ready Can Attack
    /// </summary>
    void AttackPlayer()
    {

        int damage = Random.Range(5, MaxDamageOnAttack);

        if (_target.GetComponent<HealthController>().Damage(damage) <= 0)
        {
            _target.SetState(PlayerController.PlayerState.Dead);
        }

        UIController._instance.SetTxtMessage($@"The {_target.playerName} loses {damage} hitpoints");
        
        SetState(EnemyState.Busy);
        RoundController._instance.EnemyRound();
        _canWalk = true;
    }


}
