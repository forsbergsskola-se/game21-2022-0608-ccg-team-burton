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
            if (timeSinceAttack >= agent.attackInterval)
            {
                agent.anim.SetTrigger(Animator.StringToHash("Enemy_Attack"));
                timeSinceAttack -= agent.attackInterval;
            }

            timeSinceAttack += Time.deltaTime;
            return State.Update;
        }
    }
}