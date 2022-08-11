using UnityEngine;

namespace NewGraph.NodeTypes.ActionNodes
{
    public class JumpNode : ActionNode
    {
        private bool _startedJump;
        
        public override void OnStart()
        {
            Debug.Log("starting jump");
            _startedJump = false;
        }

        public override void OnExit()
        {
            Debug.Log("exit jump");
        }

        public override State OnUpdate()
        {
            if (!_startedJump)
            {
                var right = agent.enemyTransform.right;
                agent.body.AddForce( new Vector2(right.x * 10f, 10f), ForceMode2D.Impulse);
                agent.anim.SetTrigger(Animator.StringToHash("Enemy_Jump"));
                _startedJump = true;    
            }

            if (_startedJump && agent.enemyEyes.GroundSeen)
            {
                return State.Success;
            }
            
            return State.Update;
        }
    }
}