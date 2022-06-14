using Entity;
using UnityEngine;

/// <summary>
/// Temporary script for damaging the player. Should be modified or removed.
/// </summary>
public class TriggerDamage : MonoBehaviour //TODO: To be replaced with whatever damages the player, enemy, barrel and so on
{
    private int _damage = 1; 

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{name} dealt damage to {col.gameObject.name}");
            // When applying damage, please do so in IDamageable
            col.gameObject.GetComponent<IDamageable>().ModifyHealth(-_damage);
        }
    }
}