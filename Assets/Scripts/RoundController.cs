using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the rounds of enemies and player
/// </summary>
public class RoundController : MonoBehaviour
{   
    public List<PlayerController> listPlayerControllers;
    public List<EnemyController> listEnemyControllers;
    private List<PlayerController> listPlayerControllersAlive;
    private List<EnemyController>  listEnemyControllersAlive;


    void Update()
    {
        listPlayerControllersAlive = listPlayerControllers.FindAll(p => p.GetCurrentState() != PlayerController.PlayerState.Dead);
        int qtdPlayerDefense = listPlayerControllers.FindAll(p => p.GetCurrentState() != PlayerController.PlayerState.Defense).Count;

        Debug.Log(listPlayerControllers.FindAll(p => p.GetCurrentState() == PlayerController.PlayerState.Defense).Count);
        
        if (qtdPlayerDefense == 0)
        {
            Debug.Log("Enemie Round");
            UIController._instance.SetTxtMessage("Enemies Round");

            listEnemyControllersAlive =
                listEnemyControllers.FindAll(e => e.GetCurrentState() != EnemyController.EnemyState.Dead);

            listEnemyControllersAlive[0].RoundEnemy();

            //listEnemyControllersAlive[0].target = listPlayerControllers[0].gameObject;
        }
    }



}
