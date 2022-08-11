using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class GridCheckerNode : ActionNode
    {
        private CubeFacts _current;

        private bool _plusMinus;

        private bool _choiceMade;
        
        public override void OnStart()
        {
            _plusMinus = agent.enemyTransform.right.x > 0;
            _current = agent.grid.GetSquareFromPoint(agent.enemyTransform.position);
            
            CheckOptions();
        }

        public override void OnExit()
        {
           
        }

        private void CheckOptions()
        {
            
       //     var xPos = _current.pointsList.FindAll(x => x.pointType == LevelElements.Gap)[0].location.x;
       agent.currentDestination = new Vector3(_current.max.x ,agent.enemyTransform.position.y);
            _choiceMade = true;
            return;
            
       
        }
        
        private bool CheckIfLookingAtTarget()
        {
            var dirFromAtoB = (agent.enemyTransform.position - agent.currentDestination).normalized;
            var dotProd = Vector2.Dot(dirFromAtoB, agent.enemyTransform.right);
            return dotProd > 0.9f;
        }

        public override State OnUpdate()
        {
            return _choiceMade ? State.Success : State.Update;
        }
    }
}