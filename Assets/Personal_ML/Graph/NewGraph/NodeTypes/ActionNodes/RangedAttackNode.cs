using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class RangedAttackNode : ActionNode
    {
        private float _timeSinceAttack;

        public override void OnStart()
        {
        }

        public override void OnExit()
        {
            
        }

        private void SpawnProjectile()
        {
            var right = agent.enemyTransform.right;
            var pos = agent.attackPointTrans.position;
            var temp = Instantiate(agent.projectile, pos, Quaternion.identity, agent.enemyTransform).GetComponent<Bullet_ML>();
            temp.travelVector = right;
            temp.moveSpeed = agent.moveSpeed;
            temp.damageAmount = agent.damageAmount;
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
            
            if (!agent.anim.GetCurrentAnimatorStateInfo(0).IsName("Cannon_Shoot"))
            {
                _timeSinceAttack += Time.deltaTime;
            }
            
            
            if (!playerEncounter.HasFlag(CompoundActions.PlayerNoticed))
            {
                return State.Success;
            }
            
            
            return State.Update;
        }
    }
}