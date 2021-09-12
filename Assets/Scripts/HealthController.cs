using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Heart System to characters and player
/// </summary>
public class HealthController : MonoBehaviour
{
    // Can be change in Inspector
    public int maxHealth = 3;
    private int currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    public int Damage(int damage)
    {
        return currentHealth - damage;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
