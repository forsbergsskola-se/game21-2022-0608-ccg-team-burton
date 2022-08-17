using Entity;
using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class AttackNode : ActionNode
    {
        private float timeSinceAttack;
        
        public override void OnStart()
        {
            agent.body.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public override void OnExit()
        {
            agent.body.constraints = RigidbodyConstraints2D.FreezeRotation;
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