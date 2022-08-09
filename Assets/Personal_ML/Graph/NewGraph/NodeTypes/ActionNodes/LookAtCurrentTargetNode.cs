using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class LookAtCurrentTargetNode : ActionNode
    {
        public override void OnStart()
        {
            
        }

        public override void OnExit()
        {
            
        }
        
        private bool CheckIfLookingAtTarget()
        {
            var dirFromAtoB = (agent.enemyTransform.position - agent.currentDestination).normalized;
            var dotProd = Vector3.Dot(dirFromAtoB, agent.enemyTransform.forward);
            return dotProd > 0.99f;
        }

        public override State OnUpdate()
        {
            if (!CheckIfLookingAtTarget())
            {
                var targetDirection = agent.currentDestination - agent.enemyTransform.position;
                var singleStep = Time.deltaTime * 1;
                var newDirection = Vector3.RotateTowards(agent.enemyTransform.forward, targetDirection, singleStep, 0.0f);
                agent.enemyTransform.rotation = Quaternion.LookRotation(newDirection);
            }
            else
            {
                return State.Success;
            }
            
            return State.Update;
        }
    }
}