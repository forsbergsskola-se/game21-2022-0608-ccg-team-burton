using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("Enemy Got Hit!");
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
