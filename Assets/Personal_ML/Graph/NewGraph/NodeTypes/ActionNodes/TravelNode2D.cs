using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class TravelNode2D : ActionNode
    {
        public float waitForExit;
        public float waitForTurn;
        public bool canTurn;
        
        public override void OnStart()
        {
            Debug.Log("start walking");
            agent.anim.SetBool(Animator.StringToHash("Enemy_Walk2"), true);
            waitForExit = 0;
            canTurn = true;
         
            if (CheckIfLookingAtTarget() || agent.compoundAction.HasFlag(CompoundActions.Rotate))
            {
                RotateEnemy();
            }
        }

        private void RotateEnemy()
        {
            agent.enemyTransform.Rotate(new Vector3(0, 1,0), 180);
        }
        
        public override void OnExit()
        {
            Debug.Log("stop walking");
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
            return Vector3.Distance(agent.enemyTransform.position, agent.currentDestination) < 2.5f;
        }
        

        public override State OnUpdate()
        {
            var comp = agent.enemyEyes.compoundActions;
            if (comp.HasFlag(CompoundActions.EnemyDead))
            {
                return State.Success;
            }

            waitForExit += Time.deltaTime;
            if (waitForExit < 0.5f) return State.Update;


            agent.enemyTransform.position += agent.enemyTransform.right * (Time.deltaTime * agent.moveSpeed);

            if (comp.HasFlag(CompoundActions.WallInTurnRange) && canTurn)
            {
                canTurn = false;
                RotateEnemy();
            }

            if (!canTurn)
            {
                waitForTurn += Time.deltaTime;
                if (waitForTurn > 0.9f)
                {
                    waitForTurn -= 0.9f;
                    canTurn = true;
                }
            }
            
            if (!agent.keepWalking)
            {
                if (ArrivedAtTarget() || !comp.HasFlag(CompoundActions.GroundSeen))
                {
                    return State.Success;
                }
            }
            
            else
            {
                if (!comp.HasFlag(CompoundActions.GroundSeen) || comp.HasFlag(CompoundActions.PlayerNoticed))
                {
                    return State.Success;
                }
                if (comp.HasFlag(CompoundActions.PlayerNoticed))
                {
                    return State.Success;
                }
            }

            return State.Update;
        }
    }
}