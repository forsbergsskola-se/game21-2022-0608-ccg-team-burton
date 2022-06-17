using Entity;
using UnityEngine;


namespace Entity.Hazard
{
    /// <summary>
    /// Temporary script for damaging the player. Should be modified or removed.
    /// </summary>
    public class DamageOnContact : MonoBehaviour
    {
        [SerializeField]
        private int _damage = 1;

        [SerializeField] private float _knockbackMultiplier=1;
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Debug.Log($"{name} dealt damage to {col.gameObject.name}");
                col.gameObject?.GetComponent<Knockback>().DoKnockBack(col.gameObject.GetComponent<Rigidbody2D>(),
                    -col.GetContact(0).normal, _knockbackMultiplier);
                // Debug.DrawRay(col.GetContact(0).point, -col.GetContact(0).normal*100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f );
                col.gameObject.GetComponent<IDamageable>().ModifyHealth(-_damage);
            }
        }
        
    }   
}