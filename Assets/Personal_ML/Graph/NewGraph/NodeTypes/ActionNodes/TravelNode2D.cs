using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class TravelNode2D : ActionNode
    {
        public float waitForExit;
        
        public override void OnStart()
        {
            Debug.Log("starting to walk");
            agent.anim.SetBool(Animator.StringToHash("Enemy_Walk2"), true);
            waitForExit = 0;
         
            if (CheckIfLookingAtTarget() || !agent.enemyEyes.GroundSeen)
            {
                Debug.Log("turning");
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
            return Vector3.Distance(agent.enemyTransform.position, agent.currentDestination) < 3f;
        }
        

        public override State OnUpdate()
        {
            waitForExit += Time.deltaTime;
            if (waitForExit < 0.1f) return State.Update;
            
            agent.enemyTransform.position += agent.enemyTransform.right * (Time.deltaTime * agent.moveSpeed);

            if (!agent.keepWalking)
            {
                if (ArrivedAtTarget() || !agent.enemyEyes.GroundSeen)
                {
                    Debug.Log("arriving at target");
                    return State.Success;
                }
            }
            
            else
            {
                if (!agent.enemyEyes.GroundSeen || agent.enemyEyes.playerEncounter.HasFlag(PlayerEncounter.PlayerNoticed))
                {
                    Debug.Log("target reached");
                    return State.Success;
                }
                if (agent.enemyEyes.playerEncounter.HasFlag(PlayerEncounter.PlayerNoticed))
                {
                    return State.Success;
                }
            }

            return State.Update;
        }
    }
}