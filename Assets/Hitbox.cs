using System;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]private Projectile projectile;
    [SerializeField]private GameObject parentTank;
    [SerializeField]private TagHandle parentTankTag;
    
    private void OnEnable()
    {
        projectile = GetComponentInParent<Projectile>();
        parentTank = projectile.parentTank;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!parentTank)
        {
            parentTank = projectile.parentTank;
        }
        
        if (other.gameObject.name != parentTank.name)
        {
            other.gameObject.GetComponent<IDamageable>()?.TakeDamage(1);
        }
    }
}
