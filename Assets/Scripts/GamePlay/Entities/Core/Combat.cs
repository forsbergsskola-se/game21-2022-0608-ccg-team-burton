using System;
using Design.Jocke.Jocke_testSceneScripts;
using Entity;
using UnityEngine;

public class Combat : MonoBehaviour
{
    //PUBLIC VARIABLES
    public float CamIntensity = 5f;
    public float CamTime = 0f;
    public float AttackRange;
    public Transform AttackPoint;
    public LayerMask EnemyLayers;
    
    //PRIVATE VARIABLES
    private CameraShake _cameraShake;
    private Animator _anim;
    [SerializeField] private int _meleeDamage;
    
    [Header("DEBUGSTATS")] 
    [SerializeField]
     private int _knockbackMultiplier = 1;
     public GameObject DebugProjectile;
     public Transform FirePoint;
     
     void Awake()
     {
         _anim = GetComponent<Animator>();
         _cameraShake = FindObjectOfType<CameraShake>().GetComponent<CameraShake>();
     }

     private void Start()
     {
         if (gameObject.CompareTag("Player"))
         {
             _meleeDamage = (int)PlayerPrefs.GetFloat("buequipment.weapon.attributevalue") + (int)PlayerPrefs.GetFloat("gems.redgem.bonusid");
         }

     }

     public void MeleeAttack()
     {
         EnemyLayers = LayerMask.GetMask("Enemies");
         Debug.Log("attack");
         //Play Melee Attack
        _anim.SetTrigger("Attack");
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);
        foreach (Collider2D enemy in hitEnemies)
            
        {
            DealDamage(enemy);
            _cameraShake.ShakeCamera(CamIntensity,CamTime);
            Debug.Log("Camera shakiee");
        }
    }

     //Is triggered in rat attack animation event
     public void EnemyAttack()
     {
         EnemyLayers = LayerMask.GetMask("Player");
         
         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);
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
        Debug.Log("player found");
        //TODO: Cleanup - GetComponent is inefficient but we need to get that particular enemy script. Looking into make this better.
        Debug.Log($"DAMAGE: {_meleeDamage}");
        enemy.GetComponent<IDamageable>().ModifyHealth(-_meleeDamage); // TODO: Implement damage of weapon here
        Debug.Log($"{enemy.name} Got Hit!");
        enemy.GetComponent<Knockback>()?.DoKnockBack(enemy.GetComponent<Rigidbody2D>(), AttackPoint.position, _knockbackMultiplier); // TODO: Implement KnockbackMult of weapon here
        GetComponent<HitEffect>().TimeStop();
        
    }

    
    
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
            Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
