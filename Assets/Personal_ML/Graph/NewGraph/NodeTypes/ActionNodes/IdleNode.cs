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
            Debug.Log("idle mode exit");
        }

        public override State OnUpdate()
        {
            
            var eyeComp = agent.enemyEyes.compoundActions;

            if (eyeComp.HasFlag(CompoundActions.PlayerNoticed))
            {
                Debug.Log("Player noticed");
                return State.Success;
            }

            return State.Update;
        }
    }
}