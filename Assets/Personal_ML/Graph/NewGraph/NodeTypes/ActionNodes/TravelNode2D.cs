using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class TravelNode2D : ActionNode
    {
        public float waitForExit;
        public float waitForTurn;
        public bool canTurn;
        private WalkableGround _currentGround;

        public override void OnStart()
        {
            _currentGround = agent.grid.GetCurrentGround(agent.enemyTransform.position);
            agent.anim.SetBool(Animator.StringToHash("Enemy_Walk"), true);
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
            agent.anim.SetBool(Animator.StringToHash("Enemy_Walk"), false);
        }
        
        private bool CheckIfLookingAtTarget()
        {
            var dirFromAtoB = (agent.enemyTransform.position - agent.currentDestination).normalized;
            var dotProd = Vector2.Dot(dirFromAtoB, agent.enemyTransform.right);
            return dotProd > 0.9f;
        }
        
        
        private bool ArrivedAtTarget()
        {
            return Vector3.Distance(agent.attackPointTrans.position, agent.currentDestination) < agent.turnDistance;
        }

        private bool InsideArea()
        {
            Vector2 futurePos = agent.attackPointTrans.position 
                                + new Vector3(agent.enemyTransform.right.x * agent.turnDistance, 0);
            return agent.grid.IsPointInCube(_currentGround, futurePos);
        }

        public override State OnUpdate()
        {
            var comp = agent.enemyEyes.compoundActions;
       
            waitForExit += Time.deltaTime;
            if (waitForExit < 0.5f) return State.Update;
            
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
            
            if (comp.HasFlag(CompoundActions.EnemyDead)) return State.Success;
            if (comp.HasFlag(CompoundActions.PlayerBehind)) return State.Success;
            if (!comp.HasFlag(CompoundActions.GroundSeen))
            {
                if (!comp.HasFlag(CompoundActions.LowerGroundSeen))
                {
                    return State.Success;
                }    
            }
            if (comp.HasFlag(CompoundActions.PlayerInAttackRange))return State.Success;
            //if (comp.HasFlag(CompoundActions.HigherGroundSeen))return State.Success;
            if (!InsideArea())
            {
                Debug.Log($"arrived at target: {agent.enemyTransform.name}");
                agent.enemyEyes.compoundActions |= CompoundActions.ArrivedAtTarget;
                return State.Success;
            }
            
            if (ArrivedAtTarget())
            {
                Debug.Log($"arrived at target: {agent.enemyTransform.name}");
                agent.enemyEyes.compoundActions |= CompoundActions.ArrivedAtTarget;
                return State.Success;
            }

            agent.enemyTransform.position += agent.enemyTransform.right * (Time.deltaTime * agent.moveSpeed);
            return State.Update;
        }
    }
}