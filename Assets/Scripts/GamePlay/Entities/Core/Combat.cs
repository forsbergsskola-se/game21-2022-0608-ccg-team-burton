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
            enemy.GetComponent<IDamageable>()?.ModifyHealth(-1); // damage goes here!
            Debug.Log($"{enemy.name} Got Hit!");
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
