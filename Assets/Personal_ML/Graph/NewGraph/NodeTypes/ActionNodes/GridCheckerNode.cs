using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class GridCheckerNode : ActionNode
    {
        private CubeFacts _current;
        public override void OnStart()
        {
            _current = agent.grid.GetSquareFromPoint(agent.enemyTransform.position);
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

        public override State OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}