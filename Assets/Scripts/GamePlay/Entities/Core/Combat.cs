using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Animator anim;
    public float attackRange;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public void MeeleAttack()
    {
        //Play Meele Attack 
        //anim.SetTrigger();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            //deal Damage!
            DealDamage(enemy);
        }
    }

    private void DealDamage(Collider2D enemy)
    {
        //TODO: Cleanup - Getcomponent is inefficient but we need to get that particular enemy script. Looking into make this better.
        enemy.GetComponent<IDamageable>()?.ModifyHealth(-1); // TODO: Implement damage of weapon here
        Debug.Log($"{enemy.name} Got Hit!");
        enemy.GetComponent<Knockback>()?.DoKnockBack(enemy.GetComponent<Rigidbody2D>(), attackPoint.position, 1f); // TODO: Implement KnockbackMult of weapon here
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
