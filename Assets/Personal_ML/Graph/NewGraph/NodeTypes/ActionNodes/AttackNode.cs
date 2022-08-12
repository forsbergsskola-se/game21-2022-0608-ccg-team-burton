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
            var playerEncounter = agent.enemyEyes.compoundActions;
            if (timeSinceAttack >= agent.attackInterval)
            {
                agent.anim.SetTrigger(Animator.StringToHash("Enemy_Attack"));
                timeSinceAttack -= agent.attackInterval;
            }

            if (!playerEncounter.HasFlag(CompoundActions.PlayerInAttackRange))
            {
                return State.Success;
            }
            
            if (playerEncounter.HasFlag(CompoundActions.PlayerBehind))
            {
                return State.Success;
            }

            timeSinceAttack += Time.deltaTime;
            return State.Update;
        }
    }
}