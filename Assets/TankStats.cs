using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TankStats : MonoBehaviour, IDamageable
{
    [Header("Health")] 
    public int health;
    public int startHealth;
    public int maxHealth;

    private void Start()
    {
        health = startHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
