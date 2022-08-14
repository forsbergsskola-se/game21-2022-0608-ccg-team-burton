using Entity;
using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class AttackNode : ActionNode
    {
        private float timeSinceAttack;
        
        public override void OnStart()
        {
            
        }

        public override void OnExit()
        {
            
        }
        
        public void MeleeAttack()
        {
            var pLayer = 1 << 8;
            agent.anim.SetTrigger(Animator.StringToHash("Attack_Trigger"));

            Collider2D[] hitEnemies = Physics2D
                .OverlapCircleAll(agent.attackPointTrans.position, agent.attackRange, pLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                DealDamage(enemy);
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
                agent.anim.SetTrigger(Animator.StringToHash("Attack_Trigger"));
                timeSinceAttack -= agent.attackInterval;
            }
            if (!agent.anim.GetCurrentAnimatorStateInfo(0).IsName("Rat_Attack"))
            {
                timeSinceAttack += Time.deltaTime;
            }
            
            
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