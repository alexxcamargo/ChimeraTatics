using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Add enemies on list you can attack with the Mage Character
/// </summary>
public class MagicAttack : MonoBehaviour
{
    private PlayerController _playerMagicController;
    
    void Awake()
    {
        _playerMagicController = GetComponentInParent<PlayerController>();
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "AI")
        {
            EnemyController enemy = col.gameObject.GetComponent<EnemyController>();
            ChangeListEnemiesPlayer(enemy, true);
            enemy.SetState(EnemyController.EnemyState.EnableToAttack);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "AI")
        {
            EnemyController enemy = col.gameObject.GetComponent<EnemyController>();
            ChangeListEnemiesPlayer(enemy, false);
            enemy.SetState(EnemyController.EnemyState.Ready);
        }
    }

    private void ChangeListEnemiesPlayer(EnemyController enemyController, bool add)
    {
        if (add)
        {
            if (!_playerMagicController.enemiesToAttack.Contains(enemyController))
                _playerMagicController.enemiesToAttack.Add(enemyController);
        }
        else
        {
            if (_playerMagicController.enemiesToAttack.Contains(enemyController))
                _playerMagicController.enemiesToAttack.Remove(enemyController);
        }
    }
}
