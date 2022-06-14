using Entity;
using UnityEngine;

/// <summary>
/// Temporary script for damaging the player. Should be modified or removed.
/// </summary>
public class TriggerDamage : MonoBehaviour
{
    private int _damage = 1; 

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{name} dealt damage to {col.gameObject.name}");
            // When applying damage, please do so in IDamageable
            col.gameObject.GetComponent<IDamageable>().ModifyHealth(-_damage);
        }
    }
}