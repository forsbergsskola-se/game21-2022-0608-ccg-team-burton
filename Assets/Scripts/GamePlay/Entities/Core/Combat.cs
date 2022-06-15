using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;
using Color = System.Drawing.Color;

public class Combat : MonoBehaviour
{
    public Animator anim;
    public float attackRange;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("DEBUGSTATS")] 
    [SerializeField]
    private int _meleeDamage =1;
    [SerializeField]
     private int _knockbackMultiplier = 1;
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
        enemy.GetComponent<IDamageable>()?.ModifyHealth(-_meleeDamage); // TODO: Implement damage of weapon here
        Debug.Log($"{enemy.name} Got Hit!");
        GetComponent<Knockback>()?.DoKnockBack(enemy.GetComponent<Rigidbody2D>(), attackPoint.position, _knockbackMultiplier); // TODO: Implement KnockbackMult of weapon here
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
