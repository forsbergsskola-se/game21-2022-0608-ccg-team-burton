using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class IdleNode : ActionNode
    {
        public override void OnStart()
        {
           
        }

        public override void OnExit()
        {
            
        }

        public override State OnUpdate()
        {
            var eyeComp = agent.enemyEyes.compoundActions;

            if (eyeComp.HasFlag(CompoundActions.PlayerNoticed))
            {
                return State.Success;
            }

            return State.Update;
        }
    }
}