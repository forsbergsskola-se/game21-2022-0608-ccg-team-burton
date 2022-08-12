using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class RangedAttackNode : ActionNode
    {
        private float _timeSinceAttack;

        public override void OnStart()
        {
            Debug.Log("enter attack mode");
        }

        public override void OnExit()
        {
            
        }

        private void SpawnProjectile()
        {
            var right = agent.enemyTransform.right;
            var pos = agent.enemyTransform.position;
            var temp = Instantiate(agent.projectile, pos, Quaternion.identity);
            temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(right.x * 10, 10), ForceMode2D.Impulse);
        }
        
        public override State OnUpdate()
        {
            var playerEncounter = agent.enemyEyes.compoundActions;
            if (_timeSinceAttack >= agent.attackInterval)
            {
                agent.anim.SetTrigger(Animator.StringToHash("Enemy_Attack"));
                _timeSinceAttack -= agent.attackInterval;
                SpawnProjectile();
            }
            _timeSinceAttack += Time.deltaTime;
            
            if (!playerEncounter.HasFlag(CompoundActions.PlayerNoticed))
            {
                return State.Success;
            }

            if (!playerEncounter.HasFlag(CompoundActions.PlayerInFront))
            {
              //  return State.Success;
            }

            return State.Update;
        }
    }
}