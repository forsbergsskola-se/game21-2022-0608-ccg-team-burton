using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class RotateNode : ActionNode
    {
        public override void OnStart()
        {
            
        }

        public override void OnExit()
        {
        }

        public override State OnUpdate()
        {
            agent.enemyTransform.Rotate(new Vector3(0, 1, 0), 3 * Time.deltaTime);

            return State.Success;
        }
    }
}