using Entity;
using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class AttackNode : ActionNode
    {
        private float timeSinceAttack;
        
        public override void OnStart()
        {
            agent.anim.SetTrigger(Animator.StringToHash("Idle_Trigger"));
        }

        public override void OnExit()
        {
            
        }
        
        public void MeleeAttack()
        {
            var pLayer = 1 << 8;
            //agent.anim.SetTrigger(Animator.StringToHash("Enemy_Attack"));
            
            Collider2D[] hitEnemies = Physics2D
                .OverlapCircleAll(agent.attackPointTrans.position, agent.attackRange, pLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
             //   DealDamage(enemy);
               // _cameraShake.ShakeCamera(CamIntensity,CamTime);
            }
        }

        
        private void DealDamage(Collider2D enemy)
        {
            enemy.GetComponent<IDamageable>().ModifyHealth(-agent.damageAmount);
            enemy.GetComponent<Knockback>()?.DoKnockBack(enemy.GetComponent<Rigidbody2D>(), agent.attackPointTrans.position, 1); 
            agent.enemyTransform.GetComponent<HitEffect>().TimeStop();
        
        }

        public override State OnUpdate()
        {
            var comp = agent.enemyEyes.compoundActions;
            
            if (comp.HasFlag(CompoundActions.EnemyDead))
            {
                return State.Success;
            }

            if (timeSinceAttack >= agent.attackInterval && !comp.HasFlag(CompoundActions.EnemyDead))
            {
                MeleeAttack();
                timeSinceAttack -= agent.attackInterval;
            }
            timeSinceAttack += Time.deltaTime;
            
            if (!comp.HasFlag(CompoundActions.PlayerInAttackRange))
            {
                return State.Success;
            }

            if (comp.HasFlag(CompoundActions.PlayerBehind))
            {
                return State.Success;
            }

            return State.Update;
        }
    }
}