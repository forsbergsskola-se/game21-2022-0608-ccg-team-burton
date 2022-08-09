using UnityEngine;

namespace NewGraph.NodeTypes.CompositeNodes
{
    public class MoveController : CompositeNode
    {
        public override void OnStart()
        {
            
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