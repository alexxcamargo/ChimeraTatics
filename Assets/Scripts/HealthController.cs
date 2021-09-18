using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Heart System to player and enemies
/// </summary>
public class HealthController : MonoBehaviour
{
    
    public int maxHealth = 3;
    public int currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    public int Damage(int damage)
    {
        currentHealth -= damage;
        return currentHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
