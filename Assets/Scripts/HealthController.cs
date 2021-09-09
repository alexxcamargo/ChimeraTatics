using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth;
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
