using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class TravelNode2D : ActionNode
    {
        public override void OnStart()
        {
            if (!CheckIfLookingAtTarget())
            {
                agent.enemyTransform.Rotate(new Vector3(0, 1,0), 180);
            }
        }

        public override void OnExit()
        {
            
        }
        
        private bool CheckIfLookingAtTarget()
        {
            var dirFromAtoB = (agent.enemyTransform.position - agent.currentDestination).normalized;
            var dotProd = Vector2.Dot(dirFromAtoB, agent.enemyTransform.right);
            return dotProd > 0.9f;
        }
        
        
        private bool ArrivedAtTarget()
        {
            return Vector3.Distance(agent.enemyTransform.position, agent.currentDestination) < 1f;
        }
        

        public override State OnUpdate()
        {
            agent.enemyTransform.position += agent.enemyTransform.right * (Time.deltaTime * 1);
            return ArrivedAtTarget() ? State.Success : State.Update;
        }
    }
}