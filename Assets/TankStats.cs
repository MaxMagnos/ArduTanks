using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TankStats : MonoBehaviour, IDamageable
{
    [Header("Health")] 
    public int health;
    public int startHealth;
    public int maxHealth;

    [Header("Ammo")] 
    public int ammo;
    public int startAmmo;
    public int maxAmmo;

    private void Start()
    {
        health = startHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
