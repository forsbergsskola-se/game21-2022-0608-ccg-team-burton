using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class AttackNode : ActionNode
    {
        public override void OnStart()
        {
            agent.anim.SetBool(Animator.StringToHash("Enemy_Walk2"), true);
        }

        public override void OnExit()
        {
            
        }

        public override State OnUpdate()
        {
            return State.Update;
        }
    }
}