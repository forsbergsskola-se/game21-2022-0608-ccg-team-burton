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

        public override State OnUpdate()
        {
            var comp = agent.enemyEyes.compoundActions;
            
            if (comp.HasFlag(CompoundActions.EnemyDead))
            {
                return State.Success;
            }

            if (timeSinceAttack >= agent.attackInterval && !comp.HasFlag(CompoundActions.EnemyDead))
            {
                agent.anim.SetTrigger(Animator.StringToHash("Enemy_Attack"));
                timeSinceAttack -= agent.attackInterval;
            }

            if (!comp.HasFlag(CompoundActions.PlayerInAttackRange))
            {
                return State.Success;
            }
            
            if (comp.HasFlag(CompoundActions.PlayerBehind))
            {
                return State.Success;
            }

            timeSinceAttack += Time.deltaTime;
            return State.Update;
        }
    }
}