using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the rounds of enemies and player
/// </summary>
public class RoundController : MonoBehaviour
{   
    public List<PlayerController> listPlayerControllers;
    
    void Update()
    {
        Debug.Log(listPlayerControllers.FindAll(p => p.GetCurrentState() == PlayerController.PlayerState.Defense).Count);
        
        if (listPlayerControllers.FindAll(p => p.GetCurrentState() != PlayerController.PlayerState.Defense).Count == 0)
        {
            Debug.Log("Enemie Round");
            UIController._instance.SetTxtMessage("Enemies Round");
        }
    }

}
