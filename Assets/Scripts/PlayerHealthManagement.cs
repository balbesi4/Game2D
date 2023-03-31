using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManagement : MonoBehaviour
{
    public float MaxHealth { get; private set; }
    public float Health { get; private set; }

    private void Start()
    {
        MaxHealth = 100;
        Health = MaxHealth;
    }

    public void Heal(float healValue)
    {
        Health = healValue + Health > MaxHealth ? MaxHealth : healValue + Health;
    }

    public void DealDamage(float damage)
    {
        Health -= damage;
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (Health <= 0)
        {
            Health = 0;
            Destroy(gameObject);
        }
    }
}
