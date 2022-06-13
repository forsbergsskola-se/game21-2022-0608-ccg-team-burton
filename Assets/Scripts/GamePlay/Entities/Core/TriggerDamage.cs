using Entity;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    private int _damage = 1;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{name} dealt damage to {col.gameObject.name}");
            col.gameObject.GetComponent<Health>().ModifyHealth(-_damage);
        }
    }
}
