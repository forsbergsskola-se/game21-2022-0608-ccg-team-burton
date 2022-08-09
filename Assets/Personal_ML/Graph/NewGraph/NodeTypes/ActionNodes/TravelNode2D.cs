using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class TravelNode2D : ActionNode
    {
        public override void OnStart()
        {
            agent.anim.SetBool(Animator.StringToHash("Enemy_Walk2"), true);
         
            if (CheckIfLookingAtTarget())
            {
                agent.enemyTransform.Rotate(new Vector3(0, 1,0), 180);
            }
        }

        public override void OnExit()
        {
            agent.anim.SetBool(Animator.StringToHash("Enemy_Walk2"), false);
        }
        
        private bool CheckIfLookingAtTarget()
        {
            var dirFromAtoB = (agent.enemyTransform.position - agent.currentDestination).normalized;
            var dotProd = Vector2.Dot(dirFromAtoB, agent.enemyTransform.right);
            return dotProd > 0.9f;
        }
        
        
        private bool ArrivedAtTarget()
        {
            return Vector3.Distance(agent.enemyTransform.position, agent.currentDestination) < 0.6f;
        }
        

        public override State OnUpdate()
        {
            agent.enemyTransform.position += agent.enemyTransform.right * (Time.deltaTime * 1);

            if (!agent.keepWalking)
            {
                if (ArrivedAtTarget() || agent.quitNode || agent.enemyEyes.QuitNode)
                {
                    return State.Success;
                }
            }
            
            else
            {
                if (agent.quitNode || !agent.enemyEyes.GroundSeen)
                {
                    return State.Success;
                }
            }

            return State.Update;
        }
    }
}