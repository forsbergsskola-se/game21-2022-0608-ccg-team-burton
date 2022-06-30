using Design.Jocke.Jocke_testSceneScripts;
using Entity;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private CameraShake _cameraShake;
    public float CamIntensity = 5f;
    public float CamTime = 0f;
    private Animator anim;
    public float attackRange;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    
    [Header("DEBUGSTATS")] 
    [SerializeField]
    private int _meleeDamage =1;
    [SerializeField]
     private int _knockbackMultiplier = 1;
     public GameObject DebugProjectile;
     public Transform FirePoint;

     
     
     void Awake()
     {
         anim = GetComponent<Animator>();
         _cameraShake = FindObjectOfType<CameraShake>().GetComponent<CameraShake>();
     }
     


     public void MeleeAttack()
    {
        //Play Melee Attack
        anim.SetTrigger("Attack");
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
            
        {
            DealDamage(enemy);
            _cameraShake.ShakeCamera(CamIntensity,CamTime);
            Debug.Log("Camera shakiee");
        }
    }
    
    
    
    public void RangedAttack()
    {
        var projectile = Instantiate(DebugProjectile, FirePoint.position, Quaternion.identity);
    }
    
    
    
    private void DealDamage(Collider2D enemy)
    {
        //TODO: Cleanup - GetComponent is inefficient but we need to get that particular enemy script. Looking into make this better.
        enemy.GetComponent<IDamageable>().ModifyHealth(-_meleeDamage); // TODO: Implement damage of weapon here
        Debug.Log($"{enemy.name} Got Hit!");
        enemy.GetComponent<Knockback>()?.DoKnockBack(enemy.GetComponent<Rigidbody2D>(), attackPoint.position, _knockbackMultiplier); // TODO: Implement KnockbackMult of weapon here
        GetComponent<HitEffect>().TimeStop();
        
    }

    
    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
