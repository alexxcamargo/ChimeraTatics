using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Heart System to player and enemies
/// </summary>
public class HealthController : MonoBehaviour
{
    
    public int maxHealth = 20;
    private int _currentHealth;

    public void Awake()
    {
        _currentHealth = maxHealth;
    }

    public int Damage(int damage)
    {
        _currentHealth -= damage;
        return _currentHealth;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }
}
