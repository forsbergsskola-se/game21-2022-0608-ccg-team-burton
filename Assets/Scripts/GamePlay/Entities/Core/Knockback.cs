using System.Collections;
using UnityEngine;

namespace Entity
{
 /// <summary>
 /// Knockback script. The knockback has a base knockback.
 /// The knockback is also affected by a multiplier that can come from thing calling the knockback (e.g. Werapon knockback multiplier)
 /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Knockback : MonoBehaviour
    {
        [SerializeField]
        private float baseKnockback = 1f;

        [SerializeField]
        private float knockbackDuration;
 
        public void DoKnockBack(Rigidbody2D hitTargetRb2d, Vector3 hitPosition, float knockbackMultiplier)
        {
            var knockBackDir = (transform.position - hitPosition).normalized;
            var originalVelocity = hitTargetRb2d.velocity;
            hitTargetRb2d.velocity = knockBackDir * (baseKnockback * knockbackMultiplier);
            StartCoroutine(HaltKnockBack(hitTargetRb2d,knockbackDuration, originalVelocity));
        }

        private IEnumerator HaltKnockBack(Rigidbody2D hitTargetRb2d, float knockbackDuration, Vector2 originalVelocity)
        {
            Debug.Log(knockbackDuration);
            yield return new WaitForSeconds(knockbackDuration);
            hitTargetRb2d.velocity = originalVelocity;
        }

    }
   
}