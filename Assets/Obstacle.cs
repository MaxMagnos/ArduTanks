using UnityEngine;
using UnityEngine.EventSystems;

public class Obstacle : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        Destroy(gameObject);
    }
}
